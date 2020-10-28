using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Mvc;
using Dapper;
using static PlayerHelp.BusisnessLogic.PlayerProcessor;
using PlayerHelp.Models;
using System.Security.Cryptography.X509Certificates;
using Newtonsoft.Json;
using Microsoft.Ajax.Utilities;
using System.Web.Services.Description;
using System.Threading.Tasks;
using System.Security.Principal;
using Microsoft.AspNetCore.Identity;
using System.Web;
using System.Web.Security;
using PlayerHelpMVC.Models;

namespace PlayerReplacement.Controllers
{
    public class HomeController : Controller
    {
        private PlayerModel playerModel = new PlayerModel();


        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }
        public ActionResult PlayerRegistration()
        {
            ViewBag.Title = "Register";

            return View();
        }

        public ActionResult Login()
        {
            ViewBag.Title = "Login";

            return View();
        }

        public ActionResult AdminLogin()
        {
            ViewBag.Title = "AdminLogin";

            return View();
        }

        [HttpPost]
        public ActionResult Authorise(PlayerModel playerModel)
        {
            using (IDbConnection ppp = new SqlConnection(LoadConnectionstring()))
            {
                var UserDetails = ppp.Query("select * from PlayerLogin where Username =  @Username and PlayerPassword = @Password", playerModel);
                if(UserDetails.Count() == 0)
                {
                    TempData["Message"] = "Username or Password is incorrect!";
                    return View("Login", playerModel);
                }
                else
                {
                    foreach ( var row in UserDetails)
                    {
                        playerModel.EmailAddress = row.EmailAddress;
                        playerModel.PlayerLoginID = row.PlayerLoginID;
                        //model.Position = row.Position;
                        playerModel.PlayerLoginID = row.PlayerLoginID;
                    }

                    Session["PlayerLoginID"] = playerModel.PlayerLoginID;
                    ViewBag.PlayerLogin = playerModel;
                    return View("Dashboard", playerModel);
                }
            }
        }

        //public ActionResult Players(PlayerModel playerModel)
        private void GetPlayers() //Created a void for storing my model inside a ViewBag which I call in my AdminDashboard
        {
            using (SqlConnection conn = new SqlConnection(LoadConnectionstring()))
            {
                string sql = "SELECT Username, Position FROM PlayerLogin";

                var model = new List<PlayerModel>();

                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn); //SqlCommand -> Reads a forward-only stream of rows from an SQL Database
                SqlDataReader rdr = cmd.ExecuteReader(); //ExecuteReader -> Sends the CommandText (Get's or Set's the SQL statement) to the Connection and builds the SqlDataReader
                while (rdr.Read())
                    {
                        var players = new PlayerModel();
                        players.Username = rdr["Username"].ToString(); //Convert the Username and Position to String so it can be read
                        players.Position = rdr["Position"].ToString();

                        model.Add(players); //The model adds the players
                    }
                ViewBag.Players = model;
            }
        }


        [HttpPost]
        public ActionResult AdminAuthorise(AdminModel adminModel)
        {
            using(IDbConnection aaa = new SqlConnection(LoadConnectionstring()))
            {
                var AdminDetails = aaa.Query("SELECT * FROM AdminUser where AdminUsername = @AdminUsername and AdminPassword = @AdminPassword",adminModel);
                if(AdminDetails.Count() == 0)
                {
                    TempData["AdminMessage"] = "Username or Password is incorrect!";
                    return View("AdminLogin", adminModel);
                }
                else
                {
                    GetPlayers();
                    return View("AdminDashboard", adminModel);
                }
            }
        }

        [HttpPost]
        public ActionResult UpdatePos(int PlayerLoginID, string PlayerPosition)
        {
            //string constr = ConfigurationManager.ConnectionStrings["Constring"].ConnectionString;
            using (SqlConnection con = new SqlConnection(LoadConnectionstring()))
            {
                //string query = "INSERT INTO PlayerLogin (Position) VALUES (@Position)";
                //query += "SELECT SCOPE_IDENTITY()";
                string query = "UPDATE PlayerLogin SET Position = @Position WHERE PlayerLoginID = @PlayerLoginID";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@Position", PlayerPosition); //Adds Parameters at the end of the SQLParameterCollection (Represents a collection of parameters)
                    cmd.Parameters.AddWithValue("@PlayerLoginID", PlayerLoginID);
                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery(); //Executes an SQL statement and returns a number of row affected
                }
                ViewData["Updated"] = "Position has been updated to " + PlayerPosition;
                return View("Dashboard", playerModel);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        
        public ActionResult PlayerRegistration(PlayerModel model)
        {

            if (ModelState.IsValid)
            {
               int recordsCreated = CreatePlayer(model.PlayerLoginID, model.Username, model.EmailAddress, model.Password);
                return RedirectToAction("Index");
            }

            using (IDbConnection cnn = new SqlConnection(LoadConnectionstring()))
            {
                cnn.Execute("insert into PlayerLogin (PlayerLoginID, Username, EmailAddress, PlayerPassword ) values (@PlayerLoginID, @Username, @EmailAddress, @PlayerPassword)", model);
            }
            TempData["Registration"] = "You have successfully registered ";
            return View();            
        }


        private static string LoadConnectionstring(string id = "PlayerHelpDB")
        {
            return ConfigurationManager.ConnectionStrings[id].ConnectionString;
        }

        public ActionResult Logout()
        {
            //Session.Abandon();
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");

        }
    }
}