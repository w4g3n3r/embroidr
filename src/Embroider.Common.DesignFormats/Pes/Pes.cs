// MyClass.cs created with MonoDevelop
// User: brian at 7:36 PM 11/5/2008
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.IO;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Embroidr.Common;

namespace Embroider.Common.DesignFormats
{	
	[DesignFormatAttribute("PES File", new byte[]{23, 50, 45, 53}, new string[]{"pes"})]		
	public class Pes : IDesignFormat
	{
		private int _xOffset = 0;
		private int _yOffset = 0;
		
		private string _filePath;
		public string FilePath
		{
			get { return _filePath; }
			set 
			{ 
				LoadFromFile(value); 
			}
		}
		
		public string FileName
		{
			get
			{
				//TODO: Create a proper exception.
				if (!File.Exists(_filePath)) throw new FileNotFoundException("Pes file wasn't found", _filePath);
				FileInfo fi = new FileInfo(_filePath);
				return fi.Name;
			}
		}
		
		private string _designName;
		public string DesignName
		{
			get { return _designName; }
		}
		
		private int _stitchCount;
		public int StitchCount
		{
			get { return _stitchCount; }
		}
		
		private int _width;
		public int Width
		{
			get { return _width; }
		}
		
		private int _height;
		public int Height
		{
			get { return _height; }
		}
		
		private int _pixelWidth;
		public int PixelWidth
		{
			get { return _pixelWidth; }
		}
		
		private int _pixelHeight;
		public int PixelHeight
		{
			get { return _pixelHeight; }
		}
		
		private List<ThreadColor> _threadColors;
		public ReadOnlyCollection<ThreadColor> ThreadColors
		{
			get { return _threadColors.AsReadOnly(); }
		}
		
		private List<StitchBlock> _stitchBlocks;
		public ReadOnlyCollection<StitchBlock> StitchBlocks
		{
			get { return _stitchBlocks.AsReadOnly(); }
		}
		
		public Pes()
		{
			_threadColors = new List<ThreadColor>();
			_stitchBlocks = new List<StitchBlock>();
		}
		
		public void LoadFromFile(string path)
		{			
			if (path == null) throw new ArgumentNullException("path");
			if (!File.Exists(path)) throw new FileNotFoundException("File not found.", path);
			
			_threadColors.Clear();
			_stitchBlocks.Clear();
			
			_filePath = path;
			string pesVersion;
			
			//log.InfoFormat("Reading file: {0}", _filePath);
			BinaryReader pesData = new BinaryReader(File.OpenRead(_filePath));
			
			string pesHeader = new string(pesData.ReadChars(4));
			//log.DebugFormat("Header string: {0}", pesHeader);
			if (pesHeader != "#PES") throw new FileLoadException("The specified file is not a valid PES file.", _filePath);
			
			pesVersion = new string(pesData.ReadChars(4));
			//log.DebugFormat("Pes version: {0}", _pesVersion);
			
			uint pecStart = pesData.ReadUInt32();
			//log.DebugFormat("Pec start: {0}", pecStart);
			
			pesData.BaseStream.Position = pecStart + 3;
			
			_designName = new string(pesData.ReadChars(16));
			_designName = _designName.Trim();
			//log.DebugFormat("Internal name: {0}", _internalName);
			
			pesData.BaseStream.Position = pecStart + 48;
			int colorCount = pesData.ReadByte() + 1;
			//log.DebugFormat("Color count: {0}", colorCount);
			
			//log.Info("Reading color data...");
			for (int i = 0; i < colorCount; i++)
			{
				//TODO: Read the colors
				int colorIndex = pesData.ReadByte();
				_threadColors.Add(new ThreadColor("Black", 0, 0, 0));
			}
			
			pesData.BaseStream.Position = pecStart + 514;
			uint pecLength = pesData.ReadUInt16();
			uint pecEnd = pecStart + 514 + pecLength;
			//log.DebugFormat("Pec Length: {0}", pecLength);
			//log.DebugFormat("Pec End: {0}", pecEnd);
			
			//pesData.BaseStream.Position = pecStart + 520;
			_width = pesData.ReadUInt16();
			_height = pesData.ReadUInt16();
			//log.DebugFormat("Width: {0}cm", _width);
			//log.DebugFormat("Height: {0}cm", _height);
			
			pesData.BaseStream.Position = pecStart + 532;
			int bx, by, dx, dy, x, y, mx, my, nx, ny;
			bx = by = dx = dy = x = y = mx = my = nx = ny = 0;
			
			//int x = Convert.ToInt16(_width) / 2;
			//int y = Convert.ToInt16(_height) / 2;
			
			int c = 0;
			bool jmp = false;
			StitchBlock sb = new StitchBlock(_threadColors[c]);
			//log.Info("Reading stitch data...");
			while(pesData.BaseStream.Position < pecEnd)
			{
				bx = pesData.ReadByte();
				by = pesData.ReadByte();
				//log.DebugFormat("Bytes x={0}, y={1}", bx, by);
				
				if (bx == 255 && by == 0)
				{
					//log.Info("End of stitch marker");
					_stitchBlocks.Add(sb);
					break;
				}
				else if (bx == 254 && by == 176)
				{
					//log.Info("End of color block.");
					c++;
					_stitchBlocks.Add(sb);
					sb = new StitchBlock(_threadColors[c]);
					
					pesData.BaseStream.Position++;
				}
				else
				{
					//Regular stitch
					dx = dy = 0;
                    if ((bx & 128) == 128)//$80
                    {
                        //log.Info("Jump stitch on x");
                        dx = ((bx & 15) * 256) + by;
						dx = ((dx & 2048) == 2048) ? (int) (dx | 4294963200) : dx;
                        by = pesData.ReadByte();
						jmp = true;
                    }
                    else
                    {
						dx = (bx > 63) ? bx - 128 : bx;
                    }

                    if ((by & 128) == 128)//$80
                    {
                        //log.Info("Jump stitch on y");
                        bx = pesData.ReadByte();
						dy = ((by & 15) * 256) + bx;
						dy = ((dy & 2048) == 2048) ? (int)(dy | 4294963200) : dy;
						jmp = true;
                    }
                    else
                    {
                        //normal stitch
						dy = (by > 63) ? by - 128 : by;
                    }
					//log.DebugFormat("Stitch point: dx={0}, dy={1}", dx, dy);
					x += dx;
					y += dy;
					nx = (x < nx) ? x : nx;
					ny = (y < ny) ? y : ny;
					mx = (x > mx) ? x : mx;
					my = (y > my) ? y : my;
					StitchType type = (jmp) ? StitchType.Jump : StitchType.Normal;
					sb.AddStitch(new Stitch(new Point(x, y), type));
					jmp = false;
					             
				}
				_pixelWidth = mx - nx;
				_pixelHeight = my - ny;
				_xOffset = -nx;
				_yOffset = -ny;
			}
			//log.Info("Closing the file.");
			pesData.Close();
		}
		
