using System;
using Microsoft.AspNetCore.Mvc;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PokemonController : Controller {
	public readonly IPokemonRepository _pokemonRepository;
	public PokemonController(IPokemonRepository pokemonRepository) {
		_pokemonRepository = pokemonRepository;
	}

	[HttpGet("allPokemon")]
	[ProducesResponseType(200, Type = typeof(ICollection<Pokemon>))]
	public IActionResult GetPokemons() {
		var pokemons = _pokemonRepository.GetPokemons();

		if (!ModelState.IsValid)
			return BadRequest(ModelState);

		return Ok(pokemons);
	}
}