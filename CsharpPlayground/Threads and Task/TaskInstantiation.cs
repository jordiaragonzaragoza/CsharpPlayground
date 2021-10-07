﻿namespace TaskInstantiation
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public class TaskInstantiation
    {
        public static async Task Start()
        {
            SimpleInstanciation();

            await TaskFactoryStartNew();

            TaskIsCompleted();

            Console.ReadLine();
        }

        private static void SimpleInstanciation()
        {
            Action<object> action = (object @object) =>
            {
                Console.WriteLine("Task={0}, obj={1}, Thread={2}", Task.CurrentId, @object, Thread.CurrentThread.ManagedThreadId);
            };

            // Create a task but do not start it.
            var t1 = new Task(action, "alpha");

            // Construct a started task
            Task t2 = Task.Factory.StartNew(action, "beta");

            // Wait for the task to finish or the specified time. What happens first. Block the main thread to demonstrate that t2 is executing
            t2.Wait(10000);

            // Launch t1 
            t1.Start();
            Console.WriteLine("t1 has been launched. (Main Thread={0})", Thread.CurrentThread.ManagedThreadId);

            // Wait for the task to finish. Block the main thread
            t1.Wait();

            // Construct a started task using Task.Run.
            string taskData = "delta";
            var t3 = Task.Run(() =>
            {
                Console.WriteLine("Task={0}, obj={1}, Thread={2}", Task.CurrentId, taskData, Thread.CurrentThread.ManagedThreadId);
            });

            // Wait for the task to finish. Block the main thread
            t3.Wait();

            // Construct an unstarted task
            var t4 = new Task(action, "gamma");

            // Run it synchronously (main thread).
            t4.RunSynchronously();

            // Although the task was run synchronously, it is a good practice
            // to wait for it in the event exceptions were thrown by the task.
            t4.Wait();
        }

        private static async Task TaskFactoryStartNew()
        {
            await Task.Factory.StartNew(() => {
                // Just loop.
                int ctr = 0;
                for (ctr = 0; ctr <= 1000000; ctr++)
                { }
                Console.WriteLine("Finished {0} loop iterations", ctr);
                Console.WriteLine("Task={0}, Thread={1}", Task.CurrentId, Thread.CurrentThread.ManagedThreadId);
            });
        }

        private static void TaskIsCompleted()
        {
            // Wait on a single task with a timeout specified.
            Task taskA = Task.Run(() => Thread.Sleep(2000));
            try
            {
                taskA.Wait(1000);       // Wait for completion or 1 second. What happens first.
                bool completed = taskA.IsCompleted;
                Console.WriteLine("Task A completed: {0}, Status: {1}",
                                 completed, taskA.Status);
                if (!completed)
                    Console.WriteLine("Timed out before task A completed.");
            }
            catch (AggregateException ae)
            {
                Console.WriteLine("Exception in taskA.");
            }
        }
    }
}