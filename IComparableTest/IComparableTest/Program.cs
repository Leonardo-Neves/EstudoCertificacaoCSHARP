using System;
using System.Collections;
using System.Collections.Generic;

namespace IComparableTest
{
    class Person : IDisposable
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public Person(string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;
        }


        public override string ToString()
        {
            return FirstName + " " + LastName;
        }
    }
    class People : IEnumerable<Person>
    {
        Person[] people;

        public People(Person[] people)
        {
            this.people = people;
        }

        public IEnumerator<Person> GetEnumerator()
        {
            for (int index = 0; index < people.Length; index++)
            {
                yield return people[index];
            }
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

    }


    class Order : IComparable
    {
        public DateTime Created { get; set; }
        public int CompareTo(object obj)
        {
            if (obj == null) return 1;
            Order o = obj as Order;
            if (o == null)
            {
                throw new ArgumentException("Object is not an Order");
            }
            return this.Created.CompareTo(o.Created); // Comparando se a Order que veio 
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            List<Order> orders = new List<Order>
            {
                 new Order { Created = new DateTime(2012, 12, 1 )},
                 new Order { Created = new DateTime(2012, 1, 6 )},
                 new Order { Created = new DateTime(2012, 7, 8 )},
                 new Order { Created = new DateTime(2012, 2, 20 )},
            };

            orders.Sort();
            //   Esse metodo Sort só funcioca se a classe Order tiver o metodo CompareTo do IComparable
            //   O metodo sort usa uma comparação entre os objetos para tirar conclusões deles, nesse caso
            // as data, se uma é maior que a outra. 

            foreach (var item in orders)
            {
                Console.WriteLine(item.Created);
            }
        }
    }
}
