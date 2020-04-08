using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace ThreadPoolTest
{
    class Program
    {
        static void Main(string[] args)
        {
            
            int maxThread, out1;
            int minThread, out2;
            int availableThread, out3;

            ThreadPool.GetMaxThreads(out maxThread, out out1); // Pega o maximo de Thread disponiveis
            ThreadPool.GetMinThreads(out minThread, out out2); // Pega o Minino de Threads disponiveis
            ThreadPool.GetAvailableThreads(out availableThread, out out3); // pega o maximo d Thread disponiveis


            ThreadPool.QueueUserWorkItem(method1());

            Console.WriteLine("Max Thread: {0}, I don't know what it is: {1}", maxThread, out1);
            Console.WriteLine("Min Thread: {0}, I don't know what it is: {1}", minThread, out2);
            Console.WriteLine("Available Thread: {0}, I don't know what it is: {1}", availableThread, out3);

            Console.WriteLine();

            Console.WriteLine("Press a button to end the program ...");
            Console.ReadKey();
        }

        static void method1()
        {
            Console.WriteLine("Metodo 1");
        }
        public static void method2()
        {
            Console.WriteLine("Metodo 2");
        }
        public static void method3()
        {
            Console.WriteLine("Metodo 3");
        }
    }
}
