// Documenter.cs - base XML documenter code
// Copyright (C) 2001  Kral Ferch, Jason Diamond
//
// This program is free software; you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation; either version 2 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA

using System;
using System.Collections;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Xml;

namespace NDoc.Core
{
	/// <summary>Provides the base class for documenters.</summary>
	abstract public class BaseDocumenter : IDocumenter, IComparable
	{
		IDocumenterConfig config;

		Hashtable documenterNamespaceSummaries;

		Assembly currentAssembly;
		XmlDocument currentSlashDoc;

		XmlDocument xmlDocument;

		/// <summary>Initialized a new BaseDocumenter instance.</summary>
		protected BaseDocumenter(string name)
		{
			_Name = name;
			xmlDocument = new XmlDocument();
		}

		/// <summary>Compares the currrent document to another documenter.</summary>
		public int CompareTo(object obj)
		{
			return String.Compare(Name, ((IDocumenter)obj).Name);
		}

		/// <summary>See <see cref="IDocumenter"/>.</summary>
		public IDocumenterConfig Config
		{
			get { return config; }
			set { config = value; }
		}

		/// <summary>Gets the XmlDocument containing the combined relected metadata and /doc comments.</summary>
		protected XmlDocument Document
		{
			get { return xmlDocument; }
		}

		private string _Name;

		/// <summary>Gets the display name for the documenter.</summary>
		public string Name
		{
			get { return _Name; }
		}

		/// <summary>See <see cref="IDocumenter"/>.</summary>
		public event DocBuildingEventHandler DocBuildingStep;
		/// <summary>See <see cref="IDocumenter"/>.</summary>
		public event DocBuildingEventHandler DocBuildingProgress;

		/// <summary>Raises the DocBuildingStep event.</summary>
		/// <param name="step">The overall percent complete value.</param>
		/// <param name="label">A description of the work currently beeing done.</param>
		protected void OnDocBuildingStep(int step, string label)
		{
			if (DocBuildingStep != null)
				DocBuildingStep(this, new ProgressArgs(step, label));
		}

		/// <summary>Raises the DocBuildingProgress event.</summary>
		/// <param name="progress">Percentage progress value</param>
		protected void OnDocBuildingProgress(int progress)
		{
			if (DocBuildingProgress != null)
				DocBuildingProgress(this, new ProgressArgs(progress, ""));
		}

		/// <summary>See <see cref="IDocumenter"/>.</summary>
		abstract public void Clear();
		/// <summary>See <see cref="IDocumenter"/>.</summary>
		abstract public void Build(Project project);
		/// <summary>See <see cref="IDocumenter"/>.</summary>
		abstract public void View();

		/// <summary>Builds an XmlDocument combining the reflected metadata with the /doc comments.</summary>
		/// <param name="assemblySlashDocs">The assembly and /doc filenames.</param>
		/// <param name="namespaceSummaries">The namespace summaries.</param>
		protected void MakeXml(ArrayList assemblySlashDocs, Hashtable namespaceSummaries)
		{
			// Sucks that there's no XmlNodeWriter.  Instead we
			// have to write out to a file then load back in.
			XmlWriter   writer = new XmlTextWriter(@".\temp.xml", null);
			string      currentAssemblyFilename = "";

			try
			{
				documenterNamespaceSummaries = namespaceSummaries;

				// Use indenting for readability
				//writer.Formatting = Formatting.Indented;
				//writer.Indentation=4;

				// Start the document with the XML declaration tag
				writer.WriteStartDocument();

				// Start the root element
				writer.WriteStartElement("ndoc");

				int step = 100 / assemblySlashDocs.Count;
				int i = 0;
				
				foreach (AssemblySlashDoc assemblySlashDoc in assemblySlashDocs)
				{
					OnDocBuildingProgress(i * step);

					currentAssemblyFilename = assemblySlashDoc.AssemblyFilename;
					string path = Path.GetFullPath(currentAssemblyFilename);
					currentAssembly = Assembly.LoadFrom(path);

					currentSlashDoc = new XmlDocument();
					currentSlashDoc.Load(assemblySlashDoc.SlashDocFilename);

					Write(writer);

					i++;
				}

				OnDocBuildingProgress(100);

				writer.WriteEndElement();

				writer.WriteEndDocument();

				writer.Close();

				xmlDocument.Load(@".\temp.xml");
			}
			catch (Exception e)
			{
				throw new DocumenterException(
					"Error reflecting against the " +
					Path.GetFileName(currentAssemblyFilename) +
					" assembly: \n" + e.Message, e);
			}
			finally
			{
				writer.Close();
				File.Delete(@".\temp.xml");
			}
		}

		private void Write(XmlWriter writer)
		{
			writer.WriteStartElement("assembly");
			writer.WriteAttributeString("name", currentAssembly.GetName().Name);

			foreach(Module module in currentAssembly.GetModules())
			{
				WriteModule(writer, module);
			}

			writer.WriteEndElement(); // assembly
		}

		/// <summary>Writes documentation about a module out as XML.</summary>
		/// <param name="writer">XmlWriter to write on.</param>
		/// <param name="module">Module to document.</param>
		private void WriteModule(XmlWriter writer, Module module)
		{
			writer.WriteStartElement("module");
			writer.WriteAttributeString("name", module.ScopeName);
			WriteNamespaces(writer, module);
			writer.WriteEndElement();
		}

