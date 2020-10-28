using PlayerHelp.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SQLite;
using System.Data;
using System.Linq;
using System.Web;
using Dapper;
using System.Data.SqlClient;

namespace PlayerHelp
{
    public static class SqliteDataAccess
    {
        public static List<PlayerModel> LoadPlayers()
        {
            using (IDbConnection cnn = new SqlConnection(LoadConnectionString())) //Protect us from leaving the connection open with our code
            {
                var output = cnn.Query<PlayerModel>("select * from PlayerLogin", new DynamicParameters());
                return output.ToList();
            }
        }
        public static void SavePlayer(PlayerModel player)
        {
            using (IDbConnection cnn = new SqlConnection(LoadConnectionString()))
            {
                cnn.Execute("insert into PlayerLogin (Username, EmailAddress, PlayerPassword) values (@Username, @EmailAddress, @PlayerPassword)", player);
            }
        }
        private static string LoadConnectionString(string id = "PlayerHelpDB") //Find the Default inside Web.config, if you find it return the ConnectionString value
        {
            return ConfigurationManager.ConnectionStrings[id].ConnectionString;
        }
    }

}