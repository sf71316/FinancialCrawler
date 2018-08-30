using System;
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
    public class CurrencyController : ControllerBase
    {
        IUnityContainer _Container;
        public CurrencyController(IUnityContainer container)
        {
            _Container = container;
        }
        [HttpGet]
        public ActionResult<IEnumerable<ExchangeRateModel>> Get(string tc, string sc = "TWD")
        {
            var data = _Container.Resolve<ICurrencyData>();
            return data.GetExchangeRates(sc, tc).OrderBy(p=>p.ExchangeTime).ToList();
        }
        [HttpGet("GetByDay")]
        public ActionResult<IEnumerable<ExchangeRateModel>> GetByDate(string tc, string sc = "TWD")
        {
            var data = _Container.Resolve<ICurrencyData>();
            return data.GetExchangeRates(sc, tc)
                .GroupBy(g=>g.ExchangeTime.Date)
                .Select(p=>new ExchangeRateModel {ExchangeRate=p.Average(x=>x.ExchangeRate),ExchangeTime=p.Key })
                .OrderBy(p => p.ExchangeTime).ToList();
        }
    }
}