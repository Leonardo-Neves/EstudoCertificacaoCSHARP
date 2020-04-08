using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;

namespace iDisposableTest
{
    class Program 
    {
        static void Main(string[] args)
        {
            dynamic obj = new SampleObject();
            Console.WriteLine(obj.SomeProperty); // Displays ‘SomeProperty’


            var source = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20 };

            // Source is ordered; let's preserve it.
            var parallelQuery = from num in source.AsParallel().AsOrdered() where num % 3 == 0 select num;

            // Use foreach to preserve order at execution time.
            foreach (var v in parallelQuery)
                Console.Write("{0} ", v);

            // Some operators expect an ordered source sequence.
            var lowValues = parallelQuery.Take(10);
        }

       
    }
    public class SampleObject : DynamicObject
    {
        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            result = binder.Name;
            return true;
        }
    }
   
}
