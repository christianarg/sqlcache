using Dapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SqlCaching.Caching;
using System;
using System.Data.SqlClient;
using System.Linq;

namespace SqlCaching.Test
{
    [TestClass]
    public class SqlCacheTest
    {
        private const string connectionString = "Data Source=(LocalDb)\\MSSQLLocalDB;Initial Catalog=SqlCacheTests;Integrated Security=SSPI;";

        [TestInitialize]
        public void Init()
        {
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Execute("TRUNCATE TABLE Cache");
            }
        }

        [TestMethod]
        public void AddItem()
        {
            // ACT
            string key = "AddItem";
            string data = "data";

            // ACT
            SqlCache cache = new SqlCache(connectionString);
            cache.Add(key, data, DateTime.Now.AddMinutes(30));

            // ASSERT
            var result = cache.Contains(key);
            Assert.AreEqual(result, true);
            cache.Remove(key); // Clean DB for further testing
        }

        [TestMethod]
        public void RemoveItem()
        {
            // ARRANGE
            string key = "RemoveItem";
            string data = "data";
            SqlCache cache = new SqlCache(connectionString);

            cache.Add(key, data, DateTime.Now.AddMinutes(30));

            // ACT
            cache.Remove(key);

            // ASSERT
            var result = cache.Contains(key);

            Assert.AreEqual(result, false);
        }

        [TestMethod]
        public void GetItem()
        {
            // ARRANGE
            string key = "GetItem";
            string data = "data";
            SqlCache cache = new SqlCache(connectionString);
            cache.Add(key, data, DateTime.Now.AddMinutes(30));

            // ACT
            var getData = cache.Get(key);

            // ASSERT
            Assert.AreEqual(data, getData);
        }

        [TestMethod]
        public void AddOrGetExisting()
        {
            string key = "AddOrGetExisting";
            string data = "data";
            SqlCache cache = new SqlCache(connectionString);

            // Try to get data
            var getData = cache.Get(key);
            Assert.IsNull(getData);

            // Add new entry
            getData = cache.AddOrGetExisting(key, data, DateTime.Now.AddMinutes(30));
            Assert.IsNull(getData);

            // Retrieve added entry
            getData = cache.AddOrGetExisting(key, data, DateTime.Now.AddMinutes(30));
            Assert.IsNotNull(getData);
        }

        [TestMethod]
        public void GetCount()
        {
            // ARRANGE
            SqlCache cache = new SqlCache(connectionString);
            cache.Add("1", "GetCountValue1", DateTime.Now.AddMinutes(30));
            cache.Set("2", "GetCountValue2", new System.Runtime.Caching.CacheItemPolicy { AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(1) });

            cache.Set("3", "GetCountValue3", new System.Runtime.Caching.CacheItemPolicy { SlidingExpiration = TimeSpan.FromMinutes(1) });
            // ACT
            var count = cache.GetCount();

            // ASSERT
            Assert.AreEqual(3, count);
        }


        [TestMethod]
        public void GetEnumerable()
        {
            // ARRANGE
            var values = new string[] { "GetEnumerableValue1", "GetEnumerableValue2", "GetEnumerableValue3" };
            SqlCache cache = new SqlCache(connectionString);
            cache.Add("1", values[0], DateTime.Now.AddMinutes(30));
            cache.Set("2", values[0], new System.Runtime.Caching.CacheItemPolicy { AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(1) });

            cache.Set("3", values[0], new System.Runtime.Caching.CacheItemPolicy { SlidingExpiration = TimeSpan.FromMinutes(1) });
            // ACT
            foreach (var item in cache)
            {
                // ASSERT
                Assert.IsTrue(values.Any(x => x == item.Value.ToString()));
            }
        }
    }
}
