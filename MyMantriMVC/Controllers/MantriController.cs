using System;
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
using MyMantriMVC.Models;
using MyMantriMVC.Repository;

namespace MyMantriMVC.Controllers
{
    public class MantriController : Controller
    {
        IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly MyMantriRepository _repObj;

        public MantriController(IConfiguration configuration, IMapper mapper, MyMantriRepository repObj)
        {
            _configuration = configuration;
            _mapper = mapper;
            _repObj = repObj;
        }
        public IActionResult Index()
        {
            _repObj.UpadteStatusExpired(); //Important
            int id = Convert.ToInt32(HttpContext.Session.GetString("mantriId"));
            var temp = _repObj.ViewMantriDetails(id);

            HttpContext.Session.SetString("cons", temp.Constituency);
            HttpContext.Session.SetString("mUID", temp.MantriUid);
            if (HttpContext.Session.GetString("username") == "")
            {
                return RedirectToAction("Index", "Login");
            }
            return View();
        }

        public IActionResult ViewComplaintsInProgress()
        {
            if (HttpContext.Session.GetString("username") == "")
            {
                return RedirectToAction("Index", "Login");
            }
            string MantriId = HttpContext.Session.GetString("mantriId");
            string cons = _repObj.GetConstituency(Convert.ToInt32(MantriId));
            var AllComplaintForMantri = _repObj.ViewAllComplaintForMantri(cons);
            List<Models.Complaint> InProgress = new List<Models.Complaint>();

            foreach (var item in AllComplaintForMantri)
            {
                if (item.Status == "N")
                {
                    InProgress.Add(_mapper.Map<Models.Complaint>(item));
                }
            }
            return View(InProgress);
        }

        public IActionResult ViewComplaintsInSolved()
        {
            if (HttpContext.Session.GetString("username") == "")
            {
                return RedirectToAction("Index", "Login");
            }
            string MantriId = HttpContext.Session.GetString("mantriId");
            string cons = _repObj.GetConstituency(Convert.ToInt32(MantriId));
            var AllComplaintForMantri = _repObj.ViewAllComplaintForMantri(cons);

            List<Models.Complaint> SolvedComplaint = new List<Models.Complaint>();
            foreach (var item in AllComplaintForMantri)
            {

                if (item.Status == "Y")
                {
                    SolvedComplaint.Add(_mapper.Map<Models.Complaint>(item));
                }

            }
            return View(SolvedComplaint);
        }

        public IActionResult ViewComplaintsDetails(int complaintId)
        {
            try
            {
                if (HttpContext.Session.GetString("username") == "")
                {
                    return RedirectToAction("Index", "Login");
                }

                var comp = _mapper.Map<Models.Complaint>(_repObj.ComplaintDetails(complaintId));

                return View(comp);
            }
            catch (Exception ex)
            {
                return View("Error");
            }

        }

        public IActionResult UpdateComplaintStatus(int complaintId)
        {
            if (HttpContext.Session.GetString("username") == "")
            {
                return RedirectToAction("Index", "Mantri");
            }
            _repObj.UpdateComplaintStatus(complaintId);
            var temp = _repObj.ComplaintDetails(complaintId);
            var voterid = temp.VoterId;
            var emailObj = _repObj.ViewVoterDetails(voterid);
            var email = emailObj.EmailId;
            _repObj.SolvedEmail(email, complaintId);
            return View("UpdateSuccess");
        }

        public IActionResult AddWorkdone()
        {
            if (HttpContext.Session.GetString("username") == "")
            {
                return RedirectToAction("Index", "Login");
            }
            return View();
        }

        public IActionResult SaveAddedWork(Models.WorkDone work)
        {
            bool status = false;
            if (ModelState.IsValid)
            {
                if (HttpContext.Session.GetString("username") == "")
                {
                    return RedirectToAction("Index", "Login");
                }
                try
                {
                    status = _repObj.AddWorkdone(_mapper.Map<Workdone>(work));
                    if (status)
                        return View("SuccessWork");
                    else
                        return View("Error");
                }
                catch (Exception)
                {
                    return View("Error");
                }
            }
            return RedirectToAction("AddWorkdone");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.SetString("username", "");
            return RedirectToAction("Index", "Login");
        }

        public IActionResult OldWork()
        {
            int id = Convert.ToInt32(HttpContext.Session.GetString("mantriId"));
            
            var temp = _repObj.WorkDone(id);
            List<Models.WorkDone> lst = new List<WorkDone>();
            foreach (var item in temp)
            {
                if (item.Title != "MantriFeedback")
                {
                    lst.Add(_mapper.Map<WorkDone>(item));
                }
            }
            return View(lst);
        }

        public IActionResult EditWork(int workId)
        {
            var obj = _mapper.Map<Models.WorkDone>(_repObj.WorkDetails(workId));
            return View(obj);
        }

        public IActionResult SaveEditedWork(Models.WorkDone obj)
        {
            bool status = false;
            if (ModelState.IsValid)
            {
                try
                {
                    status = _repObj.EditWork(_mapper.Map<Workdone>(obj));
                    if (status)
                        return View("SuccessEdit");
                    else
                        return View("Error");
                }
                catch (Exception)
                {
                    return View("Error");
                }
            }
            return View("EditWork");
        }

        public ActionResult DeleteWork(int workId)
        {
            try
            {
                var obj = _mapper.Map<Models.WorkDone>(_repObj.WorkDetails(workId));
                return View(obj);
            }
            catch (Exception ex)
            {
                return View("Error");
            }
        }
        public ActionResult DeleteWorkFinal(int WorkId)
        {
            bool status = false;
            try
            {
                status = _repObj.DeleteWork(WorkId);
                if (status)
                    return View("SuccessDelete");
                else
                    return View("Error");
            }
            catch (Exception)
            {
                return View("Error");
            }
        }
        
