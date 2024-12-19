using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Drawing;

namespace OverloadClientTool
{
    public class RoundCorneredButton : Button
    {
        private int _borderThickness;
        private Color _borderColor;
        private int _cornerRadius;
        private Color _backgroundColor;

        public Color RoundCorneredButtonBackColor { get; set; } = Theme.GetDarkBlueTheme.ButtonEnabledBackColor;
        public Color RoundCorneredButtonForeColor { get; set; } = Theme.GetDarkBlueTheme.ButtonEnabledForeColor;
        public int RoundCorneredButtonRadius { get; set; } = 4;

        public RoundCorneredButton()
        {
            FlatStyle = FlatStyle.Flat;

            FlatAppearance.BorderSize = 0; // Disable the default border
            FlatAppearance.MouseDownBackColor = RoundCorneredButtonBackColor;
            FlatAppearance.MouseOverBackColor = RoundCorneredButtonBackColor;

            ForeColor = RoundCorneredButtonForeColor;

            Size = new Size(200, 100);

            // Set default background colors.
            BackColor = Color.Transparent;
            _normalBackColor = RoundCorneredButtonBackColor;
            _mouseOverBackColor = Color.LightGray;
            _mouseDownBackColor = Color.Gray;
        }

        private Color _normalBackColor;
        private Color _mouseOverBackColor;
        private Color _mouseDownBackColor;

        private bool _isMouseOver;
        private bool _isMouseDown;

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            _isMouseOver = true;
            Invalidate();
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            _isMouseOver = false;
            _isMouseDown = false;
            Invalidate();
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (e.Button == MouseButtons.Left)
            {
                _isMouseDown = true;
                Invalidate();
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            _isMouseDown = false;
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            // Calculate the adjusted bounds for the border and background.
            Rectangle adjustedBounds = new Rectangle(_borderThickness / 2, _borderThickness / 2, Width - _borderThickness, Height - _borderThickness);

            using (GraphicsPath path = new GraphicsPath())
            {
                int diameter = 2 * _cornerRadius;

                // Adjust the corner rectangle positions.
                RectangleF topLeftCornerRect = new RectangleF(adjustedBounds.Left, adjustedBounds.Top, diameter, diameter);
                RectangleF topRightCornerRect = new RectangleF(adjustedBounds.Right - diameter, adjustedBounds.Top, diameter, diameter);
                RectangleF bottomRightCornerRect = new RectangleF(adjustedBounds.Right - diameter, adjustedBounds.Bottom - diameter, diameter, diameter);
                RectangleF bottomLeftCornerRect = new RectangleF(adjustedBounds.Left, adjustedBounds.Bottom - diameter, diameter, diameter);

                path.AddArc(topLeftCornerRect, 180, 90);
                path.AddArc(topRightCornerRect, 270, 90);
                path.AddArc(bottomRightCornerRect, 0, 90);
                path.AddArc(bottomLeftCornerRect, 90, 90);
                path.CloseAllFigures();

                using (Pen borderPen = new Pen(_borderColor, _borderThickness))
                {
                    e.Graphics.DrawPath(borderPen, path);
                }
            }

            Color buttonColor = _normalBackColor;

            if (_isMouseOver)
            {
                buttonColor = _mouseOverBackColor;
            }

            if (_isMouseDown)
            {
                buttonColor = _mouseDownBackColor;
            }

            using (Brush brush = new SolidBrush(buttonColor))
            {
                e.Graphics.FillPath(brush, RoundedRectangle(adjustedBounds, _cornerRadius));
            }

            TextRenderer.DrawText(e.Graphics, Text, Font, adjustedBounds, ForeColor, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
        }

        private GraphicsPath RoundedRectangle(Rectangle bounds, int radius)
        {
            GraphicsPath path = new GraphicsPath();

            int diameter = radius * 2;
            Rectangle arcRect = new Rectangle(bounds.Location, new Size(diameter, diameter));

            // Top left arc.
            path.AddArc(arcRect, 180, 90);

            // Top right arc.
            arcRect.X = bounds.Right - diameter;
            path.AddArc(arcRect, 270, 90);

            // Bottom right arc.
            arcRect.Y = bounds.Bottom - diameter;
            path.AddArc(arcRect, 0, 90);

            // Bottom left arc.
            arcRect.X = bounds.Left;
            path.AddArc(arcRect, 90, 90);

            path.CloseFigure();

            return path;
        }
    }
}