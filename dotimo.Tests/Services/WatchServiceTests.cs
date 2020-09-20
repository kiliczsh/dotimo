using dotimo.Business.IServices;
using dotimo.Core;
using dotimo.Data.Context;
using dotimo.Data.Entities;
using dotimo.Data.Enums;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dotimo.Business.Services.Tests
{
    [TestFixture()]
    public class WatchServiceTests
    {
        public IUnitOfWork<Watch> _unitOfWork;
        public IWatchService _watchService;

        [SetUp]
        public void Setup()
        {
            var builder = new DbContextOptionsBuilder<DotimoDbContext>();
            builder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=dotimo-dev-1;Trusted_Connection=True;MultipleActiveResultSets=true");
            _watchService = new WatchService(new UnitOfWork<Watch>(new DotimoDbContext(builder.Options)));
        }

        [Test()]
        public async Task CreateAsyncTest()
        {
            var watch = new Watch
            {
                Id = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                IsActive = true,
                UpdatedDate = DateTime.Now,
                Name = "WatchServiceTest",
                Email = "a@b.com",
                UrlString = "www.google.com",
                MonitoringTimePeriod = MonitoringTimePeriod.Weekly
            };

            watch.UserId = _watchService.GetAllAsync().Result.FirstOrDefault()?.UserId ?? Guid.Parse("3E553495-254A-404B-7D22-08D85C80C51D");

            Watch w = await _watchService.CreateAsync(watch);

            Assert.AreEqual(w, watch);
        }

        [Test()]
        public async Task DeleteAsyncTest()
        {
            var watch = _watchService.Find(w => string.Equals(w.Name, "WatchServiceTest") && string.Equals(w.Email, "a@b.com")).FirstOrDefault();
            await _watchService.DeleteAsync(watch);
            var w = _watchService.GetByGuidAsync(watch.Id).Result;
            Assert.IsNull(w);
        }

        [Test()]
        public void DeleteByIdAsyncTest()
        {
            var watch = _watchService.Find(w => string.Equals(w.Name, "WatchServiceTest") && string.Equals(w.Email, "a@b.com") && w.IsActive).FirstOrDefault();
            _watchService.DeleteByIdAsync(watch.Id);
            var w = _watchService.GetByGuidAsync(watch.Id).Result;
            Assert.AreEqual(w.IsActive, false);
        }

        [Test()]
        public async Task GetAllAsyncTest()
        {
            var watches = await _watchService.GetAllAsync();
            Assert.IsInstanceOf<IEnumerable<Watch>>(watches);
        }

        [Test()]
        public void GetAllByUserIdTest()
        {
            var userId = _watchService.GetAllAsync().Result.FirstOrDefault()?.UserId ?? Guid.Parse("3E553495-254A-404B-7D22-08D85C80C51D");
            var watches = _watchService.GetAllByUserId(userId);
            Assert.IsInstanceOf<IEnumerable<Watch>>(watches);
            foreach (var watch in watches)
            {
                Assert.AreEqual(userId, watch.UserId);
                Assert.AreEqual(true, watch.IsActive);
            }
        }

        [Test()]
        public async Task UpdateAsyncTest()
        {
            var watch = _watchService.GetAllAsync().Result.LastOrDefault();
            watch.Name = "Updated";
            await _watchService.UpdateAsync(watch);
            var w = await _watchService.GetByGuidAsync(watch.Id);
            Assert.AreEqual("Updated", w.Name);
        }

        [Test()]
        public void ExistsTest()
        {
            var watch = _watchService.GetAllAsync().Result.LastOrDefault();
            Assert.IsNotNull(watch);
        }
    }
}