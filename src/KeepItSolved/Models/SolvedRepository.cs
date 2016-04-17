using System;
using System.Collections.Generic;
using System.Linq;

namespace KeepItSolved.Models
{
	public class SolvedRepository : ISolvedRepository
	{
		private SolvedContext _context;

		public SolvedRepository(SolvedContext context)
		{
			_context = context;
		}

		public void AddCard(Flashcard newCard)
		{
			_context.Add(newCard);
		}

		public IEnumerable<Flashcard> GetAllFlashcards()
		{
			return _context.Flashcards.ToList();
		}

		public IEnumerable<Flashcard> GetUserFlashcards(string name)
		{
			try
			{
				return _context.Flashcards
					.Where(t => t.UserName == name)
					.ToList();
			}
			catch(Exception)
			{
				return null;
			}
		}

		public bool SaveAll()
		{
			return _context.SaveChanges() > 0;
		}
	}
}
