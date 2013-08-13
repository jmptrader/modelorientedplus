﻿/*<copyright>
Mo+ Solution Builder is a model oriented programming language and IDE, used for building models and generating code and other documents in a model driven development process.

Copyright (C) 2013 Dave Clemmer, Intelligent Coding Solutions, LLC

This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU General Public License for more details.

You should have received a copy of the GNU General Public License along with this program.  If not, see <http://www.gnu.org/licenses/>.
</copyright>*/
using System;
using System.Linq;
using System.Collections;
using System.Collections.Specialized;
using System.Xml;
using System.Xml.Serialization;
using System.Text;

namespace MoPlus.Data
{
	///--------------------------------------------------------------------------------
	/// <summary>This class is used for storing and retrieving object values by name.</summary>
	///
	/// <remarks>Initial version retrieved from MSDN:
	/// http://msdn.microsoft.com/en-us/library/system.collections.specialized.nameobjectcollectionbase.aspx.</remarks>
	///--------------------------------------------------------------------------------
	[Serializable()]
	public class NameObjectCollection : NameObjectCollectionBase
	{
		#region "Fields and Properties"
		///--------------------------------------------------------------------------------
		/// <summary>This indexer gets or sets the value associated with the specified key.</summary>
		/// 
		/// <param name="key">Key of item to get or set.</param>
		///--------------------------------------------------------------------------------
		public Object this[String key]
		{
			get
			{
				return (this.BaseGet(key));
			}
			set
			{
				this.BaseSet(key, value);
			}
		}

		///--------------------------------------------------------------------------------
		/// <summary>This indexer gets or sets the value associated with the specified index.</summary>
		/// 
		/// <param name="index">Index of item to get or set.</param>
		///--------------------------------------------------------------------------------
		public Object this[int index]
		{
			get
			{
				return (this.BaseGet(index));
			}
			set
			{
				this.BaseSet(index, value);
			}
		}

		///--------------------------------------------------------------------------------
		/// <summary>This property gets a String array that contains all the keys in the collection.</summary>
		/// 
		///--------------------------------------------------------------------------------
		public String[] AllKeys
		{
			get
			{
				return (this.BaseGetAllKeys());
			}
		}

		///--------------------------------------------------------------------------------
		/// <summary>This property gets an Object array that contains all the values in the collection.</summary>
		/// 
		///--------------------------------------------------------------------------------
		public Array AllValues
		{
			get
			{
				return (this.BaseGetAllValues());
			}
		}

		///--------------------------------------------------------------------------------
		/// <summary>This property gets a String array that contains all the values in the collection.</summary>
		/// 
		///--------------------------------------------------------------------------------
		public String[] AllStringValues
		{
			get
			{
				return ((String[])this.BaseGetAllValues(typeof(string)));
			}
		}

		///--------------------------------------------------------------------------------
		/// <summary>This property gets a value indicating if the collection contains keys that are not null.</summary>
		/// 
		///--------------------------------------------------------------------------------
		public Boolean HasKeys
		{
			get
			{
				return (this.BaseHasKeys());
			}
		}

		#endregion "Fields and Properties"

		#region "Methods"
		///--------------------------------------------------------------------------------
		/// <summary>This method determines if a given key is in the collection.</summary>
		/// 
		///	<param name="key">Key to check.</param>
		///--------------------------------------------------------------------------------
		public bool HasKey(String key)
		{
			foreach (string loopKey in AllKeys)
			{
				if (loopKey == key) return true;
			}
			return false;
		}

		///--------------------------------------------------------------------------------
		/// <summary>This method adds an entry to the collection.</summary>
		/// 
		///	<param name="key">Key of item to add.</param>
		///	<param name="value">Value of item to add.</param>
		///--------------------------------------------------------------------------------
		public void Add(String key, Object value)
		{
			this.BaseAdd(key, value);
		}

		///--------------------------------------------------------------------------------
		/// <summary>This method removes an entry with the specified key from the collection.</summary>
		/// 
		/// <param name="key">Key of item to remove.</param>
		///--------------------------------------------------------------------------------
		public void Remove(String key)
		{
			this.BaseRemove(key);
		}

		///--------------------------------------------------------------------------------
		/// <summary>This method removes an entry in the specified index from the collection.</summary>
		///
		///	<param name="index">Index of item to remove.</param>
		///--------------------------------------------------------------------------------
		public void Remove(int index)
		{
			this.BaseRemoveAt(index);
		}

		///--------------------------------------------------------------------------------
		/// <summary>This method clears all the elements in the collection.</summary>
		///--------------------------------------------------------------------------------
		public void Clear()
		{
			this.BaseClear();
		}

		#endregion "Methods"

		#region "Constructors"
		///--------------------------------------------------------------------------------
		/// <summary>This constructor creates an empty collection.</summary>
		///--------------------------------------------------------------------------------
		public NameObjectCollection()
		{
		}

		///--------------------------------------------------------------------------------
		/// <summary>This constructor adds elements from an IDictionary into the new collection.</summary>
		/// 
		///	<param name="d">Input IDictionary.</param>
		///	<param name="bReadOnly">Flag to determine if collection is read only.</param>
		///--------------------------------------------------------------------------------
		public NameObjectCollection(IDictionary d, Boolean bReadOnly)
		{
			foreach (DictionaryEntry de in d)
			{
				this.BaseAdd((String)de.Key, de.Value);
			}
			this.IsReadOnly = bReadOnly;
		}

		#endregion "Constructors"

	}
}


