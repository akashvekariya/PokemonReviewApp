using PokemonReviewApp.Data;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Repository {
	public class CategoryRepository : ICategoryRepository {

		private readonly DataContext _context;
		public CategoryRepository(DataContext context) {
			_context = context;
		}

		public bool CategoryExists(int id) {
			return (from c in _context.Categories
					  where c.Id == id
					  select c).Any();
		}

		public bool CreateCategory(Category category) {
			_context.Add(category);
			return Save();
		}

		public bool DeleteCategory(Category category) {
			_context.Remove(category);
			return Save();
		}

		public ICollection<Category> GetCategories() {
			return (from c in _context.Categories
					  select c).ToList();
		}

		public Category GetCategory(int id) {
			return (from c in _context.Categories
					  where c.Id == id
					  select c).FirstOrDefault();
		}

		public ICollection<Pokemon> GetPokemonsByCategory(int categoryId) {
			return (from c in _context.PokemonCategories
					  where c.Category.Id == categoryId
					  select c.Pokemon).ToList();
		}

		public bool Save() {
			var saved = _context.SaveChanges();
			return saved > 0;
		}

		public bool UpdateCategory(Category category) {
			_context.Update(category);
			return Save();
		}
	}
}
