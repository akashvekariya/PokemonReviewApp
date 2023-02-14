using AutoMapper;
using PokemonReviewApp.Data;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Repository {
	public class CountryRepository : ICountryRepository {
		private readonly DataContext _context;

		public CountryRepository(DataContext context) {
			_context = context;
		}
		public bool CountryExists(int id) {
			return (from c in _context.Countries
					  where c.Id == id
					  select c).Any();
		}

		public bool CreateCountry(Country country) {
			_context.Add(country);
			return Save();
		}

		public bool DeleteCountry(Country country) {
			_context.Remove(country);
			return Save();
		}

		public ICollection<Country> GetCountries() {
			return (from c in _context.Countries
					  select c).ToList();
		}

		public Country GetCountry(int id) {
			return (from c in _context.Countries
					  where c.Id == id
					  select c).FirstOrDefault();
		}

		public Country GetCountryByOwner(int ownerId) {
			return (from c in _context.Owners
					  where c.Id == ownerId
					  select c.Country).FirstOrDefault();
		}

		public ICollection<Owner> GetOwnersFromACountry(int countryId) {
			return (from c in _context.Owners
					  where c.Country.Id == countryId
					  select c).ToList();
		}

		public bool Save() {
			var saved = _context.SaveChanges();
			return saved > 0;
		}

		public bool UpdateCountry(Country country) {
			_context.Update(country);
			return Save();
		}
	}
}
