using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace lab_14
{
    public class Program
    {
        public static TestCollection col;

        public static void Main(string[] args)
        {
            col = new TestCollection(10);
            ShowCollection(col);

            Select(10);
            Count(10);
            Except(10, 10);
            Aggregate();
            GroupBy(10);

            Console.ReadLine();
        }

        public static void Select(int workersNumberMoreThan)
        {
            Console.WriteLine($"SELECT: Выборка всех цехов с кол-во работников > {workersNumberMoreThan}");

            // LINQ
            var linq = from shop in col.collection_1TKey where shop.WorkersNumber > workersNumberMoreThan select shop;
            foreach (Shop shop in linq)
            {
                shop.ShowInfo();
            }

            // расширяющие методы (WHERE)
            var expansion = col.collection_1TKey.Where(shop => shop.WorkersNumber > workersNumberMoreThan).Select(shop => shop);

            // проверка
            Console.WriteLine($"The same: {linq.Count() == expansion.Count()}");
            Console.WriteLine();
        }

        public static void Count(int workersNumberMoreThan)
        {
            Console.WriteLine($"COUNT: Кол-во цехов с кол-во работников > {workersNumberMoreThan}");

            //linq
            var linq = (from shop in col.collection_1TKey where shop.WorkersNumber > workersNumberMoreThan select shop).Count<Shop>();

            //expansion
            var expansion = (col.collection_1TKey.Where(shop => shop.WorkersNumber > workersNumberMoreThan).Select(shop => shop))
                .Count<Shop>();

            Console.WriteLine($"Кол-во: {linq}, the same: {linq == expansion}");
            Console.WriteLine();
        }

        public static void Except(int workersNumberMoreThan, int engineersNumberLessThan)
        {
            Console.WriteLine($"EXCEPT: цехи с кол-во работников > {workersNumberMoreThan}, но с кол-вом инженеров <= {engineersNumberLessThan}");

            //linq
            var linq = (from shop in col.collection_1TKey where shop.WorkersNumber > workersNumberMoreThan select shop)
                .Except(from shop in col.collection_1TKey where shop.MainWorkerNumber > engineersNumberLessThan select shop);

            //expansion
            var expansion =
                (col.collection_1TKey.Where(shop => shop.WorkersNumber > workersNumberMoreThan).Select(shop => shop)).Except(
                    col.collection_1TKey.Where(shop => shop.MainWorkerNumber > engineersNumberLessThan).Select(shop => shop));

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

        public static void GroupBy(int engineersNumberLessThan)
        {
            Console.WriteLine($"GROUP BY: Engineers in Shop > {engineersNumberLessThan}");

            //linq
            var linq = from shop in col.collection_1TKey
                group shop by shop.MainWorkerNumber > engineersNumberLessThan;

            //expansion
            var expansion = col.collection_1TKey.GroupBy(shop => shop.MainWorkerNumber > engineersNumberLessThan);

            foreach (IGrouping<bool, Shop> g in linq)
            {
                Console.WriteLine(g.Key ? $"Engineers > {engineersNumberLessThan}" : $"Engineers <= {engineersNumberLessThan}");
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


    public static class Generator
    {
        public static Random randomizer = new Random();

        public static Stack CreateCollectionFirstTask(int numberShop)
        {
            Stack tmp = new Stack();

            for (int i = 0; i < numberShop; i++)
            {
                tmp.Push(GenerateObject(new Shop()));
            }

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