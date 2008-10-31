// IndexFile.cs created with MonoDevelop
// User: brian at 11:01 PM 10/28/2008
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Embroidr.IO
{
	[System.Xml.Serialization.XmlTypeAttribute(Namespace="Embroidr.FileManager")]
    [System.Xml.Serialization.XmlRootAttribute(ElementName="files", Namespace="Embroidr.FileManager", IsNullable=false)]
    public class IndexFile 
	{
        [System.Xml.Serialization.XmlElementAttribute(ElementName="file")]
        public List<DataFile> DataFiles;
    }
    

    [System.Xml.Serialization.XmlTypeAttribute(Namespace="Embroidr.FileManager")]
    public class DataFile 
	{
		private string _fileName;
		private string _filePath;
		private string _fileHash;
		private string _svgPath;
		private string _iconPath;
		private FileStatus _status;
		
		public DataFile(){}
		public DataFile(string name, string path)
		{
			_fileName = name;
			_filePath = path;
		}		

        [System.Xml.Serialization.XmlAttributeAttribute(AttributeName="name", Namespace="")]
        public string FileName
		{
			get { return _fileName; }
			set { _fileName = value; }
		}        

        [System.Xml.Serialization.XmlAttributeAttribute(AttributeName="path", Namespace="")]
        public string FilePath
		{
			get { return _filePath; }
			set { _filePath = value; }
		}        

        [System.Xml.Serialization.XmlAttributeAttribute(AttributeName="md5", Namespace="")]
        public string FileHash
		{
			get { return _fileHash; }
			set { _fileHash = value; }
		}        

        [System.Xml.Serialization.XmlAttributeAttribute(AttributeName="svg", Namespace="")]
        public string SvgPath
		{
			get { return _svgPath; }
			set { _svgPath = value; }
		}        

        [System.Xml.Serialization.XmlAttributeAttribute(AttributeName="png", Namespace="")]
        public string IconPath
		{
			get { return _iconPath; }
			set { _iconPath = value; }
		}
		
		[System.Xml.Serialization.XmlAttributeAttribute(AttributeName="status", Namespace="")]
		public FileStatus Status
		{
			get { return _status; }
			set { _status = value; }
		}
    }
	
	public enum FileStatus
	{
		InLibrary,
		Deleted,
		Duplicate
	}
}
