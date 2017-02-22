using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace AnonymousFunctionsDemo
{
    public class Program
    {
        private static  Dictionary<int, MethodInfo> AvailableMethods = new Dictionary<int, MethodInfo>();

        public static void Main(string[] args)
        {
           FindAllAvailableMethods();

            while (true)
            {
                ListAllAvailableMethodsToCall();
                var indexOfMethodToInvoke = Console.ReadLine();
                var parsedIndex = int.Parse(indexOfMethodToInvoke);
                InvokeMethod(parsedIndex);

                Thread.Sleep(1000);
                Console.WriteLine();
                Console.WriteLine();
            }
        }

        private static void FindAllAvailableMethods()
        {
            var programType = typeof(Program);
            var methodsInClass = programType.GetMethods(BindingFlags.NonPublic | BindingFlags.Static);

            var index = 0;
            foreach (var method in methodsInClass)
            {
                if (method.CustomAttributes.Any(customAttributes => customAttributes.AttributeType == typeof(Available)))
                {
                    AvailableMethods.Add(index, method);
                    index++;
                }
            }
        }

        private static void ListAllAvailableMethodsToCall()
        {
            Console.WriteLine("Available methods to call:");

            foreach (var method in AvailableMethods)
            {
                Console.WriteLine($"{method.Key}: {method.Value.Name}");
            }

            Console.WriteLine("Write the number of the method you would like to envoke and press enter:");
        }

        private static void InvokeMethod(int index)
        {
            var methodToInvoke = AvailableMethods[index];
            Console.WriteLine($"Invoking method: {methodToInvoke.Name}");
            Thread.Sleep(1000);
            methodToInvoke.Invoke(null, null);
        }

        const string GreetingPhrase = "Hello World!";

        [Available]
        private static void GreetWorldWithNamedDelegate()
        {
            // C# "1.0": Delegates needs to be initiliazed with a named delegate.
            var del = new Action<string>(WriteToConsole);

            del.Invoke(GreetingPhrase);
        }

        private static void WriteToConsole(string input) => Console.WriteLine(input);

        [Available]
        private static void GreetWordWithAnonymousMethod()
        {
            // C# 2.0: Delegates can be initialized with inline code, called an "anonymous method".
            Action<string> del = delegate (string input) { Console.WriteLine(input); };

            del.Invoke(GreetingPhrase);
        }

        [Available]
        private static void GreetWorldWithLambdaExpression()
        {
            // C# 3.0: Delegates can be initiliazed with lambda expressions
            Action<string> del = e => Console.WriteLine(e);

            del.Invoke(GreetingPhrase);
        }
    }
}
