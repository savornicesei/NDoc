// HtmlHelp.cs - helper class to create HTML Help compiler files
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
using System.Diagnostics;
using System.IO;
using System.Xml;
using System.Collections;

namespace NDoc.Core
{
	/// <summary>HTML Help file utilities.</summary>
	/// <remarks>This class is used by the MsdnHelp documenter
	/// to create the files needed by the HTML Help compiler.</remarks>
	public class HtmlHelp
	{
		string _directoryName = null;
		string _projectName = null;
		string _defaultTopic = null;

		string _htmlHelpCompiler = null;

		bool _includeFavorites = false;

		StreamWriter streamHtmlHelp = null;

		ArrayList _tocFiles = new ArrayList();

		XmlTextWriter tocWriter;

		/// <summary>Initializes a new instance of the HtmlHelp class.</summary>
		/// <param name="directoryName">The directory to write the HTML Help files to.</param>
		/// <param name="projectName">The name of the HTML Help project.</param>
		/// <param name="defaultTopic">The default topic for the compiled HTML Help file.</param>
		/// <param name="htmlHelpCompiler">The path to the HTML Help compiler.</param>
		public HtmlHelp(
			string directoryName, 
			string projectName, 
			string defaultTopic,
			string htmlHelpCompiler)
		{
			_directoryName = directoryName;
			_projectName = projectName;
			_defaultTopic = defaultTopic;

			_htmlHelpCompiler = htmlHelpCompiler;
		}

		/// <summary>Gets the directory name containing the HTML Help files.</summary>
		public string DirectoryName
		{
			get { return _directoryName; }
		}

		/// <summary>Gets the HTML Help project name.</summary>
		public string ProjectName
		{
			get { return _projectName; }
		}

		/// <summary>Gets or sets the path to the HTML Help Compiler.</summary>
		public string HtmlHelpCompiler
		{
			get { return _htmlHelpCompiler; }
			set { _htmlHelpCompiler = value; }
		}

		/// <summary>Gets or sets the IncludeFavorites property.</summary>
		/// <remarks>Setting this to true will include the "favorites" tab 
		/// in the compiled HTML Help file.</remarks>
		public bool IncludeFavorites
		{
			get { return _includeFavorites; }
			set { _includeFavorites = value; }
		}

		/// <summary>Gets or sets the DefaultTopic property.</summary>
		public string DefaultTopic
		{
			get { return _defaultTopic; }
			set { _defaultTopic = value; }
		}

		private string GetProjectFilename()
		{
			return _projectName + ".hhp";
		}

		private string GetContentsFilename()
		{
			return (_tocFiles.Count > 0) ? (string)_tocFiles[0] : string.Empty;
		}

		private string GetIndexFilename()
		{
			return _projectName + ".hhk";
		}

		private string GetLogFilename()
		{
			return _projectName + ".log";
		}

		private string GetCompiledHtmlFilename()
		{
			return _projectName + ".chm";
		}

		private string GetPathToProjectFile()
		{
			return Path.Combine(_directoryName, _projectName) + ".hhp";
		}

		private string GetPathToContentsFile()
		{
			return Path.Combine(_directoryName, GetContentsFilename());
		}

		private string GetPathToIndexFile()
		{
			return Path.Combine(_directoryName, _projectName) + ".hhk";
		}

		private string GetPathToLogFile()
		{
			return Path.Combine(_directoryName, _projectName) + ".log";
		}

		/// <summary>Gets the path the the CHM file.</summary>
		/// <returns>The path to the CHM file.</returns>
		public string GetPathToCompiledHtmlFile()
		{
			return Path.Combine(_directoryName, _projectName) + ".chm";
		}

		/// <summary>Opens an HTML Help project file for writing.</summary>
		public void OpenProjectFile()
		{
			streamHtmlHelp = new StreamWriter(File.Open(GetPathToProjectFile(), FileMode.Create));

			streamHtmlHelp.Write("[FILES]\n");
		}

		/// <summary>Adds a file to the HTML Help project file.</summary>
		/// <param name="filename">The filename to add.</param>
		public void AddFileToProject(string filename)
		{
			streamHtmlHelp.Write(filename + "\n");
		}

		/// <summary>Closes the HTML Help project file.</summary>
		public void CloseProjectFile()
		{
			string options;

			if (_includeFavorites)
			{
				options = "0x63520,220,0x383e,[86,51,872,558],,,,,,,0";
			}
			else
			{
				options = "0x62520,220,0x383e,[86,51,872,558],,,,,,,0";
			}

			streamHtmlHelp.Write(
				"\n[OPTIONS]\n" +
				"Auto Index=Yes\n" +
				"Compatibility=1.1 or later\n" +
				"Compiled file=" + GetCompiledHtmlFilename() + "\n" +
				"Default Window=MsdnHelp\n" +
				"Default topic=" + _defaultTopic + "\n" +
				"Display compile progress=No\n" +
				"Error log file=" + GetLogFilename() + "\n" +
				"Full-text search=Yes\n" +
				"Index file=" + GetIndexFilename() + "\n" +
				"Language=0x409 English (United States)\n");

			foreach( string tocFile in _tocFiles )
			{
				streamHtmlHelp.Write("Contents file=" + tocFile + "\n");
			}

			streamHtmlHelp.Write(
				"\n[WINDOWS]\n" +
				"MsdnHelp=\"" +
				_projectName + " Help\",\"" +
				GetContentsFilename() + "\",\"" +
				GetIndexFilename() + "\",,,,,,," +
				options + "\n" +
				"\n[INFOTYPES]\n");

			streamHtmlHelp.Close();
		}