		private void WriteNamespaces(XmlWriter writer, Module module)
		{
			Type[] types = module.GetTypes();
			StringCollection namespaceNames = GetNamespaceNames(types);
			string ourNamespaceName;

			foreach (string namespaceName in namespaceNames)
			{
				writer.WriteStartElement("namespace");

				if (namespaceName == null)
				{
					writer.WriteAttributeString("name", "(global)");
					ourNamespaceName = "(global)";
				}
				else
				{
					writer.WriteAttributeString("name", namespaceName);
					ourNamespaceName = namespaceName;
				}

				if ((documenterNamespaceSummaries.Count > 0) &&
					(documenterNamespaceSummaries[ourNamespaceName] != null) &&
					(((string)documenterNamespaceSummaries[ourNamespaceName]).Length > 0))
				{
					WriteStartDocumentation(writer);
					writer.WriteStartElement("summary");
					writer.WriteRaw((string)documenterNamespaceSummaries[ourNamespaceName]);
					writer.WriteEndElement();
					WriteEndDocumentation(writer);
				}
				else if (MyConfig.ShowMissingSummaries)
				{
					WriteStartDocumentation(writer);
					WriteMissingDocumentation(writer, "summary", null);
					WriteEndDocumentation(writer);
				}

				WriteClasses(writer, types, namespaceName);
				WriteInterfaces(writer, types, namespaceName);
				WriteStructures(writer, types, namespaceName);
				WriteDelegates(writer, types, namespaceName);
				WriteEnumerations(writer, types, namespaceName);

				writer.WriteEndElement();
			}
		}

		private void WriteClasses(XmlWriter writer, Type[] types, string namespaceName)
		{
			foreach (Type type in types)
			{
				if (type.IsClass && !IsDelegate(type) && type.Namespace == namespaceName)
				{
					if (type.IsPublic ||
						(type.IsNotPublic && MyConfig.DocumentInternals) ||
						type.IsNestedPublic ||
						type.IsNestedFamily ||
						type.IsNestedFamORAssem ||
						(type.IsNestedAssembly && MyConfig.DocumentInternals) ||
						(type.IsNestedFamANDAssem && MyConfig.DocumentInternals) ||
						(type.IsNestedPrivate && MyConfig.DocumentInternals))
					{
						WriteClass(writer, type);
					}
				}
			}
		}

		private void WriteInterfaces(XmlWriter writer, Type[] types, string namespaceName)
		{
			foreach (Type type in types)
			{
				if (type.IsInterface && type.Namespace == namespaceName)
				{
					if (type.IsPublic ||
						(type.IsNotPublic && MyConfig.DocumentInternals) ||
						type.IsNestedPublic ||
						type.IsNestedFamily ||
						type.IsNestedFamORAssem ||
						(type.IsNestedAssembly && MyConfig.DocumentInternals) ||
						(type.IsNestedFamANDAssem && MyConfig.DocumentInternals) ||
						(type.IsNestedPrivate && MyConfig.DocumentInternals))
					{
						WriteInterface(writer, type);
					}
				}
			}
		}

		private void WriteStructures(XmlWriter writer, Type[] types, string namespaceName)
		{
			foreach (Type type in types)
			{
				if (type.IsValueType && !type.IsEnum && type.Namespace == namespaceName)
				{
					if (type.IsPublic ||
						(type.IsNotPublic && MyConfig.DocumentInternals) ||
						type.IsNestedPublic ||
						type.IsNestedFamily ||
						type.IsNestedFamORAssem ||
						(type.IsNestedAssembly && MyConfig.DocumentInternals) ||
						(type.IsNestedFamANDAssem && MyConfig.DocumentInternals) ||
						(type.IsNestedPrivate && MyConfig.DocumentInternals))
					{
						WriteClass(writer, type);
					}
				}
			}
		}

		private void WriteDelegates(XmlWriter writer, Type[] types, string namespaceName)
		{
			foreach (Type type in types)
			{
				if (type.IsClass && IsDelegate(type) && type.Namespace == namespaceName)
				{
					if (type.IsPublic ||
						(type.IsNotPublic && MyConfig.DocumentInternals) ||
						type.IsNestedPublic ||
						type.IsNestedFamily ||
						type.IsNestedFamORAssem ||
						(type.IsNestedAssembly && MyConfig.DocumentInternals) ||
						(type.IsNestedFamANDAssem && MyConfig.DocumentInternals) ||
						(type.IsNestedPrivate && MyConfig.DocumentInternals))
					{
						WriteDelegate(writer, type);
					}
				}
			}
		}

		private void WriteEnumerations(XmlWriter writer, Type[] types, string namespaceName)
		{
			foreach (Type type in types)
			{
				if (type.IsEnum && type.Namespace == namespaceName)
				{
					if (type.IsPublic ||
						(type.IsNotPublic && MyConfig.DocumentInternals) ||
						type.IsNestedPublic ||
						type.IsNestedFamily ||
						type.IsNestedFamORAssem ||
						(type.IsNestedAssembly && MyConfig.DocumentInternals) ||
						(type.IsNestedFamANDAssem && MyConfig.DocumentInternals) ||
						(type.IsNestedPrivate && MyConfig.DocumentInternals))
					{
						WriteEnumeration(writer, type);
					}
				}
			}
		}

		private bool IsDelegate(Type type)
		{
			return type.BaseType.FullName == "System.Delegate" || 
				type.BaseType.FullName == "System.MulticastDelegate";
		}

		private int GetMethodOverload(MethodInfo method, MethodInfo[] methods)
		{
			int count = 0;
			int overload = 0;

			foreach (MethodInfo m in methods)
			{
				if (m.Name == method.Name)
				{
					++count;
				}

				if (m == method)
				{
					overload = count;
				}
			}

			return (count > 1) ? overload : 0;
		}

		private BaseDocumenterConfig MyConfig
		{
			get
			{
				return (BaseDocumenterConfig)Config;
			}
		}

