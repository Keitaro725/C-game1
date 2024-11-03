using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows;

namespace breakout
{
    public partial class Form1 : Form
    {
        Vector ballPos;
        Vector ballSpeed;
        int ballRadius;
        Rectangle paddlePos;
        public Form1()
        {
            InitializeComponent();

            this.ballPos = new Vector(200, 200);
            this.ballSpeed = new Vector(-6, -12);
            this.ballRadius = 10;
            this.paddlePos = new Rectangle(100, this.Height - 50, 100, 5);

            Timer timer = new Timer();
            timer.Interval = 33; //1秒間に30回ほど書き換え, 30FPS
            timer.Tick += new EventHandler(Update);
            timer.Start();
        }

        double DotProduct(Vector a, Vector b)
        {
            return a.X * b.X + a.Y * b.Y; //内積計算
        }

        bool LineVsCircle(Vector p1, Vector p2, Vector center, float radius)
        {
            Vector lineDir = (p2 - p1); // パドルの方向ベクトル
            Vector n=new Vector(lineDir.Y, lineDir.X); //パドルの法線
            n.Normalize();

            Vector dir1 = center - p1;
            Vector dir2 = center - p2;

            double dist = Math.Abs(DotProduct(dir1, n));
            double a1 = DotProduct(dir1, lineDir);
            double a2 = DotProduct(dir2, lineDir);

            return (a1 * a2 < 0 && dist < radius) ? true : false;
        }
        private void Update(object sender, EventArgs e)
        {
            // move ball
            ballPos += ballSpeed;
            //bound by side wall
            if (ballPos.X + ballRadius > this.Bounds.Width || ballPos.X - ballRadius < 0) 
            {
                ballSpeed.X *= -1;
            }

            //bound by over wall
            if(ballPos.Y - ballRadius < 0)
            {
                ballSpeed.Y *= -1;
            }
            //judge the crush
            if(LineVsCircle(new Vector(this.paddlePos.Left, this.paddlePos.Top),
                new Vector(this.paddlePos.Right, this.paddlePos.Top),
                ballPos, ballRadius))
            {
                ballSpeed.Y *= -1;
            }
            //redraw
            Invalidate();
        }

        private void Draw(object sender, PaintEventArgs e)
        {
            SolidBrush pinkBrush = new SolidBrush(Color.HotPink);
            SolidBrush grayBrush = new SolidBrush(Color.DimGray);

            float px = (float)this.ballPos.X - ballRadius;
            float py = (float)this.ballPos.Y - ballRadius;

            e.Graphics.FillEllipse(pinkBrush, px, py, this.ballRadius * 2, this.ballRadius * 2);
            e.Graphics.FillRectangle(grayBrush, paddlePos);
        }

        private void KeyPressed(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar == 'a')
            {
                this.paddlePos.X -= 20;
            }
            else if(e.KeyChar=='s')
            {
                this.paddlePos.X += 20;
            }
        }
    }
}
