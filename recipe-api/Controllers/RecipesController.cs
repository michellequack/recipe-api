using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecipeApi.Models;
using Microsoft.AspNetCore.Cors;

namespace RecipeApi.Controllers
{
    [EnableCors("SiteCorsPolicy")]
    [Produces("application/json")]
    [Route("api/Recipes")]
    public class RecipesController : Controller
    {
        private readonly recipeContext _context;

        public RecipesController(recipeContext context)
        {
            _context = context;
        }

        // GET: api/Recipes
        [HttpGet]
        public IEnumerable<Recipe> GetRecipes()
        {
            try
            {
                IEnumerable <Recipe> recipes = _context.Recipe.Include(r => r.Ingredient).OrderBy(r => r.Category).ThenBy(r => r.Name);
                return recipes;
            }
            catch(Exception ex)
            {
                Recipe r = new Recipe();
                r.Name = ex.Message;
                List<Recipe> rs = new List<Recipe>();
                rs.Add(r);
                return rs;
            }
            
        }

        // GET: api/Recipes/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetRecipe([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var recipe = await _context.Recipe.SingleOrDefaultAsync(m => m.Id == id);

            if (recipe == null)
            {
                return NotFound();
            }

            return Ok(recipe);
        }

        // PUT: api/Recipes/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRecipe([FromRoute] int id, [FromBody] Recipe recipe)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != recipe.Id)
            {
                return BadRequest();
            }

            _context.Entry(recipe).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RecipeExists(id))
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

        // POST: api/Recipes
        [HttpPost]
        public async Task<IActionResult> PostRecipe([FromBody] Recipe recipe)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Recipe.Add(recipe);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRecipe", new { id = recipe.Id }, recipe);
        }

        // DELETE: api/Recipes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRecipe([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var recipe = await _context.Recipe.SingleOrDefaultAsync(m => m.Id == id);
            if (recipe == null)
            {
                return NotFound();
            }

            _context.Recipe.Remove(recipe);
            await _context.SaveChangesAsync();

            return Ok(recipe);
        }

        private bool RecipeExists(int id)
        {
            return _context.Recipe.Any(e => e.Id == id);
        }
    }
}