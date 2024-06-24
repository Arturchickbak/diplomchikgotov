using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace diplomchik
{
    internal class MoneyRepos
    {
        private readonly DB _db;
        public MoneyRepos() : this(new DB())
        {

        }
        public MoneyRepos(DB db)
        {
            this._db = db;
        }

        public List<Money> GetMoney(int balance)
        {
            using var db = _db.GetSqlConnection();

            var cars = db.Query<Money>("select * from Cards").ToList();
            return cars;
        }



    }
}
    