		/// <summary>Writes XML documenting a class.</summary>
		/// <param name="writer">XmlWriter to write on.</param>
		/// <param name="type">Class to document.</param>
		private void WriteClass(XmlWriter writer, Type type)
		{
			bool isStruct = type.IsValueType;

			string memberName = GetMemberName(type);

			string fullNameWithoutNamespace = type.FullName.Replace('+', '.');

			if (type.Namespace != null)
			{
				fullNameWithoutNamespace = fullNameWithoutNamespace.Substring(type.Namespace.Length + 1);
			}

			writer.WriteStartElement(isStruct ? "structure" : "class");
			writer.WriteAttributeString("name", fullNameWithoutNamespace);
			writer.WriteAttributeString("id", memberName);
			writer.WriteAttributeString("access", GetTypeAccessValue(type));

			// structs can't be abstract and always derive from System.ValueType
			// so don't bother including those attributes.
			if (!isStruct)
			{
				if (type.IsAbstract)
				{
					writer.WriteAttributeString("abstract", "true");
				}

				if (type.BaseType != null && type.BaseType.FullName != "System.Object")
				{
					writer.WriteAttributeString("baseType", type.BaseType.Name);
				}
			}

			WriteTypeDocumentation(writer, memberName, type);

			WriteBaseType(writer, type.BaseType);

			foreach(Type interfaceType in type.GetInterfaces())
			{
				writer.WriteElementString("implements", interfaceType.Name);
			}

			WriteConstructors(writer, type);
			WriteFields(writer, type);
			WriteProperties(writer, type);
			WriteMethods(writer, type);
			WriteOperators(writer, type);
			WriteEvents(writer, type);

			writer.WriteEndElement();
		}

		private void WriteConstructors(XmlWriter writer, Type type)
		{
			int overload = 0;

			BindingFlags bindingFlags =  
				BindingFlags.Instance |
				BindingFlags.Public |
				BindingFlags.NonPublic;

			ConstructorInfo[] constructors = type.GetConstructors(bindingFlags);

			if (constructors.Length > 1)
			{
				overload = 1;
			}

			foreach (ConstructorInfo constructor in constructors)
			{
				if (constructor.IsPublic || 
					constructor.IsFamily || 
					constructor.IsFamilyOrAssembly ||
					(constructor.IsAssembly && MyConfig.DocumentInternals) ||
					(constructor.IsFamilyAndAssembly && MyConfig.DocumentInternals) ||
					(constructor.IsPrivate && MyConfig.DocumentPrivates))
				{
					WriteConstructor(writer, constructor, overload++);
				}
			}
		}

		private void WriteFields(XmlWriter writer, Type type)
		{
			BindingFlags bindingFlags =  
				BindingFlags.Instance |
				BindingFlags.Static |
				BindingFlags.Public |
				BindingFlags.NonPublic;

			foreach (FieldInfo field in type.GetFields(bindingFlags))
			{
				if (field.IsPublic || 
					field.IsFamily || 
					field.IsFamilyOrAssembly ||
					(field.IsAssembly && MyConfig.DocumentInternals) ||
					(field.IsFamilyAndAssembly && MyConfig.DocumentInternals) ||
					(field.IsPrivate && MyConfig.DocumentPrivates))
				{
					WriteField(writer, field);
				}
			}
		}

		private void WriteProperties(XmlWriter writer, Type type)
		{
			BindingFlags bindingFlags = 
				BindingFlags.Instance |
				BindingFlags.Static |
				BindingFlags.Public |
				BindingFlags.NonPublic;

			foreach (PropertyInfo property in type.GetProperties(bindingFlags))
			{
				MethodInfo getMethod = property.GetGetMethod(true);
				MethodInfo setMethod = property.GetSetMethod(true);

				bool hasGetter = (getMethod != null) &&
					(getMethod.IsPublic || 
					getMethod.IsFamily || 
					getMethod.IsFamilyOrAssembly ||
					(getMethod.IsAssembly && MyConfig.DocumentInternals) ||
					(getMethod.IsFamilyAndAssembly && MyConfig.DocumentInternals) ||
					(getMethod.IsPrivate && MyConfig.DocumentPrivates));

				bool hasSetter = (setMethod != null) &&
					(setMethod.IsPublic || 
					setMethod.IsFamily || 
					setMethod.IsFamilyOrAssembly ||
					(setMethod.IsAssembly && MyConfig.DocumentInternals) ||
					(setMethod.IsFamilyAndAssembly && MyConfig.DocumentInternals) ||
					(setMethod.IsPrivate && MyConfig.DocumentPrivates));

				if ((hasGetter || hasSetter) && !IsAlsoAnEvent(property))
				{
					WriteProperty(
						writer, 
						property, 
						property.DeclaringType.FullName != type.FullName);
				}
			}
		}

		private void WriteMethods(XmlWriter writer, Type type)
		{
			BindingFlags bindingFlags = 
				BindingFlags.Instance |
				BindingFlags.Static |
				BindingFlags.Public |
				BindingFlags.NonPublic;

			MethodInfo[] methods = type.GetMethods(bindingFlags);

			foreach (MethodInfo method in methods)
			{
				if (!(method.Name.StartsWith("get_")) && 
					!(method.Name.StartsWith("set_")) &&
					!(method.Name.StartsWith("add_")) && 
					!(method.Name.StartsWith("remove_")) &&
					!(method.Name.StartsWith("op_")))
				{
					if (method.IsPublic || 
						method.IsFamily || 
						method.IsFamilyOrAssembly ||
						(method.IsAssembly && MyConfig.DocumentInternals) ||
						(method.IsFamilyAndAssembly && MyConfig.DocumentInternals) ||
						(method.IsPrivate && MyConfig.DocumentPrivates))
					{
						WriteMethod(
							writer, 
							method, 
							method.DeclaringType.FullName != type.FullName, 
							GetMethodOverload(method, methods));
					}
				}
			}
		}

