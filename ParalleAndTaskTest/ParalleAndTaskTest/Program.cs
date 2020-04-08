using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace ParalleAndTaskTest
{
    [Serializable]
    public class ExceptionClass : Exception, ISerializable
    { 
        // Inside of this call you can create a bunch of exceptions and use them
    }

    class Program
    {
        public static volatile int Number = 0;

        static Object _lock = new object();

        static void Main(string[] args)
        {
            List<int> listInt = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9 };

            var cts = new CancellationTokenSource();

            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            CancellationToken source = cancellationTokenSource.Token;

            // Parallel
            
            Parallel.For(0, 10, (i) =>
            {
                lock (_lock)
                Number++;
            });

            Parallel.For(0, 10, (i) =>
            {
                Number++;
            });

            Parallel.For(0, 10, (i) =>
            {
                lock (_lock)
                Interlocked.Increment(ref Number); // Incersão em uma variavél com Atomicidade, serve em Task e em Parallel
            });

            Parallel.For(0, 10, (i) =>
            {
                Interlocked.Decrement(ref Number); // Decremento em uma variavél com Atomicidade
            });

            Parallel.ForEach(listInt, (i) =>
            {
                Console.WriteLine(i);
            });
            

            Parallel.Invoke(() => MakingSomethingImportant(1, 1000, source));


            // PLINQ

            var result = listInt.AsParallel().WithCancellation(cts.Token).Where(i => i == 1).Select(i => i);

            var result2 = listInt.AsParallel().WithCancellation(cts.Token).WithDegreeOfParallelism(32).Where(i => i == 1).Select(i => i);

            var result3 = listInt.AsParallel().WithCancellation(cts.Token).WithDegreeOfParallelism(32).AsOrdered().Where(i => i == 1).Select(i => i);

            // .AsParallel() => Fazer consultas LINQ em outra Thread

            // .WithCancellation(cts.Token) => Cancelando uma consulta

            // .WithDegreeOfParallelism(32) => Termina a quantidade de Paralelismo

            // .AsOrdered() => Mantem o paralelismo, mas ordena os valores que saem como resultado

            // .AsSequencial() => Mantem a sequencia de algum outro comando como o .OrderBy()

            // Task

            var test = Task.Factory.StartNew(() => {
                while(!source.IsCancellationRequested)
                {
                    Console.Write("*");
                    Thread.Sleep(1000);
                }
            }, source);

            Task task = Task.Run(() => MakingSomethingImportant(1, 1000, source)).ContinueWith((i) => MakingSomethingImportant(1, 1000, source));

            // Old Way 

            Task taskOld = new Task(() => MakingSomethingImportant(1, 1000, source));
            taskOld.Start();

            // New way
            try
            {
                var t1 = Task.Factory.StartNew(() => MakingSomethingImportant(1, 1000, source));//.ContinueWith(() => );
                var t2 = Task.Factory.StartNew(() => MakingSomethingImportant(2, 1000, source));//.ContinueWith(() => );
                var t3 = Task.Factory.StartNew(() => MakingSomethingImportant(3, 1000, source));//.ContinueWith(() => );
                var listTask = new List<Task>() { t1, t2, t3 };
                Task.WaitAll(listTask.ToArray());
                Task.WaitAll(t1, t2, t3);
            }
            catch(AggregateException e)
            {
                Console.WriteLine(e.Message);
            }

            // Create Exceptions:

            string stringTest;

            if (!string.IsNullOrWhiteSpace(stringTest))
                throw new ArgumentNullException("stringTest");

            // Resultados:

            Console.WriteLine("Number: " + Number);

            Console.WriteLine("End !");

            /*
                Console.WriteLine("Press to stop the Task");
                Console.ReadKey();
                cts.Cancel();
            */

            Console.ReadKey();
        }

        public static void MakingSomethingImportant(int Id, int Number, CancellationToken source)
        {
            Console.WriteLine("Starting: " + Id);
            Thread.Sleep(Number);
            Console.WriteLine("Done ... "+ Id);
        }

    }
}
