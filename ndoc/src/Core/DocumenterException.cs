// DocumenterException.cs - base XML documenter exception class
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

namespace NDoc.Core
{
	/// <summary>Represents the exceptions that are thrown when attempting to build documentation.</summary>
	public class DocumenterException : ApplicationException
	{
		/// <summary>Initializes a new instance of the DocumenterException class with the specified message.</summary>
		/// <param name="message">The message to display when the exception is thrown.</param>
		public DocumenterException(string message) : base(message)
		{
		}

		/// <summary>Initializes a new instance of the Exception class 
		/// with a specified error message and a reference to the 
		/// inner exception that is the root cause of this exception.</summary>
		/// <param name="message">The error message that explains the reason for the exception.</param>
		/// <param name="inner">An instance of Exception that is the cause of the current Exception. 
		/// If inner is non-null, then the current Exception is raised in a catch block handling inner.</param>
		public DocumenterException(string message, Exception inner) : base(message, inner)
		{
		}
	}
}