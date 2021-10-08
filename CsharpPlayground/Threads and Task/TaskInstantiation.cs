namespace TaskInstantiation
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public class TaskInstantiation
    {
        public static async Task Start()
        {
            SimpleInstanciation();

            BlockingMainThreadTaskFactoryStartNew();

            await TaskFactoryStartNewAsync();

            TaskIsCompleted();

            TaskExceptions();

            AsyncStateExample();

            TaskResultExample();

            ContinueWithExample();

            TaskFromResultExample();

            GetAwaiterExample();

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

        private static async Task TaskFactoryStartNewAsync()
        {
            await Task.Factory.StartNew(() => {
                Console.WriteLine("Task running for 5 seconds.");
                Thread.Sleep(5000);
                Console.WriteLine("Task finished. TaskId={0}, Thread={1}", Task.CurrentId, Thread.CurrentThread.ManagedThreadId);
            });
        }

        private static void BlockingMainThreadTaskFactoryStartNew()
        {
            var startedTask = Task.Factory.StartNew(() => {
                Console.WriteLine("Task running for 3 seconds.");
                Thread.Sleep(3000);
                Console.WriteLine("Task finished. TaskId={0}, Thread={1}", Task.CurrentId, Thread.CurrentThread.ManagedThreadId);
            });

            Console.WriteLine("Blocking the main thread ({0}) to wait a startedTask.", Thread.CurrentThread.ManagedThreadId);
            startedTask.Wait();
        }

        private static void TaskIsCompleted()
        {
            // Wait on a single task with a timeout specified.
            Task taskA = Task.Run(() => Thread.Sleep(2000));
            try
            {
                taskA.Wait(1000);       // Wait for completion or 1 second. What happens first.

                bool completed = taskA.IsCompleted;
                Console.WriteLine("Task A completed: {0}, Status: {1}", completed, taskA.Status);
                if (!completed)
                { 
                    Console.WriteLine("Timed out before task A completed."); 
                }
            }
            catch (AggregateException)
            {
                Console.WriteLine("Exception in taskA.");
            }
        }

        private static void TaskExceptions()
        {
            // Create a cancellation token and cancel it.
            var source1 = new CancellationTokenSource();
            var tokenCancelled = source1.Token;
            source1.Cancel();

            // Create a cancellation token for later cancellation.
            var cancellationTokenSourceToCancel = new CancellationTokenSource();
            var tokenWillBeCancelled = cancellationTokenSourceToCancel.Token;

            // Create a series of tasks that will complete, be cancelled, 
            // timeout, or throw an exception.
            Task[] tasks = new Task[4];
            for (int i = 0; i < 4; i++)
            {
                switch (i)
                {
                    // Task should run to completion.
                    case 0:
                        tasks[i] = Task.Run(() => Thread.Sleep(2000));
                        break;
                    // Task should be set to canceled state.
                    case 1:
                        tasks[i] = Task.Run(() => Thread.Sleep(2000), tokenCancelled);
                        break;
                    // Task should throw an exception.
                    case 2:
                        tasks[i] = Task.Run(() => { throw new NotSupportedException(); });
                        break;
                    // Task should examine cancellation token.
                    case 3:
                        tasks[i] = Task.Run(() => 
                        {
                            Thread.Sleep(2000);

                            if (tokenWillBeCancelled.IsCancellationRequested)
                            { 
                                tokenWillBeCancelled.ThrowIfCancellationRequested(); 
                            }

                            Thread.Sleep(500);
                        }, tokenWillBeCancelled);
                        break;
                }
            }
            Thread.Sleep(250); //Required to ensure to run (start) a task to be cancelled.
            cancellationTokenSourceToCancel.Cancel();

            try
            {
                Task.WaitAll(tasks);
            }
            catch (AggregateException aggregateException)
            {
                Console.WriteLine("One or more exceptions occurred:");

                foreach (var exception in aggregateException.InnerExceptions)
                { 
                    Console.WriteLine("   {0}: {1}", exception.GetType().Name, exception.Message); 
                }
            }

            Console.WriteLine("Status of tasks:");
            foreach (var task in tasks)
            {
                Console.WriteLine("   Task #{0}: {1}", task.Id, task.Status);
                if (task.Exception != null)
                {
                    foreach (var ex in task.Exception.InnerExceptions)
                        Console.WriteLine("      {0}: {1}", ex.GetType().Name,
                                          ex.Message);
                }
            }
        }

        //Use this method when creation and scheduling do not have to be separated and you require additional task creation
        //options or the use of a specific scheduler, or when you need to pass additional state into the task
        //that you can retrieve through its Task.AsyncState property
        private static void AsyncStateExample()
        {
            Task[] taskArray = new Task[10];
            for (int i = 0; i < taskArray.Length; i++)
            {
                taskArray[i] = Task.Factory.StartNew((object obj) => {
                    if (obj is not CustomData data)
                    {
                        return;
                    }

                    data.ThreadNum = Thread.CurrentThread.ManagedThreadId;
                }, new CustomData() { Name = i, CreationTime = DateTime.Now.Ticks });
            }

            Task.WaitAll(taskArray);

            foreach (var task in taskArray)
            {
                if (task.AsyncState is CustomData data)
                {
                    Console.WriteLine("Task #{0} created at {1}, ran on thread #{2}.", data.Name, data.CreationTime, data.ThreadNum);
                }
            }
        }

        private static void TaskResultExample()
        {
            var taskArray = new Task<double>[] 
            { 
                Task<double>.Factory.StartNew(() => DoComputation(1.0)),
                Task<double>.Factory.StartNew(() => DoComputation(100.0)),
                Task<double>.Factory.StartNew(() => DoComputation(1000.0)) 
            };

            var results = new double[taskArray.Length];

            double sum = 0;

            for (int i = 0; i < taskArray.Length; i++)
            {
                results[i] = taskArray[i].Result;
                Console.Write("{0:N1} {1}", results[i], i == taskArray.Length - 1 ? "= " : "+ ");
                sum += results[i];
            }

            Console.WriteLine("{0:N1}", sum);

            static double DoComputation(double start)
            {
                double sum = 0;
                for (var value = start; value <= start + 10; value += .1)
                { 
                    sum += value; 
                }

                return sum;
            }
        }

        private static void ContinueWithExample()
        {
            var getData = Task.Factory.StartNew(() => {
                Random rnd = new Random();
                int[] values = new int[100];
                for (int ctr = 0; ctr <= values.GetUpperBound(0); ctr++)
                    values[ctr] = rnd.Next();

                return values;
            });
            var processData = getData.ContinueWith((x) => {
                int n = x.Result.Length;
                long sum = 0;
                double mean;

                for (int ctr = 0; ctr <= x.Result.GetUpperBound(0); ctr++)
                    sum += x.Result[ctr];

                mean = sum / (double)n;
                return Tuple.Create(n, sum, mean);
            });
            var displayData = processData.ContinueWith((x) => {
                return String.Format("N={0:N0}, Total = {1:N0}, Mean = {2:N2}",
                                     x.Result.Item1, x.Result.Item2,
                                     x.Result.Item3);
            });
            Console.WriteLine(displayData.Result);
        }

        private static void TaskFromResultExample()
        {
            throw new NotImplementedException();
        }

        private static void GetAwaiterExample()
        {
            throw new NotImplementedException();
        }
    }

    public class CustomData
    {
        public int Name { get; set; }
        public long CreationTime { get; set; }
        public object ThreadNum { get; set; }
    }
}
