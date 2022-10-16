using System;   
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using OnRoadHelp.Models;
namespace OnRoadHelp.Controllers
{
    public class HomeController : Controller
    {
        OnRoadHelpEntities1 db = new OnRoadHelpEntities1();
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Userlogin()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Userlogin(User u)
        {
            var obj = db.Users.Where(a => a.Email.Equals(u.Email) && a.Password.Equals(u.Password)).FirstOrDefault();
            if (obj != null)
            {
                Session["userid"] = obj.id.ToString();
                Session["First_Name"] = obj.First_Name.ToString();
                Session["Email"] = obj.Email.ToString();
                System.Diagnostics.Debug.WriteLine(Session["userid"]);
                System.Diagnostics.Debug.WriteLine(Session["Email"]);
                return RedirectToAction("UserDashboard");
            }
            ViewBag.res = "No Record Found";
            return View(u);

        }
        public ActionResult Usersignup()
        {
            return View();
        }
        //database may values dalnay k liyai // post use kartay hai insertion k liyai

        [HttpPost]
        public ActionResult Usersignup(User u)
        {
            if (ModelState.IsValid)
            {

                try
                {

                    db.Users.Add(u);
                    db.SaveChanges();
                    Response.Write("<script>alert('record save')</script>");
                    return RedirectToAction("Userlogin");
                }
                catch (Exception ex)
                {
                    return View();
                }
            }
            return View();
        }
        public ActionResult UserForgotpassword()
        {
            return View();

        }
        public ActionResult Userdashboard()
        {
            if (Session["id"] == null && Session["email"] == null)
            {
                return RedirectToAction("Userlogin");
            }
            return View();
        }
        public ActionResult reqHelpuser()
        {
            if (Session["id"] == null && Session["email"] == null)
            {
                return RedirectToAction("Userlogin");
            }
            return View();
        }
        public ActionResult userprofile()
        {
            if (Session["id"] == null && Session["email"] == null)
            {
                return RedirectToAction("Userlogin");
            }
            return View();
        }
        public ActionResult usermap()
        {
            if (Session["userid"] != null && Session["Email"] != null)
            {
                string markers = "[";
                List<AddService> li = db.AddServices.ToList();
                foreach (var a in li)
                {
                    markers += "{";
                    markers += string.Format("'title': '{0}',", a.Service_Name);
                    markers += string.Format("'lat': '{0}',", a.lat);
                    markers += string.Format("'lng': '{0}',", a.lon);
                    markers += "},";
                }
                markers += "];";
                ViewBag.Markers = markers;
                return View();

            }
            else
            {
                return RedirectToAction("Userlogin");
            }
        }
        // nearest 
        [HttpPost]
        public ActionResult usermap(HelpRequest newdata)
        {
            if (Session["userid"] != null && Session["Email"] != null)
            {
                newdata.status = "Pending";
                newdata.DateTime = DateTime.Now;
                int login = Convert.ToInt32(Session["userid"]);
                newdata.u_id = login;
                var services = db.AddServices.Where(s => s.Service_Type == newdata.u_pcategory);
                if (services.Count() > 0)
                {
                    var p = services.First();
                    newdata.ResponderId = p.urid;
                    var dist = Utility.distance(newdata.lat.Value, newdata.lon.Value, p.lat.Value, p.lon.Value, 'K');
                    foreach (var item in services)
                    {
                        var dist1 = Utility.distance(newdata.lat.Value, newdata.lon.Value, item.lat.Value, item.lon.Value, 'K');
                        if (dist1 < dist)
                        {
                            dist = dist1;
                            newdata.ResponderId = item.urid;
                        }
                    }
                }
                db.HelpRequests.Add(newdata);
                db.SaveChanges();
                Response.Write("<script>alert('record save')</script>");
                return RedirectToAction("UserDashboard");
            }
            else
            {
                return RedirectToAction("Userlogin");
            }
        }

        public ActionResult userhistory()
        {
            if (Session["id"] == null && Session["email"] == null)
            {
                return RedirectToAction("Userlogin");
            }
            return View();
        }

