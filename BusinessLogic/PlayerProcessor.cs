using PlayerHelp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using System.Web.WebPages;
using PlayerHelp.DataAccess;

namespace PlayerHelp.BusisnessLogic
{
    public static class PlayerProcessor
    {
        public static int CreatePlayer(int PlayerID, string Username, string emailAddress, string Password)
        {
            PlayerModel data = new PlayerModel
            {
                PlayerLoginID = PlayerID,
                Username = Username,
                EmailAddress = emailAddress,
                Password = Password
            };

            string sql = "insert into dbo.PlayerLogin (PlayerLoginID, Username, Email, PlayerPassword) values " +
                "(@PlayerLoginID, @Username, @EmailAddress, @Password);";

            return SQLDataAccess.SaveData(sql, data);
        }
        public static List<PlayerModel> LoadPlayers()
        {
            string sql = @"select PlayerLoginID, Username, Email, Password, from dbo.PlayerLogin;";

            return SQLDataAccess.LoadData<PlayerModel>(sql);
        }
    }
}