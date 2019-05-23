using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using MyMantriMVC.Repository;
using System.Web;
using System.Text;
using System.Linq;
using MyMantriDataAccessLayer;
using System.Collections.Generic;
using MyMantriDataAccessLayer.Models;
using AutoMapper;
using MyMantriMVC.Models;

namespace MyMantriMVC.Controllers
{
    public class LoginController : Controller
    {
        //Decleration and Controller
        #region
        IConfiguration configuration;
        IMapper _mapper;
        private readonly MyMantriRepository _repObj;

        //Controller
        public LoginController(IConfiguration configuration, MyMantriRepository repObj)
        {
            this.configuration = configuration;
            _repObj = repObj;
        }
        #endregion

        //Login
        #region
        //Landing view action for login
        public ActionResult Index()
        {
            try
            {
                //Mantri can enter only those constituency which are not registered.
                //Voter can enter only those constituency which are registered.
                #region
                List<string> constituencyListForMantri = new List<string>();
                List<string> constituencyListForVoter = new List<string>();
                var constituencyList = _repObj.ConstituencyName();
                foreach (var item in constituencyList)
                {
                    if (item.Status.Equals("not registered"))
                    {
                        constituencyListForMantri.Add(item.ConstituencyName);
                    }
                    if (item.Status.Equals("registered"))
                    {
                        constituencyListForVoter.Add(item.ConstituencyName);
                    }
                }
                ViewBag.constituencyListForMantri = constituencyListForMantri;
                ViewBag.constituencyListForVoter = constituencyListForVoter;
                #endregion
                return View();
            }
            catch (Exception)
            {
                TempData["msgLogin"] = "Server Error. Please retry after sometime.";
                return RedirectToAction("Index", "Login");
            }
        }

        //Login Method
        public ActionResult Login(IFormCollection frm)
        {
            try
            {
                int userId = Convert.ToInt32(frm["userId"]);
                string pass = (frm["password"]);
                string password = HttpUtility.UrlEncode(pass);
                string checkbox = frm["RememberMe"];

                //Remember Me
                if (checkbox == "on")
                {
                    CookieOptions option = new CookieOptions();
                    option.Expires = DateTime.Now.AddDays(1);
                    Response.Cookies.Append("UserId", Convert.ToString(userId), option);
                    Response.Cookies.Append("Password", pass, option);
                }

                //Validate user and check role.
                ServiceRepository serviceRepository1 = new ServiceRepository(configuration);
                HttpResponseMessage response1 = serviceRepository1.GetResponse("api/home/ValidateUser?id="+ userId+"&pwd="+password);
                response1.EnsureSuccessStatusCode();
                int rollId = response1.Content.ReadAsAsync<int>().Result;

                //Enter into different actions according to role.

                //Admin
                if (rollId == 1)
                {
                    ServiceRepository serviceRepositorty2 = new ServiceRepository(configuration);
                    HttpResponseMessage response2 = serviceRepositorty2.GetResponse("api/Home/ViewAdminName?adminId=" + userId);
                    response2.EnsureSuccessStatusCode();
                    HttpContext.Session.SetString("username", response2.Content.ReadAsAsync<String>().Result);
                    return RedirectToAction("index","admin");
                }

                //Mantri
                else if (rollId == 2)
                {
                    ServiceRepository serviceRepositorty2 = new ServiceRepository(configuration);
                    HttpResponseMessage response2 = serviceRepositorty2.GetResponse("api/Home/ViewMantriName?mantriId=" + userId);
                    response2.EnsureSuccessStatusCode();
                    HttpContext.Session.SetString("username", response2.Content.ReadAsAsync<String>().Result);
                    HttpContext.Session.SetString("mantriId", Convert.ToString(userId));
                    return RedirectToAction("index", "mantri");
                }

                //Voter
                else if (rollId == 3)
                {
                    HttpContext.Session.SetString("voterId", Convert.ToString(userId));
                    ServiceRepository serviceRepositorty2 = new ServiceRepository(configuration);
                    HttpResponseMessage response2 = serviceRepositorty2.GetResponse("api/Home/ViewVoterName?voterId=" + userId);
                    response2.EnsureSuccessStatusCode();
                    HttpContext.Session.SetString("username", (response2.Content.ReadAsAsync<String>().Result));
                    ServiceRepository serviceRepositorty3 = new ServiceRepository(configuration);
                    HttpResponseMessage response3 = serviceRepositorty3.GetResponse("api/Home/ViewVoterCons?voterId=" + userId);
                    response3.EnsureSuccessStatusCode();
                    HttpContext.Session.SetString("constituency", (response3.Content.ReadAsAsync<String>().Result));
                    return RedirectToAction("index", "voter");
                }

                TempData["msgLogin"] = "Enter valid Username and Password.";
                return RedirectToAction("Index", "Login");
            }
            catch (Exception ex)
            {
                TempData["msgLogin"] = "Server Error. Please retry after sometime.";
                return RedirectToAction("Index","Login");
            }
        }
        #endregion

