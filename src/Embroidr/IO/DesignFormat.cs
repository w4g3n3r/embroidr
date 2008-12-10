// DesignFormat.cs created with MonoDevelop
// User: brian at 3:13 PM 11/20/2008
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.IO;
using Embroidr.Common;

namespace Embroidr.IO
{	
	public class DesignFormat
	{	
		private string[] _fileExtensions;
		public string[] FileExtensions
		{
			get { return _fileExtensions; }
		}
		
		private string _name;
		public string Name
		{
			get { return _name; }
		}
		
		private string _description;
		public string Description
		{
			get { return _description; }
		}
		
		private byte[] _fileHeader;
		public byte[] FileHeader
		{
			get { return _fileHeader; }
		}
		
		private IDesignFormat _format;
		public IDesignFormat Format
		{
			get { return _format; }
		}
		
		public DesignFormat(string name, string description, string[] fileExtensions, byte[] fileHeader, IDesignFormat format)
		{
			_name = name;
			_description = description;
			_fileExtensions = fileExtensions;
			_fileHeader = fileHeader;
			_format = format;
		}
		
		public override string ToString ()
		{
			return _name;
		}
		
		public override bool Equals (object o)
		{
			if (o is System.String)
			{
				if (File.Exists(o.ToString()))
				{
					string ext = Path.GetExtension(o.ToString());
					foreach (string s in _fileExtensions) if (s == ext) return true;
				}
				else
				{
					if (o.ToString() == _name) return true;
				}
			}
			return false;
		}

	}
}
