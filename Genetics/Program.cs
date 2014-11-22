using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Genetics
{
    class Program
    {
        public static List<int[]> population;
        public static int[] input;
        public static int length;

        public static void init()
        {
            population = new List<int[]>();

            int[] person1 = GeneratePerson();
            int[] person2 = GeneratePerson();
            population.Add(person1);
            population.Add(person2);
        }

        public static int[] GeneratePerson()
        {
            int[] a = new int[length];
            Random random = new Random();
            for (int i = 0; i < length; i++)
                a[i] = (int)random.Next(100000)%2;

            return a;
        }

        public static int[] MutatePerson(int[] p)
        {
            Random random = new Random();
            int num = random.Next(p.Length - 1);
            p[num] = 1 - p[num];
            return p;
        }

        public static Tuple<int[], int[]> CrossPersons(int[] p1, int[] p2)
        {
            int length = p1.Length;
            int[] child1 = new int[length];
            int[] child2 = new int[length];
            Random random = new Random();
            var divPos = random.Next(length - 1);
            for (int i = 0; i < divPos; i++)
            {
                child1[i] = p1[i];
            }
            for (int i = divPos; i < length-1; i++)
            {
                child1[i] = p2[i];
            }

            for (int i = 0; i < length; i++)
                if (i%2 == 0)
                    child2[i] = p1[i];
                else
                    child2[i] = p2[i];

            return Tuple.Create<int[], int[]>(MutatePerson(child1), child2);
        }

        public static void evolute()
        {
            var childs = CrossPersons(population[0], population[1]);
            population.Add(childs.Item1);
            population.Add(childs.Item2);
            population.Sort((x, y) => subtr(x) < subtr(y)?-1:1);
            population.RemoveAt(2);
            population.RemoveAt(2);
        }

        public static int subtr(int[] person)
        {
            int s = 0;
            for (int i = 0; i < input.Length; i++)
                if (person[i] == 0)
                    s += input[i];
                else
                    s -= input[i];
            return Math.Abs(s);
        }

        static void Main(string[] args)
        {
            const int IterationCount = 100000;
            input = File.ReadAllText("input.txt").Split(' ').Select(s => int.Parse(s)).ToArray();
            length = input.Length;
            init();
            for (int i = 0; i < IterationCount; i++)
            {
                evolute();
            }
            var solution = population[0];
            var delta = subtr(solution);
            for (int i = 0; i < length; i++)
            {
                Console.Write(solution[i] + " ");
            }
            Console.WriteLine();
            Console.WriteLine(delta);
        }
    }
}
