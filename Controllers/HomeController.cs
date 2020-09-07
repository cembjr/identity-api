using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CJ.Identity.Api.Controllers
{
    [Route("")]
    public class HomeController : MainController
    {
        [HttpGet("")]
        public IActionResult BemVindo() => Ok("Bem vindo a Api de Controle de Identidade!");
    }
}
