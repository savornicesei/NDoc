using System;
using System.Collections;
using System.Reflection;

namespace NDoc.Core
{
	public class AssemblyNavigator
	{
		private Assembly assembly;
		private ArrayList namespaceNames;
		private IEnumerator namespaceEnumerator;
		private string currentNamespaceName;
		private ArrayList currentTypes;
		private IEnumerator typeEnumerator;
		private Type currentType;

		public AssemblyNavigator(Assembly assembly)
		{
			this.assembly = assembly;
			this.namespaceNames = GetNamespaceNames();
			currentNamespaceName = null;
		}

		public string AssemblyName
		{
			get
			{
				return assembly.GetName().Name;
			}
		}

		private ArrayList GetNamespaceNames()
		{
			ArrayList namespaceNames = new ArrayList();

			Type[] types = assembly.GetTypes();

			foreach (Type type in types)
			{
				if (!namespaceNames.Contains(type.Namespace))
				{
					namespaceNames.Add(type.Namespace);
				}
			}

			namespaceNames.Sort();

			return namespaceNames;
		}

		public bool MoveToNamespace(string namespaceName)
		{
			if (namespaceNames.Contains(namespaceName))
			{
				currentNamespaceName = namespaceName;
				currentTypes = null;
				typeEnumerator = null;
				return true;
			}

			return false;
		}

		public bool MoveToFirstNamespace()
		{
			namespaceEnumerator = null;
			return MoveToNextNamespace();
		}

		public bool MoveToNextNamespace()
		{
			if (namespaceEnumerator == null)
			{
				namespaceEnumerator = namespaceNames.GetEnumerator();
			}

			if (namespaceEnumerator.MoveNext())
			{
				currentNamespaceName = namespaceEnumerator.Current as string;
				return true;
			}

			return false;
		}

		public string NamespaceName
		{
			get
			{
				return currentNamespaceName;
			}
		}

		public string TypeName
		{
			get
			{
				return currentType != null ? currentType.Name : null;
			}
		}

		private class TypeComparer : IComparer
		{
			int IComparer.Compare(object x, object y)
			{
				return String.Compare(((Type)x).Name, ((Type)y).Name);
			}
		}

		private bool IsDelegate(Type type)
		{
			return 
				type.IsClass &&
				(type.BaseType.FullName == "System.Delegate" ||
				type.BaseType.FullName == "System.MulticastDelegate");
		}

		public void GetClasses()
		{
			currentTypes = new ArrayList();

			foreach (Type type in assembly.GetTypes())
			{
				if (type.Namespace == currentNamespaceName && type.IsClass && !IsDelegate(type))
				{
					currentTypes.Add(type);
				}
			}

			currentTypes.Sort(new TypeComparer());

			typeEnumerator = null;
		}

		public void GetInterfaces()
		{
			currentTypes = new ArrayList();

			foreach (Type type in assembly.GetTypes())
			{
				if (type.Namespace == currentNamespaceName && type.IsInterface)
				{
					currentTypes.Add(type);
				}
			}

			currentTypes.Sort(new TypeComparer());

			typeEnumerator = null;
		}

		public void GetStructures()
		{
			currentTypes = new ArrayList();

			foreach (Type type in assembly.GetTypes())
			{
				if (type.Namespace == currentNamespaceName && type.IsValueType && !type.IsEnum)
				{
					currentTypes.Add(type);
				}
			}

			currentTypes.Sort(new TypeComparer());

			typeEnumerator = null;
		}

		public void GetDelegates()
		{
			currentTypes = new ArrayList();

			foreach (Type type in assembly.GetTypes())
			{
				if (type.Namespace == currentNamespaceName && IsDelegate(type))
				{
					currentTypes.Add(type);
				}
			}

			currentTypes.Sort(new TypeComparer());

			typeEnumerator = null;
		}

		public void GetEnumerations()
		{
			currentTypes = new ArrayList();

			foreach (Type type in assembly.GetTypes())
			{
				if (type.Namespace == currentNamespaceName && type.IsEnum)
				{
					currentTypes.Add(type);
				}
			}

			currentTypes.Sort(new TypeComparer());

			typeEnumerator = null;
		}

		public bool NamespaceHasClasses
		{
			get
			{
				GetClasses();
				return currentTypes.Count > 0;
			}
		}

		public bool NamespaceHasInterfaces
		{
			get
			{
				GetInterfaces();
				return currentTypes.Count > 0;
			}
		}

		public bool NamespaceHasStructures
		{
			get
			{
				GetStructures();
				return currentTypes.Count > 0;
			}
		}

		public bool NamespaceHasDelegates
		{
			get
			{
				GetDelegates();
				return currentTypes.Count > 0;
			}
		}

		public bool NamespaceHasEnumerations
		{
			get
			{
				GetEnumerations();
				return currentTypes.Count > 0;
			}
		}

		public bool MoveToFirstClass()
		{
			GetClasses();
			return MoveToNextType();
		}

		public bool MoveToFirstInterface()
		{
			GetInterfaces();
			return MoveToNextType();
		}

		public bool MoveToFirstStructure()
		{
			GetStructures();
			return MoveToNextType();
		}

		public bool MoveToFirstDelegate()
		{
			GetDelegates();
			return MoveToNextType();
		}

		public bool MoveToFirstEnumeration()
		{
			GetEnumerations();
			return MoveToNextType();
		}

		public bool MoveToNextType()
		{
			if (typeEnumerator == null)
			{
				typeEnumerator = currentTypes.GetEnumerator();
			}

			if (typeEnumerator.MoveNext())
			{
				currentType = typeEnumerator.Current as Type;
				return true;
			}

			return false;
		}

		public bool MoveToType(string typeName)
		{
			foreach (Type type in assembly.GetTypes())
			{
				if (type.Namespace == currentNamespaceName && type.Name == typeName)
				{
					currentType = type;
					return true;
				}
			}
			
			return false;
		}

		public Type CurrentType
		{
			get
			{
				return currentType;
			}
		}
	}
}