        // Responder show Request
        public ActionResult ToRespond()
        {
            if (Session["userid"] == null && Session["Email"] == null)
            {

                return RedirectToAction("Login");
            }
            else
            {
                var stat = "Pending";
                int login = Convert.ToInt32(Session["userid"]);
                List<HelpRequest> al = db.HelpRequests.Where(h => h.ResponderId == login && h.status != "Complete").ToList();
                return View(al);
            }
        }
        // Resonder Add service
        public ActionResult Responderaddservice()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Responderaddservice(AddService A)
        {
            if (ModelState.IsValid)
            {
                float data1 = float.Parse(TempData["key1"].ToString());
                float data2 = float.Parse(TempData["key2"].ToString());
                var t = new AddService //Make sure you have a table called test in DB
                {
                    Service_Name = A.Service_Name,
                    Service_Type = A.Service_Type,
                    DateTime = DateTime.Now,
                    lat = data1,
                    lon = data2,
                    urid = Convert.ToInt32(Session["userid"].ToString()),
                };
                db.AddServices.Add(t);
                db.SaveChanges();
                Response.Write("<script> alert('Record saved!');</script>");
                TempData["key1"] = "";
                TempData["key2"] = "";
                return RedirectToAction("Responderaddservice");
            }
            return View();
        }

        // map
        public ActionResult Responderadd()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Responderadd(AddService a)
        {
            TempData.Add("key1", a.lat);
            TempData.Add("key2", a.lon);
            Response.Write("<script> alert('Record saved!');</script>");
            return RedirectToAction("Responderaddservice");
        }
        //This will return list or display all add services
        [HttpGet]
        public ActionResult displayMyservices()
        {
            if (Session["userid"] != null && Session["Email"] != null)
            {
                int login = Convert.ToInt32(Session["userid"]);
                List<AddService> req = db.AddServices.Where(r => r.urid == login).ToList();

                return View(req);
            }
            else
            {
                return RedirectToAction("Userlogin");
            }
        }
        //Deleting
        [HttpGet]
        public ActionResult DeleteService(int aid)
        {

            var service = db.AddServices.FirstOrDefault(a => a.id == aid);
            if (service != null)
                Response.Write("<script> alert('Not Found!');</script>");
            db.AddServices.Remove(service);
            db.SaveChanges();
            return RedirectToAction("displaymyservices");
        }
        // distance map in responder  //
        [HttpGet]
        public ActionResult Responderviewmap()
        {
            if (Session["userid"] != null && Session["Email"] != null)
            {
                int login = Convert.ToInt32(Session["userid"]);
                string markers = "[";
                List<HelpRequest> li = db.HelpRequests.Where(h => h.ResponderId == login).ToList();
                foreach (var a in li)
                {
                    markers += "{";
                    markers += string.Format("'title': '{0}',", a.u_id);
                    markers += string.Format("'lat': '{0}',", a.lat);
                    markers += string.Format("'lng': '{0}',", a.lon);
                    markers += string.Format("'u_problem': '{0}'", a.u_problem);
                    markers += "},";
                }
                markers += "];";
                ViewBag.Markers = markers;
                return View();
            }
            else
            {
                return RedirectToAction("Userlogin");
            }
        }
        
        [HttpGet]
        public ActionResult Userviewmap()
        {

            if (Session["userid"] != null && Session["Email"] != null)
            {
                int login = Convert.ToInt32(Session["userid"]);
                string markers = "[";
                List<HelpRequest> li = db.HelpRequests.Where(h => h.u_id == login).ToList();
                foreach (var a in li)
                {
                    markers += "{";
                    markers += string.Format("'title': '{0}',", a.ResponderId);
                    markers += string.Format("'lat': '{0}',", a.lat);
                    markers += string.Format("'lng': '{0}',", a.lon);
                    markers += string.Format("'u_problem': '{0}'", a.u_problem);
                    markers += "},";

                }
                markers += "];";
                ViewBag.Markers = markers;
                return View();

            }
            else
            {
                return RedirectToAction("Userlogin");
            }
        }
        //
        public ActionResult ContactUs()
        {
            return View();

        }
        // accept reject // code//

