using EntityFramework.DbContextScope.Interfaces;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using Organizers.MovOrg.Domain;

namespace Infrastructure.MovOrg.EFCore
{
	public class MoviesContext : DbContext, IDbContext
	{
		public static readonly LoggerFactory _myLoggerFactory = new LoggerFactory(new[] {
		new Microsoft.Extensions.Logging.Debug.DebugLoggerProvider()
	});

		public DbSet<Movie> Movies { get; set; }

		public DbSet<BoxOffice> BoxOffices { get; set; }

		public DbSet<Trailer> Trailers { get; set; }

		public DbSet<Company> Companies { get; set; }
		public DbSet<MovieCompany> MovieCompanies { get; set; }

		public DbSet<Person> People { get; set; }

		public DbSet<MovieImageData> MovieImageDatas { get; set; }
		public DbSet<MovieActor> MovieActors { get; set; }

		public DbSet<MovieDirector> MovieDirectors { get; set; }

		public DbSet<MovieWriter> MovieWriters { get; set; }

		public DbSet<MovieSimilar> MovieSimilars { get; set; }

		public DbSet<Rating> Ratings { get; set; }

		public DbSet<RatingSource> RatingSources { get; set; }

		public MoviesContext(DbContextOptions options)
		 : base(options)
		{
		}

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			//optionsBuilder.UseLoggerFactory(_myLoggerFactory);
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<MovieActor>()
				.HasKey(c => new { c.MovieId, c.PersonId });

			modelBuilder.Entity<MovieDirector>()
				.HasKey(c => new { c.MovieId, c.PersonId });

			modelBuilder.Entity<MovieWriter>()
				.HasKey(c => new { c.MovieId, c.PersonId });

			modelBuilder.Entity<MovieCompany>()
				.HasKey(c => new { c.MovieId, c.CompanyId });

			modelBuilder.Entity<MovieSimilar>()
				.HasKey(c => new { c.MovieId, c.SimilarId });

			modelBuilder.Entity<Rating>()
				.HasKey(c => new { c.MovieId, c.SourceId });

			modelBuilder.Entity<MovieImageData>()
				.HasKey(c => new { c.MovieId, c.Id });

			modelBuilder.Entity<Rating>()
				.HasOne(x => x.Movie)
				.WithMany(x => x.Ratings)
				.HasForeignKey(x => x.MovieId);

			modelBuilder.Entity<RatingSource>()
				.HasMany(x => x.Ratings)
				.WithOne(x => x.Source)
				.HasForeignKey(x => x.SourceId);

			modelBuilder.Entity<Movie>()
				.HasOne(x => x.BoxOffice)
				.WithOne(x => x.Movie)
				.HasForeignKey<BoxOffice>(x => x.Id);

			modelBuilder.Entity<Movie>()
				.HasOne(x => x.Trailer)
				.WithOne(x => x.Movie)
				.HasForeignKey<Trailer>(x => x.Id);

			modelBuilder.Entity<Movie>()
				.HasMany(x => x.Images)
				.WithOne(x => x.Movie)
				.HasForeignKey(x => x.MovieId);

			modelBuilder.Entity<Movie>()
				.HasMany(x => x.Similars)
				.WithOne(x => x.Movie)
				.HasForeignKey(x => x.MovieId).OnDelete(DeleteBehavior.NoAction);
		}
	}
}