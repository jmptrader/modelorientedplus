/*<copyright>
Mo+ Solution Builder is a model oriented programming language and IDE, used for building models and generating code and other documents in a model driven development process.

Copyright (C) 2013 Dave Clemmer, Intelligent Coding Solutions, LLC

This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU General Public License for more details.

You should have received a copy of the GNU General Public License along with this program.  If not, see <http://www.gnu.org/licenses/>.
</copyright>*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.ComponentModel;
using MoPlus.ViewModel.Messaging;
using MoPlus.ViewModel.Events.Entities;
using MoPlus.Data;
using MoPlus.Interpreter.BLL.Entities;
using MoPlus.ViewModel.Resources;

namespace MoPlus.ViewModel.Entities
{
	///--------------------------------------------------------------------------------
	/// <summary></summary>
	///
	/// This file is for adding customizations to the PropertyReferenceViewModel view
	/// (change the Status below to something other than Generated).
	///
	/// <CreatedByUserName>INCODE-1\Dave</CreatedByUserName>
	/// <CreatedDate>1/1/2013</CreatedDate>
	/// <Status>Customized</Status>
	///--------------------------------------------------------------------------------
	public partial class PropertyReferenceViewModel : DialogEditWorkspaceViewModel
	{
		#region "Menus"
		///--------------------------------------------------------------------------------
		/// <summary>This property gets MenuLabelNewPropertyRelationship.</summary>
		///--------------------------------------------------------------------------------
		public string MenuLabelNewPropertyRelationship
		{
			get
			{
				return DisplayValues.ContextMenu_NewPropertyRelationship;
			}
		}
		#endregion "Menus"

		#region "Editing Support"
		#endregion "Editing Support"

		#region "Command Processing"
		#endregion "Command Processing"

		#region "Properties"

		///--------------------------------------------------------------------------------
		/// <summary>This property gets ReferencedEntityIDItems.</summary>
		///--------------------------------------------------------------------------------
		public EnterpriseDataObjectList<Entity> ReferencedEntityIDItems
		{
			get
			{
				return Solution.EntityList;
			}
		}

		///--------------------------------------------------------------------------------
		/// <summary>This property gets/sets ReferencedPropertyIDItems.</summary>
		///--------------------------------------------------------------------------------
		public EnterpriseDataObjectList<Property> ReferencedPropertyIDItems { get; set; }
		#endregion "Properties"

		#region "Methods"
		///--------------------------------------------------------------------------------
		/// <summary>This method refreshes the set of Properties.</summary>
		///--------------------------------------------------------------------------------
		public void RefreshProperties()
		{
			if (Solution != null)
			{
				ReferencedPropertyIDItems = new EnterpriseDataObjectList<Property>((from p in Solution.PropertyList
																					where p.EntityID == ReferencedEntityID
																					select p).ToList<Property>());
			}
			OnPropertyChanged("ReferencedPropertyIDItems");
			OnPropertyChanged("ReferencedPropertyID");
		}

		#endregion "Methods"

		#region "Constructors"
		#endregion "Constructors"
	}
}
