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
        //инициализация массива для генерации случайного списка. Можно перенести в параметры вызова функции!
        protected int[] array = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 };

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //генерация случайного списка
            int[] random = RandomPermutation(array);

            //инициализация массива всех picturebox на форме
            PictureBox[] pctBox = new PictureBox[12];
            int i = 0;
            foreach (PictureBox pb in this.Controls.OfType<PictureBox>())
            {
                pctBox[i] = pb;
                i++;
            }

            //цикл загрузки изображений. Добавить сюда генерацию тегов для сравнения карточек!
            for (int j = 0; j < 11; j++) {
                    pctBox[random[j]].Image = Image.FromFile("C:\\Users\\nnper\\Desktop\\Paxeso\\" + random[j] + ".jpg");
                    pctBox[random[j + 1]].Image = Image.FromFile("C:\\Users\\nnper\\Desktop\\Paxeso\\" + random[j] + ".jpg");
                j++;
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
