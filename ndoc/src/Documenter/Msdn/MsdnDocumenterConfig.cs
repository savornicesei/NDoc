// MsdnDocumenterConfig.cs - the MsdnHelp documenter config class
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
using System.ComponentModel;
using System.Drawing.Design;
using System.IO;
using System.Windows.Forms.Design;

using NDoc.Core;

namespace NDoc.Documenter.Msdn
{
	/// <summary>The MsdnDocumenterConfig class.</summary>
	public class MsdnDocumenterConfig : BaseDocumenterConfig
	{
		string outputDirectory;
		string htmlHelpName;
		string htmlHelpCompilerFilename;
		bool includeFavorites;

		/// <summary>Initializes a new instance of the MsdnHelpConfig class.</summary>
		public MsdnDocumenterConfig() : base("MSDN")
		{
			OutputDirectory = @".\docs\Msdn\";

			HtmlHelpName = "Documentation";

			HtmlHelpCompilerFilename = 
				Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles)
				+ @"\HTML Help Workshop\hhc.exe";

			if (!File.Exists(htmlHelpCompilerFilename))
			{
				HtmlHelpCompilerFilename = "hhc.exe";
			}

			Title = "An NDoc Documented Class Library";

			SplitTOCs = false;
			DefaulTOC = string.Empty;
		}

		/// <summary>Gets or sets the OutputDirectory property.</summary>
		[
			Category("Output"),
			Editor(typeof(FolderNameEditor), typeof(UITypeEditor)),
			Description("The directory in which .html files and the .chm file will be generated.")
		]
		public string OutputDirectory
		{
			get { return outputDirectory; }

			set
			{
				outputDirectory = value;

				if (!outputDirectory.EndsWith("\\"))
				{
					outputDirectory += "\\";
				}

				SetDirty();
			}
		}

		/// <summary>Gets or sets the HtmlHelpName property.</summary>
		/// <remarks>The HTML Help project file and the compiled HTML Help file
		/// use this property plus the appropriate extension as names.</remarks>
		[
			Category("HTML Help"),
			Description("The name of the HTML Help project and the Compiled HTML Help file.")
		]
		public string HtmlHelpName
		{
			get { return htmlHelpName; }

			set 
			{ 
				if (Path.GetExtension(value).ToLower() == ".chm") 
				{
					HtmlHelpName = Path.GetFileNameWithoutExtension(value);
				}
				else
				{
					htmlHelpName = value; 
				}

				SetDirty();
			}
		}

		/// <summary>Gets or sets the HtmlHelpCompilerFilename property.</summary>
		[
			Category("HTML Help"),
			Editor(typeof(FileNameEditor), typeof(UITypeEditor)),
			Description("The full path to the Html Help Compiler (installed with Microsoft Visual Studio .NET).")
		]
		public string HtmlHelpCompilerFilename
		{
			get { return htmlHelpCompilerFilename; }

			set 
			{ 
				htmlHelpCompilerFilename = value; 
				SetDirty();
			}
		}

		/// <summary>Gets or sets the IncludeFavorites property.</summary>
		[
			Category("HTML Help"),
			Description("Turning this flag on will include a Favorites tab in the HTML Help file.")
		]
		public bool IncludeFavorites
		{
			get { return includeFavorites; }

			set 
			{ 
				includeFavorites = value; 
				SetDirty();
			}
		}

		private string _Title;

		/// <summary>Gets or sets the Title property.</summary>
		[
			Category("HTML Help"),
			Description("This is the title displayed at the top of every page.")
		]
		public string Title
		{
			get { return _Title; }

			set 
			{ 
				_Title = value; 
				SetDirty();
			}
		}

		private bool _SplitTOCs;

		/// <summary>Gets or sets the SplitTOCs property.</summary>
		[
			Category("HTML Help"),
			Description("Turning this flag on will generate a separate TOC for each assembly.")
		]
		public bool SplitTOCs
		{
			get { return _SplitTOCs; }

			set 
			{ 
				_SplitTOCs = value; 
				SetDirty();
			}
		}

		private string _DefaultTOC;

		/// <summary>Gets or sets the DefaultTOC property.</summary>
		[
			Category("HTML Help"),
			Description("When SplitTOCs is true, this represents the default TOC to use.")
		]
		public string DefaulTOC
		{
			get { return _DefaultTOC; }

			set 
			{ 
				_DefaultTOC = value; 
				SetDirty();
			}
		}
	}
}