        //Forget password
        #region
        //Landing view to enter details for forget password
        public ActionResult ForgotPassword()
        {
            try
            {
                return View();
            }
            catch (Exception)
            {
                return View("Error");
            }
        }

        //Method to change password if forget password.
        public ActionResult SaveForgetPassword(IFormCollection frm)
        {
            string email = frm["EmailId"];
            int? userId = _repObj.ValidateEmailId(email);
            if (userId != null)
            {
                HttpContext.Session.SetString("EmailForForgot", email);
                DateTime expires = DateTime.Now + TimeSpan.FromMinutes(4);
                string hash = MakeExpiryHash(expires);
                string link = string.Format("https://localhost:44320/Login/ChangePassword?exp={0}&k={1}", userId + expires.ToString("s"), hash);
                _repObj.sendEMailThroughOUTLOOK(email, link);
                return RedirectToAction("Index");
            }
            else
            {
                TempData["msg"] = "Invalid Email ID.";
                return RedirectToAction("ForgotPassword");
            }
        }

        public static string MakeExpiryHash(DateTime expiry)
        {
            const string salt = "my mantri secure key";
            byte[] bytes = Encoding.UTF8.GetBytes(salt + expiry.ToString("s"));
            using (var sha = System.Security.Cryptography.SHA1.Create())
                return string.Concat(sha.ComputeHash(bytes).Select(b => b.ToString("x2"))).Substring(8);
        }

        public ActionResult ChangePassword(string exp, string k)
        {
            DateTime expires = DateTime.Parse(exp.Substring(6));
            string userId = exp.Substring(0, 6);
            HttpContext.Session.SetString("userId", userId);
            string hash = MakeExpiryHash(expires);
            if (k == hash)
            {
                if (expires >= DateTime.Now)
                {
                    return View();
                }
                else
                {
                    return View("Expired");
                }
            }
            else
            {
                return View("InvalidLink");
            }
        }

        public ActionResult SaveChangePassword(Models.UserCredentials userCredentials)
        {
            try
            {
                ServiceRepository serviceRepositorty = new ServiceRepository(configuration);
                HttpResponseMessage response = serviceRepositorty.PostRequest("api/Home/ForgotPassword", userCredentials);
                response.EnsureSuccessStatusCode();
                if (response.Content.ReadAsAsync<bool>().Result)
                {
                    return View("SuccessPassword");
                }
                else
                {
                    return View("ErrorPassword");
                }
            }
            catch (Exception ex)
            {
                return View("ErrorPassword");
            }
        }
        #endregion

        //Registration Page.
        #region
        public ActionResult Register()
        {
            //return/* RedirectToAction("Index", "Registration")*/;
            return View();
        }

        public ActionResult SaveRegistration(Models.User user)
        {
            if (_repObj.ValidateEmailId(user.EmailId)!=null)
            {
                TempData["msgLogin"] = "Email Id already exists.";
                return RedirectToAction("Index");
            }
            string name = HttpUtility.UrlEncode(user.Name);
            bool status = _repObj.verifyemail(name, user.EmailId, user.Password);
            if (status == false)
            {
                TempData["msgLogin"] = "Enter a valid Email Id.";
                return RedirectToAction("Index");
            }
            TempData["msgLogin"] = "Please check your inbox for registration link!!";
            return RedirectToAction("Index");
        }
        #endregion

        //Complaint status method
        #region
        //Method to see status of complaint without login, using Complaint Id.
        public ActionResult GetComplaint(IFormCollection frm)
        {
            try
            {
                ViewBag.temp = "";
                string a = frm["Id"];
                if (a == null)
                {
                    return View();
                }
                int complaintId = Convert.ToInt32(a);

                ServiceRepository serviceRepository = new ServiceRepository(configuration);
                HttpResponseMessage response = serviceRepository.GetResponse("api/home/GetComplaintStatus?complaintId=" + complaintId);
                response.EnsureSuccessStatusCode();
                string status = response.Content.ReadAsAsync<string>().Result;

                //For resolved complaints
                if (status.ToLower() == "y")
                {
                    ViewBag.temp = "Congratulations!!! Your problem has been resolved. Thank you for using MyMantri.";
                    ViewBag.ID = complaintId;
                }

                //For unresolved complaints
                else if (status.ToLower() == "n")
                {
                    ViewBag.temp = "Your problem has not been resolved yet. Please check after sometime...";
                    ViewBag.ID = complaintId;
                }

                else if (status.ToLower() == "e")
                {
                    ViewBag.temp = "Your complaint has expired without a closure. This may be due to complaint failing to fall in your Mantri's jurisdiction or non resolution of complaint in stipulated time. Sorry for the inconvenience caused.";
                    ViewBag.ID = complaintId;
                }

                //For invalid complaint
                else
                {
                    ViewBag.temp = "Please enter a valid Complaint ID.";
                }

                return View();
            }
            catch (Exception ex)
            {
                ViewBag.temp = "Please enter a valid Complaint ID.";
                return View();
            }
        }
        #endregion
    }
}