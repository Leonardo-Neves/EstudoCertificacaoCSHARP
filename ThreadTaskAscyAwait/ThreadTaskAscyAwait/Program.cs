using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ThreadTaskAscyAwait
{
    class Program
    {
        
        static void Main(string[] args)
        {
            var source = new CancellationTokenSource();

            try
            {

                /*
                    Old version
                    var t2 = new Task(() => MakingSomethingImportant(1, 2000, source.Token));
                    Thread.Sleep(2000);
                    t2.Start();

                    The code below shows how to make the code above easyly using a Task Factory method with a StarNew method
                */

                // New version
                var t1 = Task.Factory.StartNew(() => MakingSomethingImportant(1, 1000, source.Token));//.ContinueWith(() => );
                source.Cancel();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.GetType());
            }

            var t2 = Task.Factory.StartNew(() => MakingSomethingImportant(1, 1000, source.Token));//.ContinueWith(() => );
            var t3 = Task.Factory.StartNew(() => MakingSomethingImportant(1, 1000, source.Token));//.ContinueWith(() => );
            var t4 = Task.Factory.StartNew(() => MakingSomethingImportant(1, 1000, source.Token));//.ContinueWith(() => );


            var listTask = new List<Task>() { t2, t3, t4 }; 
            Task.WaitAll(listTask.ToArray()); // It gonna wait all task end after continons the rest of work

            var listOfNumber = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

            Parallel.ForEach(listOfNumber, (i) => Console.WriteLine(i)); // Executa os metodos em Paralelo, espera geralmente um lista 
            //Parallel.For();

            // This class Parallel gonna execute those numbers in a different order
            // The method ForEach gonna wait a array and a form to show this dates, and you have de variable i to manipulate those dates the way you want
            // In same way, Parallel gonna wait all information execute and after pass to the next step of your program


            //Thread.Start();
            //Parallel.ForEach();


            

        }
        static void MakingSomethingImportant(int id, int sleepTime, CancellationToken token)
        {
            if(token.IsCancellationRequested)
            {
                Console.WriteLine("Cancellation requested");
                token.ThrowIfCancellationRequested();
            }

            Console.WriteLine("Task {0} is beginning", id);
            Thread.Sleep(sleepTime);
            Console.WriteLine("Task {0} has completed", id);
        }
    }
}
