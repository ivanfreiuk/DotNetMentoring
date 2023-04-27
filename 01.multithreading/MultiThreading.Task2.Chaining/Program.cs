/*
 * 2.	Write a program, which creates a chain of four Tasks.
 * First Task – creates an array of 10 random integer.
 * Second Task – multiplies this array with another random integer.
 * Third Task – sorts this array by ascending.
 * Fourth Task – calculates the average value. All this tasks should print the values to console.
 */
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MultiThreading.Task2.Chaining
{
    class Program
    {
        private static Random random = new Random();
        private static int min = 0;
        private static int max = 100;
        static void Main(string[] args)
        {
            Console.WriteLine(".Net Mentoring Program. MultiThreading V1 ");
            Console.WriteLine("2.	Write a program, which creates a chain of four Tasks.");
            Console.WriteLine("First Task – creates an array of 10 random integer.");
            Console.WriteLine("Second Task – multiplies this array with another random integer.");
            Console.WriteLine("Third Task – sorts this array by ascending.");
            Console.WriteLine("Fourth Task – calculates the average value. All this tasks should print the values to console");
            Console.WriteLine();

            var task = Task.Factory.StartNew(CreateArray)
                .ContinueWith(antecedent => MultiplyArray(antecedent.Result))
                .ContinueWith(antecedent => SortArray(antecedent.Result))
                .ContinueWith(antecedent => Average(antecedent.Result));
            task.Wait();
            Console.WriteLine("Completed");
            Console.ReadLine();
        }

        private static int[] CreateArray()
        {
            var numbers = Enumerable
                .Repeat(0, 10)
                .Select(i => random.Next(min, max))
                .ToArray();

            Print(numbers);
            
            return numbers;
        }

        private static int[] MultiplyArray(int[] numbers)
        {
            var randomNum = random.Next(min, max);
            for (int i = 0; i < numbers.Length; i++)
            {
                numbers[i] *= randomNum;
            }

            Print(numbers);

            return numbers;
        }

        private static int[] SortArray(int[] numbers)
        {
            Array.Sort(numbers);

            Print(numbers);

            return numbers;
        }

        private static double Average(int[] numbers)
        {
            var averageValue = numbers.Average(n => n);

            Console.WriteLine($"Average value : {averageValue}");

            return averageValue;
        }

        private static void Print(int[] numbers)
        {
            foreach (var number in numbers)
            {
                Console.Write($"{number} ");
            }
            Console.WriteLine();
        }
    }
}
