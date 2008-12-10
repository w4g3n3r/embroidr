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
using log4net;

namespace Embroider.Common.DesignFormats
{	
	[DesignFormatAttribute("PES File", new byte[]{23, 50, 45, 53}, new string[]{".pes"})]		
	public class Pes : IDesignFormat
	{		
		public static readonly ILog log = LogManager.GetLogger(typeof(Pes));
		
		private List<ThreadColor> _availableColors;

		private int _xOffset = 0;
		public int XOffset
		{
			get { return _xOffset; }
		}
		
		private int _yOffset = 0;
		public int YOffset
		{
			get { return _yOffset; }
		}
		
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
			_stitchCount = 0;
			_threadColors = new List<ThreadColor>();
			_stitchBlocks = new List<StitchBlock>();
			_availableColors = new List<ThreadColor>(63);
			_availableColors.Add(new ThreadColor("Prussian Blue", 14, 31, 124));
			_availableColors.Add(new ThreadColor("Blue", 10, 85, 163));
			_availableColors.Add(new ThreadColor("Teal Green", 48, 135, 119));
			_availableColors.Add(new ThreadColor("Cornflower Blue", 75, 107, 175));
			_availableColors.Add(new ThreadColor("Red", 237, 23, 31));
			_availableColors.Add(new ThreadColor("6", 209, 92, 0));
			_availableColors.Add(new ThreadColor("Magenta", 145, 54, 151));
			_availableColors.Add(new ThreadColor("Light Lilac", 228, 154, 203));
			_availableColors.Add(new ThreadColor("Lilac", 145, 95, 172));
			_availableColors.Add(new ThreadColor("Mint Green", 157, 214, 125));
			_availableColors.Add(new ThreadColor("Deep Gold", 232, 169, 0));
			_availableColors.Add(new ThreadColor("Orange", 254, 186, 53));
			_availableColors.Add(new ThreadColor("Yellow", 255, 255, 0));
			_availableColors.Add(new ThreadColor("Lime Green", 112, 188, 31));
			_availableColors.Add(new ThreadColor("15", 145, 95, 172));
			_availableColors.Add(new ThreadColor("Silver", 168, 168, 168));
			_availableColors.Add(new ThreadColor("17", 123, 111, 0));
			_availableColors.Add(new ThreadColor("Cream Brown", 255, 255, 179));
			_availableColors.Add(new ThreadColor("Pewter", 79, 85, 86));
			_availableColors.Add(new ThreadColor("Black", 0, 0, 0));
			_availableColors.Add(new ThreadColor("21", 11, 61, 145));
			_availableColors.Add(new ThreadColor("22", 119, 1, 118));
			_availableColors.Add(new ThreadColor("Dark Gray", 41, 49, 51));
			_availableColors.Add(new ThreadColor("Dark Brown", 42, 19, 1));
			_availableColors.Add(new ThreadColor("Deep Rose", 246, 74, 138));
			_availableColors.Add(new ThreadColor("Light Brown", 178, 118, 36));
			_availableColors.Add(new ThreadColor("Salmon Pink", 252, 187, 196));
			_availableColors.Add(new ThreadColor("Vermilion", 254, 55, 15));
			_availableColors.Add(new ThreadColor("White", 240, 240, 240));
			_availableColors.Add(new ThreadColor("30", 106, 28, 138));
			_availableColors.Add(new ThreadColor("31", 168, 221, 196));
			_availableColors.Add(new ThreadColor("Sky Blue", 37, 132, 187));
			_availableColors.Add(new ThreadColor("33", 254, 179, 67));
			_availableColors.Add(new ThreadColor("34", 255, 240, 141));
			_availableColors.Add(new ThreadColor("35", 208, 166, 96));
			_availableColors.Add(new ThreadColor("Clay Brown", 209, 84, 0));
			_availableColors.Add(new ThreadColor("37", 102, 186, 73));
			_availableColors.Add(new ThreadColor("38", 19, 74, 70));
			_availableColors.Add(new ThreadColor("39", 135, 135, 135));
			_availableColors.Add(new ThreadColor("40", 216, 202, 198));
			_availableColors.Add(new ThreadColor("41", 67, 86, 7));
			_availableColors.Add(new ThreadColor("Linen", 254, 227, 197));
			_availableColors.Add(new ThreadColor("Pink", 249, 147, 188));
			_availableColors.Add(new ThreadColor("44", 0, 56, 34));
			_availableColors.Add(new ThreadColor("45", 178, 175, 212));
			_availableColors.Add(new ThreadColor("46", 104, 106, 176));
			_availableColors.Add(new ThreadColor("47", 239, 227, 185));
			_availableColors.Add(new ThreadColor("Carmine", 247, 56, 102));
			_availableColors.Add(new ThreadColor("49", 181, 76, 100));
			_availableColors.Add(new ThreadColor("50", 19, 43, 26));
			_availableColors.Add(new ThreadColor("51", 199, 1, 85));
			_availableColors.Add(new ThreadColor("Tangerine", 254, 158, 50));
			_availableColors.Add(new ThreadColor("Light Blue", 168, 222, 235));
			_availableColors.Add(new ThreadColor("Emerald Green", 0, 103, 26));
			_availableColors.Add(new ThreadColor("55", 78, 41, 144));
			_availableColors.Add(new ThreadColor("Moss Green", 47, 126, 32));
			_availableColors.Add(new ThreadColor("57", 253, 217, 222));
			_availableColors.Add(new ThreadColor("58", 255, 217, 17));
			_availableColors.Add(new ThreadColor("59", 9, 91, 166));
			_availableColors.Add(new ThreadColor("60", 240, 249, 112));
			_availableColors.Add(new ThreadColor("Fresh Green", 227, 243, 91));
			_availableColors.Add(new ThreadColor("62", 255, 200, 100));
			_availableColors.Add(new ThreadColor("63", 255, 200, 150));
			_availableColors.Add(new ThreadColor("64", 255, 200, 200));
		}
		
