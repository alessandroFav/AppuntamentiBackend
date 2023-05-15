using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using provaProgetto.Attributes;
using provaProgetto.Middlewares;
using provaProgetto.Models;

namespace provaProgetto.Controllers
{
    [Route("gestione")]
    [ApiController]
    //[Authorize]
    [provaProgetto.Attributes.Authorize]
    public class GestionePrenotazioniController:Controller
	{
        private readonly ILogger<GestionePrenotazioniController> _logger;
        private readonly IConfiguration _configuration;
        private HttpContext _context;

        private GestioneDati g;
        private GestioneUtente gUtenti;

        public GestionePrenotazioniController(ILogger<GestionePrenotazioniController> logger, IConfiguration configuration, 
            IHttpContextAccessor httpContextAccessor)
		{
			_logger = logger;
			_configuration = configuration;
            _context = httpContextAccessor.HttpContext!;
            g = new GestioneDati(_configuration);
            gUtenti = new GestioneUtente(_configuration);
		}

        [HttpGet("utenti/{id}")]
        public IActionResult GetUtente(int id)
        {
            Utente? user = gUtenti.FindUtente(id);
            if (user != null) {
                return Ok(user!);
            }
            else
            {
                return StatusCode(StatusCodes.Status404NotFound, "User not found");
            }
        }

        [HttpGet("appuntamenti/{idAppuntamento}")]
        public IActionResult getAppuntamento(int idAppuntamento)
        {
            //Utente user = (Utente)_context.Items["user"];
            Appuntamento a = g.GetAppuntamento(idAppuntamento);
            if(a!=null)
            {
                return Ok(a.id);
            }
            else
            {
                return NotFound();
            }
        }
        [HttpGet("appuntamenti")]
        public IActionResult ListaAppuntamenti()
        {
            var listaApp = g.ListaAppuntamenti();
            return Ok(listaApp);
        }


        [HttpPost("appuntamenti")]
        public IActionResult InserisciAppuntamento([FromBody] Appuntamento app)
        {
            bool esito = g.InserisciAppuntamento(app);
            if (esito)
            {
                return Ok(esito);
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError,"Errore inserimento");
            }

        }

        

        //
        [HttpGet("eventi")]
        public IActionResult ListaEventi()
        {
            var listaApp = g.ListaAppuntamenti();
            return Ok(listaApp);
        }

        [HttpPost("eventi")]
        public IActionResult InserisciEvento([FromBody] Evento e)
        {
            bool esito = g.InserisciEvento(e);
            if (esito)
            {
                return Ok(esito);
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Errore inserimento");
            }
        }

        [HttpPut("eventi/{idEvento}")]
        public IActionResult UpdateEvento([FromBody] Evento e)
        {
            bool esito = g.UpdateEvento(e);
            if (esito)
            {
                return Ok(esito);
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Errore update");
            }
        }

        //[HttpDelete("eventi/{id}")]
        //public IActionResult ListaEventi()
        //{
        //    var listaApp = g.ListaAppuntamenti();
        //    return Ok(listaApp);
        //}



    }
}

