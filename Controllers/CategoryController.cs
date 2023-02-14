using System;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CategoryController : Controller {
	public readonly ICategoryRepository _categoryRepository;
	private readonly IMapper _mapper;

	public CategoryController(ICategoryRepository categoryRepository, IMapper mapper) {
		_categoryRepository = categoryRepository;
		_mapper = mapper;
	}

	[HttpGet]
	[ProducesResponseType(200, Type = typeof(ICollection<Category>))]
	public IActionResult GetCategories() {
		var categories = _mapper.Map<List<CategoryDto>>(_categoryRepository.GetCategories());
		// var categories = _categoryRepository.GetCategories();

		if (!ModelState.IsValid)
			return BadRequest(ModelState);

		return Ok(categories);
	}

	[HttpGet("{categoryId}")]
	[ProducesResponseType(200, Type = typeof(Category))]
	[ProducesResponseType(400)]
	public IActionResult GetCategory(int categoryId) {
		if (!_categoryRepository.CategoryExists(categoryId))
			return NotFound();

		var category = _mapper.Map<CategoryDto>(_categoryRepository.GetCategory(categoryId));
		// var category = _categoryRepository.GetCategory(id);

		if (!ModelState.IsValid)
			return BadRequest();

		return Ok(category);
	}

	[HttpGet("{categoryId}/pokemons")]
	[ProducesResponseType(200, Type = typeof(decimal))]
	[ProducesResponseType(400)]
	public IActionResult GetPokemonsByCategory(int categoryId) {
		if (!_categoryRepository.CategoryExists(categoryId))
			return NotFound();

		var pokemons = _mapper.Map<PokemonDto>(_categoryRepository.GetPokemonsByCategory(categoryId));

		if (!ModelState.IsValid)
			return BadRequest();

		return Ok(pokemons);
	}

	[HttpPost]
	[ProducesResponseType(201)]
	[ProducesResponseType(400)]
	public IActionResult CreateCategory([FromBody] CategoryDto category) {
		if (category == null)
			return BadRequest(ModelState);

		if (!ModelState.IsValid)
			return BadRequest(ModelState);

		var cExists = (from c in _categoryRepository.GetCategories()
							where c.Name == category.Name
							select c).Any();

		if (cExists) {
			ModelState.AddModelError("", "Category already Exists!");
			return StatusCode(422, ModelState);
		}

		var categoryObj = _mapper.Map<Category>(category);
		if (!_categoryRepository.CreateCategory(categoryObj)) {
			ModelState.AddModelError("", "Something went wrong when saving");
			return StatusCode(500, ModelState);
		}

		return Ok("Successfully Created Category");
	}
}