		public byte[] ToSvg()
		{
			byte[] rtn;
			MemoryStream s = new MemoryStream(0);
			StreamWriter sw = new StreamWriter(s);
			//log.Debug("Writing svg header.");
			sw.WriteLine(@"<?xml version=""1.0""?>");
			sw.WriteLine(@"<!DOCTYPE svg PUBLIC ""-//W3C//DTD SVG 1.1//EN"" ""http://www.w3.org/Graphics/SVG/1.1/DTD/svg11.dtd"">");
			sw.WriteLine(@"<svg xmlns=""http://www.w3.org/2000/svg"" version=""1.1"" width=""{0}"" height=""{1}"">",
			             this.PixelWidth, this.PixelHeight);			
			int cx = 0;
			int cj = 0;
			bool jmp = true;
			bool tag = false;
			foreach (StitchBlock blk in this.StitchBlocks)
			{
				cx++;
				foreach (Stitch stitch in blk.Stitches)
				{
					if (stitch.StitchType == StitchType.Jump)
					{
						cj++;
						jmp = true;
						continue;
					}
					else if (stitch.StitchType == StitchType.Normal)
					{
						if (jmp)
						{
							if (tag) sw.WriteLine(@"""/>");
							//log.DebugFormat("Writing path {0}", cx);
							sw.WriteLine(@"    <path id=""Block{0}Jump{1}""", cx, cj);				
							sw.WriteLine(@"        fill=""none""");
							sw.WriteLine(@"        stroke=""#{0:X2}{1:X2}{2:X2}""", 
							             blk.BlockColor.Color.R,
							             blk.BlockColor.Color.G,
							             blk.BlockColor.Color.B);
							sw.WriteLine(@"{0}{0}stroke-width=""2px""", "\t");
							sw.Write(@"{0}{0}d=""M ", "\t");
							sw.Write("{0} {1} L ", stitch.StitchPoint.X + _xOffset, stitch.StitchPoint.Y + _yOffset);
							tag = true;
							jmp = false;
						}
						else
						{
							sw.Write("{0} {1} ", stitch.StitchPoint.X + _xOffset, stitch.StitchPoint.Y + _yOffset);
						}
					}
				}
				sw.WriteLine(@""" />");
				tag = false;
				jmp = true;
			}
			//log.Debug("Closing svg file.");
			sw.WriteLine("</svg>");
			sw.Flush();
			rtn = s.GetBuffer();
			sw.Close();
			sw.Dispose();
			s.Close();
			s.Dispose();
			return rtn;
		}
		
	}
}
