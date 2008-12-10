// Svg.cs created with MonoDevelop
// User: brian at 10:51 AM 11/20/2008
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.Runtime.InteropServices;

namespace Embroidr.IO.Graphics
{
	public class Svg
	{		
		[DllImport("rsvg-2")]
		private static extern IntPtr rsvg_handle_new ();
		
		[DllImport("rsvg-2")]
		private static extern bool rsvg_handle_close(IntPtr handle, out IntPtr error);
		
		
	}
}
