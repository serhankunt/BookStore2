using BookStoreServer.WebApi.Context;
using BookStoreServer.WebApi.Dtos;
using BookStoreServer.WebApi.Models;
using GSF.FuzzyStrings;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreServer.WebApi.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public sealed class CategoriesController : ControllerBase
{
    [HttpGet]
    public IActionResult Create(CreateCategoryDto request)
    {
        AppDbContext context = new();
        var checkNameIsUnique = context.Categories.Any(p=>p.Name == request.Name);
        if (checkNameIsUnique)
        {
            return BadRequest("Kategori adı daha önce kullanılmıştır.");
        }

        Category category = new()
        {
            Name = request.Name,
            IsActive = true,
            IsDeleted = false
        };
        
        context.Categories.Add(category);
        context.SaveChanges();
        return NoContent();
    }
    [HttpGet("{id}")] //localhost:7082/api/Categories/RemoveById/1
    public IActionResult RemoveById(int id)
    {
        AppDbContext context = new();
        Category category = context.Categories.Find(id);
        if (category == null) { return NotFound(); }
        category.IsDeleted = true;
        context.SaveChanges();
        return NoContent();
    }
    [HttpPost]
    public IActionResult Update(UpdateCategoryDto request)
    {
        AppDbContext context = new();
        Category category = context.Categories.Find(request.Id);
        if(category == null) { return NotFound(); }

        category.Name = request.Name;
        context.SaveChanges();
        return NoContent();
    }



    [HttpGet]
    public IActionResult GetAll()
    {
        return Ok(GetAllCategory());
    }
    private List<Category> GetAllCategory()
    {
        AppDbContext context = new();
        var categories =
            context.Categories
            .Where(p => p.IsActive == true && p.IsDeleted == false)
            .OrderBy(o => o.Name)
            .ToList();//Select *From Categories where IsActive = 1 and IsDeleted = 0 
        return categories;
    }
}