﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Unity;
using Financial.Data.Models;
using Financial.Data.Entity;
using Financial.Data.interfaces;
using Microsoft.AspNetCore.Authorization;

namespace Financial.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CurrencyController : ControllerBase
    {
        IUnityContainer _container;
        public CurrencyController(IUnityContainer container)
        {
            _container = container;
        }
        [HttpGet]
        public ActionResult<IEnumerable<ExchangeRateModel>> Get(string tc, string sc = "TWD")
        {
            var data = _container.Resolve<ICurrencyData>();
            return data.GetExchangeRates(sc, tc).ToList();
        }
    }
}