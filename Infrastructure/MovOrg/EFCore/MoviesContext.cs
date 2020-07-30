using EntityFramework.DbContextScope.Interfaces;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using Organizers.MovOrg.Domain;

namespace Infrastructure.EFCore
{
	public class MoviesContext : DbContext, IDbContext
	{
		public static readonly LoggerFactory _myLoggerFactory = new LoggerFactory(new[] {
		new Microsoft.Extensions.Logging.Debug.DebugLoggerProvider()
	});

		public DbSet<Movie> Movies { get; set; }

		public DbSet<BoxOffice> BoxOffices { get; set; }

		public DbSet<Trailer> Trailers { get; set; }
		public DbSet<Director> Directors { get; set; }

		public DbSet<Actor> Actors { get; set; }

		public DbSet<Company> Companies { get; set; }
		public DbSet<Writer> Writers { get; set; }

		public DbSet<MovieDirector> MoviesDirectors { get; set; }
		public DbSet<MovieActor> MoviesActors { get; set; }
		public DbSet<MovieCompany> MoviesCompanies { get; set; }

		public DbSet<MovieWriter> MoviesWriters { get; set; }

		public DbSet<Rating> Ratings { get; set; }

		public DbSet<RatingSource> RatingSources { get; set; }

		public MoviesContext(DbContextOptions options)
		 : base(options)
		{
		}

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseLoggerFactory(_myLoggerFactory);
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<MovieDirector>()
							 .HasKey(c => new { c.MovieId, c.DirectorId });

			modelBuilder.Entity<MovieActor>()
							.HasKey(c => new { c.MovieId, c.ActorId });

			modelBuilder.Entity<MovieCompany>()
						.HasKey(c => new { c.MovieId, c.CompanyId });

			modelBuilder.Entity<MovieWriter>()
						.HasKey(c => new { c.MovieId, c.WriterId });

			modelBuilder.Entity<Rating>()
				.HasKey(c => new { c.MovieId, c.RatingSourceId });

			modelBuilder.Entity<Rating>()
				.HasOne(x => x.Movie)
				.WithMany(x => x.Ratings)
				.HasForeignKey(x => x.MovieId);

			modelBuilder.Entity<RatingSource>()
				.HasMany(x => x.Ratings)
				.WithOne(x => x.RatingSource)
				.HasForeignKey(x => x.RatingSourceId);

			modelBuilder.Entity<Movie>()
				.HasOne(x => x.BoxOffice)
				.WithOne(x => x.Movie)
				.HasForeignKey<BoxOffice>(x => x.Id);

			modelBuilder.Entity<Movie>()
				.HasOne(x => x.Trailer)
				.WithOne(x => x.Movie)
				.HasForeignKey<Trailer>(x => x.Id);
		}
	}
}