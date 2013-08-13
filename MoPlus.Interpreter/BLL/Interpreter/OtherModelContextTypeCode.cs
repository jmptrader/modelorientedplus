/*<copyright>
Mo+ Solution Builder is a model oriented programming language and IDE, used for building models and generating code and other documents in a model driven development process.

Copyright (C) 2013 Dave Clemmer, Intelligent Coding Solutions, LLC

This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU General Public License for more details.

You should have received a copy of the GNU General Public License along with this program.  If not, see <http://www.gnu.org/licenses/>.
</copyright>*/
using System;
using System.ComponentModel;
using System.Xml;
using System.Xml.Serialization;
using MoPlus.Common;
using MoPlus.Data;
using MoPlus.Interpreter;

namespace MoPlus.Interpreter.BLL.Interpreter
{
	///--------------------------------------------------------------------------------
	/// <summary>This enumeration is used to hold values used by business rules for
	/// other model context types.</summary>
	/// <CreatedByUserName>DAVE\JavaDave</CreatedByUserName>
	/// <CreatedDate>3/10/13</CreatedDate>
	/// <Status>Customized</Status>
	///--------------------------------------------------------------------------------
	public enum OtherModelContextTypeCode
	{
		/// <summary>None.</summary>
		None = 0,
		/// <summary>For BaseEntity model context.</summary>
		BaseEntity = 1,
		/// <summary>For ReferencedEntity model context.</summary>
		ReferencedEntity = 2,
		/// <summary>For ReferencedProperty model context.</summary>
		ReferencedProperty = 3,
		/// <summary>For Tag model context.</summary>
		Tag = 4,
	}
}