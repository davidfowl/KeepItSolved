using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Data.Entity;

namespace KeepItSolved.Models
{
	public class SolvedContext : IdentityDbContext<IdentityUser>
    {
		public SolvedContext()
		{
			Database.EnsureCreated();
		}

		public DbSet<Flashcard> Flashcards { get; set; }

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			var connString = Startup.Configuration["Data:SolvedContextConnection"];

			optionsBuilder.UseSqlServer(connString);

			base.OnConfiguring(optionsBuilder);
		}
    }
}
