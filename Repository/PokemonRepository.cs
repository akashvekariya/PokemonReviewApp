using PokemonReviewApp.Data;
using PokemonReviewApp.Models;
using PokemonReviewApp.Interfaces;

namespace PokemonReviewApp.Repository;

public class PokemonRepository : IPokemonRepository {
	public readonly DataContext _context;
	public PokemonRepository(DataContext context) {
		_context = context;
	}

	public ICollection<Pokemon> GetPokemons()
		=> (from pokemons in _context.Pokemons
			 orderby pokemons.Id
			 select pokemons).ToList();
}