using AutoMapper;
using dotimo.Application.Models.Request;
using dotimo.Business.Services;
using dotimo.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dotimo.Application
{
    [Authorize]
    public class WatchesController : Controller
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<WatchesController> _logger;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly IWatchService _watchService;
        public WatchesController(IWatchService watchService, IHttpContextAccessor httpContextAccessor, UserManager<User> userManager,
            IMapper mapper, ILogger<WatchesController> logger)
        {
            _watchService = watchService;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
            _mapper = mapper;
            _logger = logger;
        }

        // GET: Watches/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Watches/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,UrlString,Email,MonitoringTimePeriod")] Watch watch)
        {
            if (ModelState.IsValid)
            {
                var user = _userManager.FindByNameAsync(User.Identity.Name).Result;

                watch.Id = Guid.NewGuid();
                watch.UserId = user.Id;
                watch.IsActive = true;
                watch.UpdatedDate = DateTime.Now;

                await _watchService.CreateAsync(watch);
                _logger.LogInformation(string.Format("User {0} created Watch {1}. Data: {2}", user.Id, watch.Id, watch));
                return RedirectToAction(nameof(Index));
            }
            else
            {
                _logger.LogError(string.Format("Model State is not valid. {0}", watch));
            }
            return View(watch);
        }

        // GET: Watches/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                _logger.LogError(string.Format("Watch Id -{0}- is NULL.", id));
                return NotFound();
            }

            var watch = await _watchService.GetByGuidAsync((Guid)id);

            return View(watch);
        }

        // POST: Watches/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            await _watchService.DeleteByIdAsync((Guid)id);
            return RedirectToAction(nameof(Index));
        }

        // GET: Watches/Details/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                _logger.LogError(string.Format("Watch Id -{0}- is NULL.", id));
                return NotFound();
            }
            Guid guid = (Guid)id;

            Watch watch = await _watchService.GetByGuidAsync(guid);
            //_logger.LogInformation(string.Format("Watch-{0}: {1}", watch.Id, watch));
            if (watch == null)
            {
                _logger.LogError(string.Format("Watch Id: {0}  NOT FOUND!", id));
                return NotFound();
            }
            return View(watch);
        }

        // GET: Watches/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                _logger.LogError(string.Format("Watch Id -{0}- is NULL.", id));
                return NotFound();
            }

            var watch = await _watchService.GetByGuidAsync((Guid)id);
            if (watch == null)
            {
                _logger.LogError(string.Format("Watch Id: {0}  NOT FOUND!", id));
                return NotFound();
            }
            return View(watch);
        }

        // POST: Watches/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id, Name,UrlString,Email,MonitoringTimePeriod")] WatchEditRequest watchRequest)
        {
            Watch watch = null;
            if (id != watchRequest.Id)
            {
                _logger.LogError(string.Format("Watch Id != Request Id: {0} != {1}", id, watchRequest.Id));
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var watchDb = _watchService.GetByGuidAsync(watchRequest.Id).Result;
                    watchDb.Name = watchRequest.Name;
                    watchDb.UrlString = watchRequest.UrlString;
                    watchDb.MonitoringTimePeriod = watchRequest.MonitoringTimePeriod;
                    watchDb.Email = watchRequest.Email;
                    watchDb.UpdatedDate = DateTime.Now;

                    await _watchService.UpdateAsync(watchDb);
                    watch = watchDb;
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WatchExists(watchRequest.Id))
                    {
                        _logger.LogError(string.Format("Watch Id: {0}  NOT FOUND!", watchRequest.Id));
                        return NotFound();
                    }
                    else
                    {
                        _logger.LogError("Watch Edit Failed.");
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            else
            {
                _logger.LogError(string.Format("Model State is not valid. {0}", watchRequest));
            }
            return View(watch);
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
                _logger.LogInformation(string.Format("User Email: {0} Total Watchs: {1}", user.Email, watches.Count()));
            }
            catch (Exception ex)
            {
                watches = new List<Watch>();
            }

            return View(watches);
        }
        private bool WatchExists(Guid id)
        {
            return _watchService.Exists(id);
        }
    }
}