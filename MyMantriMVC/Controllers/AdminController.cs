using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MyMantriMVC.Repository;

namespace MyMantriMVC.Controllers
{
    public class AdminController : Controller
    {

        IConfiguration configuration;
        private readonly IMapper _mapper;

        public AdminController(IConfiguration configuration,IMapper mapper)
        {
            this.configuration = configuration;
            _mapper = mapper;
        }

        // GET: Admin
        public ActionResult Index()
        {
            if (HttpContext.Session.GetString("username") == "")
            {
                return RedirectToAction("Index", "Login");
            }
            return View();
        }

        public ActionResult GetMantri()
        {
            try
            {
                if (HttpContext.Session.GetString("username") == "")
                {
                    return RedirectToAction("Index", "Login");
                }
                ServiceRepository serviceRepository = new ServiceRepository(configuration);
                HttpResponseMessage response = serviceRepository.GetResponse("api/Home/GetMantri");
                response.EnsureSuccessStatusCode();
                List<Models.Mantri> mantri = response.Content.ReadAsAsync<List<Models.Mantri>>().Result;
                return View(mantri);
            }
            catch (Exception ex)
            {
                return View("Error");
            }
        }

        public ActionResult EditMantri(Models.Mantri mantri)
        {
            try
            {
                if (HttpContext.Session.GetString("username") == "")
                {
                    return RedirectToAction("Index", "Login");
                }
                char[] gender = new char[3] { 'M', 'F', 'O' };
                ViewBag.Gender = gender;
                return View(mantri);
            }
            catch (Exception ex)
            {
                return View("Error");
            }
        }

        public ActionResult EditVoter(Models.Voter voter)
        {
            try
            {
                if (HttpContext.Session.GetString("username") == "")
                {
                    return RedirectToAction("Index", "Login");
                }
                char[] gender = new char[3] { 'M', 'F', 'O' };
                ViewBag.Gender = gender;
                return View(voter);
            }
            catch (Exception ex)
            {
                return View("Error");
            }
        }

        public ActionResult UpdateVoter(Models.Voter voter)
        {
            try
            {
                if (HttpContext.Session.GetString("username") == "")
                {
                    return RedirectToAction("Index", "Login");
                }
                voter.Gender = Request.Form["Gender"].ToString();
                ServiceRepository serviceRepository = new ServiceRepository(configuration);
                HttpResponseMessage response = serviceRepository.PutRequest("api/Home/UpdateVoter", voter);
                response.EnsureSuccessStatusCode();
                if (response.Content.ReadAsAsync<bool>().Result)
                    return View("Success");
                return View("Error");
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        public ActionResult UpdateMantri(Models.Mantri mantri, IFormCollection form)
        {
            try
            {
                if (HttpContext.Session.GetString("username") == "")
                {
                    return RedirectToAction("Index", "Login");
                }
                mantri.Gender = Request.Form["Gender"].ToString();
                ServiceRepository serviceRepository = new ServiceRepository(configuration);
                HttpResponseMessage response = serviceRepository.PostRequest("api/Home/UpdateMantri", mantri);
                response.EnsureSuccessStatusCode();
                if (response.Content.ReadAsAsync<bool>().Result)
                    return View("Success");
                return View("Error");
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        public ActionResult MantriDetails(string mantriId)
        {
            try
            {
                if (HttpContext.Session.GetString("username") == "")
                {
                    return RedirectToAction("Index", "Login");
                }
                ServiceRepository serviceRepositorty = new ServiceRepository(configuration);
                HttpResponseMessage response = serviceRepositorty.GetResponse("api/Home/MantriDetails?mantriId=" + mantriId);
                response.EnsureSuccessStatusCode();
                Models.Mantri mantri = response.Content.ReadAsAsync<Models.Mantri>().Result;
                if(mantri.Gender == "M")
                {
                    mantri.Gender = "Male";
                }
                else if(mantri.Gender == "F")
                {
                    mantri.Gender = "Female";
                }
                else
                {
                    mantri.Gender = "Others";
                }
                return View(mantri);
            }
            catch (Exception ex)
            {
                return View("Error");
            }
        }

        public ActionResult DeleteMantri(Models.Mantri mantri)
        {
            try
            {
                if (HttpContext.Session.GetString("username") == "")
                {
                    return RedirectToAction("Index", "Login");
                }
                return View(mantri);
            }
            catch (Exception ex)
            {
                return View("Error");
            }
        }

        public ActionResult DeleteMantriFinal(Models.Mantri mantri)
        {
            try
            {
                if (HttpContext.Session.GetString("username") == "")
                {
                    return RedirectToAction("Index", "Login");
                }
                ServiceRepository serviceRepository = new ServiceRepository(configuration);
                HttpResponseMessage response = serviceRepository.DeleteRequest("api/Home/DeleteMantri?mantriId=" + mantri.MantriId);
                response.EnsureSuccessStatusCode();
                if (response.Content.ReadAsAsync<bool>().Result)
                    return View("Success");
                return View("Error");
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        public ActionResult GetVoter()
        {
            try
            {
                if (HttpContext.Session.GetString("username") == "")
                {
                    return RedirectToAction("Index", "Login");
                }
                ServiceRepository serviceRepository = new ServiceRepository(configuration);
                HttpResponseMessage response = serviceRepository.GetResponse("api/Home/GetVoters");
                response.EnsureSuccessStatusCode();
                List<Models.Voter> voters = response.Content.ReadAsAsync<List<Models.Voter>>().Result;
                return View(voters);
            }
            catch (Exception ex)
            {
                return View("Error");
            }
        }

       

        public ActionResult VoterDetails(string voterId)
        {
            try
            {
                if (HttpContext.Session.GetString("username") == "")
                {
                    return RedirectToAction("Index", "Login");
                }
                ServiceRepository serviceRepositorty = new ServiceRepository(configuration);
                HttpResponseMessage response = serviceRepositorty.GetResponse("api/Home/VoterDetails?voterId=" + voterId);
                response.EnsureSuccessStatusCode();
                Models.Voter voter = response.Content.ReadAsAsync<Models.Voter>().Result;
                if (voter.Gender == "M")
                {
                    voter.Gender = "Male";
                }
                else if (voter.Gender == "F")
                {
                    voter.Gender = "Female";
                }
                else
                {
                    voter.Gender = "Others";
                }
                return View(voter);
            }
            catch (Exception ex)
            {
                return View("Error");
            }
        }


        public ActionResult DeleteVoter(Models.Voter voter)
        {
            try
            {
                if (HttpContext.Session.GetString("username") == "")
                {
                    return RedirectToAction("Index", "Login");
                }
                return View(voter);
            }
            catch (Exception ex)
            {
                return View("Error");
            }
        }

        public ActionResult DeleteVoterFinal(Models.Voter voter)
        {
            try
            {
                if (HttpContext.Session.GetString("username") == "")
                {
                    return RedirectToAction("Index", "Login");
                }
                ServiceRepository serviceRepository = new ServiceRepository(configuration);
                HttpResponseMessage response = serviceRepository.DeleteRequest("api/Home/DeleteVoter?voterId=" + voter.VoterId);
                response.EnsureSuccessStatusCode();
                if (response.Content.ReadAsAsync<bool>().Result)
                    return View("Success");
                return View("Error");
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        public ActionResult Logout()
        {
            HttpContext.Session.SetString("username", "");
            return RedirectToAction("Index","Login");
        }
    }
}