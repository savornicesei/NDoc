// NamespaceSummariesForm.cs - form for adding namespace summaries
// Copyright (C) 2001  Kral Ferch, Keith Hill
//
// Modified by: Keith Hill on Sep 28, 2001.
//   Tweaked the layout, made the dialog not show up in the task bar.
//
// Modified by: Jason Diamond on Oct 19, 2001.
//   Updated to work with the new NDoc.Core.Project interface.
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

namespace NDoc.Gui
{
	using System;
	using System.Drawing;
	using System.Collections;
	using System.ComponentModel;
	using System.Windows.Forms;

	using NDoc.Core;

	/// <summary>
	///    Summary description for NamespaceSummariesForm.
	/// </summary>
	public class NamespaceSummariesForm : System.Windows.Forms.Form
	{
		/// <summary>
		///    Required designer variable.
		/// </summary>
		private System.Windows.Forms.Button cancelButton;
		private System.Windows.Forms.Button okButton;
		private System.Windows.Forms.TextBox summaryTextBox;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ComboBox namespaceComboBox;
		private string selectedText;

		private ArrayList _Namespaces;
		private Project _Project;

		/// <summary>Allows the user to associate a summaries with the
		/// namespaces found in the assemblies that are being 
		/// documented.</summary>
		public NamespaceSummariesForm(ArrayList namespaces, Project project)
		{
			_Namespaces = namespaces;
			_Project = project;

			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			foreach (string namespaceName in _Namespaces)
			{
				namespaceComboBox.Items.Add(namespaceName);
			}

			namespaceComboBox.SelectedIndex = 0;
		}

		/// <summary>
		///    Required method for Designer support - do not modify
		///    the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.okButton = new System.Windows.Forms.Button();
			this.cancelButton = new System.Windows.Forms.Button();
			this.summaryTextBox = new System.Windows.Forms.TextBox();
			this.namespaceComboBox = new System.Windows.Forms.ComboBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// okButton
			// 
			this.okButton.Anchor = (System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right);
			this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.okButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.okButton.Location = new System.Drawing.Point(320, 16);
			this.okButton.Name = "okButton";
			this.okButton.TabIndex = 4;
			this.okButton.Text = "OK";
			this.okButton.Click += new System.EventHandler(this.okButton_Click);
			// 
			// cancelButton
			// 
			this.cancelButton.Anchor = (System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right);
			this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cancelButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.cancelButton.Location = new System.Drawing.Point(320, 48);
			this.cancelButton.Name = "cancelButton";
			this.cancelButton.TabIndex = 5;
			this.cancelButton.Text = "Cancel";
			// 
			// summaryTextBox
			// 
			this.summaryTextBox.Anchor = (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.summaryTextBox.Location = new System.Drawing.Point(8, 64);
			this.summaryTextBox.Multiline = true;
			this.summaryTextBox.Name = "summaryTextBox";
			this.summaryTextBox.Size = new System.Drawing.Size(296, 93);
			this.summaryTextBox.TabIndex = 3;
			this.summaryTextBox.Text = "";
			// 
			// namespaceComboBox
			// 
			this.namespaceComboBox.Anchor = ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.namespaceComboBox.DropDownWidth = 192;
			this.namespaceComboBox.Location = new System.Drawing.Point(112, 16);
			this.namespaceComboBox.Name = "namespaceComboBox";
			this.namespaceComboBox.Size = new System.Drawing.Size(192, 21);
			this.namespaceComboBox.Sorted = true;
			this.namespaceComboBox.TabIndex = 0;
			this.namespaceComboBox.SelectedIndexChanged += new System.EventHandler(this.namespaceComboBox_SelectedIndexChanged);
			// 
			// label1
			// 
			this.label1.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label1.Location = new System.Drawing.Point(8, 18);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(104, 16);
			this.label1.TabIndex = 1;
			this.label1.Text = "Select Namespace:";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label2
			// 
			this.label2.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label2.Location = new System.Drawing.Point(8, 48);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(56, 16);
			this.label2.TabIndex = 2;
			this.label2.Text = "Summary:";
			// 
			// NamespaceSummariesForm
			// 
			this.AcceptButton = this.okButton;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.AutoScroll = true;
			this.CancelButton = this.cancelButton;
			this.ClientSize = new System.Drawing.Size(408, 166);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.cancelButton,
																		  this.okButton,
																		  this.summaryTextBox,
																		  this.label2,
																		  this.label1,
																		  this.namespaceComboBox});
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(416, 200);
			this.Name = "NamespaceSummariesForm";
			this.ShowInTaskbar = false;
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.Text = "Edit Namespace Summaries";
			this.ResumeLayout(false);

		}

		/// <summary>
		/// Saves the summary text for the currently selected namespace
		/// before exiting the form.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void okButton_Click (object sender, System.EventArgs e)
		{
			_Project.SetNamespaceSummary(selectedText, summaryTextBox.Text);
		}

		/// <summary>
		/// Saves the currently entered text with the appropriate namespace
		/// and then puts the newly selected namespace's summary in the edit box.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void namespaceComboBox_SelectedIndexChanged (object sender, System.EventArgs e)
		{
			if (selectedText != null)
			{
				_Project.SetNamespaceSummary(selectedText, summaryTextBox.Text);
			}

			summaryTextBox.Text = _Project.GetNamespaceSummary(namespaceComboBox.Text);
			summaryTextBox.Focus();

			selectedText = namespaceComboBox.Text;
		}
	}
}
