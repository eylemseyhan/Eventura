﻿using Microsoft.AspNetCore.Mvc;

namespace EventsProject.Areas.Member.Controllers
{
    public class FavoritesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
