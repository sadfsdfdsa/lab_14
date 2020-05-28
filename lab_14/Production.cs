using System;
using System.Collections;

namespace lab_14
{
    public interface IPrint
    {
        void Print();
    }

    public class Production : IComparable, IPrint, ICloneable
    {
        private int _workersNumber;

        public int WorkersNumber
        {
            get => _workersNumber;
            set => _workersNumber = value;
        }

        public virtual void ShowInfo()
        {
            Console.WriteLine($"Workers: {WorkersNumber}");
        }

        // todo work only with new key
        public void ShowInfoFake()
        {
            Console.WriteLine($"Workers: {WorkersNumber}");
        }

        public int CompareTo(object obj)
        {
            Production temp = (Production) obj;
            if (WorkersNumber > temp.WorkersNumber) return 1;
            if (WorkersNumber < temp.WorkersNumber) return -1;
            return 0;
        }

        public void Print()
        {
            ShowInfo();
        }

        public object Clone()
        {
            return new Production {WorkersNumber = WorkersNumber};
        }
    }


    public class Shop : Production, ICloneable
    {
        public Production BaseProduction

        {
            get
            {
                return new Production() {WorkersNumber = WorkersNumber}; //возвращает объект базового класса
            }
        }

        private string _shopName;

        public string ShopName
        {
            get => _shopName;
            set => _shopName = value;
        }

        public int MainWorkerNumber { get; set; }

        public override void ShowInfo()
        {
            Console.WriteLine($"Engineer number: {MainWorkerNumber}, Workers: {WorkersNumber}, ShopName: {ShopName}");
        }

        public new object Clone()
        {
            return new Shop {ShopName = ShopName, WorkersNumber = WorkersNumber, MainWorkerNumber = MainWorkerNumber};
        }

        public override string ToString()
        {
            return _shopName;
        }
    }
}