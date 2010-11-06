using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Aont
{
    public partial class IsingForm : Form
    {
        int N, M;


        public const int Pixel = 2;

        int[,] SpinStates;

        Bitmap bitmap;
        static Random random = new Random();
        public IsingForm(int N, int M)
        {
            InitializeComponent();

            this.SuspendLayout();

            this.N = N;
            this.M = M;
            this.bitmap = new Bitmap(N, M);
            this.pictureBox1.Size = new Size(N * Pixel, M * Pixel);
            this.SpinStates = new int[N, M];
            for (int n = 0; n < N; n++)
            {
                for (int m = 0; m < M; m++)
                {
                    SetSpin(n, m, 2 * random.Next(2) - 1);


                }
            }
            this.pictureBox1.Image = this.bitmap;


            this.ResumeLayout(false);
            this.PerformLayout();

            this.timer1.Enabled = true;
        }


        public double J = 1.5e-21;
        public double H = 0;
        public double T = 350;
        const double Kb = 1.4e-23;
        public void Advance(int n, int m)
        {
            int Spin_nm = SpinStates[n, m];
            double P = 0;
            if (n > 0)
                P -= SpinStates[n - 1, m];
            if (n < N - 1)
                P -= SpinStates[n + 1, m];
            if (m > 0)
                P -= SpinStates[n, m - 1];
            if (m < M - 1)
                P -= SpinStates[n, m + 1];

            P = (J * P - H) * -2 * Spin_nm;

            if (P < 0)
            {
                SetSpin(n, m, -Spin_nm);
            }
            else
            {
                P = Math.Exp(-P / (Kb * T));
                //P /= 1 + P;
                if (P > random.NextDouble())
                {
                    SetSpin(n, m, -Spin_nm);
                }
            }

        }

        void SetSpin(int n, int m, int States)
        {
            SpinStates[n, m] = States;
            switch (States)
            {
                case 1:
                    this.bitmap.SetPixel(m, n, Color.Black);
                    break;
                case -1:
                    this.bitmap.SetPixel(m, n, Color.White);
                    break;
                default:
                    throw new Exception();
            }
            //this.pictureBox1.Invalidate();
        }
        public void Advance()
        {
            int n = random.Next(N);
            int m = random.Next(M);
            Advance(n, m);
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            for (int i = 0; i * i < 16 * M * N; i++)
            {
                this.Advance();
            }
            this.pictureBox1.Invalidate();
            int averagespin = 0;
            for (int n = 0; n < N; n++)
            {
                for (int m = 0; m < M; m++)
                {
                    averagespin += SpinStates[n, m];
                }
            }

            this.AverageSpintoolStripLabel.Text = ((double)averagespin / (N * M)).ToString();
        }


        private SettingsForm settingform;
        private void SettingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (settingform == null || settingform.IsDisposed)
            {
                settingform = new SettingsForm(this);
            }
            settingform.Show();

        }

    }
}