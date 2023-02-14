﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CountryController : Controller {
	private readonly ICountryRepository _countryRepository;
	private readonly IMapper _mapper;

	public CountryController(ICountryRepository countryRepository, IMapper mapper) {
		_countryRepository = countryRepository;
		_mapper = mapper;
	}

	[HttpGet]
	[ProducesResponseType(200, Type = typeof(IEnumerable<Country>))]
	public IActionResult GetCountries() {
		var countries = _mapper.Map<List<CountryDto>>(_countryRepository.GetCountries());

		if (!ModelState.IsValid)
			return BadRequest(ModelState);

		return Ok(countries);
	}

	[HttpGet("{countryId}")]
	[ProducesResponseType(200, Type = typeof(Country))]
	[ProducesResponseType(400)]
	public IActionResult GetCountry(int countryId) {
		if (!_countryRepository.CountryExists(countryId))
			return NotFound();

		var country = _mapper.Map<CountryDto>(_countryRepository.GetCountry(countryId));

		if (!ModelState.IsValid)
			return BadRequest(ModelState);

		return Ok(country);
	}

	[HttpGet("/owners/{ownerId}")]
	[ProducesResponseType(400)]
	[ProducesResponseType(200, Type = typeof(Country))]
	public IActionResult GetCountryOfAnOwner(int ownerId) {
		var country = _mapper.Map<CountryDto>(
			 _countryRepository.GetCountryByOwner(ownerId));

		if (!ModelState.IsValid)
			return BadRequest();

		return Ok(country);
	}

	[HttpPost]
	[ProducesResponseType(201)]
	[ProducesResponseType(400)]
	public IActionResult CreateCountry([FromBody] CountryDto country) {
		if (country == null)
			return BadRequest(ModelState);

		if (!ModelState.IsValid)
			return BadRequest(ModelState);

		var cExists = (from c in _countryRepository.GetCountries()
							where c.Name == country.Name
							select c).Any();

		if (cExists) {
			ModelState.AddModelError("", "Country already Exists!");
			return StatusCode(422, ModelState);
		}

		var countryObj = _mapper.Map<Country>(country);
		if (!_countryRepository.CreateCountry(countryObj)) {
			ModelState.AddModelError("", "Something went wrong when saving");
			return StatusCode(500, ModelState);
		}

		return Ok("Successfully Created Country");
	}

}