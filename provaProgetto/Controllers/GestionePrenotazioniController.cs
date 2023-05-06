using System;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using provaProgetto.Models;

namespace provaProgetto.Controllers
{
    [Route("gestione")]
    [ApiController]
    public class GestionePrenotazioniController:Controller
	{
        private readonly ILogger<GestionePrenotazioniController> _logger;
        private readonly IConfiguration _configuration;

        private GestioneDati g;

        public GestionePrenotazioniController(ILogger<GestionePrenotazioniController> logger, IConfiguration configuration)
		{
			_logger = logger;
			_configuration = configuration;
            g = new GestioneDati(_configuration);
		}

        [HttpGet("appuntamenti/get/{id}")]
        public IActionResult getAppuntamento(int id)
        {
            Appuntamento a = g.GetAppuntamento(id);
            if(a!=null)
            {
                return Ok(a.id);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost("appuntamenti/put")]
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

        [Route("appuntamenti/get")]
        public IActionResult ListaAppuntamenti()
        {
            var listaApp = g.ListaAppuntamenti();
            return Ok(listaApp);
        }

        [Route("eventi/get")]
        public IActionResult ListaEventi()
        {
            var listaApp = g.ListaAppuntamenti();
            return Ok(listaApp);
        }

        [HttpPost("evento/put")]
        public IActionResult InserisciEvento([FromBody] Evento e)
        {
            bool esito = g.InsierisciEvento(e);
            if (esito)
            {
                return Ok(esito);
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Errore inserimento");
            }

        }

    }
}

