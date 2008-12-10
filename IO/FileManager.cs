// FileManager.cs created with MonoDevelop
// User: brian at 11:00 PM 10/28/2008
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.IO;
using System.Xml.Serialization;

namespace Embroidr.IO
{	
	public static class FileManager
	{
		public static IndexFile OpenIndexFile(string path)
		{
			if (File.Exists(path))
			{
				return OpenIndexFile(new FileStream(path, FileMode.Open));
			}
			return null;
		}
		public static IndexFile OpenIndexFile(Stream s)
		{
			XmlSerializer xs = new XmlSerializer(typeof(IndexFile));
			return (IndexFile) xs.Deserialize(s);
			s.Close();
		}
	}
}
