using System.IO;

namespace CovarianceWithDelegates
{
    public class CovarianceWithDelegates
    {
        public delegate TextWriter CovarianceDelegate();
        public StreamWriter MethodStream() { return null; }
        public StringWriter MethodString() { return null; }

        public void Start()
        {
            CovarianceDelegate @delegate;
            @delegate = MethodStream;
            @delegate = MethodString;
        }
    }
}
