using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace LearningReflection
{
    public static class LearningReflection
    {
        public static void Start()
        {
            //ReflectionToExecuteAMethod();
            var plugins= CreatePluginsInstances("CsharpPlayground");
            Console.WriteLine($"Plugins Created: {plugins.Count()}");

            Console.ReadLine();
        }

        private static void ReflectionToExecuteAMethod()
        {
            var i = 42;
            var compareToMethod = i.GetType().GetMethod("CompareTo", new Type[] { typeof(int) });

            var result = (int)compareToMethod.Invoke(i, new object[] { 41 });
        }

        //Init classes using reflection in the assembly
        private static IEnumerable<IPlugin> CreatePluginsInstances(string assemblyString)
        {
            var pluginAssembly = Assembly.Load(assemblyString);
            var plugins = from type in pluginAssembly.GetTypes()
                          where typeof(IPlugin).IsAssignableFrom(type) && !type.IsInterface
                          select type;

            return plugins.Select(pluginType => Activator.CreateInstance(pluginType) as IPlugin).ToList();
        }
    }


    public interface IPlugin
    {
        string Name { get; }
        string Description { get; }
        bool Load(object application);
    }

    public class MyPlugin : IPlugin
    {
        public string Name => "MyPlugin";

        public string Description => "My Sample Plugin";

        public bool Load(object application)
        {
            return true;
        }
    }
}
