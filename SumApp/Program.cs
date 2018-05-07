using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SumApp
{
    class Program
    {
        static void Main(string[] args)
        {
            List<int> array = new List<int>();

            //инициализация параметров
            Params param = new Params();
            param.SetParams();

            //генерация массива по параметрам
            array = ArrayGen(param);
            //array = new List<int> { 1, 1, 2, 1, 1, 0, 1 };

            //вывод сгенерированного массива
            Console.WriteLine("Исходный массив:");
            for (int i = 0; i < array.Count; i++)
            {
                Console.Write("{0} ", array[i]);
            }

            //получение коллекции пар чисел
            List<MatchItem> matchCollection = GetMatchCollection(array, param.sum);

            //вывод коллекции совпадений
            Console.WriteLine();
            Console.WriteLine();
            if (matchCollection.Count == 0)
            {
                Console.WriteLine("Нет совпадений.");
            }
            else
            {
                Console.WriteLine("Найденные пары:");
                for (int i = 0; i < matchCollection.Count; i++)
                {
                    Console.Write("({0}, {1})", matchCollection[i].valA, matchCollection[i].valB);
                }
            }
            Console.ReadKey();
        }

        //логика алгоритма: 
        //- ставим указатель на нулевой элемент коллекции и ищем ему пару.
        //- если пара найдена, то добавляем эту пару в коллекцию пар, при этом:
        //      - удаляем пару из исходной коллекции, чтобы не попалась снова
        //      - и двигаем указатель вправо, чтобы не делать лишнего удаления из коллекции
        //- если пара не найдена, то просто двигаем указатель
        //и так пока не упремся в конец коллекции
        //
        //алгоритм можно оптимизироавть отсортировав исходную коллекцию, после чего использовать бинарный поиск для пары
        //(но это будет эффективно только с большим объемом данных)
        public static List<MatchItem> GetMatchCollection(List<int> array, int sum)
        {
            List<MatchItem> res = new List<MatchItem>();

            int pos = 0;  //указатель на текущий элемент
            while (pos < array.Count - 1)
            {
                //posVal и iVal предназначены для некоторой оптимизации,
                //поскольку выделение памяти и присвоение значения менее ресурсозатратные, чем обращение к элементу списка(List)
                //выглядит менее привлекательно, зато работает чуточку быстрее)) 
                //(конечно, на малых объемах эта оптимизация незаметна)
                int posVal = array[pos];  //значение текущего элемента
                int matchKey = sum - posVal;  //вычисляем значение пары для текущего элемента

                for (int i = pos + 1; i < array.Count; i++)
                {
                    int iVal = array[i];  //значение искомого элемента
                    if (iVal == matchKey)
                    {
                        //если пара найдена, то добавляем в коллекцию пар, и удаляем пару текущего элемента из исходной коллекции
                        res.Add(new MatchItem() { valA = posVal, valB = iVal });   
                        array.RemoveAt(i);
                        break;
                    }
                }
                //двигаем указатель вправо
                pos++;  
            }

            return res;
        }

        //генерация исходной коллекции по парам етрам
        public static List<int> ArrayGen(Params param)
        {
            List<int> res = new List<int>();
            Random rnd = new Random();

            for (int i = 0; i < param.count; i++)
            {
                res.Add(rnd.Next(param.from, param.to));
            }

            return res;
        }

    }

    //элемент коллекции пар
    class MatchItem
    {
        public int valA;
        public int valB;
    }

    //параметры генерации и искомой суммы
    class Params
    {
        public int from;    //минимальный элемент генерации
        public int to;      //максимальный элемент генерации
        public int count;   //количество генерируемых элементов
        public int sum;     //искомая сумма

        //установка параметров 
        public void SetParams()
        {
            Console.Write("Введите минимальное значение генерации: ");
            while (!int.TryParse(Console.ReadLine(), out from))
            {
                Console.WriteLine("Неверный формат.");
                Console.Write("Введите минимальное значение генерации: ");
            }

            Console.Write("Введите максимальное значение генерации: ");
            while (!int.TryParse(Console.ReadLine(), out to) || from > to)
            {
                if (from > to)
                    Console.WriteLine("Минимальное значение не может быть больше максимального.");
                else
                    Console.WriteLine("Неверный формат.");
                Console.Write("Введите максимальное значение генерации: ");
            }

            Console.Write("Введите количество элементов: ");
            while (!int.TryParse(Console.ReadLine(), out count) || count < 2)
            {
                if (count < 2)
                    Console.WriteLine("Количество элементов должно быть больше 1.");
                else
                    Console.WriteLine("Неверный формат.");
                Console.Write("Введите количество элементов: ");
            }

            Console.Write("Введите искомую сумму: ");
            while (!int.TryParse(Console.ReadLine(), out sum))
            {
                Console.WriteLine("Неверный формат.");
                Console.Write("Введите искомую сумму: ");
            }

            Console.Clear();
        }
    }

}
