using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task_7
{
    class Element : IComparable<Element>
    {
        public List<int> number; //количество задействованных символов 
        public double info; //частота появления символа 
        public Element left, right; //для дерева, левое и правое поддеревья 

        public Element() //конструктор без параметров 
        {
            number = null;
            info = 0;
            left = null;
            right = null;
        }

        public Element(int numb, double inf, Element l, Element r) //конструктор для задания элемента-символа 
        {
            number = new List<int> { numb };
            info = inf;
            left = l;
            right = r;
        }

        public int CompareTo(Element e) //компаратор для своей сортировки 
        {
            if (info > e.info)
                return 1;
            if (info < e.info)
                return -1;
            else
            {
                if (number.Count > e.number.Count)
                    return 1;
                if (number.Count < e.number.Count)
                    return -1;
                else return 0;
            }
        }

    }

    class Program
    {
        static List<string> codes = new List<string>(0); //все кодовые слова 
        static double Sum(Element[] arr) //функция суммы вероятностей, для проверки 
        {
            double temp = 0;
            for (int i = 0; i < arr.Length; i++)
            {
                temp += arr[i].info;
            }
            return temp;
        }

        static List<int> Unite(List<int> first, List<int> second) //функция объеденения списков 
        {
            List<int> temp = new List<int>(0);
            for (int i = 0; i < first.Count; i++)
            {
                temp.Add(first[i]);
            }
            for (int i = 0; i < second.Count; i++)
            {
                temp.Add(second[i]);
            }
            return temp;
        }

        static void Run(Element e, string code) //функция прохода по дереву 
        {
            if (e.left != null)
            {
                Run(e.left, code + "1"); //если идём в правое поддерево 
            }
            if (e.right != null)
            {
                Run(e.right, code + "0"); //если идём в левое поддерево 
            }
            if ((e.left == null) && (e.right == null)) //если вершина без потомков, то добавляем кодовое слово в коллекцию 
            {
                codes.Add(code);
            }
            return;
        }

        static void Main(string[] args)
        {
            bool enter;
            bool okSum;
            Element[] freq;
            do //проверка на корректный ввод 
            {
                enter = false;
                okSum = false;
                Console.WriteLine("Введите частоты через пробел в формате x,xx..."); //ввод вероятностей 
                string[] spl = Console.ReadLine().Split();
                freq = new Element[spl.Length];
                for (int i = 0; i < spl.Length; i++)
                {
                    try
                    {
                        freq[i] = new Element(i, Convert.ToDouble(spl[i]), null, null);
                        enter = true;
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("Некорректный ввод");
                        enter = false;
                        break;
                    }
                }
                if (enter)
                {
                    okSum = Sum(freq) == 1; //проверка, что сумма вероятностей равна 1 
                    if (!okSum)
                        Console.WriteLine("Некорректный ввод");
                }
            } while ((!enter) || (!okSum));
            while (freq.Length != 1)
            {
                Array.Sort(freq); //сортировка массива 
                Array.Reverse(freq); //обращение массива 
                Element min = freq[freq.Length - 1], premin = freq[freq.Length - 2]; //нахождение первого минимального и второго минимального 
                Element[] freqNext = new Element[freq.Length - 1];
                for (int i = 0; i < freqNext.Length; i++) //задание нового массива с количеством элементов на 1 меньше 
                {
                    freqNext[i] = new Element();
                }
                for (int i = 0; i < freqNext.Length; i++)
                {
                    freqNext[i].info = freq[i].info; //копируем информацию 
                    freqNext[i].number = freq[i].number;
                    freqNext[i].left = freq[i].left;
                    freqNext[i].right = freq[i].right;
                    if (i == freqNext.Length - 1)
                    {
                        freqNext[i].info = min.info + premin.info; //объединяем два элемента в один, строится дерево 
                        freqNext[i].number = Unite(min.number, premin.number);
                        freqNext[i].left = min;
                        freqNext[i].right = premin;
                    }
                }
                freq = freqNext; //замена старого массива на созданный 
            }

            Run(freq[0], ""); //обход дерева, заполнение списка кодовых слов 

            for (int i = 0; i < codes.Count; i++) //показ кодовых слов в консоли по алфавиту 
            {
                Console.WriteLine(codes[codes.Count - 1 - i]);
            }

        }
    }
}