        public ActionResult WorkDetails(int workId)
        {
            try
            {

                var obj = _mapper.Map<Models.WorkDone>(_repObj.WorkDetails(workId));
                return View(obj);
            }
            catch (Exception ex)
            {
                return View("Error");
            }            
        }

        public IActionResult Deadline(int mantriid)
        {
            string MantriId = HttpContext.Session.GetString("mantriId");
            string cons = _repObj.GetConstituency(Convert.ToInt32(MantriId));
            var AllComplaintForMantri = _repObj.ViewAllComplaintForMantri(cons);
            List<Models.Complaint> NearDeadline = new List<Models.Complaint>();

            foreach (var item in AllComplaintForMantri)
            {
                if (item.Category.ToLower() == "water")
                {
                    bool chk = ((DateTime.Now - item.ComplaintDateTime).TotalDays > 2) && ((DateTime.Now - item.ComplaintDateTime).TotalDays <= 5);
                    if (item.Status == "N" && chk)
                    {
                        NearDeadline.Add(_mapper.Map<Models.Complaint>(item));
                    }
                }
                if (item.Category.ToLower() == "electricity")
                {
                    bool chk = ((DateTime.Now - item.ComplaintDateTime).TotalDays > 2) && ((DateTime.Now - item.ComplaintDateTime).TotalDays <= 5);
                    if (item.Status == "N" && chk)
                    {
                        NearDeadline.Add(_mapper.Map<Models.Complaint>(item));
                    }
                }
                if (item.Category.ToLower() == "sanitation")
                {
                    bool chk = ((DateTime.Now - item.ComplaintDateTime).TotalDays > 7) && ((DateTime.Now - item.ComplaintDateTime).TotalDays <= 10);
                    if (item.Status == "N" && chk)
                    {
                        NearDeadline.Add(_mapper.Map<Models.Complaint>(item));
                    }
                }
                if (item.Category.ToLower() == "road")
                {
                    bool chk = ((DateTime.Now - item.ComplaintDateTime).TotalDays > 10) && ((DateTime.Now - item.ComplaintDateTime).TotalDays <= 15);
                    if (item.Status == "N" && chk)
                    {
                        NearDeadline.Add(_mapper.Map<Models.Complaint>(item));
                    }
                }
                if (item.Category.ToLower() == "other")
                {
                    bool chk = ((DateTime.Now - item.ComplaintDateTime).TotalDays > 10) && ((DateTime.Now - item.ComplaintDateTime).TotalDays <= 15);
                    if (item.Status == "N" && chk)
                    {
                        NearDeadline.Add(_mapper.Map<Models.Complaint>(item));
                    }
                }
            }
            return View(NearDeadline);
        }

        public IActionResult Expired(int mantriid)
        {
            string MantriId = HttpContext.Session.GetString("mantriId");
            string cons = _repObj.GetConstituency(Convert.ToInt32(MantriId));
            var AllComplaintForMantri = _repObj.ViewAllComplaintForMantri(cons);
            List<Models.Complaint> expired = new List<Models.Complaint>();


            foreach (var item in AllComplaintForMantri)
            {

               
                if (item.Status == "E")
                {
                   
                    expired.Add(_mapper.Map<Models.Complaint>(item));
                }
            }
            return View(expired);
        }

        public IActionResult ViewFeedBack(int complaintId)
        {
                      
            var temp = _repObj.FindFeedback(complaintId);
            if (temp == null)
            {
                return View("nodata");
            }
            return View(temp);
           
        }
        public ActionResult ViewProfile(int id)
        {
            try
            {
                if (HttpContext.Session.GetString("username") == "")
                {
                    return RedirectToAction("Index", "Login");
                }
                ServiceRepository serviceRepositorty = new ServiceRepository(_configuration);
                HttpResponseMessage response = serviceRepositorty.GetResponse("api/Home/MantriDetails?mantriId=" + id);
                response.EnsureSuccessStatusCode();
                Models.Mantri mantri = response.Content.ReadAsAsync<Models.Mantri>().Result;
                if (mantri.Gender == "M")
                {
                    mantri.Gender = "Male";
                }
                else if (mantri.Gender == "F")
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

        public ActionResult EditMantri(int id)
        {
            try
            {
                var mantri = _repObj.ViewMantriDetails(id);
                Models.Mantri Mntri = _mapper.Map<Models.Mantri>(mantri);
                if (HttpContext.Session.GetString("username") == "")
                {
                    return RedirectToAction("Index", "Login");
                }
                char[] gender = new char[3] { 'M', 'F', 'O' };
                ViewBag.Gender = gender;
                return View(Mntri);
            }
            catch (Exception ex)
            {
                return View("Error");
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
                if (mantri.DateOfBirth >= DateTime.Now.AddYears(-18))
                {
                    TempData["msg"] = "Minimum age to register is 18 years";
                    return RedirectToAction("EditVoter", new { id = mantri.MantriId });

                }
                mantri.Gender = Request.Form["Gender"].ToString();
                ServiceRepository serviceRepository = new ServiceRepository(_configuration);
                HttpResponseMessage response = serviceRepository.PostRequest("api/Home/UpdateMantri", mantri);
                response.EnsureSuccessStatusCode();
                if (response.Content.ReadAsAsync<bool>().Result)
                    return View("SuccessEdit");
                return View("Error");
            }
            catch (Exception ex)
            {
                return View("EditMantrti");
            }
        }

        public IActionResult UpdatePassword()

        {


            return View();
        }


        public IActionResult SaveUpdatePassword(IFormCollection frm)
        {
            int id = Convert.ToInt32(HttpContext.Session.GetString("mantriId"));
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