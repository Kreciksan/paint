﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace paintcik
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        // 1 pencil - 2 fill - 3 eraser - 4 line - 5 rectangle - 6 circle
        int action = 1;
        Bitmap bm = new Bitmap(1000,500);
        Bitmap picked = new Bitmap(100,50);
        Graphics g;
        Graphics pickedG;
        SolidBrush brush = new SolidBrush(Color.Black);
        Rectangle PickedSize = new Rectangle(0, 0, 100, 50);

        Pen pen;
        Pen eraser = new Pen(Color.White, 5);
        Point p1, p2;
        bool paint = false;
        Color color = Color.Black;

        private void Form1_Load_1(object sender, EventArgs e)
        {
            g = Graphics.FromImage(bm);
            pickedG = Graphics.FromImage(picked);
            pen = new Pen(color, 2);
            pictureBox1.Image = bm;
            pictureBox2.Image = picked;
            pickedG.FillRectangle(brush, PickedSize);
            pictureBox2.Refresh();
            saveFileDialog1.Filter = "Image(*.jpg)|*.jpg|(*.*)|*.*";
        }

        private void pencilBtn_Click(object sender, EventArgs e)
        {
            action = 1;
            
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            paint = true;
            pen.Color = color;
            p1 = e.Location;

        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            p2 = e.Location;
            Rectangle r = new Rectangle(p1.X, p1.Y, p2.X - p1.X, p2.Y - p1.Y);

            if (action == 4)
            {
                g.DrawLine(pen, p1, p2);
            }

            if (action == 5)
            {
                g.DrawRectangle(pen, r);
            }

            if (action == 6)
            {
                g.DrawEllipse(pen, r);
            }

            paint = false;
        }

        private void colorBtn_Click(object sender, EventArgs e)
        {
            if(colorDialog1.ShowDialog() == DialogResult.OK)
            {
                color = colorDialog1.Color;
                brush.Color = color;
                pickedG.FillRectangle(brush, PickedSize);
                pictureBox2.Refresh();
            }
        }

        private void fillBtn_Click(object sender, EventArgs e)
        {
            action = 2;
        }

        private void eraserBtn_Click(object sender, EventArgs e)
        {
            action = 3;
        }

        private void lineBtn_Click(object sender, EventArgs e)
        {
            action = 4;
        }

        private void rectangleBtn_Click(object sender, EventArgs e)
        {
            action = 5;
        }

        private void circleBtn_Click(object sender, EventArgs e)
        {
            action = 6;
        }

        private void Validate(Bitmap bm, Stack<Point>pixel,int x, int y,Color old_color,Color new_color)
        {
            Color cx = bm.GetPixel(x, y);
            if(cx == old_color)
            {
                pixel.Push(new Point(x, y));
                bm.SetPixel(x, y, new_color);
            }
        }

        public void Fill(Bitmap bm, int x, int y, Color new_clr)
        {
            Color old_color = bm.GetPixel(x, y);
            Stack<Point> pixel = new Stack<Point>();
            pixel.Push(new Point(x, y));
            bm.SetPixel(x, y, new_clr);

            if (old_color == new_clr) return;

            while(pixel.Count > 0)
            {
                Point pt = pixel.Pop();
                if(pt.X > 0 && pt.Y > 0 && pt.X < bm.Width - 1 && pt.Y < bm.Height - 1)
                {
                    Validate(bm, pixel,pt.X - 1,pt.Y,old_color,new_clr);
                    Validate(bm, pixel, pt.X, pt.Y - 1, old_color, new_clr);
                    Validate(bm, pixel, pt.X +1, pt.Y, old_color, new_clr);
                    Validate(bm, pixel, pt.X, pt.Y +1, old_color, new_clr);

                }
            }
        }


        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            if (action == 2)
            {
                Fill(bm,e.X,e.Y,color);
            }
        }

        private void clearBtn_Click(object sender, EventArgs e)
        {
            brush.Color = Color.White;
            g.FillRectangle(brush, 0,0,1000,500);
            pictureBox1.Refresh();
        }

        private void saveBtn_Click(object sender, EventArgs e)
        { 
            if(saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                bm.Save(saveFileDialog1.FileName,ImageFormat.Jpeg);
            }
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if(paint)
            {
                if (action == 1)
                {
                    p2 = e.Location;
                    g.DrawLine(pen, p1, p2);
                    p1 = p2;
                }
 

                if (action == 3)
                {
                    p2 = e.Location;
                    g.DrawLine(eraser, p1, p2);
                    p1 = p2;
                }
            }
            pictureBox1.Refresh();
        }
    }
}
