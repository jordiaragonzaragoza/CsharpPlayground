using System;
using System.Threading;

namespace Threading
{

    public static class Threading
    {
        public static void Start()
        {
            //A shared writable state can introduce the kind of intermittent errors for which multithreading is notorious.

            ThreadSharedField.Main();
            ThreadSharedFieldOnLambda.Main();
            ThreadSharedStaticField.Main();

            ThreadLockSafe.Main();
            ThreadPassingData.Main();
            ThreadsExceptionHandling.Main();

            ThreadSignaling.Main();

            new ThreadBeginInvooke();
            new ThreadSyncronizationContext();
        }


    }

    //Share the _done field. This results in “Done” being printed once
    public class ThreadSharedField
    {
        bool _done;

        public static void Main()
        {
            var threadTest = new ThreadSharedField(); // Create a common instance
            new Thread(threadTest.Go).Start();
            threadTest.Go();
        }

        public void Go() // Note that this is an instance method
        {
            if (!_done)
            {
                _done = true;
                Console.WriteLine("Done");
            }
        }
    }

    //Share the _done field. This results in “Done” being printed once
    public class ThreadSharedFieldOnLambda
    {
        public static void Main()
        {
            bool done = false;
            ThreadStart action = () =>
            {
                if (!done)
                {
                    done = true;
                    Console.WriteLine("Done");
                }
            };
            new Thread(action).Start();
            action();
        }
    }

    // Static fields are shared between all threads
    // in the same application domain.
    public class ThreadSharedStaticField
    {
        static bool _done;

        public static void Main()
        {
            new Thread(Go).Start();
            Go();
        }

        public static void Go()
        {
            if (!_done)
            {
                _done = true;
                Console.WriteLine("Done");
            }
        }
    }

    //Theads Lock intro
    public class ThreadLockSafe
    {
        static bool _done;
        static readonly object _locker = new object();

        public static void Main()
        {
            new Thread(Go).Start();
            Go();
        }

        public static void Go()
        {
            //Protects the entry on sharing fields.
            lock (_locker)
            {
                if (!_done)
                {
                    Console.WriteLine("LockSafe");
                    _done = true;
                }
            }
        }
    }

    public class ThreadPassingData
    {
        public static void Main()
        {
            Thread t = new Thread(() => Print("Hello from thread!"));
            t.Start();
        }

        public static void Print(string message)
        {
            Console.WriteLine(message);
        }
    }

    public class ThreadsExceptionHandling
    {
        public static void Main()
        {
            try
            {
                new Thread(Go).Start();
            }
            catch (Exception ex)
            {
                // We'll never get here!
                Console.WriteLine("Exception!");
            }
        }

        //You need an exception handler on all thread entry methods in production applications
        public static void Go()
        {
            try
            {
                throw null; // The NullReferenceException will get caught below
            }
            catch (NullReferenceException ex)
            {
                Console.WriteLine(ex.Message);
            }
        } 
    }

    //Calling WaitOne on a ManualResetEvent blocks the current thread until
    //another thread “opens” the signal by calling Set.
    public class ThreadSignaling
    {
        public static void Main()
        {
            var signal = new ManualResetEvent(false);

            new Thread(() =>
            {
                Console.WriteLine("Waiting for signal...");
                signal.WaitOne();
                signal.Dispose();
                Console.WriteLine("Got signal!");
            }).Start();

            Thread.Sleep(4000);

            signal.Set(); // "Open" the signal
            signal.Reset();
        }
    }


    public class ThreadBeginInvooke
    {
        private string _textOnUIControl;
        public ThreadBeginInvooke()
        {
            new Thread(Work).Start();
        }

        private void Work()
        {
            Thread.Sleep(5000); // Simulate time-consuming task
            UpdateMessage("The answer");
        }
        private void UpdateMessage(string message)
        {
            Action action = () => _textOnUIControl = message;
            //This will update the UI Thread. Commented due to now we are on console application
            //System.Windows.Threading.Dispatcher.BeginInvoke(action);
        }
    }

    //SynchronizationContext that enables the generalization of thread marshaling
    //SynchronizationContext.Current (while running on a UI thread)
    //lets you later “post” to UI controls from a worker thread
    public class ThreadSyncronizationContext
    {
        SynchronizationContext _uiSyncContext;
        private string _textOnUIControl;
        public ThreadSyncronizationContext()
        {  
            // Capture the synchronization context for the current UI thread:
            _uiSyncContext = SynchronizationContext.Current;
            new Thread(Work).Start();
        }
        void Work()
        {
            Thread.Sleep(5000); // Simulate time-consuming task
            UpdateMessage("The answer");
        }
        void UpdateMessage(string message)
        {
            // Marshal the delegate to the UI thread:
            //Calling Post is equivalent to calling BeginInvoke on a Dispatcher or Control;
            _uiSyncContext.Post(_ => _textOnUIControl = message, null);

            //there’s also a Send method, which is equivalent to Invoke.
            _uiSyncContext.Send(_ => _textOnUIControl = message, null);
        }
    }
}
