using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TacoAPI.Models;

namespace TacoAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CombosController : ControllerBase
    {
        TacoDbContext dbContext = new TacoDbContext();

        [HttpGet()]
        public IActionResult GetAll()
        {
            List<Combo> result = dbContext.Combos.ToList();

            return Ok(result);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            Combo combo = dbContext.Combos.FirstOrDefault(c => c.Id == id);

            if (combo == null)
            {
                return NotFound("Id not found");
            }
            else
            {
                return Ok(combo);
            }
        }
        [HttpPost()]
        public IActionResult AddCombo([FromBody] Combo newCombo)
        {
            newCombo.Id = 0;
            dbContext.Combos.Add(newCombo);

            dbContext.SaveChanges();

            return Created($"/api/Drink/{newCombo.Id}", newCombo);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteCombo(int id)
        {
            Combo c = dbContext.Combos.FirstOrDefault(x => x.Id == id);
            if(c == null)
            {
                return NotFound("No matching id");
            }
            else
            {
    
                // Prevent table relation errors
                List<Combo> matchingCombos= dbContext.Combos.Where(x => x.Id == c.Id).ToList();

                dbContext.Combos.RemoveRange(matchingCombos);

                dbContext.Combos.Remove(c);
                dbContext.SaveChanges();
                return NoContent();
            }
        }

    }
    
}