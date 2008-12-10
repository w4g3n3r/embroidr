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
		
		public void ToSvg(string path)
		{
			ToSvg(path, Embroidr.UI.Configuration.IgnoreJumpStitches);
		}
		
		public void ToSvg(string path, bool ignoreJumpStitches)
		{
			using (StreamWriter sw = new StreamWriter(path, false, System.Text.Encoding.Default))
			{
				sw.NewLine = Environment.NewLine;
				//log.Debug("Writing svg header.");
				sw.WriteLine("<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"no\"?>");
				//sw.WriteLine(@"<!DOCTYPE svg PUBLIC ""-//W3C//DTD SVG 1.1//EN"" ""http://www.w3.org/Graphics/SVG/1.1/DTD/svg11.dtd"">");
				sw.WriteLine("<svg xmlns=\"http://www.w3.org/2000/svg\" version=\"1.1\" width=\"{0}\" height=\"{1}\">",
				             this.Format.PixelWidth, this.Format.PixelHeight);			
				int cx = 0;
				int cj = 0;
				bool jmp = true;
				bool tag = false;
				foreach (StitchBlock blk in this.Format.StitchBlocks)
				{
					cx++;
					foreach (Stitch stitch in blk.Stitches)
					{
						if (ignoreJumpStitches && stitch.StitchType == StitchType.Jump)
						{
							cj++;
							jmp = true;
							//continue;
						}
						//else if (stitch.StitchType == StitchType.Normal)
						//{
							if (jmp)
							{
								if (tag) sw.WriteLine("\"/>");
								//log.DebugFormat("Writing path {0}", cx);
								sw.WriteLine("    <path id=\"Block{0}Jump{1}\"", cx, cj);				
								sw.WriteLine("        fill=\"none\"");
								sw.WriteLine("        stroke=\"#{0:X2}{1:X2}{2:X2}\"", 
								             blk.BlockColor.Color.R,
								             blk.BlockColor.Color.G,
								             blk.BlockColor.Color.B);
								sw.WriteLine("{0}{0}stroke-width=\"2px\"", "\t");
								sw.Write("{0}{0}d=\"M ", "\t");
								sw.Write("{0} {1} L ", stitch.StitchPoint.X + this.Format.XOffset, stitch.StitchPoint.Y + this.Format.YOffset);
								tag = true;
								jmp = false;
							}
							else
							{
								sw.Write("{0} {1} ", stitch.StitchPoint.X + this.Format.XOffset, stitch.StitchPoint.Y + this.Format.YOffset);
							}
						//}
					}
					sw.WriteLine("\" />");
					tag = false;
					jmp = true;
				}
				//log.Debug("Closing svg file.");
				sw.WriteLine("</svg>");					
				sw.Flush();
			}
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
