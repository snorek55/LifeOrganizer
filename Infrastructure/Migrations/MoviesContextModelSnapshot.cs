﻿// <auto-generated />
using System;
using Infrastructure.MovOrg.EFCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Infrastructure.Migrations
{
    [DbContext(typeof(MoviesContext))]
    partial class MoviesContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Organizers.MovOrg.Domain.BoxOffice", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Budget")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CumulativeWorldwideGross")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("GrossUSA")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("OpeningWeekendUSA")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("BoxOffices");
                });

            modelBuilder.Entity("Organizers.MovOrg.Domain.Company", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Companies");
                });

            modelBuilder.Entity("Organizers.MovOrg.Domain.Movie", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Awards")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Countries")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CoverImage")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Genres")
                        .HasColumnType("nvarchar(max)");

                    b.Property<float?>("IMDbRating")
                        .HasColumnType("real");

                    b.Property<bool>("IsFavorite")
                        .HasColumnType("bit");

                    b.Property<bool>("IsMustWatch")
                        .HasColumnType("bit");

                    b.Property<bool>("IsWatched")
                        .HasColumnType("bit");

                    b.Property<string>("Keywords")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Languages")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("LastUpdatedDetails")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("LastUpdatedTop250")
                        .HasColumnType("datetime2");

                    b.Property<string>("Plot")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("Rank")
                        .HasColumnType("int");

                    b.Property<string>("ReleaseDate")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RuntimeStr")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Tagline")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("WikipediaUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Year")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Movies");
                });

            modelBuilder.Entity("Organizers.MovOrg.Domain.MovieActor", b =>
                {
                    b.Property<string>("MovieId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("PersonId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("AsCharacter")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsStar")
                        .HasColumnType("bit");

                    b.HasKey("MovieId", "PersonId");

                    b.HasIndex("PersonId");

                    b.ToTable("MovieActors");
                });

            modelBuilder.Entity("Organizers.MovOrg.Domain.MovieCompany", b =>
                {
                    b.Property<string>("MovieId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("CompanyId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("MovieId", "CompanyId");

                    b.HasIndex("CompanyId");

                    b.ToTable("MovieCompanies");
                });

            modelBuilder.Entity("Organizers.MovOrg.Domain.MovieDirector", b =>
                {
                    b.Property<string>("MovieId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("PersonId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("MovieId", "PersonId");

                    b.HasIndex("PersonId");

                    b.ToTable("MovieDirectors");
                });

            modelBuilder.Entity("Organizers.MovOrg.Domain.MovieImageData", b =>
                {
                    b.Property<string>("MovieId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Image")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("MovieId", "Id");

                    b.ToTable("MovieImageDatas");
                });

            modelBuilder.Entity("Organizers.MovOrg.Domain.MovieSimilar", b =>
                {
                    b.Property<string>("MovieId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("SimilarId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("MovieId", "SimilarId");

                    b.HasIndex("SimilarId");

                    b.ToTable("MovieSimilars");
                });

            modelBuilder.Entity("Organizers.MovOrg.Domain.MovieWriter", b =>
                {
                    b.Property<string>("MovieId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("PersonId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("MovieId", "PersonId");

                    b.HasIndex("PersonId");

                    b.ToTable("MovieWriters");
                });

            modelBuilder.Entity("Organizers.MovOrg.Domain.Person", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Awards")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("BirthDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DeathDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Height")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ImageUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Role")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Summary")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("People");
                });

            modelBuilder.Entity("Organizers.MovOrg.Domain.Rating", b =>
                {
                    b.Property<string>("MovieId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("SourceId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(max)");

                    b.Property<float?>("Score")
                        .HasColumnType("real");

                    b.HasKey("MovieId", "SourceId");

                    b.HasIndex("SourceId");

                    b.ToTable("Ratings");
                });

            modelBuilder.Entity("Organizers.MovOrg.Domain.RatingSource", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LogoUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("RatingSources");
                });

            modelBuilder.Entity("Organizers.MovOrg.Domain.Trailer", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LinkEmbed")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ThumbnailUrl")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Trailers");
                });

            modelBuilder.Entity("Organizers.MovOrg.Domain.BoxOffice", b =>
                {
                    b.HasOne("Organizers.MovOrg.Domain.Movie", "Movie")
                        .WithOne("BoxOffice")
                        .HasForeignKey("Organizers.MovOrg.Domain.BoxOffice", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Organizers.MovOrg.Domain.MovieActor", b =>
                {
                    b.HasOne("Organizers.MovOrg.Domain.Movie", "Movie")
                        .WithMany("ActorList")
                        .HasForeignKey("MovieId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Organizers.MovOrg.Domain.Person", "Person")
                        .WithMany("ActedIn")
                        .HasForeignKey("PersonId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Organizers.MovOrg.Domain.MovieCompany", b =>
                {
                    b.HasOne("Organizers.MovOrg.Domain.Company", "Company")
                        .WithMany()
                        .HasForeignKey("CompanyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Organizers.MovOrg.Domain.Movie", "Movie")
                        .WithMany("CompanyList")
                        .HasForeignKey("MovieId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Organizers.MovOrg.Domain.MovieDirector", b =>
                {
                    b.HasOne("Organizers.MovOrg.Domain.Movie", "Movie")
                        .WithMany("DirectorList")
                        .HasForeignKey("MovieId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Organizers.MovOrg.Domain.Person", "Person")
                        .WithMany("DirectorOf")
                        .HasForeignKey("PersonId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Organizers.MovOrg.Domain.MovieImageData", b =>
                {
                    b.HasOne("Organizers.MovOrg.Domain.Movie", "Movie")
                        .WithMany("Images")
                        .HasForeignKey("MovieId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Organizers.MovOrg.Domain.MovieSimilar", b =>
                {
                    b.HasOne("Organizers.MovOrg.Domain.Movie", "Movie")
                        .WithMany("Similars")
                        .HasForeignKey("MovieId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("Organizers.MovOrg.Domain.Movie", "Similar")
                        .WithMany()
                        .HasForeignKey("SimilarId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Organizers.MovOrg.Domain.MovieWriter", b =>
                {
                    b.HasOne("Organizers.MovOrg.Domain.Movie", "Movie")
                        .WithMany("WriterList")
                        .HasForeignKey("MovieId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Organizers.MovOrg.Domain.Person", "Person")
                        .WithMany()
                        .HasForeignKey("PersonId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Organizers.MovOrg.Domain.Rating", b =>
                {
                    b.HasOne("Organizers.MovOrg.Domain.Movie", "Movie")
                        .WithMany("Ratings")
                        .HasForeignKey("MovieId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Organizers.MovOrg.Domain.RatingSource", "Source")
                        .WithMany("Ratings")
                        .HasForeignKey("SourceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Organizers.MovOrg.Domain.Trailer", b =>
                {
                    b.HasOne("Organizers.MovOrg.Domain.Movie", "Movie")
                        .WithOne("Trailer")
                        .HasForeignKey("Organizers.MovOrg.Domain.Trailer", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
