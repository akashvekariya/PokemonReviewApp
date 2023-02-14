using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PokemonReviewApp.Data;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Repository {
	public class ReviewerRepository : IReviewerRepository {
		private readonly DataContext _context;

		public ReviewerRepository(DataContext context) {
			_context = context;
		}

		public bool CreateReviewer(Reviewer reviewer) {
			_context.Add(reviewer);
			return Save();
		}

		public bool DeleteReviewer(Reviewer reviewer) {
			_context.Remove(reviewer);
			return Save();
		}

		public Reviewer GetReviewer(int reviewerId) {
			return _context.Reviewers.Where(r => r.Id == reviewerId).Include(e => e.Reviews).FirstOrDefault();
		}

		public ICollection<Reviewer> GetReviewers() {
			return (from r in _context.Reviewers
					  select r).ToList();
		}

		public ICollection<Review> GetReviewsByReviewer(int reviewerId) {
			return (from r in _context.Reviews
					  where r.Reviewer.Id == reviewerId
					  select r).ToList();
		}

		public bool ReviewerExists(int reviewerId) {
			return (from r in _context.Reviewers
					  where r.Id == reviewerId
					  select r).Any();
		}

		public bool Save() {
			var saved = _context.SaveChanges();
			return saved > 0;
		}

		public bool UpdateReviewer(Reviewer reviewer) {
			_context.Update(reviewer);
			return Save();
		}
	}
}
