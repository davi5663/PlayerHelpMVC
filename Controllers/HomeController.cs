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

        [HttpPost]
        public ActionResult Authorise(PlayerModel playerModel)
        {
            using (IDbConnection ppp = new SqlConnection(LoadConnectionstring()))
            {
                var UserDetails = ppp.Query("select * from PlayerLogin where Username =  @Username and PlayerPassword = @Password", playerModel);
                if(UserDetails.Count() == 0)
                {
                    return View("Index", playerModel);
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
                    cmd.Parameters.AddWithValue("@Position", PlayerPosition);
                    cmd.Parameters.AddWithValue("@PlayerLoginID", PlayerLoginID);
                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();
                }
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
                cnn.Execute("insert into PlayerLogin (PlayerLoginID, Username, Email, PlayerPassword ) values (@PlayerLoginID, @Username, @EmailAddress, @PlayerPassword)", model);
            }

            return View();            
        }


        private static string LoadConnectionstring(string id = "PlayerHelpDB")
        {
            return ConfigurationManager.ConnectionStrings[id].ConnectionString;
        }

        public ActionResult Logout()
        {
            Session.Abandon();
            return RedirectToAction("Index");
        }
    }
}