// Configuration.cs created with MonoDevelop
// User: brian at 10:21 PM 9/30/2008
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.Configuration;

namespace Embroidery.Formats.Pes
{	
	public class Configuration : ConfigurationSection
	{
		public static Configuration Current
		{
			get { return ConfigurationManager.GetSection("pes.config") as Configuration; }
		}
		
		[ConfigurationProperty("threadColors")]
		public ThreadColorCollection Colors
		{
			get { return this["threadColors"] as ThreadColorCollection; }
		}
	}
	
	public class ThreadColorCollection : ConfigurationElementCollection
	{
		public ThreadColorConfiguration this[int index]
		{
			get { return BaseGet(index) as ThreadColorConfiguration; }
		}
		
		protected override ConfigurationElement CreateNewElement ()
		{
			return new ThreadColorConfiguration();
		}
		
		protected override object GetElementKey (ConfigurationElement element)
		{
			return ((ThreadColorConfiguration)element).Name;
		}
	}
	
	public class ThreadColorConfiguration : ConfigurationElement
	{
		[ConfigurationProperty("name", IsRequired=true)]
		public string Name
		{
			get { return this["name"] as string; }
		}
		
		[ConfigurationProperty("red", IsRequired=true)]
		[IntegerValidator(MinValue = 0, MaxValue = 255)]
		public int Red
		{
			get { return (int)this["red"]; }
		}
		
		[ConfigurationProperty("green", IsRequired=true)]
		[IntegerValidator(MinValue = 0, MaxValue = 255)]
		public int Green
		{
			get { return (int)this["green"]; }
		}
		
		[ConfigurationProperty("blue", IsRequired=true)]
		[IntegerValidator(MinValue = 0, MaxValue = 255)]
		public int Blue
		{
			get { return (int)this["blue"]; }
		}
	}
}
