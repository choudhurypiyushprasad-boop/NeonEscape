using System;
using System.Drawing;

namespace NeonEscape
{
    internal class EnergyOrb
    {
        public Rectangle Bounds;

        public EnergyOrb(int x, int y)
        {
            Bounds = new Rectangle(x, y, 20, 20);
        }

        public void Draw(Graphics g)
        {
            using (SolidBrush glowBrush =
                   new SolidBrush(Color.FromArgb(60, 0, 255, 255)))
            {
                g.FillEllipse(
                    glowBrush,
                    Bounds.X - 7,
                    Bounds.Y - 7,
                    Bounds.Width + 14,
                    Bounds.Height + 14
                );
            }

            using (SolidBrush orbBrush =
                   new SolidBrush(Color.Yellow))
            {
                g.FillEllipse(orbBrush, Bounds);
            }

            using (Pen outline =
                   new Pen(Color.White, 2))
            {
                g.DrawEllipse(outline, Bounds);
            }
        }
    }
}