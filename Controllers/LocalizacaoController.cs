using System.Collections.Generic;
using System.Threading.Tasks;
using GUFOS_BackEnd.Domains;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GUFOS_BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocalizacaoController : ControllerBase
    {
        GufosContext _context = new GufosContext();


        // GET: api/Localizacao/
        [HttpGet]
        public async Task<ActionResult<List<Localizacao>>> Get()
        {
            var localizacao = await _context.Localizacao.ToListAsync();

            if (localizacao == null)
            {
                return NotFound();
            }

            return localizacao;
        }

        // GET: api/Localizacao/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Localizacao>> Get(int id)
        {
            var localizacao = await _context.Localizacao.FindAsync(id);

            if (localizacao == null)
            {
                return NotFound();
            }

            return localizacao;
        }

        // POST: api/Localizacao/
        [HttpPost]
        public async Task<ActionResult<Localizacao>> Post(Localizacao localizacao)
        {
            try
            {
                await _context.AddAsync(localizacao);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            return localizacao;
        }        


        // PUT: api/Localizacao/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(long id, Localizacao localizacao)
        {
            if (id != localizacao.LocalizacaoId)
            {
                return BadRequest();
            }

            _context.Entry(localizacao).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                var localizacao_valido = await _context.Localizacao.FindAsync(id);

                if (localizacao_valido == null)
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/Localizacao/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Localizacao>> Delete(int id)
        {
            var localizacao = await _context.Localizacao.FindAsync(id);
            if (localizacao == null)
            {
                return NotFound();
            }

            _context.Localizacao.Remove(localizacao);
            await _context.SaveChangesAsync();

            return localizacao;
        }



    }
}