		private void WriteOperators(XmlWriter writer, Type type)
		{
			BindingFlags bindingFlags = 
				BindingFlags.Instance |
				BindingFlags.Static |
				BindingFlags.Public |
				BindingFlags.NonPublic;

			MethodInfo[] methods = type.GetMethods(bindingFlags);

			foreach (MethodInfo method in methods)
			{
				if (method.Name.StartsWith("op_"))
				{
					if (method.IsPublic || 
						method.IsFamily || 
						method.IsFamilyOrAssembly ||
						(method.IsAssembly && MyConfig.DocumentInternals) ||
						(method.IsFamilyAndAssembly && MyConfig.DocumentInternals) ||
						(method.IsPrivate && MyConfig.DocumentPrivates))
					{
						WriteOperator(
							writer, 
							method, 
							GetMethodOverload(method, methods));
					}
				}
			}
		}

		private void WriteEvents(XmlWriter writer, Type type)
		{
			BindingFlags bindingFlags =  
				BindingFlags.Instance |
				BindingFlags.Static |
				BindingFlags.Public |
				BindingFlags.NonPublic |
				BindingFlags.DeclaredOnly;

			foreach (EventInfo eventInfo in type.GetEvents(bindingFlags))
			{
				WriteEvent(writer, eventInfo);
			}
		}

		private bool IsAlsoAnEvent(Type type, string fullName)
		{
			bool  isEvent = false;

			BindingFlags bindingFlags =  
				BindingFlags.Instance |
				BindingFlags.Static |
				BindingFlags.Public |
				BindingFlags.NonPublic |
				BindingFlags.DeclaredOnly;

			foreach (EventInfo eventInfo in type.GetEvents(bindingFlags))
			{
				if (eventInfo.EventHandlerType.FullName == fullName)
				{
					isEvent = true;
					break;
				}
			}

			return isEvent;
		}

		private bool IsAlsoAnEvent(FieldInfo field)
		{
			return IsAlsoAnEvent(field.DeclaringType, field.FieldType.FullName);
		}

		private bool IsAlsoAnEvent(PropertyInfo property)
		{
			return IsAlsoAnEvent(property.DeclaringType, property.PropertyType.FullName);
		}

		/// <summary>Writes XML documenting an interface.</summary>
		/// <param name="writer">XmlWriter to write on.</param>
		/// <param name="type">Interface to document.</param>
		private void WriteInterface(XmlWriter writer, Type type)
		{
			string memberName = GetMemberName(type);

			writer.WriteStartElement("interface");
			writer.WriteAttributeString("name", type.Name.Replace('+', '.'));
			writer.WriteAttributeString("id", memberName);
			writer.WriteAttributeString("access", GetTypeAccessValue(type));

			WriteTypeDocumentation(writer, memberName, type);

			foreach(Type interfaceType in type.GetInterfaces())
			{
				writer.WriteElementString("implements", interfaceType.Name);
			}

			WriteFields(writer, type);
			WriteProperties(writer, type);
			WriteMethods(writer, type);
			WriteEvents(writer, type);

			writer.WriteEndElement();
		}

		/// <summary>Writes XML documenting a delegate.</summary>
		/// <param name="writer">XmlWriter to write on.</param>
		/// <param name="type">Delegate to document.</param>
		private void WriteDelegate(XmlWriter writer, Type type)
		{
			string memberName = GetMemberName(type);

			writer.WriteStartElement("delegate");
			writer.WriteAttributeString("name", type.Name.Replace('+', '.'));
			writer.WriteAttributeString("id", memberName);
			writer.WriteAttributeString("access", GetTypeAccessValue(type));

			BindingFlags bindingFlags =  
				BindingFlags.Instance |
				BindingFlags.Static |
				BindingFlags.Public |
				BindingFlags.NonPublic;

			MethodInfo[] methods = type.GetMethods(bindingFlags);

			foreach (MethodInfo method in methods)
			{
				if (method.Name == "Invoke")
				{
					writer.WriteAttributeString("returnType", method.ReturnType.FullName);

					WriteDelegateDocumentation(writer, memberName, type, method);

					foreach (ParameterInfo parameter in method.GetParameters())
					{
						WriteParameter(writer, GetMemberName(method), parameter);
					}
				}
			}

			writer.WriteEndElement();
		}

		/// <summary>Writes XML documenting an enumeration.</summary>
		/// <param name="writer">XmlWriter to write on.</param>
		/// <param name="type">Enumeration to document.</param>
		private void WriteEnumeration(XmlWriter writer, Type type)
		{
			string memberName = GetMemberName(type);

			writer.WriteStartElement("enumeration");
			writer.WriteAttributeString("name", type.Name.Replace('+', '.'));
			writer.WriteAttributeString("id", memberName);
			writer.WriteAttributeString("access", GetTypeAccessValue(type));

			WriteEnumerationDocumentation(writer, memberName);

			BindingFlags bindingFlags =  
				BindingFlags.Instance |
				BindingFlags.Static |
				BindingFlags.Public |
				BindingFlags.NonPublic |
				BindingFlags.DeclaredOnly;

			foreach (FieldInfo field in type.GetFields(bindingFlags))
			{
				// *** Not sure what this field is but don't want to document it for now.
				if (field.Name != "value__")
				{
					WriteField(writer, field);
				}
			}

			writer.WriteEndElement();
		}

		private void WriteBaseType(XmlWriter writer, Type type)
		{
			if (!"System.Object".Equals(type.FullName))
			{
				writer.WriteStartElement("base");
				writer.WriteAttributeString("name", type.Name);
				writer.WriteAttributeString("id", GetMemberName(type));
				writer.WriteAttributeString("type", type.FullName.Replace('+', '.'));

				WriteBaseType(writer, type.BaseType);

				writer.WriteEndElement();
			}
		}

