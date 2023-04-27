/*
 * 4.	Write a program which recursively creates 10 threads.
 * Each thread should be with the same body and receive a state with integer number, decrement it,
 * print and pass as a state into the newly created thread.
 * Use Thread class for this task and Join for waiting threads.
 * 
 * Implement all of the following options:
 * - a) Use Thread class for this task and Join for waiting threads.
 * - b) ThreadPool class for this task and Semaphore for waiting threads.
 */

using System;
using System.Threading;

namespace MultiThreading.Task4.Threads.Join
{
    class Program
    {
        private static SemaphoreSlim semaphore = new SemaphoreSlim(0, 10);
        static void Main(string[] args)
        {
            Console.WriteLine("4.	Write a program which recursively creates 10 threads.");
            Console.WriteLine("Each thread should be with the same body and receive a state with integer number, decrement it, print and pass as a state into the newly created thread.");
            Console.WriteLine("Implement all of the following options:");
            Console.WriteLine();
            Console.WriteLine("- a) Use Thread class for this task and Join for waiting threads.");
            Console.WriteLine("- b) ThreadPool class for this task and Semaphore for waiting threads.");

            Console.WriteLine();
            var initialState = 10;
            ThreadDecrement(initialState);
            ThreadPoolDecrement(initialState);
            
            for (int i = 0; i < 10; i++)
            {
                semaphore.Wait();
            }

            Console.ReadLine();
        }

        private static void ThreadDecrement(object state)
        {
            var value = (int)state;            

            if (value > 0)
            {
                Console.WriteLine($"Thread: {Thread.CurrentThread.ManagedThreadId}. Value: {value}");
                var thread = new Thread(ThreadDecrement);
                thread.Start(--value);
                thread.Join();
            }
        }

        private static void ThreadPoolDecrement(object state)
        {
            var value = (int)state;

            if (value > 0)
            {
                Console.WriteLine($"Thread: {Thread.CurrentThread.ManagedThreadId}. Value: {value}");
                ThreadPool.QueueUserWorkItem(ThreadPoolDecrement, --value);
            }

            semaphore.Release();
        }
    }
}
