﻿#region Imports

using System;
using System.Drawing;
using ReaLTaiizor.Util;
using ReaLTaiizor.Manager;
using System.Windows.Forms;
using System.ComponentModel;
using ReaLTaiizor.Enum.Poison;
using ReaLTaiizor.Design.Poison;
using ReaLTaiizor.Drawing.Poison;
using ReaLTaiizor.Interface.Poison;
using ReaLTaiizor.Extension.Poison;

#endregion

namespace ReaLTaiizor.Controls
{
    #region PoisonButton

    [Designer(typeof(PoisonButtonDesigner))]
    [ToolboxBitmap(typeof(System.Windows.Forms.Button))]
    [DefaultEvent("Click")]
    public class PoisonButton : System.Windows.Forms.Button, IPoisonControl
    {
        #region Interface

        [Category(PoisonDefaults.PropertyCategory.Appearance)]
        public event EventHandler<PoisonPaintEventArgs> CustomPaintBackground;
        protected virtual void OnCustomPaintBackground(PoisonPaintEventArgs e)
        {
            if (GetStyle(ControlStyles.UserPaint) && CustomPaintBackground != null)
            {
                CustomPaintBackground(this, e);
            }
        }

        [Category(PoisonDefaults.PropertyCategory.Appearance)]
        public event EventHandler<PoisonPaintEventArgs> CustomPaint;
        protected virtual void OnCustomPaint(PoisonPaintEventArgs e)
        {
            if (GetStyle(ControlStyles.UserPaint) && CustomPaint != null)
            {
                CustomPaint(this, e);
            }
        }

        [Category(PoisonDefaults.PropertyCategory.Appearance)]
        public event EventHandler<PoisonPaintEventArgs> CustomPaintForeground;
        protected virtual void OnCustomPaintForeground(PoisonPaintEventArgs e)
        {
            if (GetStyle(ControlStyles.UserPaint) && CustomPaintForeground != null)
            {
                CustomPaintForeground(this, e);
            }
        }

        private ColorStyle poisonStyle = ColorStyle.Default;
        [Category(PoisonDefaults.PropertyCategory.Appearance)]
        [DefaultValue(ColorStyle.Default)]
        public ColorStyle Style
        {
            get
            {
                if (DesignMode || poisonStyle != ColorStyle.Default)
                {
                    return poisonStyle;
                }

                if (StyleManager != null && poisonStyle == ColorStyle.Default)
                {
                    return StyleManager.Style;
                }

                if (StyleManager == null && poisonStyle == ColorStyle.Default)
                {
                    return PoisonDefaults.Style;
                }

                return poisonStyle;
            }
            set => poisonStyle = value;
        }

        private ThemeStyle poisonTheme = ThemeStyle.Default;
        [Category(PoisonDefaults.PropertyCategory.Appearance)]
        [DefaultValue(ThemeStyle.Default)]
        public ThemeStyle Theme
        {
            get
            {
                if (DesignMode || poisonTheme != ThemeStyle.Default)
                {
                    return poisonTheme;
                }

                if (StyleManager != null && poisonTheme == ThemeStyle.Default)
                {
                    return StyleManager.Theme;
                }

                if (StyleManager == null && poisonTheme == ThemeStyle.Default)
                {
                    return PoisonDefaults.Theme;
                }

                return poisonTheme;
            }
            set => poisonTheme = value;
        }

        private PoisonStyleManager poisonStyleManager = null;
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public PoisonStyleManager StyleManager
        {
            get => poisonStyleManager;
            set => poisonStyleManager = value;
        }

        private bool useCustomBackColor = false;
        [DefaultValue(false)]
        [Category(PoisonDefaults.PropertyCategory.Appearance)]
        public bool UseCustomBackColor
        {
            get => useCustomBackColor;
            set => useCustomBackColor = value;
        }

