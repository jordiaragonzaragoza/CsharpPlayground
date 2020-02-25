using System.IO;

namespace ContravarianceWithDelegates
{
    public class ContravarianceWithDelegates
    {
        public void DoSomething(TextWriter tw) { }
        public delegate void ContravarianceDel(StreamWriter tw);

        public void Start()
        {
            ContravarianceDel del = DoSomething;
        }
    }
}
