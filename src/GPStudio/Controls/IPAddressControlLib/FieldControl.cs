using System;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;


namespace IPAddressControlLib
{
   internal enum Direction
   {
      Forward,
      Reverse
   }

   internal enum Selection
   {
      None,
      All
   }

   internal class CedeFocusEventArgs : EventArgs
   {
      private int _fieldId;
      private Direction _direction;
      private Selection _selection;

      public int FieldId
      {
         get
         {
            return _fieldId;
         }
         set
         {
            _fieldId = value;
         }
      }

      public Direction Direction
      {
         get
         {
            return _direction;
         }
         set
         {
            _direction = value;
         }
      }

      public Selection Selection
      {
         get
         {
            return _selection;
         }
         set
         {
            _selection = value;
         }
      }
   }

   internal class TextChangedEventArgs : EventArgs
   {
      private int _fieldId;
      private String _text;

      public int FieldId
      {
         get
         {
            return _fieldId;
         }
         set
         {
            _fieldId = value;
         }
      }

      public String Text
      {
         get
         {
            return _text;
         }
         set
         {
            _text = value;
         }
      }
   }

   internal class FieldControl : TextBox
   {
      #region Public Constants

      public const byte MinimumValue = 0;
      public const byte MaximumValue = 255;

      #endregion // Public Constants

      #region Public Events

      public event EventHandler<TextChangedEventArgs> TextChangedEvent;
      public event EventHandler<CedeFocusEventArgs> CedeFocusEvent;

      #endregion // Public Events

      #region Public Properties

      public bool Blank
      {
         get
         {
            return ( Text.Length == 0 );
         }
      }

      public int FieldId
      {
         get
         {
            return _fieldId;
         }
         set
         {
            _fieldId = value;
         }
      }

      public override Size MinimumSize
      {
         get
         {
            return TextRenderer.MeasureText( Properties.Resources.FieldMeasureText, Font );
         }
      }

      public byte RangeLower
      {
         get
         {
            return _rangeLower;
         }
         set
         {
            if ( value < MinimumValue )
            {
               _rangeLower = MinimumValue;
            }
            else if ( value > _rangeUpper )
            {
               _rangeLower = _rangeUpper;
            }
            else
            {
               _rangeLower = value;
            }

            if ( Value < _rangeLower )
            {
               Text = _rangeLower.ToString( CultureInfo.InvariantCulture );
            }
         }
      }

      public byte RangeUpper
      {
         get
         {
            return _rangeUpper;
         }
         set
         {
            if ( value < _rangeLower )
            {
               _rangeUpper = _rangeLower;
            }
            else if ( value > MaximumValue )
            {
               _rangeUpper = MaximumValue;
            }
            else
            {
               _rangeUpper = value;
            }

            if ( Value > _rangeUpper )
            {
               Text = _rangeUpper.ToString( CultureInfo.InvariantCulture );
            }
         }
      }

      public byte Value
      {
         get
         {
            byte result;

            if ( !Byte.TryParse( Text, out result ) )
            {
               result = RangeLower;
            }

            return result;
         }
      }

      #endregion // Public Properties

      #region Public Methods

      public void TakeFocus( Direction direction, Selection selection )
      {
         Focus();

         if ( selection == Selection.All )
         {
            SelectionStart = 0;
            SelectionLength = TextLength;
         }
         else
            if ( direction == Direction.Forward )
            {
               SelectionStart = 0;
            }
            else
            {
               SelectionStart = TextLength;
            }
      }

      public override string ToString()
      {
         return Value.ToString( CultureInfo.InvariantCulture );
      }

      #endregion // Public Methods

      #region Constructors

      public FieldControl()
      {
         BorderStyle = BorderStyle.None;
         MaxLength = 3;
         Size = MinimumSize;
         TabStop = false;
         TextAlign = HorizontalAlignment.Center;
      }

      #endregion //Constructors

      #region Protected Methods

