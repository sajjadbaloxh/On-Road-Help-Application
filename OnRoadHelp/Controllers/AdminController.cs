//using OnRoadHelp.Models;
//using System;
//using System.Collections.Generic;
//using System.Configuration;
//using System.Data.SqlClient;
//using System.Linq;
//using System.Web;
//using System.Web.Mvc;

//namespace OnRoadHelp.Controllers
//{
//    public class AdminController : Controller
//    {

//        static string constr = ConfigurationManager.ConnectionStrings["constringg"].ConnectionString;
//        SqlConnection con = new SqlConnection(constr);
//        // GET: Admin
//        public ActionResult Adminlogin()
//        {
//            return View();
//        }
//        [HttpPost]
//        public ActionResult Adminlogin(adminlogin a)
//        {
//            con.Open();
//            SqlCommand cmd = new SqlCommand("select * from adminlogin where UserName='" + a.UserName + "' and password='" + a.Password + "'", con);
//            SqlDataReader dr = cmd.ExecuteReader();
//            if (dr.Read())
//            {
//                Session["id"] = dr["id"].ToString();
//                Session["UserName"] = dr["UserName"].ToString();
//                return RedirectToAction("Admindashboard");
//            }
//            else
//            {
//                dr.Close();
//                ViewBag.res = "No Record Found";
//            }
//            return View();
//        }


//        public ActionResult AdminForgetpassword()
//        {
//            return View();
//        }
//        public ActionResult Admindashboard()
//        {
//            return View();
//        }

//        //Display All User TO Admin
//        private List<User> displayAllUser()
//        {
//            List<User> alist = new List<User>();
//            SqlConnection con = new SqlConnection(constr);
//            con.Open();

//            string q = "select * from UserLogin";
//            SqlCommand cmd = new SqlCommand(q, con);
//            SqlDataReader sdr = cmd.ExecuteReader();
//            while (sdr.Read())
//            {
//                User a = new User();
//                //a.id = int.Parse(sdr["id"].ToString());

//                //Model                    //Database
//                a.id = int.Parse(sdr["id"].ToString());
//                a.Email = sdr["Email"].ToString();
//                a.First_Name = sdr["FName"].ToString();
//                a.Last_Name = sdr["LName"].ToString();


//                alist.Add(a);
//            }
//            con.Close();
//            return alist;
//        }

//        public ActionResult displayalluser()
//        {

//            if (Session["id"] == null && Session["email"] == null)
//            {

//                return RedirectToAction("Login");
//            }
//            else
//            {

//                List<User> al = displayAllUser();
//                return View(al);
//            }

//        }

//        //Deleting

//        [HttpGet]
//        public ActionResult DeleteUser(int aid)
//        {

//            SqlConnection con = new SqlConnection(constr);
//            con.Open();
//            SqlCommand cmd = new SqlCommand("Delete from userLogin where id ='" + aid + "'", con);
//            cmd.ExecuteNonQuery();
//            con.Close();
//            return RedirectToAction("displayalluser");
//        }


//        //Display All Responder TO Admin
        
//        private List<DisplayAllResponder> displayAllResponder()
//        {
//            List<DisplayAllResponder> alist = new List<DisplayAllResponder>();
//            SqlConnection con = new SqlConnection(constr);
//            con.Open();

//            string q = "SELECT AddService.id as aid,AddService.ServiceName,AddService.ServiceLoaction,AddService.ServiceType,AddService.lat, AddService.lang,responderlogin.id as urid,responderlogin.FName+' '+responderlogin.LName as [Full Name], responderlogin.Email FROM AddService JOIN responderlogin ON AddService.rid = responderlogin.id";

//            SqlCommand cmd = new SqlCommand(q, con);
//            SqlDataReader sdr = cmd.ExecuteReader();
//            while (sdr.Read())
//            {
//                DisplayAllResponder a = new DisplayAllResponder();
//                //a.id = int.Parse(sdr["id"].ToString());

//                //Model        //Database
//                //a.id = int.Parse(sdr["aid"].ToString());
//                a.id = int.Parse(sdr["aid"].ToString());
//                a.FullName = sdr["Full Name"].ToString();
//                a.Email = sdr["Email"].ToString();
//                a.ServiceName = sdr["ServiceName"].ToString();
//                a.ServiceType = sdr["ServiceType"].ToString();
//                a.ServiceLocation = sdr["ServiceLoaction"].ToString();
//                a.Latval = float.Parse(sdr["lat"].ToString());
//                a.Lngval = float.Parse(sdr["lang"].ToString());

//                alist.Add(a);
//            }
//            con.Close();
//            return alist;
//        }
//        [HttpGet]

//        public ActionResult displayallresponder()
//        {

//            if (Session["id"] == null && Session["email"] == null)
//            {

//                return RedirectToAction("AdminLogin");
//            }
//            else
//            {

//                List<DisplayAllResponder> al = displayAllResponder();
//                return View(al);

//            }
//        }
//        //Deleting

//        [HttpGet]
//        public ActionResult DeleteResponder(int aid)
//        {

//            SqlConnection con = new SqlConnection(constr);
//            con.Open();
//            SqlCommand cmd = new SqlCommand("Delete from AddService where id ='" + aid + "'", con);
//            cmd.ExecuteNonQuery();
//            con.Close();
//            return RedirectToAction("displayallresponder");
//        }

//        public ActionResult practise()
//        {
//            return View();
//        }
//    }

//}