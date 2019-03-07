﻿using System.Diagnostics;
using DashBot.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Server.Models;

namespace Server.Controllers
{
    public class HomeController : Controller
    {
        private readonly IDiscordBot _bot;

        public HomeController(IDiscordBot bot)
        {
            _bot = bot;
        }

        public IActionResult Index()
        {
            var model = new OverviewViewModel();

            if (_bot.Account != null)
            {
                model.ActiveAccount = new BotAccountViewModel
                {
                    AvatarUrl = _bot.Account.AvatarUrl,
                    Name = _bot.Account.Name
                };
            }

            return View(model);
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
