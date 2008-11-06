// MainWindow.cs created with MonoDevelop
// User: brian at 10:52 PM 9/29/2008
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//
using System;
using System.IO;
using System.Text;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using Gtk;
using Gdk;
//using Embroidery.Formats.Pes;
using Embroidr.Common;
using Embroidr.IO;
using Cairo;
using Rsvg;
using log4net;

namespace Embroidr.UI
{
	public partial class MainWindow: Gtk.Window
	{	
		public static readonly ILog log = LogManager.GetLogger(typeof(MainWindow));
		IndexFile index = null;
		private List<IDesignFormat> availableFormats;
		//PesFile pes = null;
	//	Gtk.TextView txtOutput;
	//	Gtk.FileChooserButton btnFile;
		
		public MainWindow (): base (Gtk.WindowType.Toplevel)
		{
			Build ();
			
			availableFormats = new List<IDesignFormat>();
			
			if (Directory.Exists(Configuration.DesignFormatsPath))
			{
				string[] files = Directory.GetFiles(Configuration.DesignFormatsPath, "*.dll");
				foreach(string f in files)
				{
					Assembly a = Assembly.LoadFile(f);
					Type[] types = a.GetTypes();
					foreach(Type t in types)
					{
						if(t.GetInterface("IDesignFormat") != null)
						{
							if(t.GetCustomAttributes(typeof(DesignFormatAttribute), false).Length == 1)
							{
								log.InfoFormat("Loaded design format {0}", f);
								
								availableFormats.Add((IDesignFormat)Activator.CreateInstance(t));
							}
						}
					}
				}
			}
			
			object[] attributes = availableFormats[0].GetType().GetCustomAttributes(false);
			foreach(DesignFormatAttribute dfa in attributes)
			{
				log.InfoFormat("Attribute: {0}", dfa.Name);
			}
		}
		
		protected void OnDeleteEvent (object sender, DeleteEventArgs a)
		{
			Application.Quit ();
			a.RetVal = true;
		}

		protected virtual void OnFindActionActivated (object sender, System.EventArgs e)
		{
			log.Debug("OnFindActionActivated event fired");
			index = FileManager.OpenIndexFile(Configuration.IndexFilePath);
			if (index != null && index.DataFiles != null)
			{
				log.DebugFormat("{0} file(s) loaded.", index.DataFiles.Count);
				foreach (DataFile f in index.DataFiles)
				{
					log.DebugFormat("Path: {0}", f.FilePath);
				}
			}
		}

		protected virtual void OnSaveActionActivated (object sender, System.EventArgs e)
		{
			if (index != null)
			{
				FileManager.refreshIndexFile(Configuration.RepositoryPath.Split(';'), ref index);
				FileManager.SaveIndexFile(index, Configuration.IndexFilePath);
			}
		}
	}
}

//	protected virtual void OnBtnFileSelectionChanged (object sender, System.EventArgs e)
//	{
//		StringBuilder sb = new StringBuilder();
//		int cx = 0;
//		
//		log.DebugFormat("Selected file: {0}", btnFile.Filename);
//		pes = new PesFile(btnFile.Filename);
//
//		sb.AppendLine(pes.FilePath);
//		sb.AppendLine(pes.PesVersion);
//		
//		log.Debug("Reading stitch blocks.");
//		foreach (StitchBlock blk in pes.StitchBlocks)
//		{
//			cx++;
//			log.DebugFormat("Reading stitch block {0} color {1} stitches {2}",
//			        cx, blk.BlockColor.Name, blk.Stitches.Count);
//			sb.AppendLine("Stitch block: " + blk.BlockColor.Name);
//			sb.AppendLine("\tStitches: " + blk.Stitches.Count.ToString());
//		}
//		//txtOutput.Buffer.SetText(sb.ToString());
//	}
//
//	protected virtual void OnBtnToSvgClicked (object sender, System.EventArgs e)
//	{
//		if (pes != null)
//		{
//			FileStream s = null;
//			if (File.Exists("./test.svg"))
//			{
//				log.Debug("Truncating existing svg");
//				s = new FileStream("./test.svg", FileMode.Truncate);
//			}
//			else
//			{
//				log.Debug("Creating new svg.");
//				s = new FileStream("./test.svg", FileMode.Create);
//			}
//			pes.ToSvg(s);
//			s.Close();
//			Gdk.Pixbuf img = Rsvg.Tool.PixbufFromFileAtMaxSize("./test.svg", 128, 128);
//			img.Save("./test.png", "png");
//			img.RenderToDrawable(da.GdkWindow,
//			                     Gtk.Gc.Get(24, Rgb.Colormap, GCValues.Zero, GCValuesMask.Background),
//			                     0, 0, 0, 0, img.Width, img.Height, RgbDither.Normal, 0, 0);
//			img.Dispose();
//			//h.Close();
//			
//		}
//	}
