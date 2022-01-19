using System;
using TradingSystem.inventory.data;
using TradingSystem.inventory.data.enterprise;
using TradingSystem.inventory.data.store;

namespace TradingSystem
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using (var db = new DataContext())
            {
                Enterprise enterprise = new Enterprise(1000, "Aldi Enterprise");
                Store store = new Store(1212, "Aldi");
                store.Location = "Wiesbaden";
                enterprise.Stores.Add(store);

                db.Add(enterprise);
                db.Add(store);
                db.SaveChanges();

                var store1 = db.Stores.Find(1212);
                Console.Write(store1);
            }
        }
    }
}