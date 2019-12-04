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
using System.Windows.Forms.DataVisualization.Charting;
using System.Drawing.Printing;
namespace WindowsFormsApp1
{
    class FileOperations:List
    {
       

        /// <summary>
        /// Заполнение List рандомными значениями от низней до верней границы.
        /// нижняя и верхняя границы - из аргументов
        /// </summary>
        /// <param name="lowValue"></param>
        /// <param name="highValue"></param>
        public static void fillListByRandom(List numbers, int lowValue, int highValue, int length)
        {
            Random rnd = new Random();
            Random rnd1 = new Random();
            int randomLength = rnd.Next(1, length);

            for (int i = 0; i < randomLength; i++)
            {
                numbers.Add(rnd1.Next(lowValue, highValue));
            }
        }


        /// <summary>
        /// Заполнение List из файла.
        /// </summary>
        /// <param name="fileName"></param>
        public static void fillListFromFile(List numbers, string fileName)
        {           
            List<string> numbersSting = openBinaryFile(fileName);
            for (int i = 0; i < numbersSting.Count; i++)
            {
                string tempNumberString = numbersSting[i];

                if (tempNumberString != "")                
                    numbers.Add(Convert.ToInt32(tempNumberString));  
                else
                    break;
                    
            }
        }


        /// <summary>
        /// Открытие файла и возврат полученного текста
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        private static string[] openFile(string fileName)
        {
            string[] s = new string[255];
            s = System.IO.File.ReadAllLines(fileName);
            return s;
        }

        /// <summary>
        /// Открытие файла и возврат полученного текста
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        private static List<string> openBinaryFile(string fileName)
        {
            List<string> stringList = new List<string>();
            using (BinaryReader reader = new BinaryReader(File.Open(fileName, FileMode.Open)))
            {
                while (reader.PeekChar() > -1)
                {
                    stringList.Add(reader.ReadString());
                }
                reader.Close();
            }
            return stringList;
        }

        /// <summary>
        /// Вывод List в файл.
        /// </summary>
        /// <param name="filePath"></param>
        public static void printListToFile(List numbers, string filePath)
        {
            try
            {
                string[] listText = new string[numbers.Count];
                for (int i = 0; i < numbers.Count; i++)
                {
                    listText[i] += numbers[i].ToString();
                }
                System.IO.File.WriteAllLines(@"" + filePath, listText);

            }
            catch { MessageBox.Show("Ошибка сохранения в файл!"); }
        }

        /// <summary>
        /// Вывод List в бинарный файл.
        /// </summary>
        /// <param name="filePath"></param>
        public static void printListToBinaryFile(List numbers, string filePath)
        {
            try
            {
                BinaryWriter bClear = new BinaryWriter(new FileStream(filePath, FileMode.OpenOrCreate));

                for (int i = 0; i < 10000; i++)
                {
                    bClear.Write("");
                }
                bClear.Close();


                BinaryWriter bWriter = new BinaryWriter(new FileStream(filePath, FileMode.OpenOrCreate));   

                for (int i = 0; i < numbers.Count; i++)
                {
                    bWriter.Write(numbers[i] + "\n");
                }                

                bWriter.Close();
            }
            catch { MessageBox.Show("Ошибка сохранения в файл!"); }
        }


        /// <summary>
        /// Вывод информации о характеристиках List в файл.
        /// </summary>
        /// <param name="options"></param>
        /// <param name="filePath"></param>
        public static void printInfoToFile(string options, string filePath)
        {
            System.IO.File.WriteAllText(@"" + filePath, options);
        }

        /// <summary>
        /// Отправка на печать с помощью принтера
        /// </summary>
        static string gPrintText = "";
        public static void printInfoByPrinter(List numbers, string printText)
        {
            try
            {
                gPrintText = printText;
                PrintDocument printDocument = new PrintDocument();
                printDocument.PrintPage += printPageHandler;

                PrintDialog printDialog = new PrintDialog();
                printDialog.Document = printDocument;
                if (printDialog.ShowDialog() == DialogResult.OK)
                    printDocument.Print();
                MessageBox.Show("Печать прошла успешно!");
            }
            catch { MessageBox.Show("Ошибка печати!"); }
        }
        /// <summary>
        /// Печать. Параметры: текст, шрифт, 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void printPageHandler(object sender, PrintPageEventArgs e)
        {
            e.Graphics.DrawString(gPrintText, new Font("Arial", 14), Brushes.Black, 35, 25);
        }
    }
}
