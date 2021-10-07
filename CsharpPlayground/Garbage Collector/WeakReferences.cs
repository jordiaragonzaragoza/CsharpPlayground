using System;
using System.Collections.Generic;

namespace WeakReferences
{
    public static class WeakReferences
    {
        //data as a weak reference will be cleaned when gc collect. 
        private static WeakReference _data;

        public static void Start()
        {
            var result = GetData();
            //GC.Collect(); //Uncommenting this line will make data.Target null
            result = GetData();
        }

        //Meanwhile is not collected data will be in available memory. This a simple caching model.
        private static object GetData()
        {
            if (_data == null)
            {
                _data = new WeakReference(LoadLargeList());
            }

            if (_data.Target == null)
            {
                _data.Target = LoadLargeList();
            }
            return _data.Target;
        }

        public static List<int> LoadLargeList()
        {
            return new List<int>() {1, 2, 3, 4};
        }
    }
}
