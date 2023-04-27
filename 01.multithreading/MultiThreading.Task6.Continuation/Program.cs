/*
*  Create a Task and attach continuations to it according to the following criteria:
   a.    Continuation task should be executed regardless of the result of the parent task.
   b.    Continuation task should be executed when the parent task finished without success.
   c.    Continuation task should be executed when the parent task would be finished with fail and parent task thread should be reused for continuation
   d.    Continuation task should be executed outside of the thread pool when the parent task would be cancelled
   Demonstrate the work of the each case with console utility.
*/
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MultiThreading.Task6.Continuation
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Create a Task and attach continuations to it according to the following criteria:");
            Console.WriteLine("a.    Continuation task should be executed regardless of the result of the parent task.");
            Console.WriteLine("b.    Continuation task should be executed when the parent task finished without success.");
            Console.WriteLine("c.    Continuation task should be executed when the parent task would be finished with fail and parent task thread should be reused for continuation.");
            Console.WriteLine("d.    Continuation task should be executed outside of the thread pool when the parent task would be cancelled.");
            Console.WriteLine("Demonstrate the work of the each case with console utility.");
            Console.WriteLine();

            var tokenSource = new CancellationTokenSource();
            var token = tokenSource.Token;
            tokenSource.Cancel();
            
            var parentTask = new Task(() =>
            {
                if (token.IsCancellationRequested)
                {
                    Console.WriteLine($"Parent task cancellation requested. Thread id: {Thread.CurrentThread.ManagedThreadId}");
                    Thread.Sleep(2000);
                    token.ThrowIfCancellationRequested();
                }
            });

            parentTask
            .ContinueWith((antecedent) =>
            {
                Console.WriteLine($"Continuation task thread id: {Thread.CurrentThread.ManagedThreadId}\n" +
                $"a. Continuation task executed regardless of the result of the parent task.\n" +
                $"Parant task status: {antecedent.Status}\n");
                Thread.Sleep(2000);
            }, TaskContinuationOptions.None);
            
            parentTask
            .ContinueWith((antecedent) =>
            {
                Console.WriteLine($"Continuation task thread id: {Thread.CurrentThread.ManagedThreadId}\n" +
                $"b. Continuation task executed when the parent task finished without success.\n" +
                $"Parant task status: {antecedent.Status}\n");
                Thread.Sleep(2000);
            }, TaskContinuationOptions.OnlyOnFaulted);

            Task.Run(() =>
            {
                if (token.IsCancellationRequested)
                {
                    Console.WriteLine($"Parent task failed. Thread id: {Thread.CurrentThread.ManagedThreadId}");
                    token.ThrowIfCancellationRequested();
                }
            }, token)
            .ContinueWith((antecedent) =>
            {
                Console.WriteLine($"Continuation task thread id: {Thread.CurrentThread.ManagedThreadId}\n" +
                $"c. Continuation task executed outside of the thread pool when the parent task would be cancelled.\n" +
                $"Parant task status: {antecedent.Status}\n");
                Thread.Sleep(2000);
            }, CancellationToken.None, TaskContinuationOptions.LongRunning | TaskContinuationOptions.OnlyOnCanceled, TaskScheduler.Default);

            parentTask
                .ContinueWith((antecedent) =>
                {
                    Console.WriteLine($"Continuation task thread id: {Thread.CurrentThread.ManagedThreadId}\n" +
                    $"d. Continuation task executed when the parent task would be finished with fail and parent task thread should be reused for continuation.\n" +
                    $"Parant task status: {antecedent.Status}\n");
                    Thread.Sleep(2000);
                }, CancellationToken.None, TaskContinuationOptions.ExecuteSynchronously | TaskContinuationOptions.OnlyOnFaulted, TaskScheduler.Default);

            parentTask.Start();
            Console.ReadLine();
        }
    }
}
