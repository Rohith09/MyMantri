using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MyMantriDataAccessLayer;
using MyMantriMVC.Models;

namespace MyMantriMVC.Controllers
{
    public class HomeController : Controller
    {
        Microsoft.AspNetCore.Http.IHttpContextAccessor httpContextAccessor;
        IConfiguration configuration;
        private readonly IMapper _mapper;
        private readonly MyMantriRepository _repObj;

        //Controller
        public HomeController(IConfiguration configuration, IMapper mapper, MyMantriRepository repObj)
        {
            this.configuration = configuration;
            _mapper = mapper;
            _repObj = repObj;
        }

        //Method to see different mantri without login.
        public IActionResult KnowYourMantri()
        {
            var temp = _repObj.GetAllConstituency();
            List<Models.Mantri> lst = new List<Mantri>();
            foreach (var item in temp)
            {
                lst.Add(_mapper.Map<Models.Mantri>(item));
            }
            return View(lst);
        }


        //Method to see work done by different mantri.
        public IActionResult WorkDone(int id)
        {
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

        public IActionResult PublicReview(int id)
        {
            var temp = _repObj.WorkDone(id);
            List<Models.WorkDone> lst = new List<WorkDone>();
            foreach (var item in temp)
            {
                if (item.Title.Equals("MantriFeedback"))
                {
                    lst.Add(_mapper.Map<WorkDone>(item));
                }
            }
            return View(lst);
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