		/// <summary>Writes XML documenting a field.</summary>
		/// <param name="writer">XmlWriter to write on.</param>
		/// <param name="field">Field to document.</param>
		private void WriteField(XmlWriter writer, FieldInfo field)
		{
			string memberName = GetMemberName(field);

			writer.WriteStartElement("field");
			writer.WriteAttributeString("name", field.Name);
			writer.WriteAttributeString("id", memberName);
			writer.WriteAttributeString("access", GetFieldAccessValue(field));
			writer.WriteAttributeString("type", field.FieldType.FullName.Replace('+', '.'));

			if (field.DeclaringType != field.ReflectedType)
			{
				writer.WriteAttributeString("declaringType", field.DeclaringType.FullName);
			}

			if (field.IsStatic)
			{
				writer.WriteAttributeString("contract", "Static");
			}

			if (field.IsInitOnly)
			{
				writer.WriteAttributeString("initOnly", "true");
			}

			WriteFieldDocumentation(writer, memberName);

			writer.WriteEndElement();
		}

		/// <summary>Writes XML documenting an event.</summary>
		/// <param name="writer">XmlWriter to write on.</param>
		/// <param name="eventInfo">Event to document.</param>
		private void WriteEvent(XmlWriter writer, EventInfo eventInfo)
		{
			string memberName = GetMemberName(eventInfo);

			writer.WriteStartElement("event");
			writer.WriteAttributeString("name", eventInfo.Name);
			writer.WriteAttributeString("id", memberName);
			writer.WriteAttributeString("access", GetMethodAccessValue(eventInfo.GetAddMethod(true)));
			writer.WriteAttributeString("contract", GetMethodContractValue(eventInfo.GetAddMethod(true)));
			writer.WriteAttributeString("type", eventInfo.EventHandlerType.FullName.Replace('+', '.'));

			if (eventInfo.DeclaringType != eventInfo.ReflectedType)
			{
				writer.WriteAttributeString("declaringType", eventInfo.DeclaringType.FullName);
			}

			if (eventInfo.IsMulticast)
			{
				writer.WriteAttributeString("multicast", "true");
			}

			WriteEventDocumentation(writer, memberName);

			writer.WriteEndElement();
		}

		/// <summary>Writes XML documenting a constructor.</summary>
		/// <param name="writer">XmlWriter to write on.</param>
		/// <param name="constructor">Constructor to document.</param>
		/// <param name="overload">If &gt; 0, indicates this is the nth overloaded constructor.</param>
		private void WriteConstructor(XmlWriter writer, ConstructorInfo constructor, int overload)
		{
			string memberName = GetMemberName(constructor);

			writer.WriteStartElement("constructor");
			writer.WriteAttributeString("name", constructor.Name);
			writer.WriteAttributeString("id", memberName);
			writer.WriteAttributeString("access", GetMethodAccessValue(constructor));

			if (overload > 0)
			{
				writer.WriteAttributeString("overload", overload.ToString());
			}

			WriteConstructorDocumentation(writer, memberName, constructor);

			foreach (ParameterInfo parameter in constructor.GetParameters())
			{
				WriteParameter(writer, GetMemberName(constructor), parameter);
			}

			writer.WriteEndElement();
		}

		/// <summary>Writes XML documenting a property.</summary>
		/// <param name="writer">XmlWriter to write on.</param>
		/// <param name="property">Property to document.</param>
		/// <param name="inherited">true if a declaringType attribute should be included.</param>
		private void WriteProperty(XmlWriter writer, PropertyInfo property, bool inherited)
		{
			string memberName = GetMemberName(property);

			writer.WriteStartElement("property");
			writer.WriteAttributeString("name", property.Name);
			writer.WriteAttributeString("id", memberName);
			writer.WriteAttributeString("access", GetPropertyAccessValue(property));

			if (inherited)
			{
				writer.WriteAttributeString("declaringType", property.DeclaringType.FullName);
			}

			writer.WriteAttributeString("type", property.PropertyType.FullName.Replace('+', '.'));
			writer.WriteAttributeString("contract", GetPropertyContractValue(property));
			writer.WriteAttributeString("get", property.GetGetMethod(true) != null ? "true" : "false");
			writer.WriteAttributeString("set", property.GetSetMethod(true) != null ? "true" : "false");

			WritePropertyDocumentation(writer, memberName, property);

			foreach(ParameterInfo parameter in GetIndexParameters(property))
			{
				WriteParameter(writer, memberName, parameter);
			}

			writer.WriteEndElement();
		}

		private string GetPropertyContractValue(PropertyInfo property)
		{
			return GetMethodContractValue(property.GetAccessors(true)[0]);
		}

		private ParameterInfo[] GetIndexParameters(PropertyInfo property)
		{
			// The ParameterInfo[] returned by GetIndexParameters()
			// contains ParameterInfo objects with empty names so
			// we have to get the parameters from the getter or
			// setter instead.

			ParameterInfo[] parameters;
			int length = 0;

			if (property.GetGetMethod(true) != null)
			{
				parameters = property.GetGetMethod(true).GetParameters();

				if (parameters != null)
				{
					length = parameters.Length;
				}
			}
			else
			{
				parameters = property.GetSetMethod(true).GetParameters();

				if (parameters != null)
				{
					// If the indexer only has a setter, we neet
					// to subtract 1 so that the value parameter
					// isn't displayed.

					length = parameters.Length - 1;
				}
			}

			ParameterInfo[] result = new ParameterInfo[length];

			if (length > 0)
			{
				for (int i = 0; i < length; ++i)
				{
					result[i] = parameters[i];
				}
			}

			return result;
		}

