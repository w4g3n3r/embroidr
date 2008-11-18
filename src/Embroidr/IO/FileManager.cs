// FileManager.cs created with MonoDevelop
// User: brian at 11:00 PM 10/28/2008
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.IO;
using log4net;
using System.Xml;
using System.Xml.Serialization;
using System.Configuration;
using System.Collections;
using System.Security.Cryptography;
using System.Text;

namespace Embroidr.IO
{	
	/// <summary>
	/// Static class used to manage the IO opperations of the index files.
	/// </summary>
	public static class FileManager
	{
		public static readonly ILog log = LogManager.GetLogger(typeof(FileManager));
		/// <summary>
		/// Deserializes the index file into an IndexFile object.
		/// </summary>
		/// <param name="path">
		/// A <see cref="System.String"/> representing the path to the index file.
		/// </param>
		/// <returns>
		/// A <see cref="IndexFile"/> representing the data contained in the index file.
		/// </returns>
		public static IndexFile OpenIndexFile(string path)
		{
			log.InfoFormat("Opening index file from path: {0}", path);
			if (File.Exists(path))
			{
				IndexFile ixf = null;
				Stream s = null;
				try
				{
					s = new FileStream(path, FileMode.Open);
					ixf = OpenIndexFile(s);
					return ixf;
				}
				catch (FileLoadException ex) { throw ex; } // We've already logged it in OpenIndexFile(Stream s)
				catch (Exception ex)
				{
					log.Fatal("An exception was thrown when opening the index file.");
					log.Fatal(ex.Message);
					log.Fatal(ex.StackTrace);
					throw ex;
				}
				finally
				{
					if (s != null) s.Dispose();
				}
			}
			log.Fatal("Could not open file path. File does not exist.");
			throw new FileNotFoundException("Could not open file. File does not exist", path);
		}
		/// <summary>
		/// Deserializes the index file into an IndexFile object.
		/// </summary>
		/// <param name="s">
		/// A <see cref="Stream"/> from the index file.
		/// </param>
		/// <returns>
		/// A <see cref="IndexFile"/> representing the data contained in the index file.
		/// </returns>
		public static IndexFile OpenIndexFile(Stream s)
		{
			log.Info("Opening index from stream.");
			if (s.CanRead && s.CanSeek)
			{
				s.Position = 0;
				log.Info("Deserializing index file.");
				XmlSerializer xs = new XmlSerializer(typeof(IndexFile));
				return (IndexFile) xs.Deserialize(s);
			}
			log.Fatal("Could not open stream for reading or seeking.");
			throw new IOException("Could not read or seek file stream.");
		}
		
		/// <summary>
		/// Serializes an <see cref="IndexFile"/> object to an xml file.
		/// </summary>
		/// <param name="index">
		/// A <see cref="IndexFile"/> containing the data to serialize.
		/// </param>
		/// <param name="path">
		/// A <see cref="System.String"/> representing an xml file path that will contain the serialized data.
		/// </param>
		public static void SaveIndexFile(IndexFile index, string path)
		{
			log.InfoFormat("Saving index file to path: {0}", path);
			if (index == null) throw new ArgumentNullException("index");
			if (File.Exists(path))
			{
				Stream s = null;
				try
				{
					s = new FileStream(path, FileMode.Truncate);
					SaveIndexFile(index, s);
					return;
				}
				catch (Exception ex)
				{
					log.Fatal("An exception was thrown while serializing the index file.");
					log.Fatal(ex.Message);
					log.Fatal(ex.StackTrace);
					throw ex;
				}
				finally
				{
					if (s != null) s.Dispose();
				}
			}
			log.Fatal("Could not open the index file. File does not exist.");
			throw new FileNotFoundException("Could not open file path. File does not exist.", path);
		}
		
