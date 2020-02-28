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

        public static async Task Start()
        {
            Coffee cup = PourCoffee();
            Console.WriteLine("-----coffee is ready");
            var eggsTask =  FryEggsAsync(2);
            var baconTask = FryBaconAsync(3);
            var toastTask = MakeToastWithButterAndJamAsync(2);

            // <SnippetAwaitAnyTask>
            var allTasks = new List<Task> { eggsTask, baconTask, toastTask };
            while (allTasks.Any())
            {
                Task finished = await Task.WhenAny(allTasks);
                if (finished == eggsTask)
                {
                    Console.WriteLine("-----eggs are ready");
                }
                else if (finished == baconTask)
                {
                    Console.WriteLine("-----bacon is ready");
                }
                else if (finished == toastTask)
                {
                    Console.WriteLine("-----toast is ready");
                }
                allTasks.Remove(finished);
            }
            Juice oj = PourOJ();
            Console.WriteLine("-----oj is ready");
            Console.WriteLine("-----Breakfast is ready!");
            // </SnippetAwaitAnyTask>

            Console.ReadLine();

        }


        // </SnippetMain>
        private static Coffee PourCoffee()
        {
            Console.WriteLine("Pouring coffee");
            return new Coffee();
        }

        private static Juice PourOJ()
        {
            Console.WriteLine("Pouring Orange Juice");
            return new Juice();
        }

        private static void ApplyJam(Toast toast) => Console.WriteLine("Putting jam on the toast");

        private static void ApplyButter(Toast toast) => Console.WriteLine("Putting butter on the toast");

        private static async Task<Egg> FryEggsAsync(int howMany)
        {
            Console.WriteLine("-----Task FryEggsAsync sent");
            Console.WriteLine($"Thead: {Thread.CurrentThread.ManagedThreadId}");

            Console.WriteLine("Warming the egg pan...");
            await Task.Delay(3000);
            Console.WriteLine($"cracking {howMany} eggs");
            Console.WriteLine("cooking the eggs ...");
            await Task.Delay(3000);
            Console.WriteLine("Put eggs on plate");
            return new Egg();
        }

        private static async Task<Bacon> FryBaconAsync(int slices)
        {
            Console.WriteLine("-----Task FryBaconAsync sent");
            Console.WriteLine($"Thead: {Thread.CurrentThread.ManagedThreadId}");

            Console.WriteLine($"putting {slices} of bacon in the pan");
            Console.WriteLine("cooking first side of bacon...");
            await Task.Delay(3000);
            for (int slice = 0; slice < slices; slice++)
                Console.WriteLine("flipping a slice of bacon");
            Console.WriteLine("cooking the second side of bacon...");
            await Task.Delay(3000);
            Console.WriteLine("Put bacon on plate");
            return new Bacon();
        }

        private static async Task<Toast> MakeToastWithButterAndJamAsync(int number)
        {
            Console.WriteLine("-----Task MakeToastWithButterAndJamAsync sent");
            Console.WriteLine($"Thead: {Thread.CurrentThread.ManagedThreadId}");

            //Composed tasks: async task + sync task = async task
            var toast = await ToastBreadAsync(number); //Async task
            ApplyButter(toast);// Sync task
            ApplyJam(toast);// Sync task
            return toast;
        }

        private static async Task<Toast> ToastBreadAsync(int slices)
        {
            Console.WriteLine("-----Task FryBaconAsync sent");
            Console.WriteLine($"Thead: {Thread.CurrentThread.ManagedThreadId}");

            for (int slice = 0; slice < slices; slice++)
                Console.WriteLine("Putting a slice of bread in the toaster");
            Console.WriteLine("Start toasting...");
            await Task.Delay(3000);
            Console.WriteLine("Remove toast from toaster");
            return new Toast();
        }
    }

    public class Bacon
    {
    }

    public class Egg
    {
    }

    public class Toast
    {
    }

    public class Juice
    {
    }

    public class Coffee
    {
    }
}
