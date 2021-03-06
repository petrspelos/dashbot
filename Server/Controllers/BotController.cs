﻿using System;
using AutoMapper;
using DashBot.Abstractions;
using DashBot.Entities;
using Microsoft.AspNetCore.Mvc;
using Server.Models;
using System.Linq;

namespace Server.Controllers
{
    public class BotController : Controller
    {
        private readonly ICredentials _botCredentials;
        private readonly IDiscordBot _bot;
        private readonly BotEvents _botEvents;
        private readonly ILogger _logger;

        public BotController(ICredentials botCredentials, IDiscordBot bot, BotEvents botEvents, ILogger logger)
        {
            _botCredentials = botCredentials;
            _bot = bot;
            _botEvents = botEvents;
            _logger = logger;
        }

        public IActionResult Authentication()
        {
            var accounts = _botCredentials.GetAllAccounts();
            var model = new BotAuthViewModel
            {
                SavedBotAccounts = accounts.Select(Mapper.Map<BotAccountViewModel>),
                BotIsRunning = _bot.IsRunning()
            };

            return View(model);
        }

        public IActionResult OneTimeToken()
        {
            return View();
        }

        [HttpPost]
        public IActionResult OneTimeToken([FromForm]OneTimeBot oneTimeBot)
        {
            if (_bot.IsRunning())
            {
                ModelState.AddModelError("Token", "You cannot change the account while the bot is running.");
                return View(oneTimeBot);
            }

            _bot.SetActiveBotAccount(new BotAccount(oneTimeBot.Token));

            return RedirectToAction("Index", "Home");
        }

        public IActionResult StartBot()
        {
            if (_bot.IsRunning())
            {
                return BadRequest("Bot is already running.");
            }

            _bot.Connect();
            return RedirectToAction("Index", "Home");
        }

        public IActionResult StopBot()
        {
            if (!_bot.IsRunning())
            {
                return BadRequest("Bot is not running.");
            }

            _bot.Stop();
            return RedirectToAction("Index", "Home");
        }

        public IActionResult UseStoredAccount(string name)
        {
            var account = _botCredentials.GetAccountByName(name);
            if (account is null) { return BadRequest($"No account with the name {name} exists."); }
            _bot.SetActiveBotAccount(account);
            return RedirectToAction("Index", "Home");
        }

        public IActionResult NewBotAccount()
        {
            return View();
        }

        [HttpPost]
        public IActionResult NewBotAccount([FromForm] BotAccountModel account)
        {
            if (!ModelState.IsValid)
            {
                return View(account);
            }

            try
            {
                _botCredentials.StoreAccount(Mapper.Map<BotAccount>(account));
            }
            catch (Exception)
            {
                ModelState.AddModelError("Name", "A bot with this name already exists.");
                return View(account);
            }

            return RedirectToAction("Authentication");
        }

        public IActionResult Log()
        {
            var model = _logger.GetAll().Reverse();
            return View(model);
        }
    }
}
