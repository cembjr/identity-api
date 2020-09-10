using CB.Identidade.Api.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using static CB.Identidade.Api.Models.UsuarioViewModels;

namespace CB.Identidade.Api.Controllers
{
    [Route("api/identidade")]
    public class AuthController : MainController
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        
        public AuthController(SignInManager<IdentityUser> signInManager,
                              UserManager<IdentityUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [HttpPost("registrar")]
        public async Task<IActionResult> Registrar(UsuarioRegistro usuarioRegistro)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var usuarioACadastrar = new IdentityUser
            {
                UserName = usuarioRegistro.Email,
                Email = usuarioRegistro.Email,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(usuarioACadastrar, usuarioRegistro.Senha);

            if (result.Succeeded)
            {
                var usuarioCadastrado = await _userManager.FindByEmailAsync(usuarioACadastrar.Email);
                var claims = await _userManager.GetClaimsAsync(usuarioCadastrado);
                return CustomResponse(TokenService.GerarToken(usuarioCadastrado, claims));
            }

            result.Errors.ToList().ForEach(error => AdicionarErro(error.Description));
            return CustomResponse();
        }

        [HttpPost("autenticar")]
        public async Task<IActionResult> Login(UsuarioLogin usuarioLogin)
        {            
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var result = await _signInManager.PasswordSignInAsync(usuarioLogin.Email, usuarioLogin.Senha, false, true);

            if (result.Succeeded)
            {
                var user = await _userManager.FindByEmailAsync(usuarioLogin.Email);
                var claims = await _userManager.GetClaimsAsync(user);
                return CustomResponse(TokenService.GerarToken(user, claims));
            }

            if (result.IsLockedOut)
            {
                AdicionarErro("Usuário temporariamente bloqueado por tentativas inválidas");
                return CustomResponse();
            }

            AdicionarErro("Usuário ou Senha incorretos");
            return CustomResponse();
        }
       
    }
}
