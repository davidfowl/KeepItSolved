using System.Collections.Generic;

namespace KeepItSolved.Models
{
	public interface ISolvedRepository
	{
		IEnumerable<Flashcard> GetAllFlashcards();
		void AddCard(Flashcard newCard);
		bool SaveAll();
		IEnumerable<Flashcard> GetUserFlashcards(string name);
	}
}