        [HttpGet]
        public ActionResult RespondRequest(int helpId, string type)
        {
            //type: Accept, Reject
            var req = db.HelpRequests.FirstOrDefault(r => r.status == "Pending" && r.Helpid == helpId);
            req.status = "Accept";
            if (type.Equals("Reject"))
            {
                req.status = "Pending";
                var id = req.ResponderId;
                var services = db.AddServices.Where(s => s.Service_Type == req.u_pcategory && s.urid != id);
                if (services.Count() > 0)
                {
                    var p = services.First();
                    req.ResponderId = p.urid;
                    var dist = Utility.distance(req.lat.Value, req.lon.Value, p.lat.Value, p.lon.Value, 'K');
                    foreach (var item in services)
                    {
                        if (item.urid == id)
                            continue;

                        var dist1 = Utility.distance(req.lat.Value, req.lon.Value, item.lat.Value, item.lon.Value, 'K');
                        if (dist1 < dist)
                        {
                            dist = dist1;
                            req.ResponderId = item.urid;
                        }
                    }
                }
            }
            else
            {
                req.status = "Accept";
                var assigned = new HelpRequestAssigned
                {
                    AssignedTime = DateTime.Now,
                    HelpRequestId = req.Helpid,
                    UserId = req.ResponderId,
                    StatusFlag = "Accept"
                };

                db.HelpRequestAssigneds.Add(assigned);
            }
            db.SaveChanges();
            return RedirectToAction("ToRespond");
        }


        // user ny jo Request ki help  kaliya 

        [HttpGet]
        public ActionResult HelpHistory()
        {

            if (Session["userid"] != null && Session["Email"] != null)
            {
                int login = Convert.ToInt32(Session["userid"]);
                List<HelpRequest> li = db.HelpRequests.Where(a => a.u_id == login && a.status != "Complete").ToList();

                return View(li);
            }
            else
            {
                return View();

            }
        }

        // ya time kaliya function hy kitny time  tak tolenerence rakhna hy 
        [HttpPost]
        public ActionResult SolveNotRespondingRequests()
        {
           
            
                var acceptedRequests = db.HelpRequests.Where(r => r.status == "Accept");

                foreach (var req in acceptedRequests)
                {
                    var assigned = db.HelpRequestAssigneds.FirstOrDefault(d => d.HelpRequestId == req.Helpid && d.StatusFlag == "Accept");
                    if (assigned != null)
                    {
                        var availableService = db.AvailableServices.FirstOrDefault(s => s.Name == req.u_problem);
                        if (availableService != null)
                        {
                            var timeDiff = DateTime.Now.Subtract(assigned.AssignedTime.Value).TotalMinutes;
                            if (timeDiff > availableService.ToleranceTime)
                            {
                            //now search for new Responder.
                            ModifyRequest(req.Helpid, req.u_id.Value);


                        }
                        }
                    }
                }
            
            db.SaveChanges();
                {
                    return RedirectToAction("ToRespond");
                }
         
        }
    

        //


        // userCompleteTaskHistory//
        [HttpGet]
        public ActionResult userCompleteTaskHistory()
        {

            if (Session["userid"] != null && Session["Email"] != null)
            {
                int login = Convert.ToInt32(Session["userid"]);
                List<HelpRequest> li = db.HelpRequests.Where(a => a.u_id == login && a.status == "Complete").ToList();

                return View(li);
            }
            else
            {
                return View();

            }
        }
        // 
        //Deleting in User History
        [HttpGet]
        public ActionResult Deletehistoryuser(int aid)
        {

            var history = db.HelpRequests.FirstOrDefault(a => a.u_id == aid);
            if (history != null)
                Response.Write("<script> alert('Not Found!');</script>");
            db.HelpRequests.Remove(history);
            db.SaveChanges();
            return RedirectToAction("userCompleteTaskHistory");
        }
        //
       
        //

