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
		Gtk.NodeStore pesStore = null;
		//PesFile pes = null;
	//	Gtk.TextView txtOutput;
	//	Gtk.FileChooserButton btnFile;
		
		public MainWindow (): base (Gtk.WindowType.Toplevel)
		{
			Build ();
			
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
							object[] customAttributes = t.GetCustomAttributes(typeof(DesignFormatAttribute), false);
							if(customAttributes.Length == 1)
							{
								DesignFormatAttribute dfa = (DesignFormatAttribute)customAttributes[0];
								log.InfoFormat("Loaded design format {0}", dfa.Name);
								
								DesignFormat df = new DesignFormat(
								                                   dfa.Name, 
								                                   dfa.Description, 
								                                   dfa.FileExtensions, 
								                                   dfa.fileHeader,
								                                   (IDesignFormat)Activator.CreateInstance(t));
								
								FileManager.AvailableFormats.Add(df);  
							}
						}
					}
				}
			}
					
			pesView.AppendColumn("Display", new Gtk.CellRendererPixbuf(), "pixbuf" , 0);
			pesView.AppendColumn("Name", new Gtk.CellRendererText(), "text", 1);			
			pesView.ShowAll();		
			pesView.HeadersVisible = true;
			pesView.BorderWidth = 2;
			pesView.EnableGridLines = TreeViewGridLines.Horizontal;
			pesStore = new Gtk.NodeStore(typeof(PesFile));
			
			index = FileManager.OpenIndexFile(Configuration.IndexFilePath);
			
			FileManager.RefreshIndexFile(Configuration.RepositoryPath.Split('|'), ref index);
			
			loadDisplay();
		}
		
		protected void OnDeleteEvent (object sender, DeleteEventArgs a)
		{
			Application.Quit ();
			a.RetVal = true;
		}

		protected virtual void OnFindActionActivated (object sender, System.EventArgs e)
		{
			log.Debug("OnFindActionActivated event fired");
			
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
				FileManager.RefreshIndexFile(Configuration.RepositoryPath.Split(';'), ref index);
				FileManager.SaveIndexFile(index, Configuration.IndexFilePath);
			}
		}
		
		private void loadDisplay()
		{
			if (index != null)
			{
				if (pesStore != null) pesStore.Clear();
				foreach (DataFile f in index.DataFiles)
				{
					if (f.Status == FileStatus.InLibrary)
					{
						log.DebugFormat("Loading file {0}.", f.SvgPath);
						Gdk.Pixbuf icon = Rsvg.Pixbuf.FromFileAtMaxSize(f.SvgPath, 128, 128);
						log.DebugFormat("Loaded file {0}. Adding to node view.", f.FileName);
						log.DebugFormat("Size of pixbuf {0}x{1}", icon.Width, icon.Height);
						if (pesStore != null)
						{					
							pesStore.AddNode(new PesFile(icon, f.FileName + "\n" + "Test"));
						}					
					}
				}
				pesView.NodeStore = pesStore;
			}
		}
	}
	
	[Gtk.TreeNode(ListOnly=true)]
	public class PesFile : Gtk.TreeNode
	{
		private Gdk.Pixbuf _icon;
		private string _name;
		
		[Gtk.TreeNodeValueAttribute(Column=0)]
		public Gdk.Pixbuf Icon
		{
			get { return _icon; }
			set { _icon = value; }
		}
		
		[Gtk.TreeNodeValueAttribute(Column=1)]
		public string Name
		{
			get { return _name; }
			set { _name = value; }
		}
		
		public PesFile(Gdk.Pixbuf icon, string name)
		{
			_name = name;
			_icon = icon;
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