        private bool useCustomForeColor = false;
        [DefaultValue(false)]
        [Category(PoisonDefaults.PropertyCategory.Appearance)]
        public bool UseCustomForeColor
        {
            get => useCustomForeColor;
            set => useCustomForeColor = value;
        }

        private bool useStyleColors = false;
        [DefaultValue(false)]
        [Category(PoisonDefaults.PropertyCategory.Appearance)]
        public bool UseStyleColors
        {
            get => useStyleColors;
            set => useStyleColors = value;
        }

        [Browsable(false)]
        [Category(PoisonDefaults.PropertyCategory.Behaviour)]
        [DefaultValue(false)]
        public bool UseSelectable
        {
            get => GetStyle(ControlStyles.Selectable);
            set => SetStyle(ControlStyles.Selectable, value);
        }

        #endregion

        #region Fields

        private bool displayFocusRectangle = false;
        [DefaultValue(false)]
        [Category(PoisonDefaults.PropertyCategory.Appearance)]
        public bool DisplayFocus
        {
            get => displayFocusRectangle;
            set => displayFocusRectangle = value;
        }

        private bool highlight = false;
        [DefaultValue(false)]
        [Category(PoisonDefaults.PropertyCategory.Appearance)]
        public bool Highlight
        {
            get => highlight;
            set => highlight = value;
        }

        private PoisonButtonSize poisonButtonSize = PoisonButtonSize.Small;
        [DefaultValue(PoisonButtonSize.Small)]
        [Category(PoisonDefaults.PropertyCategory.Appearance)]
        public PoisonButtonSize FontSize
        {
            get => poisonButtonSize;
            set => poisonButtonSize = value;
        }

        private PoisonButtonWeight poisonButtonWeight = PoisonButtonWeight.Bold;
        [DefaultValue(PoisonButtonWeight.Bold)]
        [Category(PoisonDefaults.PropertyCategory.Appearance)]
        public PoisonButtonWeight FontWeight
        {
            get => poisonButtonWeight;
            set => poisonButtonWeight = value;
        }

        private bool isHovered = false;
        private bool isPressed = false;
        private bool isFocused = false;

        #endregion

        #region Constructor

        public PoisonButton()
        {
            SetStyle
            (
                ControlStyles.SupportsTransparentBackColor |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.ResizeRedraw |
                ControlStyles.UserPaint,
                     true
            );
        }

        #endregion

        #region Paint Methods

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            try
            {
                Color backColor = BackColor;

                if (isHovered && !isPressed && Enabled)
                {
                    backColor = PoisonPaint.BackColor.Button.Hover(Theme);
                }
                else if (isHovered && isPressed && Enabled)
                {
                    backColor = PoisonPaint.BackColor.Button.Press(Theme);
                }
                else if (!Enabled)
                {
                    backColor = PoisonPaint.BackColor.Button.Disabled(Theme);
                }
                else
                {
                    if (!useCustomBackColor)
                    {
                        backColor = PoisonPaint.BackColor.Button.Normal(Theme);
                    }
                }

                if (backColor.A == 255 && BackgroundImage == null)
                {
                    e.Graphics.Clear(backColor);
                    return;
                }

                base.OnPaintBackground(e);

                OnCustomPaintBackground(new PoisonPaintEventArgs(backColor, Color.Empty, e.Graphics));
            }
            catch
            {
                Invalidate();
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            try
            {
                if (GetStyle(ControlStyles.AllPaintingInWmPaint))
                {
                    OnPaintBackground(e);
                }

                OnCustomPaint(new PoisonPaintEventArgs(Color.Empty, Color.Empty, e.Graphics));
                OnPaintForeground(e);
            }
            catch
            {
                Invalidate();
            }
        }

