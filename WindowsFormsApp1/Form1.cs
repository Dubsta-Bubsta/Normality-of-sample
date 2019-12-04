using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Windows.Forms;
using System.IO;
using System.Windows.Forms.DataVisualization.Charting;
namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {

        List numbers = new List();
        List nonChangedNumbers = new List();        
        int getPathCallCounter = 0;
        string filePath = "";
        bool saveFileConfirmed = false;
        bool equalsLists = true;
        int lowValue = 1000;
        int highValue = 10000;
        int listLength = 10000;

        public Form1()
        {
            InitializeComponent();
            button1.Enabled = false;
            button2.Enabled = false;

            button4.Enabled = false;
        }

        private void Form1_Load(object sender, EventArgs e)
        {           
            saveFileDialog1.Filter = "Бинарный формат (*.bin)|*.bin";
            saveFileDialog2.Filter = "Текстовый формат (*.txt)|*.txt";
            openFileDialog1.Filter = "Бинарный формат (*.bin)|*.bin";
        }
        /// <summary>
        /// Довпустимые значения для textBox1
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                //Проверка на цифры
                if (e.KeyChar >= '0' && e.KeyChar <= '9')
                    return;

                //Backspace
                if ((Keys)e.KeyChar == Keys.Back)
                    return;
                e.KeyChar = '\0';
            }
            catch { MessageBox.Show("Ошибка!"); }
        }
        /// <summary>
        /// Изменение значения textBox1 - кнопка добавления вручную становится активной
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBox1_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {
                button1.Enabled = true;
            }
            else
            {
                button1.Enabled = false;
            }
        }

        /// <summary>
        /// Если нажат radioButton1 - разрешается вводить вручную (разблокируется текстовое поле и кнопка) 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RadioButton1_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                RadioButton rbtn = (RadioButton)sender;
                if (rbtn.Equals(radioButton1))
                {
                    textBox1.Enabled = true;
                    textBox1.Text = "";
                    button2.Enabled = false;
                }
                else
                {
                    textBox1.Enabled = false;
                    textBox1.Text = "";
                    button2.Enabled = true;
                }
            }
            catch { MessageBox.Show("Ошибка!"); }
        }

       
        /// <summary>
        /// Показ ошибки
        /// </summary>
        /// <param name="text"></param>
        void showError(string text)
        {
            MessageBox.Show(text);
        }

        /// <summary>
        /// Нажатие на кнопку добавления вручную. Значение проверяется. 
        /// Если все хорошо - добавляется в основной список
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button1_Click(object sender, EventArgs e)
        {
            try
            {
                int textBoxValue = Convert.ToInt32(textBox1.Text);
                if (checkValue(textBoxValue))
                {
                    numbers.Add(textBoxValue);
                    nonChangedNumbers.Add(textBoxValue);
                    equalsLists = true;
                   
                    saveFileRequest(); 
                }
            }
            catch { MessageBox.Show("Ошибка!"); }
        }

        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button2_Click(object sender, EventArgs e)
        {
            try
            {
                getPathCallCounter = 0;
                numbers.Clear();
                if (radioButton2.Checked)
                {
                    FileOperations.fillListByRandom(numbers, lowValue, highValue, listLength);

                    saveFileConfirmed = false;
                    getPathCallCounter = 0;
                }
                else if (radioButton3.Checked)
                {
                    if (openFileDialog1.ShowDialog() == DialogResult.Cancel)
                        return;

                    FileOperations.fillListFromFile(numbers, openFileDialog1.FileName);
                    saveFileConfirmed = true;
                    filePath = openFileDialog1.FileName;
                    getPathCallCounter = 1;
                }

                nonChangedNumbers.Clear();
                for (int i = 0; i < numbers.Count; i++)
                {
                    nonChangedNumbers.Add(numbers[i]);
                    equalsLists = true;
                }
                label1.Text = numbers.Count.ToString();


                saveFileRequest(); 
            }
            catch { MessageBox.Show("Ошибка задания выборки!"); }
        }

        /// <summary>
        /// Запрос на сохранение
        /// </summary>
        public void saveFileRequest()
        {
            try
            {
                //Сохранение в файл
                if (getPathCallCounter == 0)
                {
                    getPathCallCounter++;
                    DialogResult result = MessageBox.Show("Хотите сохранить выборку в файл?", "Сохранение", MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                    if (result == DialogResult.Yes)
                    {                       
                        if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                        {                            
                            filePath = saveFileDialog1.FileName;
                            saveFileConfirmed = true;
                        }
                        else
                        {                            
                            saveFileConfirmed = false;
                        }
                    }
                }
                if (saveFileConfirmed)
                {
                    FileOperations.printListToBinaryFile(numbers, filePath);
                }
            }
            catch
            {
                MessageBox.Show("Ошибка!");
            }
        }


        /// <summary>
        /// Очистка всего списка
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button6_Click(object sender, EventArgs e)
        {
            numbers.Clear();
            nonChangedNumbers.Clear();
        }


        /// <summary>
        /// Проверка вводимого значения. Если меньше нижней или выше верхней границы -         /// вызывается метод показа ошибки(MessageBox)
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        bool checkValue(int value)
        {
            if ((value > lowValue) && (value < highValue))
                return true;
            else
            {
                showError("Вводимое значение должно быть в диапазоне от " +  lowValue + " до " + highValue);
                return false;
            }
        }

        /// <summary>
        /// При переходе на вкладку "Таблица значений" - отрисовывается таблица
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex == 1)
                Statistics.CreateTable(numbers, dataGridView1);
            else if (tabControl1.SelectedIndex == 3)
            {
                var timer = new Stopwatch();
                timer.Start();
                Statistics.createGraph(numbers, chart1);
                timer.Stop();
                label8.Text = "\n" + timer.Elapsed.ToString();
            }

        }


        /// <summary>
        /// Уход со вкладки изменения значений
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TabControl1_Deselecting(object sender, TabControlCancelEventArgs e)
        {
            try
            {
                if (tabControl1.SelectedIndex == 1)
                    saveMessageBox(equalsLists);
            }
            catch { MessageBox.Show("Ошибка!"); }
        }

        /// <summary>
        /// Нажатие на кнопку "Рассчитать и вывести". 
        /// Рассчитывается нормальность и все характеристики выборки.
        /// По выбору выводится на экран, на принтер или в файл
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button3_Click(object sender, EventArgs e)
        {
            var timer = new Stopwatch();
            timer.Start();
            //Вычислить результат
            double normalityProbability = Math.Round(numbers.getNormalityProbability(), 3);           
           
            string options = numbers.getOptions();           
            string printText = options;
           
            if (numbers.isNormal() == true)
            {
                printText += "\nТак как A < Aкр и E < Eкр, то распределение нормально";
                printText += "\nВероятность того, что данная выборка подчиняется нормальному распределению:  " + normalityProbability + "(" + normalityProbability * 100 + "%)";
            }
            else
                printText += "\nТак как A > Aкр или E > Eкр, то распределение не нормально";

                   
            label4.Text = printText;
            if (radioButton4.Checked)
            {
            }
            else if (radioButton5.Checked)
            {
                //Вывод на принтер                
                FileOperations.printInfoByPrinter(numbers, printText);
            }
            else
            {
                //Вывод в файл               
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    FileOperations.printInfoToFile(printText, saveFileDialog1.FileName);
                }
            }
            timer.Stop();
            label7.Text = timer.Elapsed.ToString();
        }

        /// <summary>
        /// Изменения значения в таблице
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button4_Click(object sender, EventArgs e)
        {
            int changingIndex = Convert.ToInt32(textBox2.Text) - 1;
            int changingValue = Convert.ToInt32(textBox3.Text);

            if ((changingIndex < numbers.Count) && (changingIndex >= 0))
            {
                if (checkValue(changingValue))
                {
                    numbers[changingIndex] = changingValue;
                    Statistics.CreateTable(numbers, dataGridView1);
                }
            }
            else
            {
                MessageBox.Show("Выход за границы списка!");
            }
        }

        /// <summary>
        /// Если текстовые поля на вкладке ищменения значений не пусты - 
        /// кнопка изменения будет доступна для нажатия
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBox3_TextChanged(object sender, EventArgs e)
        {
            TextBox txbx = (TextBox)sender;
            if ((txbx.Text != "") && (txbx.Text != ""))            
                button4.Enabled = true;            
            else            
                button4.Enabled = false;            
        }

        /// <summary>
        /// Сохранение измененных значений в файл
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button5_Click(object sender, EventArgs e)
        {
            try
            {
                if (saveFileConfirmed == true)
                {
                    FileOperations.printListToBinaryFile(numbers, filePath);                   

                    nonChangedNumbers.Clear();
                    for (int i = 0; i < numbers.Count; i++)
                    {
                        nonChangedNumbers.Add(numbers[i]);                       
                    }
                    equalsLists = true;                    
                }
                else
                {
                    if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                    {
                        getPathCallCounter++;
                        filePath = saveFileDialog1.FileName;
                        saveFileConfirmed = true;

                        nonChangedNumbers.Clear();
                        for (int i = 0; i < numbers.Count; i++)
                        {
                            nonChangedNumbers.Add(numbers[i]);
                        }
                        equalsLists = true;
                    }                    

                }
                
            }
            catch { MessageBox.Show("Ошибка сохранения!");}

        }

        /// <summary>
        /// При закрытии формы, если не сохранены изменения - спросит пользователя о сохранении
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            saveMessageBox(equalsLists);
        }
        /// <summary>
        /// Предложение сохранения List в файл, если был изменен
        /// </summary>
        /// <param name="equalsLists"></param>
        void saveMessageBox(bool equalsLists)
        {
            try
            {
                equalsLists = true;
                for (int i = 0; i < numbers.Count; i++)
                {
                    if (!(nonChangedNumbers[i] == numbers[i]))
                    {
                        equalsLists = false;
                    }
                }

                if (!(equalsLists))
                {
                    DialogResult result = MessageBox.Show("Хотите сохранить введенные данные?", "Сохранение", MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                    if (result == DialogResult.Yes)
                    {
                        FileOperations.printListToBinaryFile(numbers, filePath);
                        saveFileConfirmed = true;
                    }
                }
            }
            catch { MessageBox.Show("Ошибка!"); }
        }

        private void Button7_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2(numbers);
            form2.Owner = this;
           
            form2.ShowDialog();           

        }
    }
}