		/// <summary>Writes XML documenting a method.</summary>
		/// <param name="writer">XmlWriter to write on.</param>
		/// <param name="method">Method to document.</param>
		/// <param name="inherited">true if a declaringType attribute should be included.</param>
		/// <param name="overload">If &gt; 0, indicates this it the nth overloaded method with the same name.</param>
		private void WriteMethod(
			XmlWriter writer,
			MethodInfo method,
			bool inherited,
			int overload)
		{
			if (method != null)
			{
				string memberName = GetMemberName(method);

				writer.WriteStartElement("method");
				writer.WriteAttributeString("name", method.Name);
				writer.WriteAttributeString("id", memberName);
				writer.WriteAttributeString("access", GetMethodAccessValue(method));

				if (inherited)
				{
					writer.WriteAttributeString("declaringType", method.DeclaringType.FullName);
				}

				writer.WriteAttributeString("contract", GetMethodContractValue(method));

				if (overload > 0)
				{
					writer.WriteAttributeString("overload", overload.ToString());
				}

				writer.WriteAttributeString("returnType", method.ReturnType.FullName);

				WriteMethodDocumentation(writer, memberName, method);

				foreach (ParameterInfo parameter in method.GetParameters())
				{
					WriteParameter(writer, GetMemberName(method), parameter);
				}

				writer.WriteEndElement();
			}
		}

		private void WriteParameter(XmlWriter writer, string memberName, ParameterInfo parameter)
		{
			string direction = "in";

			if (parameter.ParameterType.IsByRef)
			{
				direction = parameter.IsOut ? "out" : "ref";
			}

			writer.WriteStartElement("parameter");
			writer.WriteAttributeString("name", parameter.Name);
			writer.WriteAttributeString("type", parameter.ParameterType.FullName);
			writer.WriteAttributeString("optional", parameter.IsOptional ? "true" : "false");
			
			if (direction != "in")
			{
				writer.WriteAttributeString("direction", direction);
			}

			writer.WriteEndElement();
		}

		private void WriteOperator(
			XmlWriter writer,
			MethodInfo method,
			int overload)
		{
			if (method != null)
			{
				string memberName = GetMemberName(method);

				writer.WriteStartElement("operator");
				writer.WriteAttributeString("name", method.Name);
				writer.WriteAttributeString("id", memberName);
				writer.WriteAttributeString("access", GetMethodAccessValue(method));

				if (method.DeclaringType != method.ReflectedType)
				{
					writer.WriteAttributeString("declaringType", method.DeclaringType.FullName);
				}

				writer.WriteAttributeString("contract", GetMethodContractValue(method));

				if (overload > 0)
				{
					writer.WriteAttributeString("overload", overload.ToString());
				}

				writer.WriteAttributeString("returnType", method.ReturnType.FullName);

				WriteMethodDocumentation(writer, memberName, method);

				foreach (ParameterInfo parameter in method.GetParameters())
				{
					WriteParameter(writer, GetMemberName(method), parameter);
				}

				writer.WriteEndElement();
			}
		}

		/// <summary>Used by GetMemberName(Type type) and by 
		/// GetFullNamespaceName(MemberInfo member) functions to build 
		/// up most of the /doc member name.</summary>
		/// <param name="type"></param>
		private string GetTypeNamespaceName(Type type)
		{
			string  theNamespace = "";

			if (type.Namespace != null && type.Namespace.Length > 0)
			{
				theNamespace = type.Namespace + ".";
			}

			return theNamespace + type.Name.Replace('+', '.');
		}

		/// <summary>Used by all the GetMemberName() functions except the 
		/// Type one. It returns most of the /doc member name.</summary>
		/// <param name="member"></param>
		private string GetFullNamespaceName(MemberInfo member)
		{
			return GetTypeNamespaceName(member.ReflectedType);
		}

		/// <summary>Derives the member name ID for a type. Used to match nodes in the /doc XML.</summary>
		/// <param name="type">The type to derive the member name ID from.</param>
		private string GetMemberName(Type type)
		{
			return "T:" + type.FullName.Replace('+', '.');
		}

		/// <summary>Derives the member name ID for a field. Used to match nodes in the /doc XML.</summary>
		/// <param name="field">The field to derive the member name ID from.</param>
		private string GetMemberName(FieldInfo field)
		{
			return "F:" + GetFullNamespaceName(field) + "." + field.Name;
		}

		/// <summary>Derives the member name ID for an event. Used to match nodes in the /doc XML.</summary>
		/// <param name="eventInfo">The event to derive the member name ID from.</param>
		private string GetMemberName(EventInfo eventInfo)
		{
			return "E:" + GetFullNamespaceName(eventInfo) + "." + eventInfo.Name;
		}

		/// <summary>Derives the member name ID for a property.  Used to match nodes in the /doc XML.</summary>
		/// <param name="property">The property to derive the member name ID from.</param>
		private string GetMemberName(PropertyInfo property)
		{
			string  memberName;

			memberName = "P:" + GetFullNamespaceName(property) + "." + property.Name;

			if (property.GetIndexParameters().Length > 0)
			{
				memberName += "(";

				int i = 0;

				foreach (ParameterInfo parameter in property.GetIndexParameters())
				{
					if (i > 0)
					{
						memberName += ",";
					}

					memberName += parameter.ParameterType.FullName;

					++i;
				}

				memberName += ")";
			}

			return memberName;
		}

