﻿using dotimo.Business.Services;
using dotimo.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dotimo.Application
{
    [Authorize]
    public class WatchesController : Controller
    {
        private readonly IWatchService _watchService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<User> _userManager;

        public WatchesController(IWatchService watchService, IHttpContextAccessor httpContextAccessor, UserManager<User> userManager)
        {
            _watchService = watchService;

            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
        }

        // GET: Watches
        [HttpGet]
        public IActionResult Index()
        {
            IEnumerable<Watch> watches;
            try
            {
                var user = _userManager.FindByNameAsync(User.Identity.Name).Result;
                watches = _watchService.GetAllByUserId(user.Id);
            }
            catch (Exception ex)
            {
                watches = new List<Watch>();
            }

            return View(watches);
        }

        // GET: Watches/Details/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Guid guid = (Guid)id;

            Watch watch = await _watchService.GetByGuidAsync(guid);
            if (watch == null)
            {
                return NotFound();
            }
            return View(watch);
        }

        // GET: Watches/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Watches/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,UrlString,Email,MonitoringTimePeriod,MonitoringTimePeriodId,UserId,Id,CreatedDate,UpdatedDate,IsActive")] Watch watch)
        {
            if (ModelState.IsValid)
            {
                watch.Id = Guid.NewGuid();
                await _watchService.CreateAsync(watch);
                return RedirectToAction(nameof(Index));
            }
            return View(watch);
        }

        // GET: Watches/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var watch = await _watchService.GetByGuidAsync((Guid)id);
            if (watch == null)
            {
                return NotFound();
            }
            return View(watch);
        }

        // POST: Watches/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Name,UrlString,Email,MonitoringTimePeriod,MonitoringTimePeriodId,UserId,Id,CreatedDate,UpdatedDate,IsActive")] Watch watch)
        {
            if (id != watch.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _watchService.UpdateAsync(watch);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WatchExists(watch.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            //ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", watch.UserId);
            return View(watch);
        }

        // GET: Watches/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            await _watchService.DeleteByIdAsync((Guid)id);

            return View();
        }

        // POST: Watches/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            await _watchService.DeleteByIdAsync((Guid)id);
            return RedirectToAction(nameof(Index));
        }

        private bool WatchExists(Guid id)
        {
            return _watchService.Exists(id);
        }
    }
}