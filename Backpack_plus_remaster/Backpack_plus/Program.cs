﻿using System;
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
            example = new DataCase(250, 5001, 500000);
            //example.PrintData();

            Console.WriteLine("Прямой жадный алгоритм: ");
            System.Diagnostics.Stopwatch stopwatchGreed = new System.Diagnostics.Stopwatch();
            stopwatchGreed.Start();
            example.GreedyAlg();
            stopwatchGreed.Stop();
            TimeSpan tsGreed = stopwatchGreed.Elapsed;
            string elapsedTimeGreed = String.Format("{0:00}:{1:00}:{2:00}.{3:00}\n", tsGreed.Hours, tsGreed.Minutes, tsGreed.Seconds, tsGreed.Milliseconds / 10);
            Console.WriteLine("RunTime: " + elapsedTimeGreed);

            Console.WriteLine("Пропорциональный жадный алгоритм: ");
            System.Diagnostics.Stopwatch stopwatchProportionalGreed = new System.Diagnostics.Stopwatch();
            stopwatchProportionalGreed.Start();
            example.GreedProportional();
            stopwatchProportionalGreed.Stop();
            TimeSpan tsProportionalGreed = stopwatchProportionalGreed.Elapsed;
            string elapsedTimeProportionalGreed = String.Format("{0:00}:{1:00}:{2:00}.{3:00}\n", tsProportionalGreed.Hours, tsProportionalGreed.Minutes, tsProportionalGreed.Seconds, tsProportionalGreed.Milliseconds / 10);
            Console.WriteLine("RunTime: " + elapsedTimeProportionalGreed);

            Console.WriteLine("Динамическое программирование:");
            System.Diagnostics.Stopwatch stopwatchDyn = new System.Diagnostics.Stopwatch();
            stopwatchDyn.Start();
            example.DynamicProg();
            stopwatchDyn.Stop();
            TimeSpan tsDyn = stopwatchDyn.Elapsed;
            string elapsedTimeDyn = String.Format("{0:00}:{1:00}:{2:00}.{3:00}\n", tsDyn.Hours, tsDyn.Minutes, tsDyn.Seconds, tsDyn.Milliseconds / 10);
            Console.WriteLine("RunTime: " + elapsedTimeDyn);

            for (int i = 0; i < 5; i++)
            {
                Console.WriteLine("Генетический алгоритм: ");
                System.Diagnostics.Stopwatch stopwatchGen = new System.Diagnostics.Stopwatch();
                stopwatchGen.Start();
                example.GeneticAlg(stopwatchDyn);
                stopwatchGen.Stop();
                TimeSpan tsGen = stopwatchGen.Elapsed;
                string elapsedTimeGen = String.Format("{0:00}:{1:00}:{2:00}.{3:00}\n", tsGen.Hours, tsGen.Minutes, tsGen.Seconds, tsGen.Milliseconds / 10);
                Console.WriteLine("RunTime: " + elapsedTimeGen);
            }

            //Console.WriteLine("Полный допустимый перебор: ");
            //System.Diagnostics.Stopwatch stopwatchBrute = new System.Diagnostics.Stopwatch();
            //stopwatchBrute.Start();
            //example.BruteForce();
            //stopwatchBrute.Stop();
            //TimeSpan tsBrute = stopwatchBrute.Elapsed;
            //string elapsedTimeBrute = String.Format("{0:00}:{1:00}:{2:00}.{3:00}\n", tsBrute.Hours, tsBrute.Minutes, tsBrute.Seconds, tsBrute.Milliseconds / 10);
            //Console.WriteLine("Brute RunTime " + elapsedTimeBrute);
        }
    }

    class Management
    {
        public int[] stratedy;
        public int profit;
        static int length;

        public Management()
        {
            stratedy = new int[length];
        }

        public Management(int pos, int prof)
        {
            stratedy = new int[length];
            stratedy[0] = pos;
            profit = prof;
        }

        public Management(Management a)
        {
            stratedy = new int[length];
            for (int i = 0; i < length; i++)
                stratedy[i] = a.stratedy[i];
            profit = a.profit;
        }

        public static void SetN(int a)
        {
            length = a;
        }

        public void Copy(Management a)
        {
            for (int i = 0; i < length; i++)
                stratedy[i] = a.stratedy[i];
            profit = a.profit;
        }

        public int Print(int flag = 0)
        {
            if (flag == 0)
                return profit;
            else
            {
                Console.Write("(");
                for (int i = 0; i < length - 1; i++)
                    Console.Write(stratedy[i] + ", ");
                Console.WriteLine(stratedy[length - 1] + ");");
                return profit;
            }
        }
    }

    class Individ
    {
        int[] genotype;
        int resources;
        int profit;
        double fitness;
        static int NoC;
        static int NoAS;
        static int[,] tableOfOffers;
        static int[] sizeOfAttachments;
        static int maxResources;

        public static void StaticInit(int noc, int noas, int r, int[,] table, int[] attachments)
        {
            NoC = noc;
            NoAS = noas;
            maxResources = r;
            tableOfOffers = table;
            sizeOfAttachments = attachments;
        }

        public Individ()
        {
            genotype = new int[NoC];
        }

        public Individ(Random rnd, int flag=0)                              //Коснтруктор со случайной генерацией особи
        {
            genotype = new int[NoC];
            RandInit(rnd, flag);
        }

        public Individ(Individ a, Individ b, int pos)           //Crossover
        {
            genotype = new int[NoC];
            for (int i = 0; i < pos; i++)
                genotype[i] = a.genotype[i];
            for (int i = pos; i < NoC; i++)
                genotype[i] = b.genotype[i];
            UpdateFitness();
        }

        public void RandInit(Random rnd, int flag)                          //Метод случайной генерации особи
        {
            int rangeRandom = NoAS;
            switch (flag)
            {
                case 1:
                    for (int i = 0; i < NoC; i++)
                    {
                        int rand = rnd.Next(rangeRandom);
                        genotype[i] = rand;
                        rangeRandom -= rand;
                    }
                    break;
                case 2:
                    for (int i = NoC - 1; i >= 0; i--)
                    {
                        int rand = rnd.Next(rangeRandom);
                        genotype[i] = rand;
                        rangeRandom -= rand;
                    }
                    break;
                case 3:
                    for (int i = 0; i < NoC; i++)
                        genotype[i] = rnd.Next(NoAS / 2);
                    break;
                default:
                    for (int i = 0; i < NoC; i++)
                        genotype[i] = rnd.Next(NoAS);
                    break;
            }
            UpdateFitness();
        }

        public void UniformCrossover(Individ firstParent, Individ secondParent, Random rnd)
        {
            for (int i = 0; i < NoC; i++)
            {
                if (rnd.Next(2) == 0)
                    genotype[i] = firstParent.genotype[i];
                else
                    genotype[i] = secondParent.genotype[i];
            }
            UpdateFitness();
        }

        public Individ(Individ a)
        {
            genotype = new int[NoC];
            for (int i = 0; i < NoC; i++)
                genotype[i] = a.genotype[i];
            UpdateFitness();
        }

        public void Set(int pos, int value)
        {
            genotype[pos] = value;
            UpdateFitness();
        }

        public double Mutation(Random rnd)
        {
            double oldFitness = fitness;
            genotype[rnd.Next(NoC)] = rnd.Next(NoAS);
            UpdateFitness();
            return (fitness - oldFitness);
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

        public void Print(int flag = 0)
        {
            if (flag == 0)
                Console.WriteLine("Resources: " + resources + "; Profit: " + profit + "; Fitness: {0:0.0000}", fitness);
            else
            {
                for (int i = 0; i < NoC; i++)
                {
                    Console.Write("{0:000} ", sizeOfAttachments[genotype[i]]);
                }
                Console.WriteLine(" Resources: " + resources + "; Profit: " + profit + "; Fitness: {0:0.0000}", fitness);
            }
        }

        private void UpdateFitness()
        {
            int p = 0;
            int r = 0;
            for (int i = 0; i < NoC; i++)
            {
                p += tableOfOffers[i, genotype[i]];
                r += sizeOfAttachments[genotype[i]];
            }
            profit = p;
            resources = r;
            if (resources > maxResources)
            {
                double r2 = Math.Pow(resources, 2);
                double max2 = Math.Pow(maxResources, 2);
                double temp = r2 - max2;
                double sq = Math.Pow(temp, 0.5);
                fitness = (double)(profit - sq);      //Если перерасход ресурсов приспосеобленность уменьшается
                if (fitness < 0)
                    fitness = (double)maxResources / 10;
            }
            else
                fitness = (double)(profit + (maxResources - resources) / 2);     //Если ресурсы использованы не все, то приспособленность 
                                                                                 //fitness = (double)(profit - Math.Pow(Math.Pow(resources, 3) - Math.Pow(maxResources, 3), 1 / 3.0));
                                                                                 //увеличивается на половину разницы (теоретическая прибыль)
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

        private void RandInitValues()
        {
            int step = resources / (numberOfAttachmentSizes - 1);
            for (int i = 0; i < numberOfAttachmentSizes; i++)
                sizeOfAttachment[i] = i * step;
            Random rnd = new Random();
            for (int i = 0; i < numberOfCompany; i++)
                for (int j = 1; j < numberOfAttachmentSizes; j++)
                    tableOfOffers[i, j] = sizeOfAttachment[j] * 9 / 10 + rnd.Next((step + sizeOfAttachment[j]) * 2 / 10);
        }

        private int Method(int r, out int[] U, int depth = 0)
        {
            U = new int[numberOfCompany];
            if (depth < numberOfCompany)
            {
                int maxResult = 0;
                int maxU = 0;
                int lim = r / sizeOfAttachment[1];
                for (int i = 0; i <= lim; i++)
                {
                    int[] tempU = new int[numberOfCompany];
                    int temp = tableOfOffers[depth, i] + Method(r - sizeOfAttachment[i], out tempU, depth + 1);
                    if (temp > maxResult)
                    {
                        maxResult = temp;
                        maxU = sizeOfAttachment[i];
                        for (int j = 0; j < numberOfCompany; j++)
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

        public DataCase(int a, int b, int r)
        {
            rnd = new Random();
            if (r % (b - 1) == 0)
            {
                MemoryAllocation(a, b, r);
                RandInitValues();
            }
            else
            {
                Console.WriteLine("Недопустимые значения, инициализация по умолчанию!\n");
                MemoryAllocation();
                BaseInitValues();  //Метод инициализации проверочными значениями
            }
            Individ.StaticInit(numberOfCompany, numberOfAttachmentSizes, resources, tableOfOffers, sizeOfAttachment);
        }
        
        public void BruteForce()
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

        public void DynamicProg()
        {
            Management result = DynamicProgramming();
            Console.Write("Максимальная прибыль в размере " + result.profit + " достигается на наборе ");
            result.Print();
        }

        private Management DynamicProgramming()
        {
            Management.SetN(numberOfCompany);
            Management[,] dynamicTable = new Management[2,numberOfAttachmentSizes];
            for (int i = 0; i < numberOfAttachmentSizes; i++)
                dynamicTable[0, i] = new Management(i, tableOfOffers[0, i]);
            for (int i = 0; i < numberOfAttachmentSizes; i++)
                dynamicTable[1, i] = new Management();

            for (int i = 1; i < numberOfCompany - 1; i++)               //i - номер рассматриваемой компании       
            {
                int curr = i % 2;
                int prev = (i + 1) % 2;
                for (int j = 0; j < numberOfAttachmentSizes; j++)       //j - номер заполняемой ячейки (по доступному размеру вложений)
                {
                    Management max = new Management(dynamicTable[prev, j]);
                    for (int k = 1; k <= j; k++)                         
                    {
                        int newProfit = dynamicTable[prev, j - k].profit + tableOfOffers[i, k];
                        if (max.profit < newProfit)
                        {
                            max.Copy(dynamicTable[prev, j - k]);
                            max.stratedy[i] = k;
                            max.profit = newProfit;
                        }
                    }
                    dynamicTable[curr, j].Copy(max);
                }
            }
            int last = numberOfCompany % 2;
            Management result = new Management(dynamicTable[last, numberOfAttachmentSizes - 1]);
            for (int i = 1; i < numberOfAttachmentSizes; i++)
            {
                int newProfit = tableOfOffers[numberOfCompany - 1, i] + dynamicTable[last, numberOfAttachmentSizes - i - 1].profit;
                if (result.profit < newProfit)
                {
                    result.Copy(dynamicTable[last, numberOfAttachmentSizes - i - 1]);
                    result.stratedy[numberOfCompany - 1] = i;
                    result.profit = newProfit;
                }
            }
            return result;
        }

        private void GreedyPopulation(Individ[] population, int n)             //С помощью жадного алгоритма со сдвигом генерирует n особей для стартовой популяции
        {
            int i = 0;
            while (i < n)
            {
                switch (i / numberOfCompany)
                {
                    case 0:
                        population[i] = RightUnitGreegy(i);
                        break;
                    case 1:
                        population[i] = LeftUnitGreedy(i);
                        break;
                    default:
                        //population[i] = RightUnitGreegy(i);
                        population[i].RandInit(rnd, i % 4);
                        break;
                }
                i++;
            }
        }

        private void RandPopulation(Individ[] population, int n)
        {
            for (int i = 0; i < n; i++)
                population[i].RandInit(rnd, n % 4);
        }

        public void GeneticAlg(System.Diagnostics.Stopwatch record)        //Отвратительно работает, когда количество компаний больше количества предложений, т.е. когда в оптимальном генотипе есть множество нулей
        {
            System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();
            int populationSize = numberOfAttachmentSizes;
            Individ[] population = new Individ[populationSize];
            for (int i = 0; i < populationSize; i++)
                population[i] = new Individ();
            //RandPopulation(population, populationSize);                     //Случайная генерация стартовой популяции
            GreedyPopulation(population, populationSize);                   //Генерация стартовой популяции жадными алгоритмами со сдвигом

            //for (int i = 0; i < populationSize; i++)
            //    population[i].Print(1);

            
            //int testcount = 0;
            int iterationCounter = 0;
            while ((stopwatch.ElapsedMilliseconds < record.ElapsedMilliseconds / 2) /*| testcount++ < 100*/)
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
                    if (rnd.Next(2) == 0)
                        child[i] = SinglepointCrossover(population, populationSize);
                    else
                        child[i] = UniformCrossover(population, populationSize);
                    sumFitness += child[i].GetFitness();
                }



                for (int i = 0; i < populationSize / 10; i++)
                    sumFitness += child[rnd.Next(populationSize, populationSize * 3)].Mutation(rnd);        //Мутации производятся только среди потомков

                double averageFitness = sumFitness / (populationSize * 3);
                //Console.WriteLine(sumFitness);
                //population = WheelRotation(child, populationSize, sumFitness);
                //population = WheelRotation(child, populationSize, Convert.ToInt32(Math.Ceiling(sumFitness)));     //Старая версия
                //Console.WriteLine("Потомки:");

                //for (int i = 0; i < populationSize * 2; i++)
                //    child[i].Print();



                population = ProportionalSelection(population, child, populationSize, averageFitness);

                //Console.WriteLine("Суммарная приспособленность равна: " + sumFitness);
                //Console.WriteLine("Средняя приспособленность: " + averageFitness);                            //Округляет вверх всегда
                //for (int i = 0; i < populationSize; i++)
                //    population[i].Print();
                iterationCounter++;
            }
            stopwatch.Stop();
            Console.WriteLine("Количество итераций: {0}\nНаилучшее найденное решение: ", iterationCounter);
            population[FindBest(population, populationSize)].Print();

            //for (int i = 0; i < populationSize; i++)
            //    Console.WriteLine(population[i].GetFitness());
        }

        private Individ SinglepointCrossover(Individ[] population, int populationSize)
        {
            Individ a = new Individ(population[rnd.Next(populationSize)], population[rnd.Next(populationSize)], rnd.Next(1, numberOfCompany));
            return a;
        }

        private Individ UniformCrossover(Individ[] population, int populationSize)
        {
            Individ a = new Individ();
            int firstParent = rnd.Next(populationSize);
            int secondParent = rnd.Next(populationSize);
            a.UniformCrossover(population[firstParent], population[secondParent], rnd);
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

        private Individ[] WheelRotation(Individ[] population, int populationSize, int sumFitness)       
        {
            Individ[] newPopulation = new Individ[populationSize];
            for (int i = 0; i < populationSize; i++)
            {
                int rangePoint = rnd.Next(sumFitness);                    
                double topRange = 0;                                
                for (int j = 0; j < populationSize * 3; j++)
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

        private Individ RightUnitGreegy(int n = 0)                  //Прямой удельный жадный алгоритм, начинающий поиск с n-ой компании
        {
            Individ data = new Individ();
            int limit = n + numberOfCompany - 1;
            int counter = numberOfAttachmentSizes - 1;
            int currentPos;
            while((counter > 0) && (n < limit))
            {
                double maxGain = 0;
                int pos = 0;
                currentPos = n % numberOfCompany;
                for (int j = 1; j < counter + 1; j++)
                {
                    double gain = (double)tableOfOffers[currentPos, j] / sizeOfAttachment[j];
                    if (gain >= maxGain)
                    {
                        maxGain = gain;
                        pos = j;
                    }
                }
                data.Set(currentPos, pos);
                counter -= pos;
                n++;
            }
            data.Set(limit % numberOfCompany, counter);
            return data;
        }

        private Individ LeftUnitGreedy(int n = 0)                   //Обратный удельный жадный алгоритм, начинающий поиск с n-ой компании
         {
            Individ data = new Individ();
            int counter = numberOfAttachmentSizes - 1;
            n %= numberOfCompany;
            int currentPos = (n - 1 + numberOfCompany) % numberOfCompany;
            while((counter > 0) && (currentPos!=n))
            {
                double maxGain = 0;
                int pos = 0;
                for (int j = 1; j < counter + 1; j++)
                {
                    double gain = (double)tableOfOffers[currentPos, j] / sizeOfAttachment[j];
                    if(gain >= maxGain)
                    {
                        maxGain = gain;
                        pos = j;
                    }
                }
                data.Set(currentPos, pos);
                counter -= pos;
                currentPos = (currentPos - 1 + numberOfCompany) % numberOfCompany;
            }
            data.Set(currentPos, counter);
            return data;
        }

        public void GreedProportional()
        {
            Individ[] solutionPair = new Individ[2];
            for (int i = 0; i < 2; i++)
                solutionPair[i] = new Individ();
            solutionPair[0] = RightUnitGreegy();
            solutionPair[1] = LeftUnitGreedy();
            solutionPair[0].Print();
            solutionPair[1].Print();
        }

        public void GreedyAlg()
        {
            Individ[] solutionPair = new Individ[2];
            for (int i = 0; i < 2; i++)
                solutionPair[i] = new Individ();
            int counter = numberOfAttachmentSizes;
            for (int i = 0; i < numberOfCompany; i++)
            {
                int maxGain = -sizeOfAttachment[numberOfAttachmentSizes-1];
                int pos = -1;
                for (int j = 0; j < counter; j++)
                {
                    int gain = tableOfOffers[i, j] - sizeOfAttachment[j];
                    if(gain >= maxGain)
                    {
                        maxGain = gain;
                        pos = j;
                    }
                }
                solutionPair[0].Set(i, pos);
                counter -= pos;
            }

            counter = numberOfAttachmentSizes;
            for (int i = numberOfCompany-1; i >= 0; i--)
            {
                int maxGain = -sizeOfAttachment[numberOfAttachmentSizes - 1];
                int pos = -1;
                for (int j = 0; j < counter; j++)
                {
                    int gain = tableOfOffers[i, j] - sizeOfAttachment[j];
                    if (gain >= maxGain)
                    {
                        maxGain = gain;
                        pos = j;
                    }
                }
                solutionPair[1].Set(i, pos);
                counter -= pos;
            }
            solutionPair[0].Print();
            solutionPair[1].Print();
        }

        public void RandSolution()
        {
            int numberOfSolutions = 5;
            Individ[] RandSolution = new Individ[numberOfSolutions];
            Console.WriteLine("Случайные решения: ");
            for (int i = 0; i < numberOfSolutions; i++)
            {
                RandSolution[i] = new Individ(rnd, rnd.Next(1, 2));
                RandSolution[i].Print();
            }
            Console.WriteLine("Конец случайных решений!");

        }
    }
}
