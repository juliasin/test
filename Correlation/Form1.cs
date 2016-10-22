using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using System.Globalization;

namespace Correlation
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        int M;
        List<double> mx = new List<double>();
        List<double> my = new List<double>();
        List<double> x = new List<double>();
        List<double> y = new List<double>();
         double[] srarifm = new double[2];
        double[] disp = new double[2];
        double[] dispsm = new double[2];
        double[] srkv = new double[2];
          double[] srkvsm = new double[2];
        double[] assim = new double[2];
        double[] assimsm = new double[2];
        double[] eks = new double[2];
        double[] ekssm = new double[2];

        double[] srkvsr = new double[2];
        double[] srkvsrkv = new double[2];
        double[] srkvassim = new double[2];
        double[] srkveks = new double[2];
        const double alpha = 0.05;

        public void Count(List<double> x, List<double> y)
        {

            for (int i = 0; i < x.Count; i++)
            {
                srarifm[0] += x[i];
                srarifm[1] += y[i];
            }
            srarifm[0] = srarifm[0] / x.Count;
            srarifm[1] = srarifm[1] / y.Count;

            for (int i = 0; i < x.Count; i++)
            {
                disp[0] += Math.Pow((x[i] - srarifm[0]), 2);
                disp[1] += Math.Pow((y[i] - srarifm[1]), 2);
                dispsm[0] += Math.Pow((x[i] - srarifm[0]), 2);
                dispsm[1] += Math.Pow((y[i] - srarifm[1]), 2);
                assimsm[0] += Math.Pow((x[i] - srarifm[0]), 3);
                assimsm[1] += Math.Pow((y[i] - srarifm[1]), 3);
                ekssm[0] += Math.Pow((x[i] - srarifm[0]), 4);
                ekssm[1] += Math.Pow((y[i] - srarifm[1]), 4);
            }
            disp[0] = disp[0] / (x.Count - 1);
            disp[1] = disp[1] / (y.Count - 1);
            dispsm[0] = dispsm[0] / x.Count;
            dispsm[1] = dispsm[1] / y.Count;

            srkv[0] = Math.Sqrt(disp[0]);
            srkv[1] = Math.Sqrt(disp[1]);
            srkvsm[0] = Math.Sqrt(dispsm[0]);
            srkvsm[1] = Math.Sqrt(dispsm[1]);

            assimsm[0] = assimsm[0] / (x.Count * Math.Pow(srkvsm[0], 3));
            assimsm[1] = assimsm[1] / (y.Count * Math.Pow(srkvsm[1], 3));
            assim[0] = ((Math.Sqrt(x.Count * (x.Count - 1))) * assimsm[0]) / (x.Count - 2);
            assim[1] = ((Math.Sqrt(y.Count * (y.Count - 1))) * assimsm[1]) / (y.Count - 2);
            ekssm[0] = ekssm[0] / (x.Count * Math.Pow(srkvsm[0], 4));
            ekssm[1] = ekssm[1] / (y.Count * Math.Pow(srkvsm[1], 4));
            eks[0] = (x.Count * x.Count - 1) * (ekssm[0] - 3 + 6 / (x.Count + 1)) / ((x.Count - 2) * (x.Count - 3));
            eks[1] = (y.Count * y.Count - 1) * (ekssm[1] - 3 + 6 / (y.Count + 1)) / ((y.Count - 2) * (y.Count - 3));

            dataGridView1.ColumnCount = 5;
            dataGridView2.ColumnCount = 5;
            dataGridView1.RowCount = 6;
            dataGridView2.RowCount = 6;
            dataGridView1[0, 0].Value = "Характеристика";
            dataGridView2[0, 0].Value = "Характеристика";
            dataGridView1[1, 0].Value = "Оценка";
            dataGridView2[1, 0].Value = "Оценка";
            dataGridView1[2, 0].Value = "Среднекв.откл";
            dataGridView2[2, 0].Value = "Среднекв.откл.";
            dataGridView1[3, 0].Value = "Доверительный";
            dataGridView2[3, 0].Value = "Доверительный";
            dataGridView1[4, 0].Value = "интервал";
            dataGridView2[4, 0].Value = "интервал";
            dataGridView1[0, 1].Value = "Среднее";
            dataGridView1[1, 1].Value = srarifm[0].ToString();
            dataGridView2[0, 1].Value = "Среднее";
            dataGridView2[1, 1].Value = srarifm[1].ToString();
            dataGridView1[0, 2].Value = "Несмещ.среднекв.откл.";
            dataGridView1[1, 2].Value = srkv[0].ToString();
            dataGridView2[0, 2].Value = "Несмещ.среднекв.откл";
            dataGridView2[1, 2].Value = srkv[1].ToString();
            dataGridView1[0, 3].Value = "Смещ.среднекв.откл";
            dataGridView1[1, 3].Value = srkvsm[0].ToString();
            dataGridView2[0, 3].Value = "Смещ.среднекв.откл";
            dataGridView2[1, 3].Value = srkvsm[1].ToString();
            dataGridView1[0, 4].Value = "Асимметрия";
            dataGridView1[1, 4].Value = assim[0].ToString();
            dataGridView2[0, 4].Value = "Асимметрия";
            dataGridView2[1, 4].Value = assim[1].ToString();
            dataGridView1[0, 5].Value = "Эксцесс";
            dataGridView1[1, 5].Value = eks[0].ToString();
            dataGridView2[0, 5].Value = "Эксцесс";
            dataGridView2[1, 5].Value = eks[1].ToString();

            srkvsr[0] = srkv[0] / Math.Sqrt(x.Count);
            srkvsr[1] = srkv[1] / Math.Sqrt(y.Count);
            srkvsrkv[0] = srkv[0] / Math.Sqrt(2 * x.Count);
            srkvsrkv[1] = srkv[1] / Math.Sqrt(2 * y.Count);
            srkvassim[0] = Math.Sqrt(6.0 * (x.Count - 2) / ((x.Count + 1) * (x.Count + 3)));
            srkvassim[1] = Math.Sqrt(6.0 * (y.Count - 2) / ((y.Count + 1) * (y.Count + 3)));
            srkveks[0] = Math.Sqrt(24.0 * x.Count * (x.Count - 2) * (x.Count - 3) / ((x.Count + 1) * 1.0 * (x.Count + 1) * (x.Count + 3) * (x.Count + 5)));
            srkveks[1] = Math.Sqrt(24.0 * y.Count * (y.Count - 2) * (y.Count - 3) / ((y.Count + 1) * 1.0 * (y.Count + 1) * (y.Count + 3) * (y.Count + 5)));
            dataGridView1[2, 1].Value = Math.Round(srkvsr[0],4).ToString();
            dataGridView1[3, 1].Value = Math.Round((srarifm[0] - t(x.Count - 1,alpha/2) * Convert.ToDouble(dataGridView1[2, 1].Value)),4).ToString();
            dataGridView1[4, 1].Value =Math.Round((srarifm[0] + t(x.Count - 1, alpha / 2) * Convert.ToDouble(dataGridView1[2, 1].Value)),4).ToString();

            dataGridView2[2, 1].Value = Math.Round(srkvsr[1],4).ToString();
            dataGridView2[3, 1].Value = Math.Round((srarifm[1] - t(y.Count - 1, alpha / 2) * Convert.ToDouble(dataGridView2[2, 1].Value)),4).ToString();
            dataGridView2[4, 1].Value = Math.Round((srarifm[1] + t(y.Count - 1, alpha / 2) * Convert.ToDouble(dataGridView2[2, 1].Value)),4).ToString();

            dataGridView1[2, 2].Value = Math.Round(srkvsrkv[0],4).ToString();
            dataGridView1[3, 2].Value = Math.Round((srkv[0] - t(x.Count - 1, alpha / 2) * Convert.ToDouble(dataGridView1[2, 2].Value)),4).ToString();
            dataGridView1[4, 2].Value = Math.Round((srkv[0] + t(x.Count - 1, alpha / 2) * Convert.ToDouble(dataGridView1[2, 2].Value)),4).ToString();

            dataGridView2[2, 2].Value = Math.Round(srkvsrkv[1],4).ToString();
            dataGridView2[3, 2].Value = Math.Round((srkv[1] - t(y.Count - 1, alpha / 2) * Convert.ToDouble(dataGridView2[2, 2].Value)),4).ToString();
            dataGridView2[4, 2].Value = Math.Round((srkv[1] + t(y.Count - 1, alpha / 2) * Convert.ToDouble(dataGridView2[2, 2].Value)),4).ToString();

            dataGridView1[2, 4].Value = Math.Round(srkvassim[0],4).ToString();
            dataGridView1[3, 4].Value = Math.Round((assim[0] - t(x.Count - 1, alpha / 2) * Convert.ToDouble(dataGridView1[2, 4].Value)),4).ToString();
            dataGridView1[4, 4].Value = Math.Round((assim[0] + t(x.Count - 1, alpha / 2) * Convert.ToDouble(dataGridView1[2, 4].Value)),4).ToString();

            dataGridView2[2, 4].Value = Math.Round( srkvassim[1],4).ToString();
            dataGridView2[3, 4].Value = Math.Round((assim[1] - t(y.Count - 1, alpha / 2) * Convert.ToDouble(dataGridView2[2, 4].Value)),4).ToString();
            dataGridView2[4, 4].Value = Math.Round((assim[1] + t(y.Count - 1, alpha / 2) * Convert.ToDouble(dataGridView2[2, 4].Value)),4).ToString();

            dataGridView1[2, 5].Value = Math.Round(srkveks[0],4).ToString();
            dataGridView1[3, 5].Value = Math.Round((eks[0] - t(x.Count - 1, alpha / 2) * Convert.ToDouble(dataGridView1[2, 5].Value)),4).ToString();
            dataGridView1[4, 5].Value = Math.Round((eks[0] + t(x.Count - 1, alpha / 2) * Convert.ToDouble(dataGridView1[2, 5].Value)),4).ToString();

            dataGridView2[2, 5].Value = Math.Round( srkveks[1],4).ToString();
            dataGridView2[3, 5].Value = Math.Round((eks[1] - t(y.Count - 1, alpha / 2) * Convert.ToDouble(dataGridView2[2, 5].Value)),4).ToString();
            dataGridView2[4, 5].Value = Math.Round((eks[1] + t(y.Count - 1, alpha / 2) * Convert.ToDouble(dataGridView2[2, 5].Value)),4).ToString();
        }

        //const double alpha = 0.05;
        public static double U(double alpha)
        {
            double c0 = 2.515517, c1 = 0.802863, c2 = 0.010328, d1 = 1.432788, d2 = 0.1892659, d3 = 0.001308;
            double t = Math.Sqrt(-2 * Math.Log(alpha));
            double u = t - (c0 + c1 * t + c2 * t * t) / (1 + d1 * t + d2 * t * t + d3 * Math.Pow(t, 3));
            return u;
        }

        public static double t(int v,double alpha)
        {
            double g1 = (Math.Pow(U(alpha), 3) + U(alpha)) / 4;
            double g2 = (5 * Math.Pow(U(alpha), 5) + 16 * Math.Pow(U(alpha), 3) + 3 * U(alpha)) / 96;
            double g3 = (3 * Math.Pow(U(alpha), 7) + 19 * Math.Pow(U(alpha), 5) + 17 * Math.Pow(U(alpha), 3) - 15 * U(alpha)) / 384;
            return U(alpha) + (g1 / v) + (g2 / (v * v)) + (g3 / (v * v * v));
        }

        public double Xi(int p,double alpha)
        {
            return p * Math.Pow(1 - 2.0 / (9 * p) + U(alpha) * Math.Sqrt(2.0 / (9 * p)), 3);
        }

        public static double f(double v1, double v2, double alpha)
        {
            double sigma = 1 / v1 + 1 / v2;
            double delta = 1 / v1 - 1 / v2;
            double z1 = U(alpha) * Math.Sqrt(sigma / 2) - delta * (U(alpha) * U(alpha) + 2) / 6 + Math.Sqrt(sigma / 2) * (sigma * (U(alpha) * U(alpha) + 3 * U(alpha)) / 24 + delta * delta * (Math.Pow(U(alpha), 3) + 11 * U(alpha)) / (72 * sigma));
            double z2 = delta * sigma * (Math.Pow(U(alpha), 4) + 9 * U(alpha) * U(alpha) + 8) / 120 + Math.Pow(delta, 3) * (3 * Math.Pow(U(alpha), 4) + 7 * U(alpha) * U(alpha) - 16) / (3240 * sigma);
            double z3 = Math.Sqrt(sigma / 2) * (sigma * sigma * (Math.Pow(U(alpha), 5) + 20 * Math.Pow(U(alpha), 3) + 15 * U(alpha)) / 1920 + Math.Pow(delta, 4) * (Math.Pow(U(alpha), 5) + 44 * Math.Pow(U(alpha), 3) + 183 * U(alpha)) / 2880 + Math.Pow(delta, 4) * (9 * Math.Pow(U(alpha), 5) - 284 * Math.Pow(U(alpha), 3) - 1513 * U(alpha)) / (155520 * sigma * sigma));
            return Math.Exp(2 * (z1 - z2 + z3));
        }
        public double Kor(List<double> k, List<double> x,List <double>y ,double ysr)
        {
             M = (int)Math.Round(1 + 3.32 * Math.Log10(k.Count));  //кол-во классов
            double h = Math.Round(((x[x.Count - 1] - x[0]) / M), 3);
            double[] xb = new double[M + 1];
            xb[0] = x[0];
            xb[M] = x[x.Count - 1];
            for (int i = 1; i < M; i++)
                xb[i] = xb[i - 1] + h;  //классы
            double[] n = new double[M];
            for (int j = 0; j < k.Count; j++)
                for (int i = 0; i < M; i++)
                {

                    if ((k[j] >= xb[i]) && (k[j] < xb[i + 1])) n[i]++;
                }
            n[M - 1] = n[M - 1] + 1;

            Obj[] pairs = new Obj[x.Count];
            for (int i = 0; i < x.Count; i++)
                pairs[i] = new Obj { xx = k[i], yy = y[i], ry = 0 };
            pairs = pairs.OrderBy(m => m.xx).ToArray();
            Obj[][] xs = new Obj[M][]; int p = 0;
            for (int i = 0; i < M; i++)
            {
                xs[i] = new Obj[(int)n[i]];
                p = 0;
                for (int j = 0; j < x.Count; j++)
                {
                    if ((x[j] >= xb[i]) && (x[j] < xb[i + 1]))
                    { xs[i][p] = pairs[j]; p++; }
                }
            }
            xs[M - 1][p] = pairs[x.Count - 1];
            double[] ys = new double[M];
            for (int i = 0; i < M; i++)
            {
                for (int j = 0; j < n[i]; j++)
                {
                    ys[i] += xs[i][j].yy;
                }
                if (n[i] != 0)
                    ys[i] /= n[i];
            }
            double sum = 0;
            for (int i = 0; i < M; i++)
                sum += n[i] * Math.Pow((ys[i] - ysr), 2);
            double sum1 = 0;
            for (int i = 0; i < M; i++)
            {
                for (int j = 0; j < n[i]; j++)
                    sum1 += Math.Pow((xs[i][j].yy - ysr), 2);
            }
            return sum / sum1;


            /*   double[] fx = new double[M + 1];
               double[] pi = new double[M];
               double[] ni = new double[M];
               double[] xx1 = new double[M + 1];
               double z = 0;
               Array.Copy(xx, xx1, xx.Length);
               for (int i = 0; i < xx1.Length; i++)
                   xx1[i] = (xx1[i] - a) / sigma;//станд норм-е
               for (int i = 0; i < M + 1; i++)
                   fx[i] = chart1.DataManipulator.Statistics.NormalDistribution(xx1[i]);
               for (int i = 0; i < M; i++)
               {
                   pi[i] = fx[i + 1] - fx[i];
                   ni[i] = x.Count * pi[i];
               }

               for (int i = 0; i < M; i++)
                   z += (n[i] - ni[i]) * (n[i] - ni[i]) / ni[i];

                if (z < Xi(M - 1,alpha/2)) return true; //норм распределение
               else return false; //не норм*/


        }



        public double Pirson(List<double> x, List<double> y)
        {
            double xy = 0;
            for (int i = 0; i < x.Count; i++)
            xy += x[i] * y[i];
            xy = xy / x.Count;
            double r = (xy - srarifm[0] * srarifm[1]) / (srkvsm[0] * srkvsm[1]);
            return r;
        }

        private void button1_Click(object sender, EventArgs e)
        {
             OpenFileDialog openFileDialog1 = new OpenFileDialog();
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("en-US");
                string s = openFileDialog1.FileName;
                FileStream fs = new FileStream(s, FileMode.Open, FileAccess.Read);
                StreamReader sr = new StreamReader(fs);
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    string[] z = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                    x.Add(double.Parse(z[0]));
                    y.Add(double.Parse(z[1]));         
                }
               for (int i = 0; i < x.Count; i++)
                    chart1.Series[0].Points.AddXY(x[i], y[i]);
            }

            mx.AddRange(x); x.Sort(); my.AddRange(y); y.Sort();
            Count(x, y);
          
   //       Kor(my, y, srkvsm[1], srarifm[1]);

            //MessageBox.Show()
         



            }

        private void button2_Click(object sender, EventArgs e)
        {
            double r = Pirson(mx, my);
            double kor = Kor(mx, x,my, srarifm[1]);
            Form2 f = new Form2(M,r,alpha,mx,my,x,y,kor);
            f.Show();
           
        }
    }
}
