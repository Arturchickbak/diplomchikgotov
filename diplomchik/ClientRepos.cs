using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace diplomchik
{
    internal class ClientRepos
    {


    
            private readonly DB _db;
            public ClientRepos() : this(new DB())
            {

            }
            public ClientRepos(DB db)
            {
                this._db = db;
            }

            public List<Client> GetClient()
            {
                using var db = _db.GetSqlConnection();

                var cars = db.Query<Client>("select * from cliet").ToList();
                return cars;
            }
        public Client GetUser(int userId)
        {
            using var db = _db.GetSqlConnection();

            var client = db.QueryFirstOrDefault<Client>("SELECT * FROM client WHERE cliet_id = @UserId", new { UserId = userId });
            return client;
        }
    }
}
