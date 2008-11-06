// MyClass.cs created with MonoDevelop
// User: brian at 7:09 PM 11/5/2008
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;

namespace Embroidr.Common
{
	public interface IDesignFormat
	{
		/// <value>
		/// Read and write access to the FilePath property is required. Once the property value
		/// is changed, the contents of object should change to reflect the new file.
		/// </value>
		string FilePath {get; set;}
		/// <value>
		/// The file name property should only return the name of the file. Not the full path.
		/// </value>
		string FileName {get;}
		/// <value>
		/// Sometimes the design has an internal name assigned. If so, that value should be accessable
		/// via the DesignName property. If the design does not have an internal name, the file name (minus the
		/// file extension) should be returned.
		/// </value>
		string DesignName {get;}
		/// <value>
		/// The StitchCount property should return the total number of stitches in the design.
		/// </value>
		int StitchCount {get;}
		/// <value>
		/// The Width property should return the physical width of the stitch in centimeters.
		/// </value>
		int Width {get;}
		/// <value>
		/// The Height property should return the physical height of the stitch in centimeters.
		/// </value>
		int Height {get;}
		/// <value>
		/// The PixelWidth property should return the total width of the design in pixels.
		/// </value>
		int PixelWidth {get;}
		/// <value>
		/// The PixelHeight property should return the total height of the design in pixels.
		/// </value>
		int PixelHeight {get;}
		/// <value>
		/// The ThreadColors property should return a generic list of ThreadColors used in the 
		/// design.
		/// </value>
		ReadOnlyCollection<ThreadColor> ThreadColors {get;}
		/// <value>
		/// The StitchBlock property should return a StitchBlock object for each stitch block in
		/// the design. Stitch blocks are are seperated by jump stitches or color changes.
		/// </value>
		ReadOnlyCollection<StitchBlock> StitchBlocks {get;}		
		/// <summary>
		/// Load from file causes the contents of the object to update to reflect the file being
		/// passed in.
		/// </summary>
		/// <param name="path">
		/// A <see cref="System.String"/>
		/// </param>
		void LoadFromFile(string path);
		/// <summary>
		/// ToSvg should write valid svg markup to the supplied stream.
		/// </summary>
		/// <param name="s">
		/// A <see cref="Stream"/> to write the svg markup to.
		/// </param>
		void ToSvg(Stream s);
	}	
}
