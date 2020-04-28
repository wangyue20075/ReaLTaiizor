﻿#region Imports

using System;
using System.IO;
using System.Drawing;
using System.Drawing.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

#endregion

namespace ReaLTaiizor
{
    #region AloneNotice

    public class AloneNotice : TextBox
    {
        private Graphics G;

        private string B64;

        public AloneNotice()
        {
            this.B64 = "iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAYAAAAf8/9hAAABL0lEQVQ4T5VT0VGDQBB9e2cBdGBSgTIDEr9MCw7pI0kFtgB9yFiC+KWMmREqMOnAAuDWOfAiudzhyA/svtvH7Xu7BOv5eH2atVKtwbwk0LWGGVyDqLzoRB7e3u/HJTQOdm+PGYjWNuk4ZkIW36RbkzsS7KqiBnB1Usw49DHh8oQEXMfJKhwgAM4/Mw7RIp0NeLG3ScCcR4vVhnTPnVCf9rUZeImTdKnz71VREnBnn5FKzMnX95jA2V6vLufkBQFESTq0WBXsEla7owmcoC6QJMKW2oCUePY5M0lAjK0iBAQ8TBGc2/d7+uvnM/AQNF4Rp4bpiGkRfTb2Gigx12+XzQb3D9JfBGaQzHWm7HS000RJ2i/av5fJjPDZMplErwl1GxDpMTbL1YC5lCwze52/AQFekh7wKBpGAAAAAElFTkSuQmCC";
            this.DoubleBuffered = true;
            base.Enabled = false;
            base.ReadOnly = true;
            base.BorderStyle = BorderStyle.None;
            this.Multiline = true;
            this.Cursor = Cursors.Default;
        }

        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            base.SetStyle(ControlStyles.UserPaint, true);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            this.G = e.Graphics;
            this.G.SmoothingMode = SmoothingMode.HighQuality;
            this.G.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
            base.OnPaint(e);
            this.G.Clear(Color.White);
            using (SolidBrush solidBrush = new SolidBrush(AloneLibrary.ColorFromHex("#FFFDE8")))
            {
                using (Pen pen = new Pen(AloneLibrary.ColorFromHex("#F2F3F7")))
                {
                    using (SolidBrush solidBrush2 = new SolidBrush(AloneLibrary.ColorFromHex("#B9B595")))
                    {
                        using (Font font = new Font("Segoe UI", 9f))
                        {
                            this.G.FillPath(solidBrush, AloneLibrary.RoundRect(AloneLibrary.FullRectangle(base.Size, true), 3, AloneLibrary.RoundingStyle.All));
                            this.G.DrawPath(pen, AloneLibrary.RoundRect(AloneLibrary.FullRectangle(base.Size, true), 3, AloneLibrary.RoundingStyle.All));
                            this.G.DrawString(this.Text, font, solidBrush2, new Point(30, 6));
                        }
                    }
                }
            }
            using (Image image = Image.FromStream(new MemoryStream(Convert.FromBase64String(this.B64))))
            {
                this.G.DrawImage(image, new Rectangle(8, checked((int)Math.Round(unchecked((double)base.Height / 2.0 - 8.0))), 16, 16));
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
        }
    }

    #endregion
}