		public void CloseFile()
		{
			_stitchCount = 0;
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
			
			log.InfoFormat("Reading file: {0}", _filePath);
			BinaryReader pesData = new BinaryReader(File.OpenRead(_filePath));
			
			string pesHeader = new string(pesData.ReadChars(4));
			log.DebugFormat("Header string: {0}", pesHeader);
			if (pesHeader != "#PES") throw new FileLoadException("The specified file is not a valid PES file.", _filePath);
			
			pesVersion = new string(pesData.ReadChars(4));
			log.DebugFormat("Pes version: {0}", pesVersion);
			
			uint pecStart = pesData.ReadUInt32();
			log.DebugFormat("Pec start: {0}", pecStart);
			
			pesData.BaseStream.Position = pecStart + 3;
			
			_designName = new string(pesData.ReadChars(16));
			_designName = _designName.Trim();
			log.DebugFormat("Internal name: {0}", _designName);
			
			pesData.BaseStream.Position = pecStart + 48;
			int colorCount = pesData.ReadByte() + 1;
			log.DebugFormat("Color count: {0}", colorCount);
			
			log.Info("Reading color data...");
			for (int i = 0; i < colorCount; i++)
			{
				int colorIndex = pesData.ReadByte();
				colorIndex--;				
				if (colorIndex > 63 || colorIndex < 0)
					_threadColors.Add(_availableColors[15]);
				else					
					_threadColors.Add(_availableColors[colorIndex]);
				log.DebugFormat("Added color {0}", _threadColors[i].Name);
			}
			
			pesData.BaseStream.Position = pecStart + 514;
			uint pecLength = pesData.ReadUInt16();
			uint pecEnd = pecStart + 514 + pecLength;
			log.DebugFormat("Pec Length: {0}", pecLength);
			log.DebugFormat("Pec End: {0}", pecEnd);
			
			_width = pesData.ReadUInt16();
			_height = pesData.ReadUInt16();
			
			pesData.BaseStream.Position = pecStart + 532;
			int bx, by, dx, dy, x, y, mx, my, nx, ny;
			bx = by = dx = dy = x = y = mx = my = nx = ny = 0;
			
			int c = 0;
			bool jmp = false;
			StitchBlock sb = new StitchBlock(_threadColors[c]);
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
					_stitchCount++;
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
	}
}
