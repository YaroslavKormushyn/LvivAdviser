using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LvivAdviser.Domain.Abstract;
using LvivAdviser.Domain.Entities;
using LvivAdviser.Domain.Abstract.Interfaces;

namespace LvivAdviser.WebUI.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ViewResult Index()
        {
            return View();
        }
    }
}