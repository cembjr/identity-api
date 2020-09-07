using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CJ.Identity.Api.Controllers
{
    [ApiController]
    public class MainController : Controller
    {
        protected ICollection<string> Erros = new List<string>();

        protected IActionResult CustomResponse(object result = null)
        {
            if (IsOperacaoValida()) return Ok(result);

            return BadRequest(new 
            {
                Erros = Erros.ToArray() 
            });
        }
        protected IActionResult CustomResponse(ModelStateDictionary modelState)
        {
            modelState.Values.SelectMany(e => e.Errors).ToList().ForEach(erro => AdicionarErro(erro.ErrorMessage));
            return CustomResponse();
        }

        protected bool IsOperacaoValida() => !Erros.Any();
        protected void AdicionarErro(string erro) => Erros.Add(erro);
        protected void LimparErros() => Erros.Clear();

    }
}
