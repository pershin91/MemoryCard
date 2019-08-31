using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pexeso
{
    public partial class Form1 : Form
    {
        //инициализация массива для генерации случайного списка. Не переносить в параметр вызова функции поскольку размер поля динамический! Написать функцию автозаполнения.
        protected int[] array = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 };
        //счетчик нажатия на изображение
        protected int countClick = 0;
        //переменные для тегов изображений
        int tag1 = 0;
        int tag2 = 0;
        //
        PictureBox[] pctBox = new PictureBox[12];

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //генерация случайного списка
            int[] random = RandomPermutation(array);

            //инициализация массива всех picturebox на форме
            int i = 0;
            foreach (PictureBox pb in this.Controls.OfType<PictureBox>())
            {
                pctBox[i] = pb;
                pb.Click += new EventHandler(pb_Click);
                i++;
            }

            //цикл загрузки изображений
            for (int j = 0; j < 12; j++) {
                pctBox[random[j]].Image = Image.FromFile("C:\\Users\\nnper\\Desktop\\Paxeso\\" + random[j] + ".jpg");
                pctBox[random[j]].Tag = random[j];
                pctBox[random[j + 1]].Image = Image.FromFile("C:\\Users\\nnper\\Desktop\\Paxeso\\" + random[j] + ".jpg");
                pctBox[random[j + 1]].Tag = random[j];
                j++;
            }
        }

        //обработчик события нажатия на picturebox
        //придумать как игнорировать двойное нажатие на одну карточку!
        void pb_Click(object sender, EventArgs e)
        {
            PictureBox clickedPictureBox = sender as PictureBox;
            countClick++;
            if (countClick == 1)
            {
                tag1 = Convert.ToInt32(clickedPictureBox.Tag);
            }
            if (countClick == 2)
            {
                tag2 = Convert.ToInt32(clickedPictureBox.Tag);
            }
            if (countClick == 2) {
                if (tag1 == tag2)
                {
                    countClick = 0;
                    MessageBox.Show("Совпадение!");
                } else countClick = 0;
            }
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
    }
}
