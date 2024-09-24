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
    public class TacosController : ControllerBase
    {
        TacoDbContext dbContext = new TacoDbContext();

        [HttpGet()]
        public IActionResult GetAll(bool? softShell)
        {
            List<Taco> result = dbContext.Tacos.ToList();

            if(softShell != null)
            {
                result = result.Where(t => t.SoftShell == softShell).ToList();
            }
            return Ok(result);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            Taco taco = dbContext.Tacos.FirstOrDefault(t => t.Id == id);

            if(taco == null)
            {
                return NotFound("Id not found");
            }
            else
            {
                return Ok(taco);
            }
        }

        [HttpPost()]
        public IActionResult AddTaco([FromBody] Taco newTaco)
        {
            newTaco.Id = 0;
            dbContext.Tacos.Add(newTaco);

            dbContext.SaveChanges();

            return Created($"/api/Taco/{newTaco.Id}", newTaco);
        }


        [HttpDelete("{id}")]
        public IActionResult DeleteTaco(int id)
        {
            Taco t = dbContext.Tacos.FirstOrDefault(x => x.Id == id);
            if(t == null)
            {
                return NotFound("No matching id");
            }
            else
            {
    
                // Prevent table relation errors
                List<Taco> matchingTacos= dbContext.Tacos.Where(x => x.Id == t.Id).ToList();

                dbContext.Tacos.RemoveRange(matchingTacos);

                dbContext.Tacos.Remove(t);
                dbContext.SaveChanges();
                return NoContent();
            }
        }
    }
}