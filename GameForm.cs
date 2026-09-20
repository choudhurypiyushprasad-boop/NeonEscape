using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;

namespace NeonEscape
{
    public partial class GameForm : Form
    {
        private Rectangle player;
        private int playerSpeed = 8;

        private List<Enemy> enemies = new List<Enemy>();
        private int enemySpeed = 2;

        private int lives = 3;

        private EnergyOrb energyOrb;

        private int score = 0;
        private int highScore = 0;

        private bool gameOver = false;
        private Button restartButton;

        private int survivalTime = 0;


        private Random random = new Random();


        private bool moveUp;
        private bool moveDown;
        private bool moveLeft;
        private bool moveRight;

        private System.Windows.Forms.Timer gameTimer;
        private System.Windows.Forms.Timer difficultyTimer;
        private System.Windows.Forms.Timer survivalTimer;

        public GameForm()
        {
            InitializeComponent();

            restartButton = new Button();

            restartButton.Text = "RESTART";
            restartButton.Font = new Font("Arial", 14, FontStyle.Bold);
            restartButton.Size = new Size(150, 50);
            restartButton.Location = new Point(375, 350);
            restartButton.BackColor = Color.Cyan;
            restartButton.ForeColor = Color.Black;
            restartButton.Visible = false;

            restartButton.Click += RestartButton_Click;

            this.Controls.Add(restartButton);

            this.Text = "NEON ESCAPE";
            this.ClientSize = new Size(900, 600);
            this.BackColor = Color.FromArgb(10, 10, 25);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;

            this.KeyPreview = true;
            this.DoubleBuffered = true;

            player = new Rectangle(425, 275, 30, 30);

            enemies.Add(new Enemy(100, 150, enemySpeed));
            enemies.Add(new Enemy(700, 150, enemySpeed));
            enemies.Add(new Enemy(100, 450, enemySpeed));

            energyOrb = CreateRandomOrb();

            this.KeyDown += GameForm_KeyDown;
            this.KeyUp += GameForm_KeyUp;

            gameTimer = new System.Windows.Forms.Timer();
            gameTimer.Interval = 16;
            gameTimer.Tick += GameTimer_Tick;
            gameTimer.Start();

            difficultyTimer = new System.Windows.Forms.Timer();
            difficultyTimer.Interval = 10000;
            difficultyTimer.Tick += DifficultyTimer_Tick;
            difficultyTimer.Start();
            survivalTimer = new System.Windows.Forms.Timer();
            survivalTimer.Interval = 1000;
            survivalTimer.Tick += SurvivalTimer_Tick;
            survivalTimer.Start();
        }

        private void GameTimer_Tick(object sender, EventArgs e)
        {
            MovePlayer();

            foreach (Enemy enemy in enemies)
            {
                enemy.MoveTowards(player);
            }

            CheckCollision();

            Invalidate();
        }

        private void DifficultyTimer_Tick(object sender, EventArgs e)
        {
            enemySpeed++;
            playerSpeed++;

            foreach (Enemy enemy in enemies)
            {
                enemy.Speed = enemySpeed;
            }
        }

        private void SurvivalTimer_Tick(object sender, EventArgs e)
        {
            survivalTime++;
            Invalidate();
        }

        private void CheckCollision()
        {
            foreach (Enemy enemy in enemies)
            {
                if (enemy.Bounds.IntersectsWith(player))
                {
                    lives--;

                    player.X = 425;
                    player.Y = 275;

                    enemy.Bounds.X = random.Next(50, ClientSize.Width - 80);
                    enemy.Bounds.Y = random.Next(100, ClientSize.Height - 80);
                    if (lives <= 0)
                    {
                        gameOver = true;

                        gameTimer.Stop();
                        difficultyTimer.Stop();
                        survivalTimer.Stop();

                        restartButton.Visible = true;
                    }

                    break;
                }
            }

            if (player.IntersectsWith(energyOrb.Bounds))
            {
                score += 10;

                if (score > highScore)
                {
                    highScore = score;
                }

                energyOrb = CreateRandomOrb();
            }
        }

        private EnergyOrb CreateRandomOrb()
        {
            int x = random.Next(40, ClientSize.Width - 60);
            int y = random.Next(90, ClientSize.Height - 60);

            return new EnergyOrb(x, y);
        }

        private void MovePlayer()
        {
            int newX = player.X;
            int newY = player.Y;

            if (moveUp)
                newY -= playerSpeed;

            if (moveDown)
                newY += playerSpeed;

            if (moveLeft)
                newX -= playerSpeed;

            if (moveRight)
                newX += playerSpeed;

            if (newX >= 20 &&
                newX + player.Width <= ClientSize.Width - 20)
            {
                player.X = newX;
            }

            if (newY >= 70 &&
                newY + player.Height <= ClientSize.Height - 20)
            {
                player.Y = newY;
            }
        }

        private void GameForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.W || e.KeyCode == Keys.Up)
                moveUp = true;

            if (e.KeyCode == Keys.S || e.KeyCode == Keys.Down)
                moveDown = true;

            if (e.KeyCode == Keys.A || e.KeyCode == Keys.Left)
                moveLeft = true;