		/// <summary>Opens a HTML Help contents file for writing.</summary>
		public void OpenContentsFile(string tocName, bool isDefault)
		{
			// TODO: we would need a more robust way of maintaining the list
			//       of tocs that have been opened...

			if (tocName == string.Empty)
			{
				tocName = _projectName;
			}

			if (!tocName.EndsWith(".hhc"))
			{
				tocName += ".hhc";
			}

			if (isDefault)
			{
				_tocFiles.Insert(0, tocName);
			}
			else
			{
				_tocFiles.Add( tocName );
			}

			// Create the table of contents writer. This can't use
			// indenting because the HTML Help Compiler doesn't like
			// newlines between the <LI> and <Object> tags.
			tocWriter = new XmlTextWriter(Path.Combine(_directoryName, tocName), null);

			// We don't call WriteStartDocument because that outputs
			// the XML declaration which the HTML Help Compiler doesn't like.

			tocWriter.WriteComment("This document contains Table of Contents information for the HtmlHelp compiler.");
			tocWriter.WriteStartElement("UL");
		}

		/// <summary>Creates a new "book" in the HTML Help contents file.</summary>
		public void OpenBookInContents()
		{
			tocWriter.WriteStartElement("UL");
		}

		/// <summary>Adds a file to the contents file.</summary>
		/// <param name="headingName">The name as it should appear in the contents.</param>
		/// <param name="htmlFilename">The filename for this entry.</param>
		public void AddFileToContents(string headingName, string htmlFilename)
		{
			tocWriter.WriteStartElement("LI");
			tocWriter.WriteStartElement("OBJECT");
			tocWriter.WriteAttributeString("type", "text/sitemap");
			tocWriter.WriteStartElement("param");
			tocWriter.WriteAttributeString("name", "Name");
			tocWriter.WriteAttributeString("value", headingName.Replace('$', '.'));
			tocWriter.WriteEndElement();
			tocWriter.WriteStartElement("param");
			tocWriter.WriteAttributeString("name", "Local");
			tocWriter.WriteAttributeString("value", htmlFilename);
			tocWriter.WriteEndElement();
			tocWriter.WriteEndElement();
			tocWriter.WriteEndElement();
		}

		/// <summary>Closes the last opened "book" in the contents file.</summary>
		public void CloseBookInContents()
		{
			tocWriter.WriteEndElement();
		}

		/// <summary>Closes the contents file.</summary>
		public void CloseContentsFile()
		{
			tocWriter.WriteEndElement();
			tocWriter.Close();
		}

		/// <summary>Writes an empty index file.</summary>
		/// <remarks>The HTML Help Compiler will complain if this file doesn't exist.</remarks>
		public void WriteEmptyIndexFile()
		{
			// Create an empty index file to avoid compilation errors.

			XmlTextWriter indexWriter = new XmlTextWriter(GetPathToIndexFile(), null);

			// Don't call WriteStartDocument to avoid XML declaration.

			indexWriter.WriteStartElement("HTML");
			indexWriter.WriteStartElement("BODY");
			indexWriter.WriteComment(" http://xmarks.sourceforge.net/ ");
			indexWriter.WriteEndElement();
			indexWriter.WriteEndElement();

			// Don't call WriteEndDocument since we didn't call WriteStartDocument.

			indexWriter.Close();
		}

		/// <summary>Compiles the HTML Help project.</summary>
		public void CompileProject()
		{
			Process helpCompileProcess = new Process();

			try
			{
				try
				{
					string path = GetPathToCompiledHtmlFile();

					if (File.Exists(path))
					{
						File.Delete(path);
					}
				}
				catch (Exception e)
				{
					throw new DocumenterException("The compiled HTML Help file is probably open.", e);
				}

				ProcessStartInfo processStartInfo = new ProcessStartInfo();
				processStartInfo.FileName = _htmlHelpCompiler;
				processStartInfo.Arguments = "\"" + Path.GetFullPath(GetPathToProjectFile()) + "\"";
				processStartInfo.ErrorDialog = false;
				processStartInfo.WorkingDirectory = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);
				processStartInfo.WindowStyle = ProcessWindowStyle.Hidden;

				helpCompileProcess.StartInfo = processStartInfo;

				// Start the help compile and bail if it takes longer than 10 minutes.

				try
				{
					helpCompileProcess.Start();
				}
				catch (Exception e)
				{
					string msg = String.Format("The HTML Help compiler '{0}' was not found.", _htmlHelpCompiler);
					throw new DocumenterException(msg, e);
				}

				if (!helpCompileProcess.WaitForExit(600000))
				{
					throw new DocumenterException("Compile did not complete after 10 minutes and was aborted");
				}

				// Errors return 0 (warnings returns 1 - don't know about complete success)
				if (helpCompileProcess.ExitCode == 0)
				{
					throw new DocumenterException("Help compiler returned an error code of " + helpCompileProcess.ExitCode.ToString());
				}
			}
			finally
			{
				helpCompileProcess.Close();
			}
		}
	}
}
