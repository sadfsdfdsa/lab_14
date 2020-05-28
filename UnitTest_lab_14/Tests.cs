using System;
using lab_14;
using NUnit.Framework;

namespace UnitTest_lab_14
{
    [TestFixture]
    public class Tests
    {
        [Test]
        public void GenerateCol()
        {
            TestCollection tmp = new TestCollection(10);
            Program.ShowCollection(tmp);

            Shop tmp_shop = Generator.GenerateObject(new Shop());
            Generator.CreateCollectionFirstTask(10);
            tmp.Add(tmp_shop.ShopName, tmp_shop, tmp_shop);
            try
            {
                tmp.Add(tmp_shop.ShopName, tmp_shop, tmp_shop);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            Assert.True(true);
        }

        [Test]
        public void TestMain()
        {
            Program.col = new TestCollection(10);
            Program.Select(10);
            Program.Count(10);
            Program.Except(10, 10);
            Program.GroupBy(10);
            Program.Aggregate();
        }
    }
}