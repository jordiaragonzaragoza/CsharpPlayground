﻿using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace LearningTask
{
    //Task is higher-level abstraction—it represents a concurrent operation that may or may not be backed by a thread.
    //Tasks use pooled threads by default, which are background threads.
    public static class LearningTask
    {
        public static void Start()
        {
            //TaskRun();
            //TaskWait();
            //TaskResult();
            //LongRuningTask();
            //TaskException();
            //TaskContinuations();
            //TaskCompletionSource(); //TODO: Incomplete.
            //IntroAwaitAsync();
            DownloadAndSumAsync();
        }
        
        public static void TaskRun()
        {
            Task.Run(() => Console.WriteLine("Task.Run() executed."));

            //Old school method to run a task.
            //The Task.Run method was introduced in Framework 4.5. In Framework 4.0, you
            //can accomplish the same thing by calling Task.Factory.StartNew
            Task.Factory.StartNew(() => Console.WriteLine("Task.Factory.StartNew() executed."));

            Console.ReadLine(); //Block the main thread after starting the task. If not the main process ends and will cut the background task
        }

        //Calling Wait on a task blocks until it completes and is the equivalent of calling Join on a thread:
        public static void TaskWait()
        {
            Task task = Task.Run(() =>
            {
                Thread.Sleep(2000);
                Console.WriteLine("Foo");
            });

            Console.WriteLine(task.IsCompleted); // False
            task.Wait(); // Blocks until task is complete. Optionally specify a timeout and a cancellation token to end the wait early
        }

        public static void TaskResult()
        {
            //If the task hasn’t yet finished, accessing this property WILL BLOCK the current thread until the task finishes
            Task<string> task = Task.Run(() =>
            {
                var milliseconds = 2000;
                Thread.Sleep(milliseconds);
                return $"TaskResult() executed on {milliseconds} milliseconds";
            });
            string result = task.Result; // It BLOCKS HERE if not already finished.
            Console.WriteLine(result); // 3
        }

        public static void LongRuningTask()
        {
            //Prevent use of a pooled thread for long running tasks.
            Task<string> task = Task.Factory.StartNew(() =>
            {
                Console.WriteLine($"Is this thread pool thread?: {Thread.CurrentThread.IsThreadPoolThread}");

                var milliseconds = 5000;
                Thread.Sleep(milliseconds);

                return $"TaskResult() executed on {milliseconds} milliseconds";
            }, TaskCreationOptions.LongRunning); 

            string result = task.Result; // It BLOCKS HERE if not already finished.
            Console.WriteLine(result); // 3
        }

        public static void TaskException()
        {
            //Tasks conveniently propagate exceptions. So, if the code in
            //your task throws an unhandled exception(in other words, if your task faults), that
            //exception is automatically re - thrown to whoever calls Wait()

            // Start a Task that throws a NullReferenceException:
            Task task = Task.Run(() => throw null );
            try
            {
                task.Wait();
            }

            //The CLR wraps the exception in an AggregateException in order to play well with parallel programming scenarios
            catch (AggregateException aggregateException)
            {
                if (aggregateException.InnerException is NullReferenceException)
                {
                    Console.WriteLine(aggregateException.InnerException.Message);
                }
                else
                {
                    throw;
                }
            }
        }

        public static void TaskContinuations()
        {
            //Continuation task example using awaiter
            Task<string> twoSecondsTask = Task.Run(() =>
            {
                Console.WriteLine($"TwoSecondsTask thread id: {Thread.CurrentThread.ManagedThreadId}");

                var milliseconds = 2000;
                Thread.Sleep(milliseconds);
                return $"TaskContinuations() executed on {milliseconds} milliseconds";
            });

            //Preserve the context.
            var awaiter = twoSecondsTask.GetAwaiter();
            awaiter.OnCompleted(() =>
            {
                Console.WriteLine($"Awaiter TwoSecondsTask thread id: {Thread.CurrentThread.ManagedThreadId}");

                string result = awaiter.GetResult();
                Console.WriteLine(result); // Writes result
            });

            //Continuation task example using ContinueWith. Will change the context.
            twoSecondsTask.ContinueWith(antecedent =>
            {
                Console.WriteLine($"ContinueWith TwoSecondsTask thread id: {Thread.CurrentThread.ManagedThreadId}");
                string result = antecedent.Result;
                Console.WriteLine(result);
            });

            //Fault task Continuation example.
            Task<string> faultTask = Task.Run(() =>
            {
                Console.WriteLine($"FaultTask thread id: {Thread.CurrentThread.ManagedThreadId}");
                throw null;
                return string.Empty;
            });

            // To avoid synchronization context. ConfigureAwait to false.
            var faultTaskAwaiter = faultTask.ConfigureAwait(false).GetAwaiter();
            faultTaskAwaiter.OnCompleted(() =>
            {
                try
                {
                    Console.WriteLine($"Awaiter FaultTask thread id: {Thread.CurrentThread.ManagedThreadId}");

                    string result = faultTaskAwaiter.GetResult();
                    Console.WriteLine(result); // Writes result
                }
                //On awaiter will not wrapped on AggregateException
                catch (NullReferenceException nullReferenceException)
                {
                    Console.WriteLine(nullReferenceException.Message);
                }
            });

            Console.WriteLine($"Main thread id: {Thread.CurrentThread.ManagedThreadId}");

        }

        public static void TaskCompletionSource()
        {
            //This is ideal for I/O bound work: you get all the benefits of tasks
            //(with their ability to propagate return values, exceptions, and continuations)
            //without blocking a thread for the duration of the operation.

            var taskCompletionSource = new TaskCompletionSource<int>();
            new Thread(() =>
                    {
                        Thread.Sleep(5000);
                        taskCompletionSource.SetResult(42);
                    })
                    { IsBackground = true }
                    .Start();

            Task<int> task = taskCompletionSource.Task; // Our "slave" task.
            Console.WriteLine(task.Result); // 42

            //TaskCompletionSource with custom Run
            //Calling this method is equivalent to calling Task.Factory.StartNew with the Task
            //CreationOptions.LongRunning option to request a nonpooled thread.

            Task<TResult> CustomRun<TResult>(Func<TResult> function)
            {
                var taskCustomRun = new TaskCompletionSource<TResult>();
                new Thread(() =>
                {
                    try
                    {
                        taskCustomRun.SetResult(function());
                    }
                    catch (Exception ex)
                    {
                        taskCustomRun.SetException(ex);
                    }
                }).Start();
                return taskCustomRun.Task;
            }
            
            Task<int> customTask = CustomRun(() =>
            {
                Thread.Sleep(5000); return 42;
            });

        }

        public static async void IntroAwaitAsync()
        {
            Task<int> GetPrimesCountAsync(int start, int count)
            {
                return Task.Run(() =>
                    ParallelEnumerable.Range(start, count).Count(n =>
                        Enumerable.Range(2, (int)Math.Sqrt(n) - 1).All(i => n % i > 0)));
            }

            async Task<string> DisplayPrimeCountsAsync()
            {
                for (int i = 0; i < 10; i++)
                { Console.WriteLine(await GetPrimesCountAsync(i * 1000000 + 2, 1000000) +
                                      " primes between " + (i * 1000000) + " and " + ((i + 1) * 1000000 - 1));

                }

                return "Done DisplayPrimeCountsAsync()";
            }

            var taskResult = await DisplayPrimeCountsAsync();
            Console.WriteLine(taskResult);

            Console.WriteLine("Done IntroPreSyncAwait()");
        }

        public static async void DownloadAndSumAsync()
        {
            Console.WriteLine("Hello");
            string[] urls = "www.albahari.com www.oreilly.com www.linqpad.net".Split();
            int totalLength = 0;
            try
            {
                foreach (string url in urls)
                {
                    var uri = new Uri("http://" + url);
                    byte[] data = await new WebClient().DownloadDataTaskAsync(uri);

                    Console.WriteLine("Length of " + url + " is " + data.Length +
                                      Environment.NewLine);
                    
                    totalLength += data.Length;
                }

                Console.WriteLine("Total length: " + totalLength);
            }
            catch (WebException ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            finally
            {
                Console.WriteLine("Bye");
            }
        }
    }
}
