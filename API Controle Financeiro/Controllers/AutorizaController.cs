using ControleFinanceiroAPI.Context;
using ControleFinanceiroAPI.DTOs;
using ControleFinanceiroAPI.Models;
using ControleFinanceiroAPI.Util;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net.WebSockets;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace ControleFinanceiroAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AutorizaController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _config;
        
        public AutorizaController(AppDbContext context, IConfiguration config)
        {
            _context = context;
           _config = config;
        }

        [HttpGet]
        public ActionResult<string> Get() 
        {
            try
            {
                string msgAcessado = "AutorizaController - ACESSADO EM: " + DateTime.Now.ToLongDateString() + " às " + DateTime.Now.ToLongTimeString();
                UtilHelper.myLogTxtRequest(msgAcessado, _config, HttpContext);
                return msgAcessado;
            }
            catch (Exception)
            {
                throw;
            }         
        }

        [HttpPost("login")]
        public async Task<ActionResult> LoginApi(UserDTO uc)
        {
            try
            {
                var passMd5 = UtilHelper.MyToMD5(uc.Password);
                var userAutenticado = _context.UserCredentials.FirstOrDefault(u => u.Email.Equals(uc.Email) && u.Password.Equals(passMd5));

                if (userAutenticado == null)
                {
                    UtilHelper.myLogTxtRequest("(LoginApi) - Login API (Unauthorized)", _config, HttpContext);
                    return Unauthorized("Usuário ou senha inválido!");
                } 
                else
                {
                    var ret = await GeraToken(userAutenticado);
                    if (ret != null && ret.Authenticated)
                    {
                        UtilHelper.myLogTxtRequest("(LoginApi) - Login API. Token gerado! (Ok)", _config, HttpContext);
                        return Ok(ret.Token);
                    }

                    UtilHelper.myLogTxtRequest("(LoginApi) - Login API (BadRequest) ", _config, HttpContext);
                    return BadRequest();
                }
                  
            }
            catch (Exception ex)
            {
                UtilHelper.myLogTxtRequest("(LoginApi) - Login API (Internal Server Error) ", _config, HttpContext);
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao processar no servidor \n" + ex.Message + "\n" + ex.StackTrace);
            }   
        }

        private async Task<UsuarioToken?> GeraToken(UserCredential uc)
        {
            try
            {
                var claims = new[]
                {
                new Claim(JwtRegisteredClaimNames.UniqueName, uc.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };

                var key = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(_config["Jwt:Key"]));

                var credential = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var expiration = DateTime.UtcNow.AddHours(Convert.ToDouble(_config["TokenConfiguration:Expire"]));//em horas

                JwtSecurityToken token = new JwtSecurityToken(
                        issuer: _config["TokenConfiguration:Issuer"],
                        audience: _config["TokenConfiguration:Audience"],
                        claims: claims,
                        expires: expiration,
                        signingCredentials: credential
                 );

                return new UsuarioToken()
                {
                    Authenticated = true,
                    Token = new JwtSecurityTokenHandler().WriteToken(token),
                    Expiration = expiration,
                    Message = "Token JWT ok!"
                };
            }
            catch (Exception ex)
            {
                UtilHelper.myLogTxtSimple($"Error ao gerar token, \nusuário: {uc.Name} \n e-mail: {uc.Email} \n\n {ex.Message} \n\n {ex.StackTrace}", _config);
                return null;
            }
        }
    }
}
