// ThreadColor.cs created with MonoDevelop
// User: brian at 7:22 PM 11/5/2008
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.Drawing;

namespace Embroidr.Common
{
	public class ThreadColor
	{
		private Color _color;
		private string _name;
		
		public Color Color
		{
			get { return _color; }
		}
		public string Name
		{
			get { return _name; }
		}
		
		public ThreadColor(string name, int red, int green, int blue)
		{
			_color = System.Drawing.Color.FromArgb(red, green, blue);
			_name = name;
		}
		
//		public static ThreadColor FromIndex(int index)
//		{
//			Formats.Pes.ThreadColorConfiguration tc = Formats.Pes.Configuration.Current.Colors[index -1];
//
//			if (tc != null)
//			{
//				return new ThreadColor(tc.Name, tc.Red, tc.Green, tc.Blue);
//			}
//			
//			throw new ArgumentOutOfRangeException("index");
//		}
	}
}
