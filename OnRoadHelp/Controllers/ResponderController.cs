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
//    public class ResponderController : Controller
//    {
//        //Sql Connection
//        static string constr = ConfigurationManager.ConnectionStrings["constringg"].ConnectionString;
//        SqlConnection con = new SqlConnection(constr);
//        int noti = 0;


//        // GET: Responder
//        public ActionResult Responderlogin()
//        {
//            return View();
//        }
//        [HttpPost]
//        public ActionResult Responderlogin(Userlogin u)
//        {
//            con.Open();
//            SqlCommand cmd = new SqlCommand("select * from responderlogin where email='" + u.Email + "' and password='" + u.Password + "'", con);
//            SqlDataReader dr = cmd.ExecuteReader();
//            if (dr.Read())
//            {
//                Session["id"] = dr["id"].ToString();
//                Session["FName"] = dr["FName"].ToString();
//                Session["email"] = dr["Email"].ToString();



//                return RedirectToAction("Responderdashboard");
//            }
//            else
//            {
//                dr.Close();
//                ViewBag.res = "No Record Found";
//            }

//            return View();
//        }
//        public ActionResult Respondersignup()
//        {
//            return View();
//        }
//        [HttpPost]
//        public ActionResult Respondersignup(Userlogin r)
//        {
//            if (ModelState.IsValid)
//            {
//                SqlConnection con = new SqlConnection(constr);
//                con.Open();
//                string q = "insert into responderlogin values ('" + r.FName + "','" + r.LName + "','" + r.Email + "','" + r.Password + "','" + r.PhoneNumber + "')";
//                SqlCommand cmd = new SqlCommand(q, con);
//                cmd.ExecuteNonQuery();
//                con.Close();
//                Response.Write("<script>alert('record save')</script>");
//                return RedirectToAction("Responderlogin");
//            }
//            return View();
//        }
//        public ActionResult ResponderForgetpassword()
//        {
//            return View();
//        }
//        public ActionResult Responderdashboard()
//        {
//            // List<Locations> lh = getHelpRequest();
//            return View();
//        }
//        public ActionResult Responderaddservice()
//        {
//            return View();
//        }

//        [HttpPost]
//        public ActionResult Responderaddservice(AddService A)
//        {
//            if (ModelState.IsValid)
//            {
//                float data1 = float.Parse(TempData["key1"].ToString());
//                float data2 = float.Parse(TempData["key2"].ToString());
//                A.Status = 0;
//                SqlConnection con = new SqlConnection(constr);
//                con.Open();
//                //Response.Write("<script> alert('Connect with server!');</script>");
//                string query = "insert into AddService(ServiceName,ServiceLoaction,ServiceType,rid,Status,Lat,Lang) values ('" + A.ServiceName + "','" + A.ServiceLocation + "','" + A.ServiceType + "','" + Session["id"] + "','" + A.Status + "','" + data1 + "','" + data2 + "')";
//                SqlCommand cmd = new SqlCommand(query, con);
//                cmd.ExecuteNonQuery();
//                con.Close();
//                Response.Write("<script> alert('Record saved!');</script>");

//                TempData["key1"] = "";
//                TempData["key2"] = "";

//                return RedirectToAction("Responderaddservice");

//            }
//            return View();
//        }


//        public ActionResult Logout()
//        {
//            Session["id"] = null;
//            Session["FName"] = null;
//            Session["email"] = null;
//            Session.Abandon();

//            return RedirectToAction("ResponderLogin");
//        }
//        public ActionResult Responderviewmap()
//        {
//            con.Open();
//            // int v = 0;
//            string markers = "[";
//            string query = "SELECT uid,Lattitude,Longtitude,u_problem FROM Location";
//            SqlCommand cmd = new SqlCommand(query, con);
//            SqlDataReader sdr = cmd.ExecuteReader();
//            {
//                while (sdr.Read())
//                {
//                    markers += "{";
//                    markers += string.Format("'title': '{0}',", sdr["uid"]);
//                    markers += string.Format("'lat': '{0}',", sdr["Lattitude"]);
//                    markers += string.Format("'lng': '{0}',", sdr["Longtitude"]);
//                    markers += string.Format("'u_problem': '{0}'", sdr["u_problem"]);
//                    markers += "},";
//                }
//            }
//            con.Close();
//            markers += "];";
//            ViewBag.Markers = markers;
//            return View();
//        }
//        public ActionResult Responderadd()
//        {
//            return View();
//        }

//        [HttpPost]
//        public ActionResult Responderadd(AddService A)
//        {
//            TempData.Add("key1", A.Lat);
//            TempData.Add("key2", A.lang);

//            Response.Write("<script> alert('Record saved!');</script>");
//            return RedirectToAction("Responderaddservice");


//        }

//        //Start Pending Services Code -- Displaying

//        //End Pending Services Code -- Displaying

//        //Function for Displaying User help Request 
//        //Start
//        private List<Locations> getHelpRequest()
//        {

