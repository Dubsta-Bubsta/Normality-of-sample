using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
 
namespace WindowsFormsApp1
{
    public partial class Form2 : Form
    {
        public Form2(List<int> numbers)
        {
            InitializeComponent();
            chart1.Width = Screen.PrimaryScreen.WorkingArea.Width;
            chart1.Height = Screen.PrimaryScreen.WorkingArea.Height;

           
            for (int i = 0; i < numbers.Count; i++)
            {
                newList.Add(numbers[i]);
            }
        }
        List newList = new List();

        private void Form2_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
           
            Statistics.createGraph(newList, chart1);
        }
    }
}
