using PokemonReviewApp.Data;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Repository {
	public class OwnerRepository : IOwnerRepository {

		private readonly DataContext _context;

		public OwnerRepository(DataContext context) {
			_context = context;
		}

		public bool CreateOwner(Owner owner) {
			_context.Add(owner);
			return Save();
		}

		public bool DeleteOwner(Owner owner) {
			_context.Remove(owner);
			return Save();
		}

		public Owner GetOwner(int ownerId) {
			return (from o in _context.Owners
					  where o.Id == ownerId
					  select o).FirstOrDefault();
		}

		public ICollection<Owner> GetOwnerOfAPokemon(int pokeId) {
			return (from o in _context.Owners
					  where o.PokemonOwners.Any(p => p.Pokemon.Id == pokeId)
					  select o).ToList();
		}

		public ICollection<Owner> GetOwners() {
			return (from o in _context.Owners
					  select o).ToList();
		}

		public ICollection<Pokemon> GetPokemonByOwner(int ownerId) {
			return (from p in _context.PokemonOwners
					  where p.Owner.Id == ownerId
					  select p.Pokemon).ToList();
		}

		public bool OwnerExists(int ownerId) {
			return (from o in _context.Owners
					  where o.Id == ownerId
					  select o).Any();
		}

		public bool Save() {
			var saved = _context.SaveChanges();
			return saved > 0;
		}

		public bool UpdateOwner(Owner owner) {
			_context.Update(owner);
			return Save();
		}
	}
}
