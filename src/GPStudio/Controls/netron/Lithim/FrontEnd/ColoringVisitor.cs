using System;
using System.Drawing;
using Netron.Lithium;
namespace FrontEnd
{
	/// <summary>
	/// This example colors the level in a gradient
	/// which emphasizes the nodes on the same level.
	/// Since the level is stored in the ShapeBase you can use either the 
	/// BFT or the DFT algorithm.
	/// </summary>
	public class ColoringVisitor : IPrePostVisitor
	{
		#region Fields
		public event Messager OnInfo;
		Color[] gradient;
		#endregion

		#region Properties
		public bool IsDone
		{
			get
			{			
				return false;
			}
		}

		#endregion

		#region Constructor
		/// <summary>
		/// Default ctor
		/// </summary>
		public ColoringVisitor()
		{			
			gradient = new Color[10]{
										Color.RoyalBlue,
										Color.CornflowerBlue,
										Color.LightSteelBlue,
										Color.SteelBlue,
										Color.PowderBlue,
										Color.Lavender,
										Color.LightCyan,
										Color.Azure,
										Color.Thistle,
										Color.Plum
									};

		}
		#endregion

		#region Methods
		
		public void PreVisit(ShapeBase shape)
		{
			
		}

		public void Visit(ShapeBase shape)
		{
			shape.ShapeColor = gradient[shape.Level%10]; //if above level 9 re-use the array
		}
		public void PostVisit(ShapeBase shape)
		{
		
		}

		private void RaiseInfo(string msg)
		{
			if(OnInfo!=null)
				OnInfo(msg);
		}

		#endregion

	}
}
