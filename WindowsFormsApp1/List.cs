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
    class List:List<int>
    {
        private List<int> list;  

        public List()
        {           
            list = new List<int>();           
        }

        public List(int n)
        {
            list = new List<int>(n);            
        }     

        public double getMin()
        {
            double min = this[0];
            for (int i = 0; i < this.Count; i++){
                if (this[i] < min)
                    min = this[i];
            }

            return min;
        }
        public double getMax()
        {
            double max = this[0];
            for (int i = 0; i < this.Count; i++)
            {
                if (this[i] > max)
                    max = this[i];
            }

            return max;
        }

        /// <summary>
        /// Получение среднего значения из всей выборки
        /// </summary>
        /// <param name="numbers"></param>
        /// <returns></returns>
        public double getAverageValue()
        {
            double listSum = 0;

            for (int i = 0; i < this.Count; i++)
            {
                listSum += this[i];
            }

            return listSum / this.Count;
        }

       
        /// <summary>
        /// Определение среднего квадратичного значения
        /// </summary>
        /// <param name="numbers"></param>
        /// <returns></returns>
        public double getStandardDeviation()
        {
            double standardDeviation = 0;
            double average = this.getAverageValue();

            for (int i = 0; i < this.Count; i++)
            {
                double elemValue = this[i] - average;
                standardDeviation += Math.Pow(elemValue, 2.0);
            }

            standardDeviation /= this.Count - 1;
            standardDeviation = Math.Sqrt(standardDeviation);

            return standardDeviation;
        }


        /// <summary>
        /// Получение стандартной ошибки
        /// </summary>
        /// <returns></returns>
        public double getAverageStandardError()
        {
            double standardDeviation = getStandardDeviation();
            return standardDeviation / Math.Sqrt(this.Count);
        }

        /// <summary>
        /// Дисперсия (D = Среднеквадратическое отклонение в квадрате)
        /// </summary>
        /// <param name="numbers"></param>
        /// <returns></returns>
        public double getDispersion()
        {
            double dispersion = 0;
            double average = this.getAverageValue();

            for (int i = 0; i < this.Count; i++)
            {
                double elemValue = this[i] - average;
                dispersion += Math.Pow(elemValue, 2.0);
            }

            dispersion /= (this.Count - 1);

            return dispersion;
        }

        /// <summary>
        /// Получение медианы (Xme)
        /// </summary>
        /// <param name="numbers"></param>
        /// <returns></returns>
        public double getMedian()
        {
            double median;
            int centralPosition = this.Count / 2;
            if (this.Count % 2 == 0)
                median = (this[centralPosition] + this[centralPosition - 1]) / 2.0;
            else
                median = this[centralPosition];

            return median;
        }

        public int getMode()
        {           
            int mode = 0;

            List numbers = this;
            int[] happenArr = new int[this.Count];

            for (int i = 0; i < this.Count; i++)
            {
                for (int j = 0; j < this.Count; j++)
                {
                    if (this[i] == this[j])
                        happenArr[i]++;
                }
            }

            int maxHappens = 0;
            for (int i = 0; i < happenArr.Length; i++)
            {
                if (happenArr[i] > maxHappens)
                {
                    maxHappens = happenArr[i];
                    mode = this[i];
                }                   
            }

            if (maxHappens == 1)
                mode = 0;
            return mode;
        }


        /// <summary>
        /// Разница между максимальным и минимальным значением в выборке (R - Размах)
        /// </summary>
        /// <param name="numbers"></param>
        /// <returns></returns>
        public double getScope()
        {
            double maxValue = this[0];
            double minValue = this[0];
            for (int i = 0; i < this.Count; i++)
            {
                if (this[i] > maxValue)
                    maxValue = this[i];
                if (this[i] < minValue)
                    minValue = this[i];
            }
            return maxValue - minValue;
        }


        /// <summary>
        /// Коэффициент вариации - V
        /// </summary>
        /// <param name="numbers"></param>
        /// <returns></returns>
        public double getVariationCoefficient()
        {
            double standardDeviation = this.getStandardDeviation();
            double averageValue = this.getAverageValue();

            return (standardDeviation / averageValue) * 100;
        }

        /// <summary>
        /// коэффициент осцилляции (Ko)
        /// </summary>
        /// <param name="numbers"></param>
        /// <returns></returns>
        public double getOscillationCoefficient()
        {
            double scope = this.getScope();
            double averageValue = this.getAverageValue();

            return (scope / averageValue) * 100;
        }

        /// <summary>
        /// Asymmetry coefficient
        /// </summary>
        /// <returns></returns>
        public double getAsymmentryCoefficient()
        {
            double averageValue = this.getAverageValue();
            double standardDeviation = this.getStandardDeviation();
            
            double sum = 0;
            for (int i = 0; i < this.Count; i++)
            {
                double elem = this[i] - averageValue;
                sum += Math.Pow(elem, 3);
            }
            double asymmetryCoefficient = sum / (this.Count * Math.Pow(standardDeviation, 3)); 
            return asymmetryCoefficient;
        }
        /// <summary>
        /// Критическое значение критерия ассиметрии
        /// </summary>
        /// <returns></returns>
        public double getAsymmentryCritical()
        {
            int N = this.Count;
            double asymmetryCritical = (6.0 * (N - 1.0)) / ((N + 1.0) * (N + 3.0));
            return 3 * Math.Sqrt(asymmetryCritical);
        }

        /// <summary>
        /// Ошибка ассиметрии
        /// </summary>
        /// <returns></returns>
        public double getAsymmentryError()
        {
            double asymmentryError = Math.Sqrt(6.0 / (this.Count + 3));
            return asymmentryError;
        }

        /// <summary>
        /// Коэффициента эксцесса
        /// </summary>
        /// <returns></returns>
        public double getExcessCoefficient()
        {
            double averageValue = this.getAverageValue();
            double standardDeviation = this.getStandardDeviation();

            double sum = 0;
            for (int i = 0; i < this.Count; i++)
            {
                sum += Math.Pow((this[i] - averageValue), 4);
            }

            double excessCoefficient = sum / (this.Count * Math.Pow(standardDeviation, 4));
            return excessCoefficient - 3;
        }

        /// <summary>
        /// Критическое значение критерия эксцесса
        /// </summary>
        /// <returns></returns>
        public double getExcessCritical()
        {
            int N = this.Count;          

            double excessCritical = (24 * N) / Math.Pow((N + 1), 2);
            excessCritical *= (N - 2);
            excessCritical /= ((N + 3) * (N + 5));
            excessCritical *= (N - 3);

            return 5 * Math.Sqrt(excessCritical);           
        }

        /// <summary>
        /// Ошибка эксцесса
        /// </summary>
        /// <returns></returns>
        public double getExcessError()
        {
            double getExcessError = 2* Math.Sqrt(6.0 / (this.Count + 5));
            return getExcessError;
        }

        /// <summary>
        /// Получение всех характеристик выборки и cоздание из них текста
        /// </summary>
        /// <param name="numbers"></param>
        /// <returns></returns>
        public string getOptions()
        {
            string optionsText = "Характеристики выборки: ";
            optionsText += "\nМинимальное: " + this.getMin();
            optionsText += "\nМаксимальное: " + this.getMax();
            optionsText += "\nДисперсия: " + Math.Round(this.getDispersion(), 2);
            optionsText += "\nМедиана: " + Math.Round(this.getMedian(), 2);
            optionsText += "\nМода: " + this.getMode();
            optionsText += "\nРазмах: " + Math.Round(this.getScope(), 2);
            optionsText += "\nСреднее арифметическое: " + Math.Round(this.getAverageValue(), 2);
            optionsText += "\nСреднеквадратичное отклонение: " + Math.Round(this.getStandardDeviation(), 2);          
            optionsText += "\nКоэффициент вариации: " + Math.Round(this.getVariationCoefficient(), 2);           
            optionsText += "\nКоэффициент осцилляции: " + Math.Round(this.getOscillationCoefficient(), 2);
            optionsText += "\nСтандартная ошибка: " + Math.Round(this.getAverageStandardError(), 2);
            optionsText += "\nКоэффициент асимметрии: " + Math.Round(this.getAsymmentryCoefficient(), 3);
            optionsText += "\nОшибка асимметрии: " + Math.Round(this.getAsymmentryError(), 3);
            optionsText += "\nКритический показатель ассиметрии: " + Math.Round(this.getAsymmentryCritical(), 3);
            optionsText += "\nКоэффициент эксцесса: " + Math.Round(this.getExcessCoefficient(), 3);
            optionsText += "\nОшибка экцесса: " + Math.Round(this.getExcessError(), 3);
            optionsText += "\nКритический показатель эксцесса: " + Math.Round(this.getExcessCritical(), 3);

            return optionsText;
        }


        public bool isNormal()
        {
            double asymmentryCoefficient = Math.Round(this.getAsymmentryCoefficient(), 3);           
            double asymmentryError = Math.Round(this.getAsymmentryError(), 3);
            double excessCoefficient = Math.Round(this.getExcessCoefficient(), 3);
            double excessError = Math.Round(this.getExcessError(), 3);


            //double tAsymmetry = asymmentryCoefficient / asymmentryError;
            //double tExcess = excessCoefficient / excessError;

            if ((Math.Abs(asymmentryCoefficient) < asymmentryError * 3) && (Math.Abs(excessCoefficient) < excessError * 3))
                return true;               
            else
                return false;            

        }

        /// <summary>
        /// Определение нормальности выборки
        /// </summary>
        /// <param name="numbers"></param>
        /// <returns></returns>
        public double getNormalityProbability()
        {
            double averageValue = Math.Round(this.getAverageValue(), 2);
            double standardDeviation = Math.Round(this.getStandardDeviation(), 2);

            double f1 = (this.getMin() - averageValue) / (standardDeviation);
            f1 = laplassFunc(f1);
            double f2 = (this.getMax() - averageValue) / (standardDeviation);
            f2 = laplassFunc(f2);


             double normalityProbability = f2 - f1;
            return normalityProbability;
            //return 0;
        }


        /// <summary>
        /// false - laslass, true - NormalDistribution
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public List<double> makeGraphValues()
        {           

            List<double> graphValues = new List<double>();
            List<int> copyNumbers = sortList();
            double averageValue = Math.Round(this.getAverageValue(), 2);
            double standardDeviation = Math.Round(this.getStandardDeviation(), 2);

            for (int i = 0; i < copyNumbers.Count; i++)
            {
                double elem;                
                elem = densityOfNormalDistribution(copyNumbers[i], averageValue, standardDeviation);               
                graphValues.Add(elem);
            }
            return graphValues;
        }


        /// <summary>
        /// Получение теоретических значений для графика нормального распределения
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public List<double> makeTheoreticalValues()
        {
            double averageValue = Math.Round(this.getAverageValue(), 2);
            double standardDeviation = Math.Round(this.getStandardDeviation(), 2);

            List<double> graphValues = new List<double>();
            List<int> copyNumbers = sortList();
            List<double> normalValues = new List<double>();            

            double step = (this.getMax() - this.getMin()) / (this.Count - 1);

            double normalElem = this.getMin();
            for (int i = 0; i < this.Count; i++)
            {
                normalValues.Add(normalElem);
                normalElem += step;
            }

            for (int i = 0; i < copyNumbers.Count; i++)
            {
                double elem = densityOfNormalDistribution(normalValues[i], averageValue, standardDeviation);
                graphValues.Add(elem);
            }
            return graphValues;
        }


        /// <summary>
        /// Функция плотности нормального распределения
        /// </summary>
        /// <param name="value"></param>
        /// <param name="standardDeviation"></param>
        /// <param name="average"></param>
        /// <returns></returns>
        public double densityOfNormalDistribution(double value, double averageValue, double standardDeviation)
        {
            double elem = 1 / (Math.Sqrt(2 * Math.PI) * standardDeviation);
            double stepen = -Math.Pow((value - averageValue), 2) / (2 * Math.Pow(standardDeviation, 2));
            double e = Math.Pow(Math.E, stepen);
            elem *= e;

            return elem;
        }

        /// <summary>
        /// Функция Лапласса
        /// </summary>
        /// <param name="value"></param>
        /// <param name="precision"></param>
        /// <returns></returns>
        public double laplassFunc(double value)
        {
            double precision = 0.0001;
            if (value > 5)
                return 0.5;
            double result = 0;
            for (double i = 0; i < Math.Abs(value); i += precision)
            {
                result += precision * Math.Abs(Math.Pow(Math.E, -0.5 * Math.Pow(i, 2)) + Math.Pow(Math.E, -0.5 * Math.Pow((i + precision), 2))) / 2.0;
            }
            result *= 1.0 / Math.Pow(2 * Math.PI, 0.5);

            if (value < 0)
                return -result;
            return result;
        }

        /// <summary>
        /// Сортировка листа
        /// </summary>
        /// <returns></returns>
        List<int> sortList()
        {
            List<int> copyNumbers = new List<int>();
            for (int i = 0; i < this.Count; i++)
            {
                copyNumbers.Add(this[i]);
            }

            //Sorting
            int temp;
            for (int i = 0; i < copyNumbers.Count - 1; i++)
            {
                for (int j = i + 1; j < copyNumbers.Count; j++)
                {
                    if (copyNumbers[i] > copyNumbers[j])
                    {
                        temp = copyNumbers[i];
                        copyNumbers[i] = copyNumbers[j];
                        copyNumbers[j] = temp;
                    }
                }
            }
            return copyNumbers;
        }
    }
}