		/// <summary>Derives the member name ID for a member function.  Used to match nodes in the /doc XML.</summary>
		/// <param name="method">The method to derive the member name ID from.</param>
		private string GetMemberName(MethodBase method)
		{
			string  memberName;

			memberName = "M:" + GetFullNamespaceName(method) + "." + method.Name.Replace('.', '#');

			int i = 0;

			foreach (ParameterInfo parameter in method.GetParameters())
			{
				if (i == 0)
				{
					memberName += "(";
				}
				else
				{
					memberName += ",";
				}

				// XML Documentation file appends a "@" to reference and out types, not a "&"
				memberName += parameter.ParameterType.FullName.Replace('&', '@');

				++i;
			}

			if (i > 0)
			{
				memberName += ")";
			}

			return memberName;
		}

		private void WriteSlashDocElements(XmlWriter writer, string memberName)
		{
			string xPathExpr = "/doc/members/member[@name=\"" + memberName + "\"]";
			XmlNode xmlNode = currentSlashDoc.SelectSingleNode(xPathExpr);

			if (xmlNode != null && xmlNode.HasChildNodes)
			{
				WriteStartDocumentation(writer);
				writer.WriteRaw(xmlNode.InnerXml);
			}
		}

		private string GetTypeAccessValue(Type type)
		{
			string result = "Unknown";

			switch (type.Attributes & TypeAttributes.VisibilityMask)
			{
				case TypeAttributes.Public:
					result = "Public";
					break;
				case TypeAttributes.NotPublic:
					result = "NotPublic";
					break;
				case TypeAttributes.NestedPublic:
					result = "NestedPublic";
					break;
				case TypeAttributes.NestedFamily:
					result = "NestedFamily";
					break;
				case TypeAttributes.NestedFamORAssem:
					result = "NestedFamilyOrAssembly";
					break;
				case TypeAttributes.NestedAssembly:
					result = "NestedAssembly";
					break;
				case TypeAttributes.NestedFamANDAssem:
					result = "NestedFamilyAndAssembly";
					break;
				case TypeAttributes.NestedPrivate:
					result = "NestedPrivate";
					break;
			}

			return result;
		}

		private string GetFieldAccessValue(FieldInfo field)
		{
			string result = "Unknown";

			switch (field.Attributes & FieldAttributes.FieldAccessMask)
			{
				case FieldAttributes.Public:
					result = "Public";
					break;
				case FieldAttributes.Family:
					result = "Family";
					break;
				case FieldAttributes.FamORAssem:
					result = "FamilyOrAssembly";
					break;
				case FieldAttributes.Assembly:
					result = "Assembly";
					break;
				case FieldAttributes.FamANDAssem:
					result = "FamilyAndAssembly";
					break;
				case FieldAttributes.Private:
					result = "Private";
					break;
				case FieldAttributes.PrivateScope:
					result = "PrivateScope";
					break;
			}

			return result;
		}

		private string GetPropertyAccessValue(PropertyInfo property)
		{
			MethodInfo method;

			if (property.GetGetMethod(true) != null)
			{
				method = property.GetGetMethod(true);
			}
			else
			{
				method = property.GetSetMethod(true);
			}

			return GetMethodAccessValue(method);
		}

		private string GetMethodAccessValue(MethodBase method)
		{
			string result = "Unknown";

			switch (method.Attributes & MethodAttributes.MemberAccessMask)
			{
				case MethodAttributes.Public:
					result = "Public";
					break;
				case MethodAttributes.Family:
					result = "Family";
					break;
				case MethodAttributes.FamORAssem:
					result = "FamilyOrAssembly";
					break;
				case MethodAttributes.Assembly:
					result = "Assembly";
					break;
				case MethodAttributes.FamANDAssem:
					result = "FamilyAndAssembly";
					break;
				case MethodAttributes.Private:
					result = "Private";
					break;
				case MethodAttributes.PrivateScope:
					result = "PrivateScope";
					break;
			}

			return result;
		}

		private string GetMethodContractValue(MethodBase method)
		{
			string        result;
			MethodAttributes  methodAttributes = method.Attributes;

			if ((methodAttributes & MethodAttributes.Static) > 0)
			{
				result = "Static";
			}
			else if ((methodAttributes & MethodAttributes.Abstract) > 0)
			{
				result = "Abstract";
			}
			else if ((methodAttributes & MethodAttributes.Final) > 0)
			{
				result = "Final";
			}
			else if ((methodAttributes & MethodAttributes.Virtual) > 0)
			{
				if ((methodAttributes & MethodAttributes.NewSlot) > 0)
				{
					/*
							  if (false)
							  {
								// This is where we need to check if the class we
								// derive from has a method with our same sig.  If
								// so then this would be the 'new' keyword.
								result = "new";
							  }
							  else
							  {
					*/
					result = "Virtual";
					//          }
				}
				else
				{
					result = "Override";
				}
			}
			else
			{
				result = "Normal";
			}

			return result;
		}

		private StringCollection GetNamespaceNames(Type[] types)
		{
			StringCollection namespaceNames = new StringCollection();

			foreach (Type type in types)
			{
				if (namespaceNames.Contains(type.Namespace) == false)
				{
					namespaceNames.Add(type.Namespace);
				}
			}

			return namespaceNames;
		}

		private void CheckForMissingSummaryAndRemarks(
			XmlWriter writer,
			string memberName)
		{
			if (MyConfig.ShowMissingSummaries || MyConfig.ShowMissingRemarks)
			{
				string xPathExpr = "/doc/members/member[@name=\"" + memberName + "\"]";
				XmlNode xmlNode = currentSlashDoc.SelectSingleNode(xPathExpr);

				if (MyConfig.ShowMissingSummaries)
				{
					if (xmlNode == null || xmlNode.SelectSingleNode("summary") == null)
					{
						WriteMissingDocumentation(writer, "summary", null);
					}
				}

				if (MyConfig.ShowMissingRemarks)
				{
					if (xmlNode == null || xmlNode.SelectSingleNode("remarks") == null)
					{
						WriteMissingDocumentation(writer, "remarks", null);
					}
				}
			}
		}

