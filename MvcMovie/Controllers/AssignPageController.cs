using MvcMovie.Models;
using MvcMovie.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;


namespace MvcMovie.Controllers
{
    public class AssignPageController : Controller
    {
        // GET: AssignPage
        public ActionResult Index()
        {
            return View();

        }
        public ActionResult AssignOnVideo()
        {
            VideoAppPOCEntities2 db = new VideoAppPOCEntities2();
            UserModel ud = new UserModel();

            var model = (from r in db.AppUsers
                         where r.RoleID == 2
                         select new CheckBoxView
                         {
                             UserId = r.UserID,
                             userName = r.userName,
                             RoleId = r.RoleID,
                             //VideoID = a.VideoID
                         });
          
                var data = model.ToList();
            CheckBoxView cbv = new CheckBoxView();
            cbv.users = data;

            var usersData = db.AppUsers.ToList();

            var getVideoName = db.AppVideos.ToList();
            if (getVideoName == null)
            {
                TempData["msg"] = "no video can be assign";
            }
            else
            {
                try
                {
                    var selectedVideoId = 0;
                    if (TempData["redirectedVideoId"] != null && TempData["redirectedVideoId"].ToString() != "")
                    {
                        selectedVideoId = Convert.ToInt32(TempData["redirectedVideoId"]);
                        ViewBag.curretSelectedVideoId = selectedVideoId;
                        ud.VideoID = Convert.ToInt32(TempData["redirectedVideoId"]);
                    }
                    else
                    {
                        selectedVideoId = getVideoName[0].VideoID;
                        ud.VideoID = getVideoName[0].VideoID;
                    }

                    var checkedData = (from r in db.AppVideoUsers
                                       where r.VideoID == selectedVideoId
                                       select r).ToList();

                    ud.users = usersData;

                    foreach (var item in cbv.users)
                    {
                        foreach (var checkItem in checkedData)
                        {
                            if (item.UserId == checkItem.UserID)
                            {
                                item.isChecked = true;
                            }
                        }
                    }
                }
                catch (ArgumentOutOfRangeException e)
                {
                    Console.WriteLine(e);
                }
            }
            SelectList list = new SelectList(getVideoName, "VideoID", "VideoTitle");
            
            ViewBag.getVideo = list;
         
            return View(cbv);

        }
        [HttpPost]
        public ActionResult AssignOnVideo(UserModel um)
        {
            VideoAppPOCEntities2 db = new VideoAppPOCEntities2();
            if(ModelState.IsValid)
            {
                db.AppVideoUsers.RemoveRange(db.AppVideoUsers.Where(x => x.VideoID == um.VideoID));
                db.SaveChanges();

                foreach (var userID in um.users.Where(x => x.ischecked).Select(x => x.UserID))
                {
                    var appVideoUser = new AppVideoUser
                    {
                        UserID = userID,
                        VideoID = um.VideoID
                    };
                    db.AppVideoUsers.Add(appVideoUser);
                }
                db.SaveChanges();
                TempData["redirectedVideoId"] = um.VideoID;
                return RedirectToAction("AssignOnVideo");
            }
            return View(um);
        }

        [HttpPost]
        public string GetUserListByVideoId(GetListVideosPostRequest req)
        {

            VideoAppPOCEntities2 db = new VideoAppPOCEntities2();
            UserModel ud = new UserModel();
            var reqVideoId = Convert.ToInt32(req.VideoId);
            var data = db.AppUsers.ToList();
            var checkedData = (from r in db.AppVideoUsers
                               where r.VideoID == reqVideoId
                               select r).ToList();

            ud.users = data.Where(user => user.RoleID == 2).ToList();
            var jsonObject = new List<GetListVideosPostResult>();

            foreach (var item in ud.users)
            {
                foreach (var checkItem in checkedData)
                {
                    if (item.UserID == checkItem.UserID)
                    {
                        item.ischecked = true;
                    }
                }
                jsonObject.Add(new GetListVideosPostResult
                {
                    UserId = item.UserID,
                    IsChecked = item.ischecked
                });
            }

            System.Web.Script.Serialization.JavaScriptSerializer oSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            string sJSON = oSerializer.Serialize(jsonObject);

            return sJSON;
        }

