// Main.cs created with MonoDevelop
// User: brian at 10:52 PM 9/29/2008
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//
using System;
using System.IO;
using System.Configuration;
using Gtk;

namespace Embroidr.UI
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			// Load configuration settings.
			Embroidr.UI.Configuration.RepositoryPath = ConfigurationManager.AppSettings["RepositoryPath"];
			Embroidr.UI.Configuration.IndexFilePath = ConfigurationManager.AppSettings["IndexFilePath"];
			Embroidr.UI.Configuration.DesignFormatsPath = ConfigurationManager.AppSettings["DesignFormatsPath"];
			Embroidr.UI.Configuration.SvgPath = ConfigurationManager.AppSettings["SvgPath"];
			Embroidr.UI.Configuration.IconPath = ConfigurationManager.AppSettings["IconPath"];
			Embroidr.UI.Configuration.IgnoreJumpStitches =
				(ConfigurationManager.AppSettings["IgnoreJumpStitches"].ToLower() == "true");	
			Embroidr.UI.Configuration.FormatXmlOutput = 
				(ConfigurationManager.AppSettings["FormatXmlOutput"].ToLower() == "true");		
			
			Application.Init ();
			MainWindow win = new MainWindow ();
			win.Show ();
			Application.Run ();
		}
	}
	
	public static class Configuration
	{
		public static string RepositoryPath = string.Empty;
		public static string IndexFilePath = string.Empty;
		public static bool FormatXmlOutput = false;
		public static string DesignFormatsPath = string.Empty;
		public static string SvgPath = string.Empty;
		public static string IconPath = string.Empty;
		public static bool IgnoreJumpStitches = true;
	}
}