// BaseReflectionDocumenter.cs - base XML documenter code
// Copyright (C) 2004  Kevin Downs
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
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Xml;
using System.Xml.XPath;
using System.ComponentModel;

namespace NDoc.Core
{
	/// <summary>The base class for documenters which use the <see cref="ReflectionEngine"/> to extract 
	/// documentation from .Net assemblies.</summary>
	abstract public class BaseReflectionDocumenter : BaseDocumenter
	{
		private BaseReflectionDocumenterConfig MyConfig
		{
			get
		{
				return (BaseReflectionDocumenterConfig)Config;
			}
		}

		/// <summary>
		/// Creates a new <see cref="BaseReflectionDocumenter"/> instance.
		/// </summary>
		/// <param name="name">Documenter Name</param>
		protected BaseReflectionDocumenter(string name):base(name)
		{
		}

		/// <summary>Builds an Xml file combining the reflected metadata with the /doc comments.</summary>
		/// <returns>full pathname of XML file</returns>
		/// <remarks>The caller is responsible for deleting the xml file after use...</remarks>
		protected string MakeXmlFile(Project project)
		{
			string tempfilename = Path.GetTempFileName();

			//if this.rep.UseNDocXmlFile is set, 
			//copy it to the temp file and return.
			string xmlFile = MyConfig.UseNDocXmlFile;
			if (xmlFile.Length > 0)
			{
				Trace.WriteLine("Loading pre-compiled XML information from:\n" + xmlFile);
				File.Copy(xmlFile,tempfilename,true);
				return tempfilename;
			}

			//let's try to create this in a new AppDomain
			AppDomain appDomain=null;
			try
			{
				appDomain = AppDomain.CreateDomain("NDocReflection");
				ReflectionEngine re = (ReflectionEngine)  
					appDomain.CreateInstanceFromAndUnwrap(Assembly.GetExecutingAssembly().EscapedCodeBase, 
					"NDoc.Core.ReflectionEngine");
				ReflectionEngineParameters rep = new ReflectionEngineParameters(project, MyConfig);
				tempfilename = re.MakeXmlFile(rep);
				return tempfilename;
			}
			finally
			{
				if (appDomain!=null) AppDomain.Unload(appDomain);
			}
		}

		/// <summary>Builds an Xml string combining the reflected metadata with the /doc comments.</summary>
		/// <remarks>This now evidently writes the string in utf-16 format (and 
		/// says so, correctly I suppose, in the xml text) so if you write this string to a file with 
		/// utf-8 encoding it will be unparseable because the file will claim to be utf-16
		/// but will actually be utf-8.</remarks>
		/// <returns>XML string</returns>
		protected string MakeXml(Project project)
		{
			//if MyConfig.UseNDocXmlFile is set, 
			//load the XmlBuffer from the file and return.
			string xmlFile = MyConfig.UseNDocXmlFile;
			if (xmlFile.Length > 0)
			{
				Trace.WriteLine("Loading pre-compiled XML information from:\n" + xmlFile);
				using (TextReader reader = new StreamReader(xmlFile,Encoding.UTF8))
				{
					return reader.ReadToEnd();
				}
			}

			AppDomain appDomain=null;
			try
			{
				appDomain = AppDomain.CreateDomain("NDocReflection");
				ReflectionEngine re = (ReflectionEngine)  
					appDomain.CreateInstanceFromAndUnwrap(Assembly.GetExecutingAssembly().EscapedCodeBase, 
					"NDoc.Core.ReflectionEngine");
				ReflectionEngineParameters rep = new ReflectionEngineParameters(project, MyConfig);
				string tempString = re.MakeXml(rep);
				return tempString;
			}
			finally
			{
				if (appDomain!=null) AppDomain.Unload(appDomain);
			}
		}

	}
}