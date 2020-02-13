using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Diagnostics;

namespace Pexeso
{
    public partial class Form1 : Form
    {
        //инициализация массива для генерации случайного списка. Не переносить в параметр вызова функции поскольку размер поля динамический! Написать функцию автозаполнения.
        protected int[] array = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 };
        //счетчик нажатия на изображение
        int countClick = 0;
        //переменные для тегов изображений
        int tag1 = 0;
        int tag2 = 0;
        //переменная для хранения ссылки на первое нажатое изображение
        PictureBox clickPctBox1 = new PictureBox();
        PictureBox clickPctBox2 = new PictureBox();
        //счетчик
        int point = 0;
        //переменная для множителя подсчета очков
        bool flagPoint = false;
        //выделение массива под элементы
        PictureBox[] pctBox = new PictureBox[12];
        //создание фоновой асинхронной задачи
        BackgroundWorker loadBackToImage = new BackgroundWorker();
        BackgroundWorker visiblePb = new BackgroundWorker();
        //
        Stopwatch stopky = new Stopwatch();

        public Form1()
        {
            InitializeComponent();
            //подписка на события фоновых асинхронных задач
            loadBackToImage.DoWork += new DoWorkEventHandler(loadBackToImage_DoWork);
            loadBackToImage.RunWorkerCompleted += loadBackToImage_RunWorkerCompleted;
            visiblePb.DoWork += new DoWorkEventHandler(loadBackToImage_DoWork);
            visiblePb.RunWorkerCompleted += visiblePb_RunWorkerCompleted;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            stopky.Start();
            timer1.Start();
            //отмена подписки на события "Чистка мусора"
            foreach (PictureBox pb in this.Controls.OfType<PictureBox>())
            {
                pb.Click -= new EventHandler(pb_Click);
            }

            //генерация случайного списка
            int[] random = RandomPermutation(array);

            //сброс глобальных переменных
            clickPctBox1.Enabled = true;
            flagPoint = false;
            point = 0;
            countClick = 0;
            int n = 0;
            label1.Text = n.ToString();

            //инициализация массива всех picturebox на форме
            int i = 0;
            foreach (PictureBox pb in this.Controls.OfType<PictureBox>())
            {
                pb.Visible = true;
                pctBox[i] = pb;
                pb.Click += new EventHandler(pb_Click);
                i++;
            }

            //цикл загрузки изображений
            for (int j = 0; j < 12; j++) {
                pctBox[random[j]].Image = Image.FromFile(@"Data\back.jpg");
                pctBox[random[j]].Tag = random[j];
                pctBox[random[j + 1]].Image = Image.FromFile(@"Data\back.jpg");
                pctBox[random[j + 1]].Tag = random[j];
                j++;
            }
        }

        //обработчик события нажатия на picturebox
        void pb_Click(object sender, EventArgs e)
        {
            PictureBox clickedPictureBox = sender as PictureBox;
            clickedPictureBox.Image = Image.FromFile(@"Data\" + clickedPictureBox.Tag.ToString() + ".jpg");
            countClick++;
            //
            int finish = 0;

            if (countClick == 1)
            {
                tag1 = Convert.ToInt32(clickedPictureBox.Tag);
                clickPctBox1 = clickedPictureBox;
                clickPctBox1.Enabled = false;
            }
            if (countClick == 2)
            {
                clickPctBox1.Enabled = true;
                tag2 = Convert.ToInt32(clickedPictureBox.Tag);
                clickPctBox2 = clickedPictureBox;
                if (tag1 == tag2 && clickPctBox1.Name != clickedPictureBox.Name)
                {
                    addPoint();
                    flagPoint = true;
                    System.Media.SystemSounds.Hand.Play();
                    visiblePb.RunWorkerAsync();
                } else
                {
                    loadBackToImage.RunWorkerAsync();
                    flagPoint = false;
                }

                foreach (PictureBox pb in this.Controls.OfType<PictureBox>())
                {
                    pb.Enabled = false;
                }
                countClick = 0;
            }
        }

        private void visiblePb_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            clickPctBox2.Visible = false;
            clickPctBox1.Visible = false;
            foreach (PictureBox pb in this.Controls.OfType<PictureBox>())
            {
                pb.Enabled = true;
            }
        }

        private void loadBackToImage_DoWork(object sender, DoWorkEventArgs e)
        {
            Thread.Sleep(1000);
        }

        private void loadBackToImage_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            foreach (PictureBox pb in this.Controls.OfType<PictureBox>())
            {
                pb.Image = Image.FromFile(@"Data\back.jpg");
                pb.Enabled = true;
            }
        }

        void addPoint()
        {
            if (flagPoint)
            {
                point = point + 2;
            } else point++;

            label1.Text = point.ToString();
        }

        //функция генерации случайного списка
        static int[] RandomPermutation(int[] a)
        {
            Random random = new Random();
            var n = a.Length;
            while (n > 1)
            {
                n--;
                var i = random.Next(n + 1);
                var temp = a[i];
                a[i] = a[n];
                a[n] = temp;
            }
            return a;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            TimeSpan ts = stopky.Elapsed;
            label2.Text = String.Format("{0:00}:{1:00}:{2:00}.{3:00}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10);
        }
    }
}