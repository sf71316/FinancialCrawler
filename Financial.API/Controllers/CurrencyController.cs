using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Unity;
using Financial.Data.Models;

namespace Financial.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CurrencyController : ControllerBase
    {
        public CurrencyController(IUnityContainer container)
        {

        }

        public ActionResult<IEnumerable<dynamic>> Get(string Currency)
        {

            return null;
        }
    }
}