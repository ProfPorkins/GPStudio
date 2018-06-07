using System;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;


namespace IPAddressControlLib
{
   internal class DotControl : Control
   {
      #region Public Properties

      public override Size MinimumSize
      {
         get
         {
            return TextRenderer.MeasureText( Text, Font );
         }
      }

      public bool ReadOnly
      {
         get
         {
            return _readOnly;
         }
         set
         {
            _readOnly = value;
            Invalidate();
         }
      }

      #endregion // Public Properties

      #region Public Methods

      public override string ToString()
      {
         return Text;
      }

      #endregion // Public Methods

      #region Constructors

      public DotControl()
      {
         SetStyle( ControlStyles.ResizeRedraw, true );
         SetStyle( ControlStyles.UserPaint, true );
         SetStyle( ControlStyles.OptimizedDoubleBuffer, true );
         SetStyle( ControlStyles.AllPaintingInWmPaint, true );

         BackColor = SystemColors.Window;
         Size = MinimumSize;
         TabStop = false;
         Text = Properties.Resources.FieldSeparator;
      }

      #endregion // Constructors

      #region Protected Methods

      protected override void OnFontChanged( EventArgs e )
      {
         base.OnFontChanged( e );
         Size = MinimumSize;
      }

      protected override void OnPaint( PaintEventArgs e )
      {
         base.OnPaint( e );

         Color backColor = BackColor;

         if ( !_backColorChanged )
         {
            if ( !Enabled || ReadOnly )
            {
               backColor = SystemColors.Control;
            }
         }

         Color textColor = ForeColor;

         if ( !Enabled )
         {
            textColor = SystemColors.GrayText;
         }
         else if ( ReadOnly )
         {
            if ( !_backColorChanged )
            {
               textColor = SystemColors.WindowText;
            }
         }

         e.Graphics.FillRectangle( new SolidBrush( backColor ), ClientRectangle );

         StringFormat stringFormat = new StringFormat();
         stringFormat.Alignment = StringAlignment.Center;

         e.Graphics.DrawString( Text, Font, new SolidBrush( textColor ), ClientRectangle, stringFormat );
      }

      protected override void OnParentBackColorChanged( EventArgs e )
      {
         base.OnParentBackColorChanged( e );
         BackColor = Parent.BackColor;
         _backColorChanged = true;
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

      #endregion // Protected Methods

      #region Private Data

      private bool _backColorChanged;
      private bool _readOnly;

      #endregion // Private Data
   }
}
