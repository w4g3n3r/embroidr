// DesignFormatAttribute.cs created with MonoDevelop
// User: brian at 8:02 PM 11/5/2008
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.Collections;
using System.Collections.Generic;


namespace Embroidr.Common
{	
	[AttributeUsage(AttributeTargets.Class)]
	public class DesignFormatAttribute : Attribute
	{	
		private string[] _fileExtensions;
		private string _name;
		private string _description;
		private byte[] _fileHeader;
		
		public string[] FileExtensions
		{
			get { return _fileExtensions; }
		}
		
		public string Name
		{
			get { return _name; }
		}
		
		public string Description
		{
			get { return _description; }
			set { _description = value; }
		}
		
		public byte[] fileHeader
		{
			get { return _fileHeader; }
		}
		
		public DesignFormatAttribute(string name, byte[]fileHeader, string[] fileExtensions)
		{
			_name = name;
			_fileExtensions = fileExtensions;
			_fileHeader = fileHeader;
		}
		
	}
}
