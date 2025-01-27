using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Login_Session.Models;
using Login_Session.Pages.DatabaseConnection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Login_Session.Pages.AdminPages
{
    public class ViewUsersModel : PageModel
    {
        [BindProperty]
        public List<User> User { get; set; }

        public List<string> URole { get; set; } = new List<string> { "User", "Admin" };
        public string UserName;
        public const string SessionKeyName1 = "username";


        public string FirstName;
        public const string SessionKeyName2 = "fname";

        public string SessionID;
        public const string SessionKeyName3 = "sessionID";
        public IActionResult OnGet()
        {
            //get the session first!
            UserName = HttpContext.Session.GetString(SessionKeyName1);
            FirstName = HttpContext.Session.GetString(SessionKeyName2);
            SessionID = HttpContext.Session.GetString(SessionKeyName3);


            DatabaseConnect dbstring = new DatabaseConnect(); //creating an object from the class
            string DbConnection = dbstring.DatabaseString(); //calling the method from the class
            Console.WriteLine(DbConnection);
            SqlConnection conn = new SqlConnection(DbConnection);
            conn.Open();

            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = conn;
                command.CommandText = @"SELECT * FROM UserTable";

                var reader = command.ExecuteReader();

                User = new List<User>();
                while (reader.Read())
                {
                    User Row = new User(); //each record found from the table
                    Row.Id = reader.GetInt32(0);
                    Row.FirstName = reader.GetString(1);
                    Row.UserName = reader.GetString(2);
                    Row.Role = reader.GetString(4); // We dont get the password. The role field is in the 5th position
                    User.Add(Row);
                }
                
            }
            return Page();

        }
    }
}
