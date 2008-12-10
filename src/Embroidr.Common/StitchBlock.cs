// StitchBlock.cs created with MonoDevelop
// User: brian at 7:23 PM 11/5/2008
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;

namespace Embroidr.Common
{
	public class StitchBlock
	{
		private List<Stitch> _stitches;
		private ThreadColor _blockColor;
		
		public ThreadColor BlockColor
		{
			get { return _blockColor; }
		}
		
		public ReadOnlyCollection<Stitch> Stitches
		{
			get { return _stitches.AsReadOnly(); }
		}
		
		public StitchBlock(ThreadColor blockColor)
		{
			_stitches = new List<Stitch>();
			_blockColor = blockColor;
		}
		
		public void AddStitch(Stitch stitch)
		{
			_stitches.Add(stitch);
		}
	}
	
	public class Stitch
	{
		private Point _point;
		private StitchType _type;
		
		public Point StitchPoint
		{
			get { return _point; }
		}
		
		public StitchType StitchType
		{
			get { return _type; }
		}
		
		public Stitch(Point point, StitchType type)
		{
			_point = point;
			_type = type;
		}
	}
	
	public enum StitchType
	{
		Normal,
		Jump
	}
}
