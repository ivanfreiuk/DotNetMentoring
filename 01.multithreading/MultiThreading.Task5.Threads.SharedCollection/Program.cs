/*
 * 5. Write a program which creates two threads and a shared collection:
 * the first one should add 10 elements into the collection and the second should print all elements
 * in the collection after each adding.
 * Use Thread, ThreadPool or Task classes for thread creation and any kind of synchronization constructions.
 */
using System;
using System.Collections.Generic;
using System.Threading;

namespace MultiThreading.Task5.Threads.SharedCollection
{
    class Program
    {
        private static int ItemsCount = 10;
        private static List<int> items = new List<int>();
        private static object lockObject = new object();

        static void Main(string[] args)
        {
            Console.WriteLine("5. Write a program which creates two threads and a shared collection:");
            Console.WriteLine("the first one should add 10 elements into the collection and the second should print all elements in the collection after each adding.");
            Console.WriteLine("Use Thread, ThreadPool or Task classes for thread creation and any kind of synchronization constructions.");
            Console.WriteLine();

            var firstThread = new Thread(AddItems);
            var secondThread = new Thread(PrintItems);

            firstThread.Start();
            secondThread.Start();

            firstThread.Join();
            secondThread.Join();

            Console.ReadLine();
        }

        private static void AddItems()
        {
            for (int i = 0; i < ItemsCount; i++)
            {
                lock (lockObject)
                {
                    items.Add(i);
                }
                Thread.Sleep(2000);
            }
        }

        private static void PrintItems()
        {
            for (int i = 0; i < ItemsCount; i++)
            {
                lock (lockObject)
                {
                    Console.WriteLine($"[{string.Join(", ", items)}]");
                }
                Thread.Sleep(2000);
            }
        }
    }
}
