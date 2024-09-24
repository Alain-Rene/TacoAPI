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
    public class DrinksController : ControllerBase
    {
        TacoDbContext dbContext = new TacoDbContext();

        [HttpGet()]
        public IActionResult GetAll(string? SortByCost)
        {
            List<Drink> result = dbContext.Drinks.ToList();

            if(SortByCost != null)
            {
                if(SortByCost.ToLower().Equals("ascending"))
                {
                    result = result.OrderBy(d => d.Cost).ToList();
                }
                else if(SortByCost.ToLower().Equals("descending"))
                {
                    result = result.OrderByDescending(d => d.Cost).ToList();
                }
            }

            return Ok(result);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            Drink drink = dbContext.Drinks.FirstOrDefault(d => d.Id == id);

            if(drink == null)
            {
                return NotFound("Id not found");
            }
            else
            {
                return Ok(drink);
            }
        }

        [HttpPost()]
        public IActionResult AddDrink([FromBody] Drink newDrink)
        {
            newDrink.Id = 0;
            dbContext.Drinks.Add(newDrink);

            dbContext.SaveChanges();

            return Created($"/api/Drink/{newDrink.Id}", newDrink);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateDrinks(int id, [FromBody] Drink updatedDrink)
        {
            if (id != updatedDrink.Id) {return BadRequest("Ids don't match");}

            if(dbContext.Drinks.Any(d => d.Id == id) == false) {return NotFound("No matching IDs");}

            dbContext.Drinks.Update(updatedDrink);
            dbContext.SaveChanges();
            return Ok(updatedDrink);
        }
       
    }
}