using PokemonReviewApp.Data;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Repository {
	public class PokemonRepository : IPokemonRepository {

		private readonly DataContext _context;

		public PokemonRepository(DataContext context) {
			_context = context;
		}

		public bool CreatePokemon(int ownerId, int categoryId, Pokemon pokemon) {
			var pokemonOwnerEntity = (from o in _context.Owners
											  where o.Id == ownerId
											  select o).FirstOrDefault();
			var category = (from c in _context.Categories
								 where c.Id == categoryId
								 select c).FirstOrDefault();

			var pokemonOwner = new PokemonOwner() {
				Owner = pokemonOwnerEntity,
				Pokemon = pokemon,
			};

			_context.Add(pokemonOwner);

			var pokemonCategory = new PokemonCategory() {
				Category = category,
				Pokemon = pokemon,
			};

			_context.Add(pokemonCategory);

			_context.Add(pokemon);

			return Save();
		}

		public bool DeletePokemon(Pokemon pokemon) {
			_context.Remove(pokemon);
			return Save();
		}

		public Pokemon GetPokemon(int id) {
			return (from p in _context.Pokemons
					  where p.Id == id
					  select p).FirstOrDefault();
		}

		public Pokemon GetPokemon(string name) {
			return (from p in _context.Pokemons
					  where p.Name == name
					  select p).FirstOrDefault();
		}

		public decimal GetPokemonRating(int pokeId) {
			var review = (from r in _context.Reviews
							  where r.Pokemon.Id == pokeId
							  select r);

			if (!review.Any())
				return 0;

			return (decimal)(review.Sum(r => r.Rating) / review.Count());
		}

		public ICollection<Pokemon> GetPokemons() {
			return (from p in _context.Pokemons
					  select p).ToList();
		}

		public Pokemon GetPokemonTrimToUpper(PokemonDto pokemonCreate) {
			return (from p in _context.Pokemons
					  where string.Equals(p.Name.Trim(), pokemonCreate.Name.Trim(), StringComparison.OrdinalIgnoreCase)
					  select p).FirstOrDefault();
		}

		public bool PokemonExists(int pokeId) {
			return (from p in _context.Pokemons
					  where p.Id == pokeId
					  select p).Any();
		}

		public bool Save() {
			var saved = _context.SaveChanges();
			return saved > 0;
		}

		public bool UpdatePokemon(int ownerId, int categoryId, Pokemon pokemon) {
			_context.Update(pokemon);
			return Save();
		}
	}
}
