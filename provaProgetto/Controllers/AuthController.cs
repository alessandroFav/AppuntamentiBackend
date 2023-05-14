using System;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using provaProgetto.Models;
using BC = BCrypt.Net.BCrypt;

namespace provaProgetto.Controllers
{
    [Route("auth")]
    [ApiController]
    public class AuthController:Controller
	{
        private readonly ILogger<GestionePrenotazioniController> _logger;
        private readonly IConfiguration _configuration;

        private GestioneUtente g;
        private GestioneMail gMail;
        private JwtManager jwt;

        public AuthController(ILogger<GestionePrenotazioniController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
            g = new GestioneUtente(_configuration);
            gMail = new GestioneMail();
            jwt = new JwtManager(_configuration);
        }

        [HttpPost("registrazione")]
        public IActionResult RegistraUtante([FromBody] Registrazione userData)
        {
            if (!ModelState.IsValid) { return StatusCode(StatusCodes.Status400BadRequest, "Bad request"); }
            if (g.FindUtente(userData.mail) == null)
            {
                Utente? esito = g.InserisciUtente(userData);
                if (esito != null)
                {
                    string baseURL = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}";
                    gMail.SendMailVerificationAsync(esito!.id, baseURL);
                    return Ok(true);
                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Errore inserimento");
                }
            }
            else
            {
                return StatusCode(StatusCodes.Status400BadRequest, "Email già utilizzata");
            }
        }
        [HttpGet("emailVerification/{id}")]
        public IActionResult EmailVerification(int id)
        {
            Utente? user = g.FindUtente(id);
            if(user != null)
            {
                if(user!.VerifiedAt == null)
                {
                    string baseURL = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}";
                    gMail.SendMailVerificationAsync(user!.id, baseURL);
                    return Ok(true);
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest, "Email già verificata");
                }
            }
            else
            {
                return StatusCode(StatusCodes.Status404NotFound, "Utente non trovato");
            }
        }
        [HttpGet("verificaEmail/{id}")]
        public IActionResult VerificaEmail(int id)
        {
            Utente? user = g.FindUtente(id);
            if(user != null)
            {
                if(user.VerifiedAt == null)
                {
                    user.VerifiedAt = DateTime.Now;
                    if (g.UpdateUtente(user!))
                    {
                        return Ok(true);
                    }
                    else
                    {
                        return StatusCode(StatusCodes.Status500InternalServerError, "Errore inserimento");
                    }
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest, "Email già verificata");
                }

            }
            else
            {
                return StatusCode(StatusCodes.Status404NotFound, "Utente non trovato");
            }
        }
        [HttpPost("login")]
        public IActionResult Login([FromBody] Login userData)
        {
            if (!ModelState.IsValid) { return StatusCode(StatusCodes.Status400BadRequest, "Bad request"); }
            Utente? esito = g.FindUtente(userData.email);
            if (esito != null)
            {
                if(esito!.VerifiedAt == null) { return StatusCode(StatusCodes.Status401Unauthorized, "Email non verificata"); }
                if(BC.Verify(userData.password, esito!.password))
                {
                    var token = jwt.GenerateJwtToken(esito!);
                    return Ok(token);
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest, "Email e password non corrispondono");
                }
            }
            else
            {
                return StatusCode(StatusCodes.Status404NotFound, "Utente non trovato");
            }
        }
    }
}