            if (e.KeyCode == Keys.D || e.KeyCode == Keys.Right)
                moveRight = true;
        }

        private void GameForm_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.W || e.KeyCode == Keys.Up)
                moveUp = false;

            if (e.KeyCode == Keys.S || e.KeyCode == Keys.Down)
                moveDown = false;

            if (e.KeyCode == Keys.A || e.KeyCode == Keys.Left)
                moveLeft = false;

            if (e.KeyCode == Keys.D || e.KeyCode == Keys.Right)
                moveRight = false;
        }

        private void RestartButton_Click(object sender, EventArgs e)
        {
            player.X = 425;
            player.Y = 275;

            lives = 3;
            score = 0;
            survivalTime = 0;
            enemySpeed = 2;
            playerSpeed = 8;
            gameOver = false;

            enemies.Clear();

            enemies.Add(new Enemy(100, 150, enemySpeed));
            enemies.Add(new Enemy(700, 150, enemySpeed));
            enemies.Add(new Enemy(100, 450, enemySpeed));

            energyOrb = CreateRandomOrb();

            restartButton.Visible = false;

            gameTimer.Start();
            difficultyTimer.Start();
            survivalTimer.Start();

            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Graphics g = e.Graphics;

            g.Clear(Color.FromArgb(10, 10, 25));


            using (Pen gridPen = new Pen(Color.FromArgb(25, 0, 255, 255), 1))
            {
                for (int x = 40; x < ClientSize.Width - 20; x += 40)
                {
                    g.DrawLine(
                        gridPen,
                        x,
                        70,
                        x,
                        ClientSize.Height - 20
                    );
                }

                for (int y = 90; y < ClientSize.Height - 20; y += 40)
                {
                    g.DrawLine(
                        gridPen,
                        20,
                        y,
                        ClientSize.Width - 20,
                        y
                    );
                }
            }


            using (Pen borderPen = new Pen(Color.Cyan, 3))
            {
                g.DrawRectangle(
                    borderPen,
                    20,
                    70,
                    ClientSize.Width - 40,
                    ClientSize.Height - 90
                );
            }

            using (Font titleFont = new Font("Arial", 22, FontStyle.Bold))
            using (Brush titleBrush = new SolidBrush(Color.Cyan))
            {
                g.DrawString(
                    "NEON ESCAPE",
                    titleFont,
                    titleBrush,
                    20,
                    20
                );
            }

            using (Font livesFont = new Font("Arial", 16, FontStyle.Bold))
            using (Brush livesBrush = new SolidBrush(Color.Red))
            {
                string livesText = "LIVES: ";

                for (int i = 0; i < lives; i++)
                {
                    livesText += "♥ ";
                }

                g.DrawString(
                    livesText,
                    livesFont,
                    livesBrush,
                    650,
                    25
                );
            }

            using (Font scoreFont = new Font("Arial", 16, FontStyle.Bold))
            using (Brush scoreBrush = new SolidBrush(Color.Yellow))
            {
                g.DrawString(
                    "SCORE: " + score,
                    scoreFont,
                    scoreBrush,
                    390,
                    25
                );
            }
            using (Font highScoreFont = new Font("Arial", 16, FontStyle.Bold))
            using (Brush highScoreBrush = new SolidBrush(Color.Orange))
            {
                g.DrawString(
                    "HIGH: " + highScore,
                    highScoreFont,
                    highScoreBrush,
                    520,
                    25
                );
            }


            using (Font timeFont = new Font("Arial", 16, FontStyle.Bold))
            using (Brush timeBrush = new SolidBrush(Color.Lime))
            {
                g.DrawString(
                    "TIME: " + survivalTime + "s",
                    timeFont,
                    timeBrush,
                    270,
                    25
                );
            }

            using (SolidBrush glowBrush =
                   new SolidBrush(Color.FromArgb(40, 0, 255, 255)))
            {
                g.FillRectangle(
                    glowBrush,
                    player.X - 6,
                    player.Y - 6,
                    player.Width + 12,
                    player.Height + 12
                );
            }

            using (SolidBrush playerBrush =
                   new SolidBrush(Color.Cyan))
            {
                g.FillRectangle(playerBrush, player);
            }

            using (Pen playerPen =
                   new Pen(Color.White, 2))
            {
                g.DrawRectangle(playerPen, player);
            }

            foreach (Enemy enemy in enemies)
            {
                enemy.Draw(g);
            }

            energyOrb.Draw(g);

            if (gameOver)
            {
                using (SolidBrush overlayBrush =
                       new SolidBrush(Color.FromArgb(180, 0, 0, 0)))
                {
                    g.FillRectangle(
                        overlayBrush,
                        20,
                        70,
                        ClientSize.Width - 40,
                        ClientSize.Height - 90
                    );
                }

                using (Font gameOverFont =
                       new Font("Arial", 36, FontStyle.Bold))
                using (Brush gameOverBrush =
                       new SolidBrush(Color.Red))
                {
                    g.DrawString(
                        "GAME OVER",
                        gameOverFont,
                        gameOverBrush,
                        315,
                        200
                    );
                }

                using (Font resultFont =
                       new Font("Arial", 18, FontStyle.Bold))
                using (Brush resultBrush =
                       new SolidBrush(Color.White))
                {
                    g.DrawString(
                        "SCORE: " + score,
                        resultFont,
                        resultBrush,
                        370,
                        270
                    );

                    g.DrawString(
                        "TIME: " + survivalTime + "s",
                        resultFont,
                        resultBrush,
                        370,
                        305
                    );
                }
            }
        }
    }
}