using NUnit.Framework;
using dotimo.Business.Services;
using System;
using System.Collections.Generic;
using System.Text;
using dotimo.Business.IServices;
using dotimo.Core;
using dotimo.Data.Entities;
using Microsoft.EntityFrameworkCore;
using dotimo.Data.Context;
using System.Net;
using System.Linq;
using System.Threading.Tasks;

namespace dotimo.Business.Services.Tests
{
    [TestFixture()]
    public class CheckUpServiceTests
    {
        public IUnitOfWork<CheckUp> _unitOfWork;
        public IWatchService _watchService;
        public ICheckUpService _checkUpService;

        [SetUp]
        public void Setup()
        {
            var builder = new DbContextOptionsBuilder<DotimoDbContext>();
            builder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=dotimo-dev-1;Trusted_Connection=True;MultipleActiveResultSets=true");
            _watchService = new WatchService(new UnitOfWork<Watch>(new DotimoDbContext(builder.Options)));
            _checkUpService = new CheckUpService(new UnitOfWork<CheckUp>(new DotimoDbContext(builder.Options)));
        }

        [Test()]
        public async Task CreateAsyncTest()
        {
            var watch = _watchService.Find(w => w.IsActive).FirstOrDefault();
            CheckUp checkUp = new CheckUp
            {
                StatusCode = (short) HttpStatusCode.OK,
                UpdatedDate = DateTime.Now,
                Success = true,
                WatchId = watch.Id,
                IsActive = true
            };
            var c = await _checkUpService.CreateAsync(checkUp);
            Assert.AreEqual(checkUp, c);
        }
    }
}