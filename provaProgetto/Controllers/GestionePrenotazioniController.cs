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

        [HttpGet("appuntamenti/{id}")]
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
        [HttpGet("appuntamenti/get")]
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

        [HttpPut("appuntamenti/{id}")]
        public IActionResult UpdateAppuntamento([FromBody] Appuntamento a)
        {
            bool esito = g.UpdateAppuntamento(a);
            if (esito)
            {
                return Ok(esito);
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Errore update");
            }
        }

        [HttpDelete("appuntamenti/{id}")]
        public IActionResult DeleteAppuntamento(int id)
        {
            var listaApp = g.DeleteAppuntamento(id);
            return Ok(listaApp);
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

        [HttpPut("eventi/{id}")]
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

        [HttpDelete("eventi/{id}")]
        public IActionResult DeleteEvento(int id)
        {
            var listaApp = g.DeleteEvento(id);
            return Ok(listaApp);
        }



    }
}

