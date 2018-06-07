using System;
using Netron.Lithium;
namespace FrontEnd
{
	/// <summary>
	/// Simple print of the shape's name
	/// </summary>
	public class InfoVisitor : IVisitor
	{
		public event Messager OnInfo;
		public InfoVisitor()
		{			
		}
		
		public void Visit(ShapeBase shape)
		{
			if(OnInfo!=null)
				OnInfo("Visited: " + shape.Text);
		}

		public bool IsDone
		{
			get
			{			
				return false;
			}
		}


	}
}
