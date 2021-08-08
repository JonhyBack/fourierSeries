using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace MethodFourierSeries
{
    public partial class Form1 : Form
    {
        int _degree = 15;
        (double A, double B) _limits = (A: -4, B: 4);
        double _epsilon = 0.001;
        List<double> xValues = new List<double>();
        List<double> yValues = new List<double>();

        Func<double, double> Func1 = x => x;
        Func<double, double> Func2 = x => 2 * x;

        public Form1()
        {
            InitializeComponent();

            for (double i = _limits.A; i < 0; i += _epsilon)
            {
                xValues.Add(i);
                yValues.Add(Func1(i));
            }

            for (double i = 0; i < _limits.B; i += _epsilon)
            {
                xValues.Add(i);
                yValues.Add(Func2(i));
            }

            chart1.ChartAreas[0].AxisX.RoundAxisValues();
            chart1.ChartAreas[0].AxisY.RoundAxisValues();

            chart1.ChartAreas[0].AxisX.Interval = 0.5;
            chart1.ChartAreas[0].AxisY.Interval = 0.5;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            chart1.Series["Chart of the function"].Points.DataBindXY(xValues, yValues);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Resolve();
        }

        private void Resolve()
        {
            var result = new FourierSeries(_degree, Func1, Func2)
            {
                Limits = _limits,
                Epsilon = _epsilon
            }.GetResult();

            chart1.Series["Chart of the polinom"].Points.DataBindXY(result.xValues, result.yValues);
        }
    }
}
