using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcMovie.ViewModel;
using MvcMovie.Models;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Web.Hosting;
using Microsoft.Owin.Security;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Web.Security;
using System.Data.Entity.Validation;
using System.Data.Entity.Infrastructure;
using Microsoft.Ajax.Utilities;




//using business



namespace MvcMovie.Controllers
{
    public class HomeController : Controller
    {
        [Authorize(Roles = "adminstrator")]
        // GET: Login
        public ActionResult Index()
        {

            return RedirectToAction("LandPage");
        }
        public ActionResult Login() {
            return View();
        }
        [HttpPost]
        public ActionResult Login(AppUser lg, string returnUrl, MvcMovie.ViewModel.User userModel) {
            VideoAppPOCEntities2 ue = new VideoAppPOCEntities2();
            var dataItem = ue.AppUsers.Where(x => x.userName == lg.userName && x.passWord == lg.passWord).FirstOrDefault();
            if(dataItem != null)
            {
                FormsAuthentication.SetAuthCookie(dataItem.userName, false);
                Session["ID"] = dataItem.UserID.ToString();
                if(Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                    && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                {
                    return Redirect(returnUrl);
                }
                else
                {
                    return RedirectToAction("LandPage");
                }
            }
            else
            {
                userModel.LoginErrorMessage = "wrong username or password";
                ModelState.AddModelError("", "invalid user/pass");
                return View(userModel);
            }
           
        }
        public ActionResult LandPage()
        {
            List<table> list = new List<table>();

            using (VideoAppPOCEntities2 dc = new VideoAppPOCEntities2())
            {
                if (User.Identity.IsAuthenticated)
                {
                    if(User.IsInRole("adminstrator"))
                    {
                        var a = (from all in dc.AppVideos
                                 select new table
                                 {
                                     videoID = all.VideoID,
                                     title = all.VideoTitle,
                                     subject = all.VideoSubject,
                                     du = all.uploadDate ?? DateTime.Now
                                 });
                        list = a.ToList();
                                 
                    }
                    else
                    {
                        var f = Convert.ToInt32(Session["ID"]);
                        var v = (from a in dc.AppVideoUsers
                                 where a.UserID == f
                                 join b in dc.AppVideos on a.VideoID equals b.VideoID
                                 select new table
                                 {
                                     videoID = a.VideoID,
                                     userID = a.UserID,
                                     title = b.VideoTitle,
                                     subject = b.VideoSubject,
                                     du = b.uploadDate ?? DateTime.Now
                                 });
                        list = v.ToList();
                    }
                }
             }
         return View(list);
        }
   

        [HttpPost]
        public ActionResult saveuser(int id, string propertyName, string value) {
            var status = false;
            var message = "";
            //update data to database
            using (VideoAppPOCEntities2 dc = new VideoAppPOCEntities2()) {
                var user = dc.AppUsers.Find(id);
               
                if(user != null) {
                    
                        dc.Entry(user).Property(propertyName).CurrentValue = value;//MAKE CHANGE FOR THE AppVideos to AppUsers
                        dc.SaveChanges();
                        status = true;
                   
                   
                 
                }
               
                else {
                    message = "error!";
                }
            }
                var response = new { value = value, status = status, message = message };
            JObject o = JObject.FromObject(response);
            return Content(o.ToString());
        }
        public ActionResult savevideo(int id, string propertyName, string value)
        {
            var status = false;
            var message = "";
            //update data to database
            using (VideoAppPOCEntities2 dc = new VideoAppPOCEntities2())
            {
                var user = dc.AppVideos.Find(id);

                if (user != null)
                {

                    dc.Entry(user).Property(propertyName).CurrentValue = value;//MAKE CHANGE FOR THE AppVideos to AppUsers
                    dc.SaveChanges();
                    status = true;



                }

                else
                {
                    message = "error!";
                }
            }
            var response = new { value = value, status = status, message = message };
            JObject o = JObject.FromObject(response);
            return Content(o.ToString());
        }
        public ActionResult upload()
        {
            VideoAppPOCEntities2 db = new VideoAppPOCEntities2();
          
            var getDoctorName = db.AppUsers.ToList();
            SelectList list = new SelectList(getDoctorName, "UserID", "userName");
            ViewBag.getDoctor = list;
            return View();
        }
        [HttpPost]
        public ActionResult upload(AppVideo videoModel)
        {

            ViewBag.Message = "this page is for uploading";
            string fileName = Path.GetFileNameWithoutExtension(videoModel.VideoFile.FileName);
            string extension = Path.GetExtension(videoModel.VideoFile.FileName);
            fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
            videoModel.VideoPath = "~/Video/" + fileName;
            fileName = Path.Combine(Server.MapPath("~/Video/"), fileName);
            videoModel.VideoFile.SaveAs(fileName);
            using (VideoAppPOCEntities2 db = new VideoAppPOCEntities2())
            {

                db.AppVideos.Add(videoModel);

                try
                {
                  db.SaveChanges();
                }
            catch(DbUpdateException e)
                {
                    Console.WriteLine(e);
                }
              

            }
            ModelState.Clear();
            return RedirectToAction("LandPage");
        }
   


        public ActionResult ViewVideo(int id)
        {
            AppVideo videoModel = new AppVideo();
            using (VideoAppPOCEntities2 db = new VideoAppPOCEntities2())
            {
                videoModel = db.AppVideos.Where(x => x.VideoID == id).FirstOrDefault();

            }
            return View(videoModel);
        }
        public ActionResult DeleteVideo(int id)
        {

            businessLayer bl = new businessLayer();
            bl.updateVideoUserTb(id);
            bl.DeleteVideoInfo(id);
           
            return RedirectToAction("LandPage");
        }

        [Authorize]
        [AllowAnonymous]
        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Home");
        }
        

    }
    }
