using System;
using System.Drawing;
using System.Security.Policy;

namespace NeonEscape
{
    internal class Enemy
    {
        public Rectangle Bounds;

        public int Speed;

        private Random random;

        public Enemy(int x, int y, int speed)
        {
            Bounds = new Rectangle(x, y, 30, 30);
            Speed = speed;
            random = new Random();
        }

        public void MoveTowards(Rectangle player)
        {
            if (Bounds.X < player.X)
                Bounds.X += Speed;

            if (Bounds.X > player.X)
                Bounds.X -= Speed;

            if (Bounds.Y < player.Y)
                Bounds.Y += Speed;

            if (Bounds.Y > player.Y)
                Bounds.Y -= Speed;
        }

        public void Draw(Graphics g)
        {
            using (SolidBrush glowBrush =
                   new SolidBrush(Color.FromArgb(50, 255, 0, 100)))
            {
                g.FillRectangle(
                    glowBrush,
                    Bounds.X - 6,
                    Bounds.Y - 6,
                    Bounds.Width + 12,
                    Bounds.Height + 12
                );
            }

            using (SolidBrush enemyBrush =
                   new SolidBrush(Color.HotPink))
            {
                g.FillRectangle(enemyBrush, Bounds);
            }

            using (Pen outline =
                   new Pen(Color.White, 2))
            {
                g.DrawRectangle(outline, Bounds);
            }
        }
    }
}
