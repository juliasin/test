using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Correlation
{
  
    public partial class Form2 : Form
    {
        double r;//пирсон
        double alpha;
        double kor;double m;
        List<double> x = new List<double>();
        List<double> y = new List<double>();
        List<double> xSort = new List<double>();
        List<double> ySort = new List<double>();

        public Form2(double m,double r, double alpha, List<double> x, List<double> y, List<double> xs, List<double> ys,double kor)
        {
            this.m = m;
            this.r = r;
            this.x = x;
            this.y = y;
            xSort = xs;
            ySort = ys;
            this.alpha = alpha;
            this.kor = kor;
            InitializeComponent();
        }

       
        public void Rang(List<int> ident, List<double> rang)
        {
            int z = 0;
            while (z != x.Count)
            {
                if (ident[z] > 1)
                {
                    int b = 0;
                    for (int k = 0; k < ident[z]; k++)
                    {
                        b++;
                        rang[z + k] = ((z + 1) * ident[z]*1.0 + ident[z] - 1) / ident[z];
                    }
                    z = z + b;
                }
                else
                { rang[z] = z + 1; z++; }
            }
        }

        public void WriteRang(List<double>nrangx, List<double> rangx,List<double>x, List<double> xsort)
        {
           
            for (int i = 0; i < x.Count; i++)
            {
                for (int j = 0; j < x.Count; j++)
                {
                    if (x[i] == xsort[j]) nrangx[i] = rangx[j];
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            dataGridView1.ColumnCount = 6;
            dataGridView1.RowCount = 5;
            dataGridView1[1, 0].Value = "Значення";
            dataGridView1[2, 0].Value = "Статистика";
            dataGridView1[3, 0].Value = "Квантиль";
            dataGridView1[4, 0].Value = "Значущість";
            dataGridView1[5, 0].Value = "Дов.інтервал";
            dataGridView1[0, 1].Value = "Коефіцієнт Пірсона";
            dataGridView1[0, 2].Value = "Коефіцієнт Спірмена";
            dataGridView1[0, 3].Value = "Коефіцієнт Кендалла";
            dataGridView1[0, 4].Value = "Кореляційне відношення";
            dataGridView1[1, 1].Value = Math.Round( r,4).ToString();
            //коэфиц пирсона
            double t = r * Math.Sqrt(x.Count - 2) / Math.Sqrt(1 - r * r);
            dataGridView1[2, 1].Value = Math.Round(t,4).ToString();
            dataGridView1[3, 1].Value = Math.Round( Form1.t(x.Count - 2, alpha / 2),4).ToString();
            if (Math.Abs(Double.Parse(dataGridView1[2, 1].Value.ToString())) <= Double.Parse(dataGridView1[3, 1].Value.ToString()))
            { dataGridView1[4, 1].Value = "Не отклоняем"; } else {
              dataGridView1[4, 1].Value = "Отклоняем";
            }
           double dov1 = Math.Round(r + (r * (1 - r * r)) / (2 * x.Count) - Form1.U(alpha / 2) * (1 - r * r) / Math.Sqrt(x.Count), 4);
            double dov2 = Math.Round(r + (r * (1 - r * r)) / (2 * x.Count) + Form1.U(alpha / 2) * (1 - r * r) / Math.Sqrt(x.Count), 4);
            dataGridView1[5, 1].Value = Math.Round(dov1,4).ToString() + " ; " + Math.Round(dov2,4).ToString();

            //кор отнош
            dataGridView1[1, 4].Value = Math.Round(kor, 4).ToString();
            double ch = kor / (m - 1);
            double zn = (1 - kor) / (x.Count - m);
            double fc= ch / zn;
            dataGridView1[2, 4].Value = Math.Round(fc, 4).ToString();
            dataGridView1[3, 4].Value = Math.Round(Form1.f(m - 1, x.Count - m, alpha), 4).ToString();
            if (Double.Parse(dataGridView1[2, 4].Value.ToString()) <= Double.Parse(dataGridView1[3, 4].Value.ToString()))
            { dataGridView1[4, 4].Value = "Не отклоняем"; }
            else
            {
                dataGridView1[4, 4].Value = "Отклоняем";
            }



            //Спирмен

            List<int> identx = new List<int>();
            List<int> identy = new List<int>();
            List<double> rangx = new List<double>();
            List<double> rangy = new List<double>();
            List<double> nrangx = new List<double>();
            List<double> nrangy = new List<double>();
            List<double> objectsX = new List<double>();
            List<double> objectsY = new List<double>();

            for (int i = 0; i < x.Count; i++)
            {
                identx.Add(0);
                identy.Add(0);
                rangx.Add(0);
                rangy.Add(0);
                nrangx.Add(0);
                nrangy.Add(0);
            }

            for (int i = 0; i < x.Count; i++)
            {              //считаю для каждого элемента исходного массива количество повторений
                for (int j = 0; j < x.Count; j++)
                {
                    if (xSort[i] == xSort[j]) identx[j]++;
                    if (ySort[i] == ySort[j]) identy[j]++;
                }
            }

            List<int> svyzX = new List<int>();
            List<int> svyzY = new List<int>();
            //считаю связки
            for (int i = 0; i < identx.Count; i++)
            {
                if ((identx[i] > 1) && !(objectsX.Contains(xSort[i])))
                { svyzX.Add(identx[i]); objectsX.Add(xSort[i]); }
                if ((identy[i] > 1) && !(objectsY.Contains(ySort[i])))
                { svyzY.Add(identy[i]); objectsY.Add(ySort[i]); }
            }

            double A = 0;double B = 0;
            for (int i = 0; i < svyzX.Count; i++)
                A += Math.Pow(svyzX[i], 3) - svyzX[i];
            A = A / 12;

            for (int i = 0; i < svyzY.Count; i++)
                B += Math.Pow(svyzY[i], 3) - svyzY[i];
            B = B / 12;

            Rang(identx, rangx);
            Rang(identy, rangy);
            WriteRang(nrangx, rangx, x,xSort);
            WriteRang(nrangy, rangy, y,ySort);

            double spir = 0;
            for (int i = 0; i < x.Count; i++)
                 spir += Math.Pow((nrangx[i] - nrangy[i]), 2);
            double chisl = 0;double znam = 0;
            chisl =  x.Count * (x.Count* x.Count - 1)/6 - spir - A - B;
            znam=Math.Sqrt((x.Count * (x.Count * x.Count - 1) / 6 - 2 * A) * (x.Count * (x.Count * x.Count - 1) / 6 - 2 * B));
            spir = chisl / znam;

             t = spir * Math.Sqrt(x.Count - 2) / Math.Sqrt(1 - spir * spir);
            dataGridView1[1, 2].Value = Math.Round(spir,4).ToString();
            dataGridView1[2, 2].Value = Math.Round(t,4).ToString();
            dataGridView1[3, 2].Value = Math.Round(Form1.t(x.Count - 2, alpha / 2),4).ToString();
            if (Math.Abs(Double.Parse(dataGridView1[2, 2].Value.ToString())) <= Double.Parse(dataGridView1[3, 2].Value.ToString()))
            { dataGridView1[4, 2].Value = "Не отклоняем"; }
            else
            {
                dataGridView1[4, 2].Value = "Отклоняем";
            }

            //  кендалл
            Obj[] obj =  new Obj[x.Count];
            for (int i = 0; i < x.Count; i++)
            {
                obj[i] = new Obj { xx = x[i], yy = y[i],ry=nrangy[i] };
            }

            obj = obj.OrderBy(m => m.xx).ToArray();

            int[,] nu = new int[y.Count, y.Count];
            for (int i = 0; i < y.Count; i++)
            {
                for (int j = 0; j < y.Count; j++)
                {
                    if ((obj[i].ry < obj[j].ry) && (rangx[i] != rangx[j]))
                        nu[i, j] = 1;
                    else if ((obj[i].ry > obj[j].ry) && (rangx[i] != rangx[j]))
                        nu[i, j] = -1;
                    else
                        nu[i, j] = 0;
                }
            }
            double s = 0;
            for (int i = 0; i < x.Count - 1; i++)
            {
                for (int j = i + 1; j < x.Count; j++) { s += nu[i, j]; }
            }

            double kend = 0;
            for (int i = 0; i < svyzX.Count; i++)
                A += svyzX[i] * (svyzX[i] - 1);
            A = A / 2;

            for (int i = 0; i < svyzY.Count; i++)
                B += svyzY[i] * (svyzY[i] - 1);
            B = B / 2;

            double smax = Math.Sqrt((0.5*x.Count*(x.Count-1)-A)* (0.5 * x.Count * (x.Count - 1) - B));
            kend = s / smax;
            double u = 3*kend*Math.Sqrt(x.Count*(x.Count-1))/Math.Sqrt(2*(2*x.Count+5));
            dataGridView1[1, 3].Value = Math.Round(kend, 4).ToString();
            dataGridView1[2, 3].Value = Math.Round(u, 4).ToString();
            dataGridView1[3, 3].Value = Math.Round(Form1.U(alpha / 2), 4).ToString();
            if (Math.Abs(Double.Parse(dataGridView1[2, 3].Value.ToString())) <= Double.Parse(dataGridView1[3, 3].Value.ToString()))
            { dataGridView1[4, 3].Value = "Не отклоняем"; }
            else
            {
                dataGridView1[4, 3].Value = "Отклоняем";
            }

        }
    }

    public class Obj { public double xx { get; set; } public double yy { get; set; } public double ry { get; set; } }
}
