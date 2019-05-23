using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MyMantriDataAccessLayer;
using MyMantriDataAccessLayer.Models;
using MyMantriMVC.Repository;

namespace MyMantriMVC.Controllers
{
    public class VoterController : Controller
    {
        Microsoft.AspNetCore.Http.IHttpContextAccessor httpContextAccessor;
        IConfiguration configuration;
        private readonly IMapper _mapper;
        private readonly MyMantriRepository _repObj;

        public VoterController(IConfiguration configuration, IMapper mapper, MyMantriRepository repObj)
        {
            this.configuration = configuration;
            _mapper = mapper;
            _repObj = repObj;
        }

        public ActionResult Index()
        {
            _repObj.UpadteStatusExpired();
            try
            {
                if (HttpContext.Session.GetString("username") == "")
                {
                    return RedirectToAction("Index", "Login");
                }
                int id = Convert.ToInt32(HttpContext.Session.GetString("voterId"));
                var temp = _repObj.ViewVoterDetails(id);
                ViewBag.cons = temp.Constituency;
                ViewBag.ID = temp.VoterId;
                return View();
            }
            catch (Exception)
            {
                return View("Error");
            }
        }

        public ActionResult ViewComplaintsInProgress()
        {
            try
            {
                if (HttpContext.Session.GetString("username") == "")
                {
                    return RedirectToAction("Index", "Login");
                }
                string voterId = HttpContext.Session.GetString("voterId");
                ServiceRepository serviceRepository = new ServiceRepository(configuration);
                HttpResponseMessage response = serviceRepository.GetResponse("api/Home/ViewAllComplaintForVoter?voterId="+voterId);
                response.EnsureSuccessStatusCode();
                List<Models.Complaint> complaints = response.Content.ReadAsAsync<List<Models.Complaint>>().Result;
                List<Models.Complaint> complaintsInProgress = new List<Models.Complaint>();
                foreach (var item in complaints)
                {
                    if (item.Status == "N")
                    {
                        complaintsInProgress.Add(item);
                    }
                }
                return View(complaintsInProgress);
            }
            catch (Exception ex)
            {
                return View("Error");
            }
        }

        public ActionResult ViewResolvedComplaints()
        {
            try
            {
                if (HttpContext.Session.GetString("username") == "")
                {
                    return RedirectToAction("Index", "Login");
                }
                string voterId = HttpContext.Session.GetString("voterId");
                ServiceRepository serviceRepository = new ServiceRepository(configuration);
                HttpResponseMessage response = serviceRepository.GetResponse("api/Home/ViewAllComplaintForVoter?voterId=" + voterId);
                response.EnsureSuccessStatusCode();
                List<Models.Complaint> complaints = response.Content.ReadAsAsync<List<Models.Complaint>>().Result;
                List<Models.Complaint> Resolvedcomplaints = new List<Models.Complaint>();
                foreach (var item in complaints)
                {
                    if (item.Status == "Y")
                    {
                        Resolvedcomplaints.Add(item);
                    }
                }
                return View(Resolvedcomplaints);
            }
            catch (Exception ex)
            {
                return View("Error");
            }
        }

        public ActionResult AddComplaint()
        {
            try
            {
                if (HttpContext.Session.GetString("username") == "")
                {
                    return RedirectToAction("Index", "Login");
                }
                var categoryList = new string[5] { "Water", "Road", "Electricity", "Sanitation", "Other" };
                ViewBag.CategoryList = categoryList;
                return View();
            }
            catch (Exception ex)
            {
                return View("Error");
            }
        }

        [HttpPost]
        public ActionResult SaveAddComplaint(Models.Complaint complaint, IFormCollection form)
        {
            try
            {
                if (HttpContext.Session.GetString("username") == "")
                {
                    return RedirectToAction("Index", "Login");
                }
                complaint.Category = Request.Form["categoryId"].ToString();
                complaint.ComplaintDateTime =DateTime.Now;
                complaint.Status = "N";
                ServiceRepository serviceRepository = new ServiceRepository(configuration);
                HttpResponseMessage response = serviceRepository.PostRequest("api/Home/SaveAddComplaint", complaint);
                response.EnsureSuccessStatusCode();
                int complaintId = response.Content.ReadAsAsync<int>().Result;
                if (complaintId != 0)
                {
                    {
                        int voterId = Convert.ToInt32(HttpContext.Session.GetString("voterId"));
                        var temp = _repObj.ViewVoterDetails(voterId);
                        var Email = temp.EmailId;
                        var desr = complaint.Description;
                        var Category = complaint.Category;
                        var cons = complaint.Constituency;
                        var dateTime = complaint.ComplaintDateTime;
                        var UserName = temp.Name;

                        _repObj.complaintEMail(Email, UserName, complaintId, dateTime, Category, cons, desr);

                        TempData["complaintId"] = complaintId;
                        return View("Success");
                    }
                }
                return View("Error");
            }
            catch (Exception ex)
            {
                return View("Error");
            }

        }

