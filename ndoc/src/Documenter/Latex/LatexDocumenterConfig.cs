// The LaTeX documenter configuration class.
//
// Copyright (C) 2002 Thong Nguyen (tum_public@veridicus.com)
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
using System.IO;
using System.ComponentModel;
using System.Drawing.Design;
// In mono 0.25, most classes that should actually be in the System.Design assembly
// are in the System.Windows.Forms assembly.
#if !MONO 
using System.Windows.Forms.Design;
#endif

using NDoc.Core;

namespace NDoc.Documenter.Latex
{
	/// <summary>
	/// Summary description for LatexDocumenterConfig.
	/// </summary>
	public class LatexDocumenterConfig
		: BaseDocumenterConfig
	{
		/// <summary/>
		public LatexDocumenterConfig() : base("LaTeX")
		{			
			OutputDirectory = @".\doc\";
			TexFileBaseName = "Documentation";
			LatexCompiler = "latex";
		}

		/// <summary>Gets or sets the output directory.</summary>
		[
		Category("LaTeX"),
		Description("The directory to output the files to."),
#if !MONO //System.Windows.Forms.Design.FolderNameEditor is not implemented in mono 0.25
		Editor(typeof(FolderNameEditor), typeof(UITypeEditor))
#endif
		]
		public string OutputDirectory
		{
			get
			{
				return m_OutputDirectory;
			}

			set
			{
				m_OutputDirectory = value;
			}
		}
		private string m_OutputDirectory;

		/// <summary>Gets the full name of the LaTeX document.</summary>
		[
		ReadOnly(true),
		Category("LaTeX"),
		Description("Full name of the LaTeX document.")
		]
		public string TextFileFullName
		{
			get
			{
				return TexFileBaseName + ".tex";
			}
		}
		
		/// <summary>Gets or sets the name of the LaTeX document excluding the file extension.</summary>
		[
		Category("LaTeX"),
		Description("Name of the LaTeX document excluding the file extension."),
		]
		public string TexFileBaseName
		{
			get
			{
				return m_TexFileBaseName;
			}

			set
			{
				m_TexFileBaseName = value;
			}
		}
		private string m_TexFileBaseName;

		/// <summary>Gets or sets the LaTeX compiler path.</summary>
		[
		Category("LaTeX"),
		Description("Path to the LaTeX executable (Set to empty if you do not have LaTeX installed)."),
#if (!MONO)
		// In mono 0.25 most classes in the System.Windows.Forms.Design assembly 
		// are located in the System.Windows.Forms assembly while they should 
		// actually be in the System.Design assembly.
		Editor(typeof(FileNameEditor), typeof(UITypeEditor))
#endif
		]
		public string LatexCompiler
		{
			get
			{
				return m_LatexCompiler;
			}

			set
			{
				m_LatexCompiler = value;
			}
		}
		private string m_LatexCompiler;

		/// <summary>Gets the path of the output file.</summary>
		[
		Category("LaTeX"),
		Description("Full path to the output TeX file."),
		ReadOnly(true)
		]
		public string TexFileFullPath
		{
			get
			{
				return Path.Combine(OutputDirectory, TextFileFullName);
			}
		}
	}
} 