        [HttpGet]
        public ActionResult ResponderCompleteTaskHistory()
        {
            if (Session["userid"] != null && Session["Email"] != null)
            {
                int login = Convert.ToInt32(Session["userid"]);
                List<HelpRequest> li = db.HelpRequests.Where(a => a.ResponderId == login && a.status == "Complete").ToList();
                return View(li);
            }
            else
            {
                return View();

            }
        }
        //
        // 
        //Deleting in Responder History
        [HttpGet]
        public ActionResult DeletehistoryResponder(int respid)
        {

            var history = db.HelpRequests.FirstOrDefault(a => a.ResponderId == respid);
            if (history != null)
                Response.Write("<script> alert('Not Found!');</script>");
            db.HelpRequests.Remove(history);
            db.SaveChanges();
            return RedirectToAction("ResponderCompleteTaskHistory");
        }
        //}
        //This is for complete button code
        [HttpGet]
        public ActionResult CompleteTask(int helpId)
        {
            //type: Complete
            var req = db.HelpRequests.FirstOrDefault(r => r.status == "Accept" && r.Helpid == helpId);
            req.status = "Complete";
            ViewBag.helpid = helpId;

            db.SaveChanges();

            return RedirectToAction("AddUserRating", new { helpId = helpId });
        }

        //[HttpGet]
        public ActionResult AddUserRating(int helpid)
        {

            ViewData["helpid"] = helpid;

            return View();
        }
        //Add User Rating
        [HttpPost]
        public ActionResult AddUserRating(int Helpid, int rating1)
        {
          
            HelpRequest result = db.HelpRequests.Where(x => x.Helpid == Helpid).FirstOrDefault();
            result.RespRating = float.Parse(rating1.ToString());

            db.SaveChanges();
            Response.Write("<script> alert('Rating saved!');</script>");
            return RedirectToAction("userCompleteTaskHistory");
        }
        //  Add Responder Rating
        [HttpGet]
        public ActionResult AddRespRating(int helpid)
        {
            return View();
        }
        //Add User Rating
        [HttpPost]
        public ActionResult AddRespRating(int Helpid, int rating1)
        {
            // string rating = Request.Form["ratingHidden"];
            //HelpRequest result = (from p in db.HelpRequests
            //  where p.Helpid == Helpid
            //select p).SingleOrDefault();
            HelpRequest result = db.HelpRequests.Where(x => x.Helpid == Helpid).FirstOrDefault();
            result.UserRating = float.Parse(rating1.ToString());
            db.SaveChanges();
            Response.Write("<script> alert('Rating saved!');</script>");
            return RedirectToAction("ResponderCompleteTaskHistory");
        }
        // Summary task//
        [HttpGet]
        public ActionResult GetSummary()
        {
            int id = Convert.ToInt32(Session["userid"]);
            var data = db.HelpRequests.Where(v => v.ResponderId == id).GroupBy(v => v.u_pcategory)
                 .Select(g => new servicesummary
                 {
                     Service_Type = g.Key,
                     count = g.Count()
                 });
            return View(data);
        }
      
        //

        // Date function multiple  date to and from 

        [HttpPost]
        public ActionResult GetSummary(DateTime dfrom, DateTime dto)
        {
            int id = Convert.ToInt32(Session["userid"]);
            
            var data = db.HelpRequests.Where(v => v.ResponderId == id && v.DateTime<=dto.Date &&v.DateTime>=dfrom).GroupBy(v => v.u_pcategory)
                 .Select(g => new servicesummary
                 {
                     Service_Type = g.Key,
                     count = g.Count()
                 });
            return View(data);
        }
        //
        //  time tolerence wala ya function hy  30 or 20 ma accept kary to
        // thk warna ya cncel ho gy dosry ko assign ho jay gi 
        public RedirectToRouteResult ModifyRequest(int hid, int exId)
        {   
                HelpRequest newdata = db.HelpRequests.FirstOrDefault(h => h.Helpid == hid);

                newdata.status = "Pending";
                var services = db.AddServices.Where(s => s.Service_Type == newdata.u_pcategory && s.urid != exId);
                if (services.Count() > 0)
                {
                    var p = services.First();
                    newdata.ResponderId = p.urid;
                    var dist = Utility.distance(newdata.lat.Value, newdata.lon.Value, p.lat.Value, p.lon.Value, 'K');
                    foreach (var item in services)
                    {
                        var dist1 = Utility.distance(newdata.lat.Value, newdata.lon.Value, item.lat.Value, item.lon.Value, 'K');
                        if (dist1 < dist)
                        {
                            dist = dist1;
                            newdata.ResponderId = item.urid;
                        }

                    }
                }
                db.SaveChanges();
                {
                    return RedirectToAction("");
                }

            }



    }
}
                    
