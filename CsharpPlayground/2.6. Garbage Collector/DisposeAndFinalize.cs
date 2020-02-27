using System;
using System.IO;
using System.Xml;

namespace DisposeAndFinalize
{
    public static class DisposeAndFinalize
    {
        public static void Start()
        {
            var unmanagedWrapper = new UnmanagedWrapper();

            var stringWriter = new StringWriter();
            using (XmlWriter writer = XmlWriter.Create(stringWriter))
            {
                writer.WriteStartElement("book");
                writer.WriteElementString("price", "19.95");
                writer.WriteEndElement();
                writer.Flush();
            }
            string xml = stringWriter.ToString();

            Console.ReadLine();
        }
    }

    public class UnmanagedWrapper : IDisposable
    {
        public FileStream Stream { get; private set; }

        public UnmanagedWrapper()
        {
            Stream = File.Open("temp.dat", FileMode.Create);
        }

        public void Close()
        {
            Dispose();
        }

        // This code is called when the finalize method executes
        // The finalizer only calls Dispose passing false for disposing.

        //If the finalizer calls Dispose, you do nothing because the Stream object also implements a finalizer,
        //and the garbage collector takes care of calling the finalizer of the Stream instance.
        //You can’t be sure if it’s already called, so it’s best to leave it up to the garbage collector.

        ~UnmanagedWrapper()
        {
            Dispose(false);
        }
        
        //If Dispose is called explicitly, you close the underlying FileStream.
        //It’s important to be defensive in coding this method; always check for any source of possible exceptions.
        //It could be that Dispose is called multiple times and that shouldn’t cause any errors.
        public void Dispose()
        {
            Dispose(true);

            //This makes sure that the object is removed from the finalization list that the garbage collector is keeping track of.
            
            System.GC.SuppressFinalize(this);
            //The instance has already cleaned up after itself, so it’s not necessary that the garbage collector call the finalizer.
        }

        public void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (Stream != null)
                {
                    Stream.Close();
                }
            }
        }
    }
}