        protected virtual void OnPaintForeground(PaintEventArgs e)
        {
            Color borderColor, foreColor;

            if (isHovered && !isPressed && Enabled)
            {
                borderColor = PoisonPaint.BorderColor.Button.Hover(Theme);
                foreColor = PoisonPaint.ForeColor.Button.Hover(Theme);
            }
            else if (isHovered && isPressed && Enabled)
            {
                borderColor = PoisonPaint.BorderColor.Button.Press(Theme);
                foreColor = PoisonPaint.ForeColor.Button.Press(Theme);
            }
            else if (!Enabled)
            {
                borderColor = PoisonPaint.BorderColor.Button.Disabled(Theme);
                foreColor = PoisonPaint.ForeColor.Button.Disabled(Theme);
            }
            else
            {
                borderColor = PoisonPaint.BorderColor.Button.Normal(Theme);
                if (useCustomForeColor)
                {
                    foreColor = ForeColor;
                }
                else if (useStyleColors)
                {
                    foreColor = PoisonPaint.GetStyleColor(Style);
                }
                else
                {
                    foreColor = PoisonPaint.ForeColor.Button.Normal(Theme);
                }
            }

            using (Pen p = new Pen(borderColor))
            {
                Rectangle borderRect = new Rectangle(0, 0, Width - 1, Height - 1);
                e.Graphics.DrawRectangle(p, borderRect);
            }

            if (Highlight && !isHovered && !isPressed && Enabled)
            {
                using (Pen p = PoisonPaint.GetStylePen(Style))
                {
                    Rectangle borderRect = new Rectangle(0, 0, Width - 1, Height - 1);
                    e.Graphics.DrawRectangle(p, borderRect);
                    borderRect = new Rectangle(1, 1, Width - 3, Height - 3);
                    e.Graphics.DrawRectangle(p, borderRect);
                }
            }

            TextRenderer.DrawText(e.Graphics, Text, PoisonFonts.Button(poisonButtonSize, poisonButtonWeight), ClientRectangle, foreColor, PoisonPaint.GetTextFormatFlags(TextAlign));

            OnCustomPaintForeground(new PoisonPaintEventArgs(Color.Empty, foreColor, e.Graphics));

            if (displayFocusRectangle && isFocused)
            {
                ControlPaint.DrawFocusRectangle(e.Graphics, ClientRectangle);
            }
        }

        #endregion

        #region Focus Methods

        protected override void OnGotFocus(EventArgs e)
        {
            isFocused = true;
            isHovered = true;
            Invalidate();

            base.OnGotFocus(e);
        }

        protected override void OnLostFocus(EventArgs e)
        {
            isFocused = false;
            isHovered = false;
            isPressed = false;
            Invalidate();

            base.OnLostFocus(e);
        }

        protected override void OnEnter(EventArgs e)
        {
            isFocused = true;
            isHovered = true;
            Invalidate();

            base.OnEnter(e);
        }

        protected override void OnLeave(EventArgs e)
        {
            isFocused = false;
            isHovered = false;
            isPressed = false;
            Invalidate();

            base.OnLeave(e);
        }

        #endregion

        #region Keyboard Methods

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space)
            {
                isHovered = true;
                isPressed = true;
                Invalidate();
            }

            base.OnKeyDown(e);
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            //Remove this code cause this prevents the focus color
            //isHovered = false;
            //isPressed = false;
            Invalidate();

            base.OnKeyUp(e);
        }

        #endregion

        #region Mouse Methods

        protected override void OnMouseEnter(EventArgs e)
        {
            isHovered = true;
            Invalidate();

            base.OnMouseEnter(e);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isPressed = true;
                Invalidate();
            }

            base.OnMouseDown(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            isPressed = false;
            Invalidate();

            base.OnMouseUp(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            if (!isFocused)
            {
                isHovered = false;
            }

            Invalidate();

            base.OnMouseLeave(e);
        }

        #endregion

        #region Overridden Methods

        protected override void OnEnabledChanged(EventArgs e)
        {
            base.OnEnabledChanged(e);
            Invalidate();
        }

        #endregion
    }

    #endregion
}