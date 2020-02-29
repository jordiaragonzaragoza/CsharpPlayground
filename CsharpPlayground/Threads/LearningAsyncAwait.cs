using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LearningAsyncAwait
{
    public static class LearningAsyncAwait
    {
        //La palabra clave await proporciona un modo sin bloqueo para iniciar una tarea y,
        //después, proseguir la ejecución cuando dicha tarea se complete
        //https://docs.microsoft.com/es-es/dotnet/csharp/programming-guide/concepts/async/?WT.mc_id=EducationalAdvancedCsharp-c9-niner

        //Cualquier método async devuelve una Task por defecto  donde recogemos su estado

        public static async Task Start()
        {
            Init();

            var oneSecondTask = OneSecondTaskAsync();
            var threeSecondsTask =  ThreeSecondsTaskAsync();
            var fourSecondsTask = FourSecondsTaskAsync();

            var allTasks = new List<Task> { threeSecondsTask, fourSecondsTask, oneSecondTask };
            while (allTasks.Any())
            {
                var finishedTask = await Task.WhenAny(allTasks);
                if (finishedTask == threeSecondsTask)
                {
                    Console.WriteLine($"Result from task ThreeSeconds is: {threeSecondsTask.Result}");
                    Console.WriteLine();
                }
                else if (finishedTask == fourSecondsTask)
                {
                    Console.WriteLine($"Result from task FourSecondsTask is: {fourSecondsTask.Result}");
                    Console.WriteLine();
                }
                else if (finishedTask == oneSecondTask)
                {
                    Console.WriteLine($"Result from task OneSecondTask is: {oneSecondTask.Result}");
                    Console.WriteLine();
                }
                allTasks.Remove(finishedTask);
            }

            Finish();
        }

        private static void Init()
        {
            Console.WriteLine("Method Init()");
        }

        private static void Finish()
        {
            Console.WriteLine("Method Finish()");
        }
        
        private static async Task<int> ThreeSecondsTaskAsync()
        {
            Console.WriteLine("-----Task ThreeSecondsTaskAsync sent");
            Console.WriteLine($"Thead: {Thread.CurrentThread.ManagedThreadId}");
            Console.WriteLine($"Am I on a Pooling Thread?: {Thread.CurrentThread.IsThreadPoolThread}");
            Console.WriteLine();

            await Task.Delay(3000);
            Console.WriteLine("-----Task ThreeSecondsTaskAsync done!");

            return 3;
        }

        private static async Task<int> FourSecondsTaskAsync()
        {
            Console.WriteLine("-----Task FourSecondsTaskAsync sent");
            Console.WriteLine($"Thead: {Thread.CurrentThread.ManagedThreadId}");
            Console.WriteLine();

            await Task.Delay(4000);
            Console.WriteLine("-----Task FourSecondsTaskAsync done!");

            return 4;
        }

        private static async Task<int> OneSecondTaskAsync()
        {
            Console.WriteLine("-----Task OneSecondTaskAsync sent");
            Console.WriteLine($"Thead: {Thread.CurrentThread.ManagedThreadId}");
            Console.WriteLine();

            //Composed tasks: async task + sync task = async task
            var returnedValue = await TenSecondsAsync(); //Async task
            //Do something with the returned value...
            
            await Task.Delay(1000);
            Console.WriteLine("-----Task OneSecondTaskAsync done!");

            return 1;
        }

        private static async Task<int> TenSecondsAsync()
        {
            Console.WriteLine("-----Task TenSecondsAsync sent");
            Console.WriteLine($"Thead: {Thread.CurrentThread.ManagedThreadId}");
            Console.WriteLine();

            await Task.Delay(10000);
            Console.WriteLine("-----Task TenSecondsAsync done!");

            return 10;
        }
    }
}
