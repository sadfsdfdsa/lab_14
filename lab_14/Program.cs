using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace lab_14
{
    internal class Program
    {
        public static TestCollection col;

        public static void Main(string[] args)
        {
            col = new TestCollection(10);
            ShowCollection(col);

            Select();
            Count();
            Except();
            Aggregate();
            GroupBy();

            Console.ReadLine();
        }

        public static void Select()
        {
            Console.WriteLine("SELECT: Выборка всех цехов с кол-во работников > 10");

            // LINQ
            var linq = from shop in col.collection_1TKey where shop.WorkersNumber > 10 select shop;
            foreach (Shop shop in linq)
            {
                shop.ShowInfo();
            }

            // расширяющие методы (WHERE)
            var expansion = col.collection_1TKey.Where(shop => shop.WorkersNumber > 10).Select(shop => shop);

            // проверка
            Console.WriteLine($"The same: {linq.Count() == expansion.Count()}");
            Console.WriteLine();
        }

        public static void Count()
        {
            Console.WriteLine("COUNT: Кол-во цехов с кол-во работников > 10");

            //linq
            var linq = (from shop in col.collection_1TKey where shop.WorkersNumber > 10 select shop).Count<Shop>();

            //expansion
            var expansion = (col.collection_1TKey.Where(shop => shop.WorkersNumber > 10).Select(shop => shop))
                .Count<Shop>();

            Console.WriteLine($"Кол-во: {linq}, the same: {linq == expansion}");
            Console.WriteLine();
        }

        public static void Except()
        {
            Console.WriteLine("EXCEPT: цехи с кол-во работников > 10, но с кол-вом инженеров <= 10");

            //linq
            var linq = (from shop in col.collection_1TKey where shop.WorkersNumber > 10 select shop)
                .Except(from shop in col.collection_1TKey where shop.MainWorkerNumber > 10 select shop);

            //expansion
            var expansion =
                (col.collection_1TKey.Where(shop => shop.WorkersNumber > 10).Select(shop => shop)).Except(
                    col.collection_1TKey.Where(shop => shop.MainWorkerNumber > 10).Select(shop => shop));

            foreach (Shop shop in linq)
            {
                shop.ShowInfo();
            }

            Console.WriteLine($"The same: {linq.Count() == expansion.Count()}");
            Console.WriteLine();
        }

        public static void Aggregate()
        {
            Shop SubAggregate(Shop a, Shop b)
            {
                b.MainWorkerNumber += a.MainWorkerNumber;
                return b;
            }

            Console.WriteLine("AGGREGATE: кол-во инженеров во всех цехах:");

            //linq
            var linq = (from shop in col.collection_1TKey select shop.MainWorkerNumber).Sum();

            // expansion
            var expansion = (col.collection_1TKey.Aggregate(SubAggregate)).MainWorkerNumber;

            Console.WriteLine($"Кол-во: {linq}, the same: {linq == expansion}");

            Console.WriteLine();
        }

        public static void GroupBy()
        {
            Console.WriteLine("GROUP BY: Engineers in Shop > 10");

            //linq
            var linq = from shop in col.collection_1TKey
                group shop by shop.MainWorkerNumber > 10;

            //expansion
            var expansion = col.collection_1TKey.GroupBy(shop => shop.MainWorkerNumber > 10);

            foreach (IGrouping<bool, Shop> g in linq)
            {
                Console.WriteLine(g.Key ? "Engineers > 10" : "Engineers <= 10");
                foreach (var t in g)
                    t.ShowInfo();
                Console.WriteLine();
            }

            Console.WriteLine($"The same: {linq.Count() == expansion.Count()}");
            Console.WriteLine();
        }


        public static void ShowCollection(TestCollection col)
        {
            Console.WriteLine("Collection: ");
            foreach (Shop item in col.collection_1TKey)
            {
                item.ShowInfo();
            }

            Console.WriteLine();
        }
    }


    static class Generator
    {
        public static Random randomizer = new Random();

        public static Stack CreateCollectionFirstTask(int numberFactory, int numberWorkshop, int numberShop)
        {
            Stack tmp = new Stack();
            for (int i = 0; i < numberFactory; i++)
            {
                tmp.Push(GenerateObject(new Factory()));
            }

            for (int i = 0; i < numberWorkshop; i++)
            {
                tmp.Push(GenerateObject(new Workshop()));
            }

            for (int i = 0; i < numberShop; i++)
            {
                tmp.Push(GenerateObject(new Shop()));
            }

            return tmp;
        }

        public static Factory GenerateObject(Factory tmp)
        {
            tmp.FactoryName = "factory_id-" + randomizer.Next(1, 125414);
            tmp.WorkersNumber = randomizer.Next(1, 100);
            return tmp;
        }

        public static Workshop GenerateObject(Workshop tmp)
        {
            tmp.ManagersNumber = randomizer.Next(1, 20);
            tmp.WorkersNumber = randomizer.Next(1, 100);
            return tmp;
        }

        public static Shop GenerateObject(Shop tmp)
        {
            tmp.ShopName = "shop_id-" + randomizer.Next(1, 14512);
            tmp.MainWorkerNumber = randomizer.Next(1, 15);
            tmp.WorkersNumber = randomizer.Next(1, 100);
            return tmp;
        }
    }

    public class TestCollection
    {
        public Stack<string> collection_1String;
        public Stack<Shop> collection_1TKey;

        public Dictionary<Shop, Shop> collection_2TKeyTValue;
        public Dictionary<string, Shop> collection_2StringTValue;

        public int Length;

        public TestCollection(int length)
        {
            // init collections 
            collection_1String = new Stack<string>();
            collection_1TKey = new Stack<Shop>();

            collection_2StringTValue = new Dictionary<string, Shop>();
            collection_2TKeyTValue = new Dictionary<Shop, Shop>();

            Length = length;

            for (int i = 0; i < Length; i++)
            {
                // generate for keys collections
                Shop tmpT = Generator.GenerateObject(new Shop());
                string tmpString = "keyString-" + i;

                // add to keys collections
                collection_1String.Push(tmpString);
                collection_1TKey.Push(tmpT);

                // generate value
                Shop tmpValue = Generator.GenerateObject(new Shop());

                // add to key-value collections
                collection_2TKeyTValue.Add(tmpT, tmpValue);
                collection_2StringTValue.Add(tmpString, tmpValue);
            }
        }

        public void Add(string keyString, Shop keyT, Shop value)
        {
            if (!collection_1String.Contains(keyString) && !collection_1TKey.Contains(keyT))
            {
                Length += 1;

                // add to keys collections
                collection_1String.Push(keyString);
                collection_1TKey.Push(keyT);

                // add to key-value collections
                collection_2TKeyTValue.Add(keyT, value);
                collection_2StringTValue.Add(keyString, value);
            }
            else
            {
                throw new Exception("Duplicate key. It's must be unique.");
            }
        }
    }
}