        //[HttpGet]
        public ActionResult AssignOnUser()
        {

            List<dropDownView> getDoctorName = new List<dropDownView>();

            VideoAppPOCEntities2 db = new VideoAppPOCEntities2();
            VideoModel vd = new VideoModel();

            var model = (from r in db.AppUsers
                         where r.RoleID == 2
                         select new dropDownView
                         {
                             UserId = r.UserID,
                             userName = r.userName,
                             RoleId = r.RoleID
                         });
            getDoctorName = model.ToList();
            var selectedUserId = 0;
            if (TempData["redirectedUserId"] != null && TempData["redirectedUserId"].ToString() != "")
            {
                selectedUserId = Convert.ToInt32(TempData["redirectedUserId"]);
                vd.UserID = Convert.ToInt32(TempData["redirectedUserId"]);
            }
            else
            {
                selectedUserId = getDoctorName[0].UserId;
                vd.UserID = getDoctorName[0].UserId;
            }
            

            var data = db.AppVideos.ToList();
            var checkedData = (from r in db.AppVideoUsers
                               where r.UserID == selectedUserId
                               select r).ToList();

            
            
            vd.videos = data;

            foreach (var item in vd.videos)
            {
                foreach (var checkItem in checkedData)
                {
                    if (item.VideoID == checkItem.VideoID)
                    {
                        item.isChecked = true;
                    }
                }
            }


            SelectList list = new SelectList(getDoctorName, "UserID", "userName");
            ViewBag.getDoctor = list;           


            return View(vd);


        }
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult AssignOnUser(VideoModel vm)
        {
            VideoAppPOCEntities2 db = new VideoAppPOCEntities2();
   
            if (ModelState.IsValid)
            {
                db.AppVideoUsers.RemoveRange(db.AppVideoUsers.Where(x => x.UserID == vm.UserID));
                db.SaveChanges();

                foreach (var videoID in  vm.videos.Where(x => x.isChecked).Select(x => x.VideoID))
                {
                    var appVideoUser = new AppVideoUser
                    {
                        UserID = vm.UserID,
                        VideoID = videoID
                    };
                    db.AppVideoUsers.Add(appVideoUser);
                }
                db.SaveChanges();

                TempData["redirectedUserId"] = vm.UserID;
                return RedirectToAction("AssignOnUser");
            }
          

            return View(vm);
        }

        [HttpPost]
        public string GetList(GetListPostRequest req) {

            VideoAppPOCEntities2 db = new VideoAppPOCEntities2();
            VideoModel vd = new VideoModel();
            var reqUserId = Convert.ToInt32(req.UserId);
            var data = db.AppVideos.ToList();
            var checkedData = (from r in db.AppVideoUsers
                               where r.UserID == reqUserId
                               select r).ToList();


            vd.videos = data;
            var jsonObject = new List<GetListPostResult>();

            foreach (var item in vd.videos)
            {
                foreach (var checkItem in checkedData)
                {
                    if (item.VideoID == checkItem.VideoID)
                    {
                        item.isChecked = true;
                    }
                }
                jsonObject.Add(new GetListPostResult {
                    VideoId = item.VideoID,
                    IsChecked = item.isChecked
                });
            }

            System.Web.Script.Serialization.JavaScriptSerializer oSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            string sJSON = oSerializer.Serialize(jsonObject);

            return sJSON;
        }
        [HttpPost]
        public void delete(List<int> ids)
        {
            businessLayer bl = new businessLayer();

            int[] uId = ids.ToArray();
            foreach(int i in uId)
            {

                int deleted = bl.delete(i);
            }
        }
    }
}