using System;
using System.Windows.Forms.Design;

namespace IPAddressControlLib
{
   class IPAddressControlDesigner : ControlDesigner
   {
      public override SelectionRules SelectionRules
      {
         get
         {
            if ( Control.AutoSize )
            {
               return SelectionRules.LeftSizeable | SelectionRules.RightSizeable | SelectionRules.Moveable | SelectionRules.Visible;
            }
            else
            {
               return SelectionRules.AllSizeable | SelectionRules.Moveable | SelectionRules.Visible;
            }
         }
      }
   }
}