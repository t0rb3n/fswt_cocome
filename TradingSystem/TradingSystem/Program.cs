using System;
using Microsoft.EntityFrameworkCore;
using TradingSystem.inventory.data;
using TradingSystem.inventory.data.enterprise;
using TradingSystem.inventory.data.store;

namespace TradingSystem
{
    internal class Program
    {
        static void Main(string[] args)
        {

            IStoreQuery query = IDataFactory.GetInstance().GetStoreQuery();

            try
            {
                var store = query.QueryStoreById(1);
                Console.Write(store);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            /*using (var db = new DataContext())
            {
                var sql = db.Database.GenerateCreateScript();
                Console.Write(sql);
                System.Environment.Exit(0);
                db.Database.EnsureDeleted();
                db.Database.EnsureCreated();
                
                Enterprise enterprise = new();
                enterprise.Id = 10001;
                enterprise.Name = "AldiGroup";

                Store store = new ()
                {
                    Id = 20001,
                    Name = "Aldi",
                    Location = "Wiesbaden"
                };
                
                enterprise.Stores.Add(store);

                db.Add(enterprise);
                db.Add(store);
                db.SaveChanges();

                var store1 = db.Stores;
                foreach (var s in store1)
                {
                    Console.Write(s);
                }

                Console.Write("\n");
                
                var en1 = db.Enterprises;
                foreach (var e in en1)
                {
                    Console.Write(e);
                }
            }*/
        }
    }
}