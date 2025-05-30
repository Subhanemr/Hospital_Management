﻿using Hospital_Management.DAL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hospital_Management.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }
        [Authorize]
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ErrorPage(string error)
        {
            ViewBag.Error = error;
            if (error == null)
            {
                return RedirectToAction(nameof(Index));
            }
            return View();
        }
    }
}