        public IActionResult SaveDeletedComplaint(int ComplaintId)
        {
            bool status = false;
            try
            {
                if (HttpContext.Session.GetString("username") == "")
                {
                    return RedirectToAction("Index", "Login");
                }
                status = _repObj.DeleteComplaint(ComplaintId);
                if (status)
                    return RedirectToAction("ViewComplaintsInProgress");
                else
                    return View("Error");
            }
            catch (Exception)
            {
                return View("Error");
            }

        }

        public ActionResult ComplaintDetails(int complaintId)
        {
            try
            {
                if (HttpContext.Session.GetString("username") == "")
                {
                    return RedirectToAction("Index", "Login");
                }
                ServiceRepository serviceRepositorty = new ServiceRepository(configuration);
                HttpResponseMessage response = serviceRepositorty.GetResponse("api/Home/ComplaintDetails?complaintId=" + complaintId);
                response.EnsureSuccessStatusCode();
                Models.Complaint comp = response.Content.ReadAsAsync<Models.Complaint>().Result;
                return View(comp);
            }
            catch (Exception ex)
            {
                return View("Error");
            }
        }

        public ActionResult ComplaintDetailsResolved(int complaintId)
        {
            try
            {
                if (HttpContext.Session.GetString("username") == "")
                {
                    return RedirectToAction("Index", "Login");
                }
                ServiceRepository serviceRepositorty = new ServiceRepository(configuration);
                HttpResponseMessage response = serviceRepositorty.GetResponse("api/Home/ComplaintDetails?complaintId=" + complaintId);
                response.EnsureSuccessStatusCode();
                Models.Complaint comp = response.Content.ReadAsAsync<Models.Complaint>().Result;
                return View(comp);
            }
            catch (Exception ex)
            {
                return View("Error");
            }
        }

        public ActionResult DeleteComplaint(int complaintId)
        {
            try
            {
                if (HttpContext.Session.GetString("username") == "")
                {
                    return RedirectToAction("Index", "Login");
                }
                Complaint c = _repObj.ComplaintDetails(complaintId);
                return View(c);
            }
            catch (Exception ex)
            {
                return View("Error");
            }
        }

        public ActionResult DeleteComplaintFinal(int complaintId)
        {
            try
            {
                if (HttpContext.Session.GetString("username") == "")
                {
                    return RedirectToAction("Index", "Login");
                }
                ServiceRepository serviceRepository = new ServiceRepository(configuration);
                HttpResponseMessage response = serviceRepository.DeleteRequest("api/Home/DeleteComplaint?ComplaintId=" + complaintId);
                response.EnsureSuccessStatusCode();
                if (response.Content.ReadAsAsync<bool>().Result)
                    return View("SuccessDelete");
                return View("Error");
            }
            catch (Exception ex)
            {
                return View("Error");
            }
        }

        public ActionResult AddFeedback(Models.Feedback feedback)
        {
            try
            {
                if (HttpContext.Session.GetString("username") == "")
                {
                    return RedirectToAction("Index", "Login");
                }
                Models.Feedback feed = _mapper.Map<Models.Feedback>(_repObj.FindFeedback(feedback.ComplaintId));
                if (feed==null)
                {
                    var ratingList = new int[6] { 0, 1, 2, 3, 4, 5 };
                    ViewBag.RatingList = ratingList;
                    return View("AddFeedback", feedback);
                }
                else
                {
                    return View("ViewFeedback", feed);
                }
            }
            catch (Exception ex)
            {
                return View("Error");
            }
        }

        [HttpPost]
        public ActionResult SaveAddedFeedback(Models.Feedback feedback, IFormCollection frm)
        {
            try
            {
                if (HttpContext.Session.GetString("username") == "")
                {
                    return RedirectToAction("Index", "Login");
                }
                feedback.Rating = Convert.ToInt32(frm["Rating"]);
                feedback.FeedbackDesr = frm["FeedbackDesr"];
                ServiceRepository serviceRepository = new ServiceRepository(configuration);
                HttpResponseMessage response = serviceRepository.PostRequest("api/Home/AddFeedback", feedback);
                response.EnsureSuccessStatusCode();
                if (response.Content.ReadAsAsync<bool>().Result)
                {
                    return View("SuccessFeedback");
                }
                return View("AddFeedback");
            }
            catch (Exception ex)
            {
                return View("Error");
            }
        }