		/// <summary>
		/// Serializes an <see cref="IndexFile"/> object to an xml file.
		/// </summary>
		/// <param name="index">
		/// A <see cref="IndexFile"/> containing the data to serialize.
		/// </param>
		/// <param name="s">
		/// A <see cref="Stream"/> to which the serialized data will be written.
		/// </param>
		public static void SaveIndexFile(IndexFile index, Stream s)
		{
			log.Info("Saving index file to stream.");
			if (index == null) throw new ArgumentException("index");
			if (s == null) throw new ArgumentNullException("s");
			
			if (s.CanWrite)
			{
				XmlSerializer xsr = null;
				XmlTextWriter xtw = null;
				try
				{
					xsr = new XmlSerializer(typeof(IndexFile));
					xtw = new XmlTextWriter(s, System.Text.Encoding.Default);
					if (Embroidr.UI.Configuration.FormatXmlOutput)
					{
						xtw.Formatting = Formatting.Indented;
						xtw.Indentation = 1;
						xtw.IndentChar = '\t';
					}
					xsr.Serialize(xtw, index);
					return;
				}
				catch (Exception ex)
				{
					log.Fatal("An exception was thrown while serializing the index file.");
					log.Fatal(ex.Message);
					log.Fatal(ex.StackTrace);
					throw ex;
				}
				finally
				{
					log.Info("Closing the XmlTextWriter.");
					if (xtw != null) xtw.Close();
				}
			}
			log.Fatal("Could not write to the index file stream.");
			throw new IOException("The index file stream could not be written to.");			
		}
		
		//TODO: Finish refreshIndexFile
		//  - Duplicates get added to the index each time this function runs.
		//  - Initial version of this function will only support pes files.
		//  - Need to generate display icons in this step.
		//  - Add error checking... refactor.
		public static void refreshIndexFile(string[] paths, ref IndexFile index)
		{
			throw new NotImplementedException("The function refreshIndexFile is not in a finished state.");
			Hashtable fileLib = new Hashtable();
			log.Info("Refreshing index file.");
			foreach (DataFile f in index.DataFiles)
			{
				if (!fileLib.ContainsKey(f.FileHash)) fileLib.Add(f.FileHash, f);
				if (!File.Exists(f.FilePath) && f.Status != FileStatus.Deleted)
				{
					log.DebugFormat("Existing file has been removed: {0}", f.FilePath);
					f.Status = FileStatus.Deleted;
				}
			}
			foreach (string path in paths)
			{
				log.Info("Scanning folders for new files.");
				if (Directory.Exists(path))
				{
					log.DebugFormat("Scanning: {0}", path);
					string[] files = Directory.GetFiles(path, "*.pes", SearchOption.AllDirectories);
					foreach (string file in files)
					{
						bool isDupe = false;
						FileInfo fi = new FileInfo(file);
						if (file.EndsWith(".pes"))
						{
							log.DebugFormat("Hashing file: {0}", file);
							string hash = md5(file);
							log.Debug(hash);
							DataFile newDf = new DataFile(fi.Name, file);
							newDf.FileHash = hash;
							
							if (fileLib.ContainsKey(hash))
							{
								log.DebugFormat("Found existing hash: {0}", hash);
								DataFile f = (DataFile)fileLib[hash];
								log.DebugFormat("Existing hash belongs to: {0}", f.FilePath);
								if (f.FilePath == file) continue;
								log.Debug("File paths are different.");
								newDf.Status = FileStatus.Duplicate;	
								isDupe = true;
							}
							else
								newDf.Status = FileStatus.InLibrary;
							
							log.DebugFormat("Adding new file: {0}", fi.Name);
							index.DataFiles.Add(newDf);
							if (!isDupe) fileLib.Add(hash, newDf);							
						}
					}
				}
			}
		}
		
		//TODO: Add error checking to FileManager.md5 method.
		public static string md5(string path)
		{
			if (File.Exists(path))
			{
				StringBuilder sb = new StringBuilder();
				FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
				MD5 md5 = new MD5CryptoServiceProvider();
				byte[] hash = md5.ComputeHash(fs);
				foreach (byte b in hash) sb.AppendFormat("{0:x2}", b);
				return sb.ToString();
			}
			return string.Empty;
		}
	}
}
