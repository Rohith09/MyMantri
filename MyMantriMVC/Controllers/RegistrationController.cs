using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MyMantriDataAccessLayer;
using MyMantriMVC.Models;
using MyMantriMVC.Repository;

namespace MyMantriMVC.Controllers
{
    public class RegistrationController : Controller
    {

        //Decleration and controller
        #region
        IConfiguration configuration;
        IMapper _mapper;
        private readonly MyMantriRepository _repObj;
        static string hashsecure;

        //Controller
        public RegistrationController(IConfiguration configuration, IMapper mapper, MyMantriRepository repObj)
        {
            this.configuration = configuration;
            _mapper = mapper;
            _repObj = repObj;
        }
        #endregion

        //Registration
        #region
        //View of registeration form.

        [HttpGet]
        public ActionResult Index(string n, string e, string p,string k)
        {
            try
            {
                string hash = MakeExpiryHash(e);
                hashsecure = k;
                if (k == hash)
                {
                    #region
                    var constituencyList = _repObj.ConstituencyName();
                    List<string> constituencyListForMantri = new List<string>();
                    foreach (var item in constituencyList)
                    {
                        constituencyListForMantri.Add(item.ConstituencyName);
                    }
                    ViewBag.constituencyListForMantri = constituencyListForMantri;
                    TempData["email"] = e;
                    TempData["name"] = n;
                    TempData["password"] = p;
                    #endregion

                    return View();
                }
                else
                {
                    TempData["msgLogin"] = "You have entered an Invalid link.";
                    return RedirectToAction("Index", "Login");
                }
            }
            catch (Exception ex)
            {
                TempData["msg"] = "Server Error. Please retry after sometime.";
                return View("Index");
            }
        }

        public static string MakeExpiryHash(string secure)
        {
            const string salt = "my mantri secure key";
            byte[] bytes = Encoding.UTF8.GetBytes(salt + secure);
            using (var sha = System.Security.Cryptography.SHA1.Create())
                return string.Concat(sha.ComputeHash(bytes).Select(b => b.ToString("x2"))).Substring(8);
        }

        //Method to register user.
        [HttpPost]
        public ActionResult SaveRegistration(Models.User user)
        {
            try
            {
                //Minimum age to register is 18 years.
                #region
                if (user.DateOfBirth >= DateTime.Now.AddYears(-18))
                {
                    TempData["msg"] = "Minimum age to register is 18 years";
                    return RedirectToAction("Index", new { n=user.Name, e=user.EmailId, p=user.Password, k=hashsecure });

                }
                #endregion

                if (_repObj.ValidateEmailId(user.EmailId) != null)
                {
                    TempData["msg"] = "Email Id already exists.";
                    return RedirectToAction("Index", new { user.Name, user.EmailId, user.Password, hashsecure });
                }

                //Assigning values to enter in user credentials table.
                #region
                UserCredentials userCredential = new UserCredentials();
                ServiceRepository serviceRepository = new ServiceRepository(configuration);
                userCredential.RollId = 3;
                userCredential.Password = user.Password;
                #endregion

                //Check if mantri or not and verify mantri. 
                #region
                if (user.MantriUid != null)
                {
                    HttpResponseMessage response1 = serviceRepository.GetResponse("api/Home/ValidateMantri?mantriId=" + user.MantriUid + "&constituency=" + user.Constituency);
                    response1.EnsureSuccessStatusCode();
                    int a = response1.Content.ReadAsAsync<int>().Result;
                    if (a == 1)
                        userCredential.RollId = 2;
                    else if (a == 2)
                    {
                        TempData["msg"] = "Mantri already exists for opted constituency.";
                        return RedirectToAction("Index", new { user.Name, user.EmailId, user.Password, hashsecure });
                    }
                    else
                    {
                        TempData["msg"] = "Enter valid mantri credentials";
                        return RedirectToAction("Index", new { user.Name, user.EmailId, user.Password, hashsecure });
                    }
                }
                #endregion

                //Add user credentials to userCredentials table.
                //store User Id in Tempdata for retrieving username to display on Index page.
                #region
                HttpResponseMessage response2 = serviceRepository.PostRequest("api/Home/AddUserCredentials/", userCredential);
                response2.EnsureSuccessStatusCode();
                int userId = response2.Content.ReadAsAsync<int>().Result;
                TempData["userId"] = userId;
                #endregion

                //Create and enter details of mantri and voter on basis of role id.
                #region
                //Mantri
                if (userCredential.RollId == 2)
                {
                    user.MantriId = userId;
                    Mantri mantri = _mapper.Map<Mantri>(user);
                    HttpResponseMessage response3 = serviceRepository.PostRequest("api/Home/AddMantri/", mantri);
                    response3.EnsureSuccessStatusCode();
                    if (response3.Content.ReadAsAsync<bool>().Result)
                        return View("Success");
                }

                //Voter
                else if (userCredential.RollId == 3)
                {
                    user.VoterId = userId;
                    Voter voter = _mapper.Map<Voter>(user);
                    HttpResponseMessage response3 = serviceRepository.PostRequest("api/Home/AddVoter", voter);
                    response3.EnsureSuccessStatusCode();
                    if (response3.Content.ReadAsAsync<bool>().Result)
                        return View("Success");
                }
                #endregion

                TempData["msg"] = "Email ID already exists.";
                return RedirectToAction("Index", new { user.Name, user.EmailId, user.Password, hashsecure });
            }
            catch (Exception ex)
            {
                TempData["msg"] = "Server Error. Please retry after sometime.";
                return RedirectToAction("Index", new { user.Name, user.EmailId, user.Password, hashsecure });
            }
        }
        #endregion
    }
}