using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication2lb1.Controllers
{
    public class Lab1Controller : Controller
    {
        // GET: Lab1
        public ActionResult FirstViewMethod()
        {
            ViewBag.Vegetables = GetVegetablesList();
            return View();
        }
        public ActionResult SecondViewMethod()
        {
            return View(GetVegetablesList().OrderBy(x => x).ToList());
        }
        public ActionResult ThirdViewMethod()
        {
            return View(GetVegetablesList().OrderBy(x => x).ToList());
        }
        public List<string> GetVegetablesList() { 
            List<string> vegetables = new List<string>();
            vegetables.Add("Tomatos");
            vegetables.Add("Kartofels");
            vegetables.Add("Baklazanes");
            return vegetables;
        }

    }
}