        public ActionResult AddMantriFeedback()
        {
            ServiceRepository serviceRepository = new ServiceRepository(configuration);
            HttpResponseMessage response = serviceRepository.GetResponse("api/Home/GetMantriByConstituency?ConstituencyName="+HttpContext.Session.GetString("constituency"));
            response.EnsureSuccessStatusCode();
            Models.Mantri mantri = response.Content.ReadAsAsync<Models.Mantri>().Result;
            if (mantri == null)
            {
                return View("MantriNotPresent");
            }
            TempData["mantriId"] = mantri.MantriId;
            TempData["mantriName"] = mantri.Name;
            return View();
        }

        [HttpPost]
        public ActionResult SaveAddMantriFeedback(Models.WorkDone workdone)
        {
            try
            {
                if (HttpContext.Session.GetString("username") == "")
                {
                    return RedirectToAction("Index", "Login");
                }
                workdone.MantriId = Convert.ToInt32(TempData["mantriId"]);
                workdone.Title = "MantriFeedback";
                ServiceRepository serviceRepository = new ServiceRepository(configuration);
                HttpResponseMessage response = serviceRepository.PostRequest("api/Home/AddWorkdone", workdone);
                response.EnsureSuccessStatusCode();
                if (response.Content.ReadAsAsync<bool>().Result)
                {
                    return View("SuccessFeedback");
                }
                return View("AddMantriFeedback");
            }
            catch (Exception ex)
            {
                return View("Error");
            }
        }

        public ActionResult Logout()
        {
            HttpContext.Session.SetString("username", "");
            return RedirectToAction("Index", "Login");
        }


        public IActionResult Expired()
        {
            int Id =Convert.ToInt32(HttpContext.Session.GetString("voterId"));
            var AllComplaint = _repObj.ComplaintList(Id);
            List<Models.Complaint> expired = new List<Models.Complaint>();


            foreach (var item in AllComplaint)
            {


                if (item.Status == "E")
                {

                    expired.Add(_mapper.Map<Models.Complaint>(item));
                }
            }
            return View(expired);
        }

        public ActionResult EditVoter(int id)
        {
            try
            {   
                if (HttpContext.Session.GetString("username") == "")
                {
                    return RedirectToAction("Index", "Login");
                }
                var temp = _repObj.ViewVoterDetails(id);
                Models.Voter vtr = _mapper.Map<Models.Voter>(temp);

                char[] gender = new char[3] { 'M', 'F', 'O' };
                ViewBag.Gender = gender;
                return View(vtr);
            }
            catch (Exception ex)
            {
                return RedirectToAction("EditVoter");
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
                if (voter.DateOfBirth >= DateTime.Now.AddYears(-18))
                {
                    TempData["msg"] = "Minimum age to register is 18 years";
                    return RedirectToAction("EditVoter", new { id = voter.VoterId });

                }
                voter.Gender = Request.Form["Gender"].ToString();
                ServiceRepository serviceRepository = new ServiceRepository(configuration);
                HttpResponseMessage response = serviceRepository.PutRequest("api/Home/UpdateVoter", voter);
                response.EnsureSuccessStatusCode();
                if (response.Content.ReadAsAsync<bool>().Result)
                    return View("SuccessEdit");
                return RedirectToAction("EditVoter");
            }
            catch (Exception ex)
            {
                return RedirectToAction("EditVoter");
            }
        }
        public ActionResult ViewProfile(int id)
        {
            try
            {
                if (HttpContext.Session.GetString("username") == "")
                {
                    return RedirectToAction("Index", "Login");
                }
                ServiceRepository serviceRepositorty = new ServiceRepository(configuration);
                HttpResponseMessage response = serviceRepositorty.GetResponse("api/Home/VoterDetails?voterId=" + id);
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
        public IActionResult UpdatePassword()
        {
            return View();
        }


        public IActionResult SaveUpdatePassword(IFormCollection frm)
        {
            int id = Convert.ToInt32(HttpContext.Session.GetString("voterId"));
            string new_password = frm["NewPassword"];
            string old_password = frm["OldPassword"];

            var val = _repObj.UpdatePassword(id, old_password, new_password);

            if (val == "t")
            {
                return View("SuccessEdit");
            }

            else if (val == "f")
            {

                return RedirectToAction("ErrorPassword");
            }

            else
            {
                return View("Error");
            }
        }
    }
}