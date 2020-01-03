using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Windows.Forms.DataVisualization.Charting;
using System.Drawing.Printing;

namespace WindowsFormsApp1
{
    class Statistics
    {
        /// <summary>
        /// Создание диаграммы из значений распределения выборки
        /// </summary>
        /// <param name="numbers"></param>
        public static void createGraph(List numbers, Chart chart1)
        {
            List<double> normalDistributionValues = numbers.makeGraphValues();
            List<double> theoreticalNormalValues = numbers.makeTheoreticalValues();
            printGraph(theoreticalNormalValues, normalDistributionValues, chart1);
        }


        static void printGraph(List<double> theoreticalGraphValues, List<double> graphValues, Chart chart1)
        {
            chart1.Series[0].Points.Clear();
            chart1.Series[0].ChartType = SeriesChartType.Column;
            chart1.Series[1].Points.Clear();
            chart1.Series[1].ChartType = SeriesChartType.Spline;

            for (int i = 0; i < graphValues.Count; i++)
            {
                chart1.Series[0].Points.AddXY(i, graphValues[i]);
            }

            for (int i = 0; i < theoreticalGraphValues.Count; i++)
            {
                chart1.Series[1].Points.AddXY(i, theoreticalGraphValues[i]);
            }
        }
       
        

        public static void FillTable(List numbers, DataGridView dgv)
        {
            if (numbers.Count > 15)
            {
                dgv.Width = 166;
                dgv.Height = 324;
            }
            else
            {
                dgv.Width = 149;
                dgv.Height = numbers.Count * 21 + 22;
            }
            dgv.RowCount = numbers.Count;
            for (int i = 0; i < numbers.Count; i++)
            {
                dgv.Rows[i].Cells[0].Value = i + 1;
                dgv.Rows[i].Cells[1].Value = numbers[i];
            }
        }

        public static void CreateEmptyTable(DataGridView dgv)
        {            
            dgv.RowCount = 10000;

            dgv.Width = 166;
            dgv.Height = 324;           

            for (int i = 0; i < 10000; i++)
            {
                dgv.Rows[i].Cells[0].Value = i + 1;
            }            
        }

    }
}
