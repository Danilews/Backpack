using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backpack_plus
{
    class Program
    {
        static void Main(string[] args)
        {
            DataCase example;
            example = new DataCase(9, 31, 2100);
            example.PrintData();

            System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();
            example.DynamicProg();
            //System.Threading.Thread.Sleep(10000);
            stopwatch.Stop();
            TimeSpan ts = stopwatch.Elapsed;

            // Format and display the TimeSpan value.
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                ts.Hours, ts.Minutes, ts.Seconds,
                ts.Milliseconds / 10);
            Console.WriteLine("RunTime " + elapsedTime);
            example.GeneticAlg();
            //Test
            //Individ test = new Individ(5,11);



            //Тест
            //Random rnd = new Random();
            //test.FullRand(rnd);
            //test.Print();
            //test.Mutation(rnd);
            //test.Mutation(rnd);
            //Тест
            //test.Print();            
        }
    }

    class Individ
    {
        bool[] code;
        int profit;
        int resources;
        double fitness;
        static int fullLength;
        static int NoC;
        static int NoAS;
        static int shortLength;
        static int[,] tableOfOffers;
        static int[] sizeOfAttachments;
        static int maxResources;
        public Individ(Random rnd)                          //Конструктор создания особи со случайным допустимым генотипом
        {            
            code = new bool[fullLength];
            FullRand(rnd);
            UpdateFitness();
        }

        public Individ(Individ a, Individ b, int pos)       //Конструктор особи при кроссовере
        {
            code = new bool[fullLength];
            int codePos = pos * shortLength;
            for (int i = 0; i < codePos; i++)
                code[i] = a.code[i];
            for (int i = codePos; i < fullLength; i++)
                code[i] = b.code[i];
            UpdateFitness();
        }

        public Individ(Individ a)
        {
            code = new bool[fullLength];
            for (int i = 0; i < fullLength; i++)
                code[i] = a.code[i];
            profit = a.profit;
            resources = a.resources;
            fitness = a.fitness;
        }

        public double GetFitness()
        {
            return fitness;
        }

        public int GetProfit()
        {
            return profit;
        }

        public bool ResourcesCheck()
        {
            if (resources <= maxResources)
                return true;
            else
                return false;
        }

        private void UpdateFitness()                         //Обновляет приспособленность и количество ресурсов 
        {                                                    //РАЗДЕЛИТЬ НА ПОЛНОЕ ОБНОВЛЕНИЕ И ЧАСТИЧНОЕ!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            int p = 0;
            int r = 0;
            for (int i = 0; i < NoC; i++)
            {
                int pos = BoolToInt(i);
                p += tableOfOffers[i, pos];
                r += sizeOfAttachments[pos];
            }
            profit = p;
            resources = r;
            //fitness = (double)profit - Math.Pow((resources - maxResources), 2)/maxResources;  //Очень похожа на рабочую
            //fitness = (double)((double)profit - (double)Math.Pow((Math.Pow(resources, 2) - Math.Pow(maxResources, 2)), 1 / 2));
            if (resources > maxResources)                                                                                    //Супер рабочка
            {
                fitness = (double)(profit - Math.Pow(resources * resources - maxResources * maxResources, 1 / 2.0));      //Если перерасход ресурсов приспосеобленность уменьшается
                if (fitness < 0)
                    fitness = 0;
            }
            else
                fitness = (double)(profit + (maxResources - resources) / 2);                                                //Если ресурсы использованы не все, то приспособленность 
            //fitness = (double)(profit - Math.Pow(Math.Pow(resources, 3) - Math.Pow(maxResources, 3), 1 / 3.0));         //увеличивается на половину разницы (теоретическая прибыль)
        }

        private int BoolToInt(int pos)
        {
            int exp = 1;
            int temp = 0;

            for (int i = 0; i < shortLength; i++)
            {
                if (code[pos * shortLength + i])
                    temp += exp;
                exp *= 2;
            }
            return temp;
        }

        public static void StaticInit(int noc, int noas, int r, int[,] table, int[] attachments)
        {
            NoC = noc;
            NoAS = noas--;
            int temp = 0;
            while (noas != 0)
            {
                noas /= 2;
                temp++;
            }
            shortLength = temp;
            fullLength = shortLength * NoC;
            maxResources = r;
            tableOfOffers = table;
            sizeOfAttachments = attachments;
        }

        public bool ValidCheck(int pos)
        {
            int temp = 0;
            int exp = 1;
            for (int i = pos * shortLength; i < (pos + 1) * shortLength; i++)
            {
                temp += code[i] ? exp : 0;
                exp *= 2;
            }
            //Test
            //Console.Write(temp + " ");
            //Test
            if (temp < NoAS)
                return true;
            else
                return false;
        }

        private void Rand(Random rnd, int pos)
        {
            do
            {
                for (int i = pos * shortLength; i < (pos + 1) * shortLength; i++)
                    if (rnd.Next() % 2 == 0)
                        code[i] = false;
                    else
                        code[i] = true;
            }
            while (!ValidCheck(pos));
        }

        private void MutationPos(Random rnd, int pos)
        {
            int r;
            while (true)
            {
                r = rnd.Next(shortLength);
                code[pos * shortLength + r] = !code[pos * shortLength + r];
                if (ValidCheck(pos))
                    return;
                else
                    code[pos * shortLength + r] = !code[pos * shortLength + r];
            }
        }

        public void Mutation(Random rnd)                //Если вероятность мутации прокнула
        {
            MutationPos(rnd, rnd.Next(NoC));
        }

        public void FullRand(Random rnd)                
        {
            for (int i = 0; i < NoC; i++)
            {
                Rand(rnd, i);
            }
        }

        public void Print()
        {
            for (int i = 0; i < NoC; i++)
            {
                for (int j = 0; j < shortLength; j++)
                    if (code[i * shortLength + j])
                        Console.Write("1 ");
                    else
                        Console.Write("0 ");
                Console.Write(" ");
            }
            Console.WriteLine(" Resources: " + resources + "; Profit: " + profit + "; Fitness: {0:0.0000}", fitness);

            //Test
            //for (int i = 0; i < NoC; i++)
            //{
            //    Console.Write("   ");
            //    ValidCheck(i);
            //    Console.Write("    ");
            //}
            ////Test

        }

    }

    class DataCase
    {
        int[,] tableOfOffers;
        int numberOfCompany;
        int numberOfAttachmentSizes;
        int[] sizeOfAttachment;
        int resources;
        int[] U;
        Random rnd;

        private void BaseInitValues()
        {
            tableOfOffers[0,0] = 0; tableOfOffers[0,1] = 50; tableOfOffers[0,2] = 120; tableOfOffers[0,3] = 140; tableOfOffers[0,4] = 150; tableOfOffers[0,5] = 200; tableOfOffers[0,6] = 250;
            tableOfOffers[1,0] = 0; tableOfOffers[1,1] = 60; tableOfOffers[1,2] = 130; tableOfOffers[1,3] = 140; tableOfOffers[1,4] = 130; tableOfOffers[1,5] = 160; tableOfOffers[1,6] = 200;
            tableOfOffers[2,0] = 0; tableOfOffers[2,1] = 30; tableOfOffers[2,2] = 60; tableOfOffers[2,3] = 100; tableOfOffers[2,4] = 130; tableOfOffers[2,5] = 200; tableOfOffers[2,6] = 250;
            tableOfOffers[3,0] = 0; tableOfOffers[3,1] = 40; tableOfOffers[3,2] = 100; tableOfOffers[3,3] = 110; tableOfOffers[3,4] = 120; tableOfOffers[3,5] = 160; tableOfOffers[3,6] = 220;
            for (int i = 0; i < numberOfAttachmentSizes; i++)
                sizeOfAttachment[i] = i * 50;
        }

        private void RandInitValues() //Рандомная инициализация, мало тестов, но вроде работает!)
        {
            int step = resources / (numberOfAttachmentSizes - 1);
            for (int i = 0; i < numberOfAttachmentSizes; i++)
                sizeOfAttachment[i] = i * step;
            Random rnd = new Random();
            for (int i = 0; i < numberOfCompany; i++)
                for (int j = 1; j < numberOfAttachmentSizes; j++)
                    //tableOfOffers[i, j] = sizeOfAttachment[j] * 3 / 4 + rnd.Next(sizeOfAttachment[j] / 2);
                    tableOfOffers[i, j] = sizeOfAttachment[j] * 9 / 10 + rnd.Next((step + sizeOfAttachment[j]) * 2 / 10);
        }

        //НУЖНО СОХРАНЯТЬ ОТДЕЛЬНЫЕ У ДЛЯ КАЖДОЙ ВЕТКИ!!!!!!!!!!!!!!!!!!!!!!!!!!!! Done
        private int Method(int r, out int[] U, int depth = 0)
        {
            U = new int[numberOfCompany];
            if (depth < numberOfCompany)
            {
                int maxResult = 0;
                int maxU = 0;
                int lim = r / sizeOfAttachment[1];
                for (int i = 0; i <= lim; i++) //numberOfAttachmentSizes) && (sizeOfAttachment[i] <= r); i++)//     Убрать второе условие переписав православно; Upd: Вроде, Done
                {
                    int[] tempU = new int[numberOfCompany];
                    int temp = tableOfOffers[depth, i] + Method(r - sizeOfAttachment[i], out tempU, depth + 1);
                    if (temp > maxResult)
                    {
                        maxResult = temp;
                        maxU = sizeOfAttachment[i];
                        for (int j = 0; j < numberOfCompany; j++) //ПЕРЕДАЧА УУУУУУУУУУУУУУУУУУУУУУУУУУУУУУУУУУ Done
                            U[j] = tempU[j];
                        U[depth] = maxU;
                    }                    
                }
                return maxResult;
            }
            else
                return 0;
        }

        public void PrintData()
        {
            for (int i = 0; i < numberOfAttachmentSizes; i++)
                Console.Write(sizeOfAttachment[i] + " ");
            Console.WriteLine();
            for (int i = 0; i < numberOfCompany; i++)
            {
                for (int j = 0; j < numberOfAttachmentSizes; j++)
                    Console.Write(tableOfOffers[i,j] + " ");
                Console.WriteLine();
            }
        }

        private void MemoryAllocation(int a = 4, int b = 7, int r = 300)
        {
            numberOfCompany = a;
            numberOfAttachmentSizes = b;
            U = new int[numberOfCompany];
            sizeOfAttachment = new int[numberOfAttachmentSizes];
            tableOfOffers = new int[numberOfCompany, numberOfAttachmentSizes];
            resources = r;
        }

        public DataCase(int a, int b, int r) //   Конструктор //НАПИСАТЬ ПРОВЕРКУ НА ДОПУСТИМОСТЬ В РАНДОМ; UPD: похоже, Done!
        {
            rnd = new Random();
            if (r % (b - 1) == 0)
            {
                MemoryAllocation(a, b, r);
                RandInitValues();
            }
            else
            {
                Console.WriteLine("Недопустимые значения, инициализация по умолчанию!");
                MemoryAllocation();
                BaseInitValues();  //Метод инициализации проверочными значениями
            }
        }
        
        public void DynamicProg()
        {
            int maxProfit = Method(resources, out U);
            Console.Write("Максимальная прибыль в размере " + maxProfit + " достигается на наборе (");
            for (int i = 0; i < numberOfCompany - 1; i++)
            {
                Console.Write(U[i] + ", ");
            }
            Console.Write(U[numberOfCompany - 1] + ")");
            Console.WriteLine();
        }

        private Individ Crossover(Individ[] population, int populationSize)
        {
            Individ a = new Individ(population[rnd.Next(populationSize)], population[rnd.Next(populationSize)], rnd.Next(1, numberOfCompany));
            return a;
        }

        private int FindBest(Individ[] population, int populationSize)
        {
            int max = 0;
            int maxPos = 0;
            for (int i = 0; i < populationSize; i++)
                if (max < population[i].GetProfit() && population[i].ResourcesCheck())
                {
                    max = population[i].GetProfit();
                    maxPos = i;
                }
            return maxPos;
        }
        private Individ[] ProportionalSelection(Individ[] population, Individ[] child, int populationSize, double averageFitness) //Работает не очень, быстро сходится
        {                                                                               
            Individ[] newPopulation = new Individ[populationSize];
            int counter = 0;
            for (int i = 0; i < populationSize && counter < populationSize; i++)
                if (averageFitness < population[i].GetFitness())
                    newPopulation[counter++] = new Individ(population[i]);
            for (int i = 0; i < populationSize*2 && counter < populationSize; i++)
                if(averageFitness < child[i].GetFitness())
                    newPopulation[counter++] = new Individ(child[i]);
            while (counter < populationSize)
            {
                newPopulation[counter++] = new Individ(child[rnd.Next(populationSize * 2)]);
            }
            return newPopulation;
        }

        private Individ[] WheelRotation(Individ[] population, int populationSize, int sumFitness)       //Уже лучше, но не идеал
        {
            Individ[] newPopulation = new Individ[populationSize];
            for (int i = 0; i < populationSize; i++)
            {
                int rangePoint = rnd.Next(sumFitness);                    //Не нравится поиск, переписать, основываясь на лабе по ген алгоритмам
                double topRange = 0;                                //Конкретно не нравится интовый поиск по даблам
                for(int j = 0; j<populationSize*3; j++)
                {
                    topRange += population[j].GetFitness();
                    if (rangePoint < topRange)
                    {
                        newPopulation[i] = new Individ(population[j]);
                        break;
                    }
                }
                
            }
            return newPopulation;
        }

        public void GeneticAlg()        //Отвратительно работает, когда количество компаний больше количества предложений, т.е. когда в оптимальном генотипе есть множество нулей
        {
            int populationSize = numberOfAttachmentSizes * numberOfCompany;
            Individ.StaticInit(numberOfCompany, numberOfAttachmentSizes, resources, tableOfOffers, sizeOfAttachment);

            Individ[] population = new Individ[populationSize];
            for (int i = 0; i < populationSize; i++)
            {
                population[i] = new Individ(rnd);                           //Случайная генерация стартовой популяции
                
            }

            int counter = 0;
            while (counter++ < 100)
            {
                double sumFitness = 0;
                Individ[] child = new Individ[populationSize * 3];
                for (int i = 0; i < populationSize; i++)
                {
                    child[i] = new Individ(population[i]);
                    sumFitness += child[i].GetFitness();
                }
                
                for (int i = populationSize; i < populationSize * 3; i++)                    //Создание потомков путем одноточечного кроссовера
                {
                    child[i] = Crossover(population, populationSize);       //Можно вставить формулу, но так красивее
                    sumFitness += child[i].GetFitness();
                }
                double averageFitness = sumFitness / (populationSize * 3);


                for (int i = 0; i < populationSize / 10; i++)
                    child[rnd.Next(populationSize * 3)].Mutation(rnd);

                population = WheelRotation(child, populationSize, Convert.ToInt32(Math.Ceiling(sumFitness)));
                //Console.WriteLine("Потомки:");

                //for (int i = 0; i < populationSize * 2; i++)                
                //    child[i].Print();



                //population = ProportionalSelection(population, child, populationSize, averageFitness);

                Console.WriteLine("Суммарная приспособленность равна: " + sumFitness);
                Console.WriteLine("Средняя приспособленность: " + averageFitness);                            //Округляет вверх всегда
                //for (int i = 0; i < populationSize; i++)
                //    population[i].Print();
            }
            for (int i = 0; i < populationSize; i++)
                population[i].Print();
            Console.WriteLine("Наилучшее найденное решение: ");
            population[FindBest(population, populationSize)].Print();
        }



        //public void GreedyAlg()
        //{
        //    //double[,] relativeProfit = new double[numberOfCompany, numberOfAttachmentSizes];
        //    int[] relativeProfit = new int[numberOfCompany];
        //    for (int i = 0; i < numberOfCompany; i++)
        //        for (int j = 0; j < numberOfAttachmentSizes; j++)
        //            relativeProfit[i] =
        //}
    }
}
// To Do:

    //ВАЖНО!!!!!!!!!! ИЗМЕНИТЬ РАСЧЕТ ПРИСПОСОБЛЕННОСТИ, чтобы при увеличении перерасхода ресурсов сильно уменьшалась функция приспособленности

//  Кроссовер (Два предка)  Done
//  Копирование для родителя в новое поколение Done
//  
//  Потомков в 2 раза больше, чем родителей (Часть кроссовером, часть - копирование родителей) Done
//  Мутация (есть)
//  Отбор нового поколения Done
