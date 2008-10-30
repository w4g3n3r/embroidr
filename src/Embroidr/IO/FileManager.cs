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
	}
}