      protected override void OnKeyDown( KeyEventArgs e )
      {
         base.OnKeyDown( e );

         _invalidKeyDown = false;

         if ( !NumericKeyDown( e ) )
         {
            if ( !ValidKeyDown( e ) )
            {
               _invalidKeyDown = true;
            }
         }

         if ( ( e.KeyCode == Keys.OemPeriod ||
                e.KeyCode == Keys.Decimal ||
                e.KeyCode == Keys.Space ) &&
                TextLength != 0 &&
                SelectionLength == 0 &&
                SelectionStart != 0 )
         {
            if ( null != CedeFocusEvent )
            {
               CedeFocusEventArgs args = new CedeFocusEventArgs();
               args.FieldId = FieldId;
               args.Direction = Direction.Forward;
               args.Selection = Selection.All;
               CedeFocusEvent( this, args );
            }
         }

         if ( e.KeyCode == Keys.Left || e.KeyCode == Keys.Up )
         {
            if ( e.Modifiers == Keys.Control )
            {
               if ( null != CedeFocusEvent )
               {
                  CedeFocusEventArgs args = new CedeFocusEventArgs();
                  args.FieldId = FieldId;
                  args.Direction = Direction.Reverse;
                  args.Selection = Selection.All;
                  CedeFocusEvent( this, args );

               }
            }
            else
               if ( SelectionLength == 0 && SelectionStart == 0 )
               {
                  if ( null != CedeFocusEvent )
                  {
                     CedeFocusEventArgs args = new CedeFocusEventArgs();
                     args.FieldId = FieldId;
                     args.Direction = Direction.Reverse;
                     args.Selection = Selection.None;
                     CedeFocusEvent( this, args );
                  }
               }
         }

         if ( e.KeyCode == Keys.Right || e.KeyCode == Keys.Down )
         {
            if ( e.Modifiers == Keys.Control )
            {
               if ( null != CedeFocusEvent )
               {
                  CedeFocusEventArgs args = new CedeFocusEventArgs();
                  args.FieldId = FieldId;
                  args.Direction = Direction.Forward;
                  args.Selection = Selection.All;
                  CedeFocusEvent( this, args );
               }
            }
            else
               if ( SelectionLength == 0 && SelectionStart == Text.Length )
               {
                  if ( null != CedeFocusEvent )
                  {
                     CedeFocusEventArgs args = new CedeFocusEventArgs();
                     args.FieldId = FieldId;
                     args.Direction = Direction.Forward;
                     args.Selection = Selection.None;
                     CedeFocusEvent( this, args );
                  }
               }
         }
      }

      protected override void OnKeyPress( KeyPressEventArgs e )
      {
         if ( _invalidKeyDown )
         {
            e.Handled = true;
         }

         base.OnKeyPress( e );
      }

      protected override void OnParentBackColorChanged( EventArgs e )
      {
         base.OnParentBackColorChanged( e );
         BackColor = Parent.BackColor;
      }

      protected override void OnParentForeColorChanged( EventArgs e )
      {
         base.OnParentForeColorChanged( e );
         ForeColor = Parent.ForeColor;
      }

      protected override void OnSizeChanged( EventArgs e )
      {
         base.OnSizeChanged( e );
         Size = MinimumSize;
      }

      protected override void OnTextChanged( EventArgs e )
      {
         base.OnTextChanged( e );

         if ( !Blank )
         {
            int val;
            if ( !Int32.TryParse( Text, out val ) )
            {
               Text = String.Empty;
            }
            else
            {
               if ( val > RangeUpper )
               {
                  Text = RangeUpper.ToString( CultureInfo.InvariantCulture );
               }
               else
               {
                  Text = val.ToString( CultureInfo.InvariantCulture );
               }
            }
         }

         SelectionStart = this.TextLength;

         if ( null != TextChangedEvent )
         {
            TextChangedEventArgs args = new TextChangedEventArgs();
            args.FieldId = FieldId;
            args.Text = Text;
            TextChangedEvent( this, args );
         }

         if ( Text.Length == MaxLength && Focused )
         {
            if ( null != CedeFocusEvent )
            {
               CedeFocusEventArgs args = new CedeFocusEventArgs();
               args.FieldId = FieldId;
               args.Direction = Direction.Forward;
               args.Selection = Selection.All;
               CedeFocusEvent( this, args );
            }
         }
      }

      protected override void OnValidating( System.ComponentModel.CancelEventArgs e )
      {
         base.OnValidating( e );

         if ( !Blank )
         {
            if ( Value < RangeLower )
            {
               Text = RangeLower.ToString( CultureInfo.InvariantCulture );
            }
         }
      }

      #endregion // Protected Methods

      #region Private Methods

      private static bool NumericKeyDown( KeyEventArgs e )
      {
         if ( e.KeyCode < Keys.NumPad0 || e.KeyCode > Keys.NumPad9 )
         {
            if ( e.KeyCode < Keys.D0 || e.KeyCode > Keys.D9 )
            {
               return false;
            }
         }

         return true;
      }

      private static bool ValidKeyDown( KeyEventArgs e )
      {
         if ( e.KeyCode == Keys.Back ||
              e.KeyCode == Keys.Delete )
         {
            return true;
         }
         else
            if ( e.Modifiers == Keys.Control &&
                 ( e.KeyCode == Keys.C ||
                   e.KeyCode == Keys.V ||
                   e.KeyCode == Keys.X ) )
            {
               return true;
            }

         return false;
      }

      #endregion // Private Methods

      #region Private Data

      private int _fieldId = -1;
      private bool _invalidKeyDown;
      private byte _rangeLower; // = MinimumValue;  // this is removed for FxCop approval
      private byte _rangeUpper = MaximumValue;

      #endregion // Private Data
   }
}
