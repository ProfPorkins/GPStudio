using System;
using Netron.Lithium;
namespace FrontEnd
{
	/// <summary>
	/// This visitor will export the diagram to a traditional XML form.
	/// To be compared with the XML serialization of the diagram.
	/// Copy/paste the XML to file and edit in browser or an XML editor to view the tree-like structure
	/// </summary>
	public class XmlVisitor : IPrePostVisitor
	{
		#region Fields
		public event Messager OnInfo;
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
		public XmlVisitor()
		{			
		}
		#endregion

		#region Methods
		public void PreVisit(ShapeBase shape)
		{
			RaiseInfo("<Shape text='" + shape.Text + "'>");
		}

		public void Visit(ShapeBase shape)
		{
			//RaiseInfo("Visited: " + shape.Text);
		}
		public void PostVisit(ShapeBase shape)
		{
			RaiseInfo("</Shape>");
		}

		private void RaiseInfo(string msg)
		{
			if(OnInfo!=null)
				OnInfo(msg);
		}

		#endregion
	}
}