//            List<Locations> alist = new List<Locations>();
//            SqlConnection con = new SqlConnection(constr);
//            con.Open();
//            //string q = "select a.FirstName+' '+a.LastName+'('+a.empid+')' as [Full Name],b.leaveType,b.postingdate,b.status from AllLeave_Tbl b inner join employee a on a.id = b.eid";
//            string q = "SELECT Location.id as lid,userlogin.FName,userlogin.id,Location.u_problem,Location.complete,Location.DateTime,Location.status,ROW_NUMBER() over (order by Location.id) as [#] from Location join userlogin on Location.uid=userlogin.id where Location.complete=0";
//            SqlCommand cmd = new SqlCommand(q, con);
//            SqlDataReader sdr = cmd.ExecuteReader();
//            while (sdr.Read())
//            {
//                Locations a = new Locations();
//                a.ID = int.Parse(sdr["lid"].ToString());
//                a.FullName = sdr["FName"].ToString();
//                a.u_problem = sdr["u_problem"].ToString();
//                a.locid = int.Parse(sdr["#"].ToString());
//                a.DateTime = sdr["DateTime"].ToString();
//                a.stat = int.Parse(sdr["status"].ToString());
//                a.complete = int.Parse(sdr["complete"].ToString());


//                alist.Add(a);
//                noti++;
//                TempData["data"] = noti;
//            }
//            con.Close();
//            return alist;
//        }
//        public ActionResult helpRequestHistoryResponder()
//        {
//            if (Session["id"] == null && Session["email"] == null)
//            {

//                return RedirectToAction("Responderlogin");
//            }
//            else
//            {

//                List<Locations> lh = getHelpRequest();
//                return View(lh);
//            }
//        }
//        //End
//        /// <summary>
//        /// update Query 
//        /// </summary>
//        /// <param name="aid"></param>
//        /// <returns></returns>

//        [HttpGet]
//        public ActionResult AcceptRequest(int aid)
//        {
//            Locations a = new Locations();
//            a.stat = 1;
//            SqlConnection con = new SqlConnection(constr);
//            con.Open();
//            SqlCommand cmd = new SqlCommand("update Location set status='" + a.stat + "' where id ='" + aid + "' ", con);
//            cmd.ExecuteNonQuery();

//            con.Close();

//            return RedirectToAction("helpRequestHistoryResponder");
//        }
//        [HttpGet]
//        public ActionResult completetask(int aid)
//        {
//            Locations a = new Locations();
//            a.complete = 1;
//            SqlConnection con = new SqlConnection(constr);
//            con.Open();
//            SqlCommand cmd = new SqlCommand("update Location set complete='" + a.complete + "' where id ='" + aid + "' ", con);
//            cmd.ExecuteNonQuery();
//            con.Close();

//            return RedirectToAction("helpRequestHistoryResponder");
//        }
//        // Upadte Queries 
//        [HttpGet]
//        public ActionResult RejectRequest(int aid)
//        {
//            Locations a = new Locations();
//            a.stat = 2;
//            SqlConnection con = new SqlConnection(constr);
//            con.Open();
//            SqlCommand cmd = new SqlCommand("update Location set status='" + a.stat + "' where id ='" + aid + "' ", con);
//            cmd.ExecuteNonQuery();


//            con.Close();

//            return RedirectToAction("helpRequestHistoryResponder");
//        }



//        //Display  Responder History // List wala code hai
//        private List<Locations> displayResponderhistory()
//        {
//            List<Locations> alist = new List<Locations>();
//            SqlConnection con = new SqlConnection(constr);
//            con.Open();
//            //foreign key hai issi ko use karo 
//            string q = "SELECT Location.id as lid,userlogin.FName,userlogin.id,Location.u_problem,Location.complete,Location.DateTime,Location.status,ROW_NUMBER() over (order by Location.id) as [#] from Location join userlogin on Location.uid=userlogin.id where Location.complete=1";
//            // string r = "SELECT count(*) as NUM FROM Location GROUP BY uid";
//            SqlCommand cmd = new SqlCommand(q, con);
//            SqlDataReader sdr = cmd.ExecuteReader();
//            while (sdr.Read())
//            {
//                Locations a = new Locations();
//                //a.id = int.Parse(sdr["id"].ToString());

//                //Model                    //Database
//                a.ID = int.Parse(sdr["lid"].ToString());
//                a.FullName = sdr["FName"].ToString();
//                a.u_problem = sdr["u_problem"].ToString();
//                a.locid = int.Parse(sdr["#"].ToString());
//                a.DateTime = sdr["DateTime"].ToString();
//                a.stat = int.Parse(sdr["status"].ToString());
//                a.complete = int.Parse(sdr["complete"].ToString());


//                alist.Add(a);

//            }
//            con.Close();
//            return alist;
//        }

//        public ActionResult DisplayResponderhistory()
//        {

//            if (Session["id"] == null && Session["email"] == null)
//            {

//                return RedirectToAction("Login");
//            }
//            else
//            {

//                List<Locations> al = displayResponderhistory();
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
//            return RedirectToAction("DisplayResponderhistory");
//        }



//        //Profile Code
//        [HttpGet]
//        public ActionResult Responderprofile()
//        {
//            if (Session["id"] == null && Session["email"] == null)
//            {

//                return RedirectToAction("Responderlogin");
//            }
//            SqlConnection con = new SqlConnection(constr);
//            con.Open();
//            string q = "Select * from responderlogin where id ='" + Session["id"] + "'";
//            SqlCommand cmd = new SqlCommand(q, con);
//            SqlDataReader sdr = cmd.ExecuteReader();
//            sdr.Read();
//            Userlogin a = new Userlogin();


//            a.FName = sdr["FName"].ToString();
//            a.LName = sdr["LName"].ToString();
//            a.Email = sdr["Email"].ToString();
//            a.Password = sdr["Password"].ToString();
//            a.PhoneNumber = sdr["PhoneNumber"].ToString();

//            sdr.Close();
//            con.Close();

//            return View(a);
//        }
//        public ActionResult button()
//        {
//            return View();
//        }




//    }
//}