		private void CheckForMissingParams(
			XmlWriter writer,
			string memberName,
			ParameterInfo[] parameters)
		{
			if (MyConfig.ShowMissingParams)
			{
				foreach (ParameterInfo parameter in parameters)
				{
					string xpath = String.Format(
						"/doc/members/member[@name='{0}']/param[@name='{1}']",
						memberName,
						parameter.Name);

					if (currentSlashDoc.SelectSingleNode(xpath) == null)
					{
						WriteMissingDocumentation(writer, "param", parameter.Name);
					}
				}
			}
		}

		private void CheckForMissingReturns(
			XmlWriter writer,
			string memberName,
			MethodInfo method)
		{
			if (MyConfig.ShowMissingReturns &&
				!"System.Void".Equals(method.ReturnType.FullName))
			{
				string xpath = String.Format(
					"/doc/members/member[@name='{0}']/returns",
					memberName);

				if (currentSlashDoc.SelectSingleNode(xpath) == null)
				{
					WriteMissingDocumentation(writer, "returns", null);
				}
			}
		}

		private void CheckForMissingValue(
			XmlWriter writer,
			string memberName)
		{
			if (MyConfig.ShowMissingValues)
			{
				string xpath = String.Format(
					"/doc/members/member[@name='{0}']/value",
					memberName);

				if (currentSlashDoc.SelectSingleNode(xpath) == null)
				{
					WriteMissingDocumentation(writer, "value", null);
				}
			}
		}

		private void WriteMissingDocumentation(
			XmlWriter writer,
			string element,
			string name)
		{
			WriteStartDocumentation(writer);

			writer.WriteStartElement(element);

			if (name != null)
			{
				writer.WriteAttributeString("name", name);
			}

			writer.WriteStartElement("span");
			writer.WriteAttributeString("class", "missing");
			writer.WriteString("Missing Documentation");
			writer.WriteEndElement();

			writer.WriteEndElement();
		}

		private bool didWriteStartDocumentation = false;

		private void WriteStartDocumentation(XmlWriter writer)
		{
			if (!didWriteStartDocumentation)
			{
				writer.WriteStartElement("documentation");
				didWriteStartDocumentation = true;
			}
		}

		private void WriteEndDocumentation(XmlWriter writer)
		{
			if (didWriteStartDocumentation)
			{
				writer.WriteEndElement();
				didWriteStartDocumentation = false;
			}
		}

		private void WriteTypeDocumentation(
			XmlWriter writer,
			string memberName,
			Type type)
		{
			WriteSlashDocElements(writer, memberName);
			CheckForMissingSummaryAndRemarks(writer, memberName);
			WriteEndDocumentation(writer);
		}

		private void WriteDelegateDocumentation(
			XmlWriter writer,
			string memberName,
			Type type,
			MethodInfo method)
		{
			WriteTypeDocumentation(writer, memberName, type);
			CheckForMissingParams(writer, memberName, method.GetParameters());
			WriteEndDocumentation(writer);
		}

		private void WriteEnumerationDocumentation(XmlWriter writer, string memberName)
		{
			WriteSlashDocElements(writer, memberName);
			CheckForMissingSummaryAndRemarks(writer, memberName);
			WriteEndDocumentation(writer);
		}

		private void WriteConstructorDocumentation(
			XmlWriter writer,
			string memberName,
			ConstructorInfo constructor)
		{
			WriteSlashDocElements(writer, memberName);
			CheckForMissingSummaryAndRemarks(writer, memberName);
			CheckForMissingParams(writer, memberName, constructor.GetParameters());
			WriteEndDocumentation(writer);
		}

		private void WriteFieldDocumentation(
			XmlWriter writer,
			string memberName)
		{
			WriteSlashDocElements(writer, memberName);
			CheckForMissingSummaryAndRemarks(writer, memberName);
			WriteEndDocumentation(writer);
		}

		private void WritePropertyDocumentation(
			XmlWriter writer,
			string memberName,
			PropertyInfo property)
		{
			WriteSlashDocElements(writer, memberName);
			CheckForMissingSummaryAndRemarks(writer, memberName);
			CheckForMissingParams(writer, memberName, GetIndexParameters(property));
			CheckForMissingValue(writer, memberName);
			WriteEndDocumentation(writer);
		}

		private void WriteMethodDocumentation(
			XmlWriter writer,
			string memberName,
			MethodInfo method)
		{
			WriteSlashDocElements(writer, memberName);
			CheckForMissingSummaryAndRemarks(writer, memberName);
			CheckForMissingParams(writer, memberName, method.GetParameters());
			CheckForMissingReturns(writer, memberName, method);
			WriteEndDocumentation(writer);
		}

		private void WriteEventDocumentation(
			XmlWriter writer,
			string memberName)
		{
			WriteSlashDocElements(writer, memberName);
			CheckForMissingSummaryAndRemarks(writer, memberName);
			WriteEndDocumentation(writer);
		}

		/// <summary>Loads an assembly.</summary>
		/// <param name="filename">The assembly filename.</param>
		/// <returns>The assembly object.</returns>
		/// <remarks>This method loads an assembly into memory. If you
		/// use Assembly.Load or Assembly.LoadFrom the assembly file locks. 
		/// This method doesn't lock the assembly file.</remarks>
		public static Assembly LoadAssembly(string filename)
		{
			if (!File.Exists(filename))
			{
				throw new ApplicationException("can't find assembly " + filename);
			}

			FileStream fs = File.Open(filename, FileMode.Open, FileAccess.Read);
			byte[] buffer = new byte[fs.Length];
			fs.Read(buffer, 0, (int)fs.Length);
			fs.Close();

			return Assembly.Load(buffer);
		}
	}
}