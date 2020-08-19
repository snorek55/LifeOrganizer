using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MovOrg.Infrastructure.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Companies",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Companies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Movies",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Title = table.Column<string>(nullable: true),
                    Plot = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    IMDbRating = table.Column<float>(nullable: true),
                    CoverImageUrl = table.Column<string>(nullable: true),
                    Year = table.Column<string>(nullable: true),
                    Tagline = table.Column<string>(nullable: true),
                    Keywords = table.Column<string>(nullable: true),
                    RuntimeStr = table.Column<string>(nullable: true),
                    Awards = table.Column<string>(nullable: true),
                    Genres = table.Column<string>(nullable: true),
                    Countries = table.Column<string>(nullable: true),
                    Languages = table.Column<string>(nullable: true),
                    LastUpdatedTop250 = table.Column<DateTime>(nullable: true),
                    LastUpdatedDetails = table.Column<DateTime>(nullable: true),
                    ReleaseDate = table.Column<string>(nullable: true),
                    Rank = table.Column<int>(nullable: true),
                    WikipediaUrl = table.Column<string>(nullable: true),
                    IsFavorite = table.Column<bool>(nullable: false),
                    IsWatched = table.Column<bool>(nullable: false),
                    IsMustWatch = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Movies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "People",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Role = table.Column<string>(nullable: true),
                    Summary = table.Column<string>(nullable: true),
                    BirthDate = table.Column<DateTime>(nullable: true),
                    DeathDate = table.Column<DateTime>(nullable: true),
                    Awards = table.Column<string>(nullable: true),
                    Height = table.Column<string>(nullable: true),
                    ImageUrl = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_People", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RatingSources",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    LogoUrl = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RatingSources", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BoxOffices",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Budget = table.Column<string>(nullable: true),
                    OpeningWeekendUSA = table.Column<string>(nullable: true),
                    GrossUSA = table.Column<string>(nullable: true),
                    CumulativeWorldwideGross = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BoxOffices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BoxOffices_Movies_Id",
                        column: x => x.Id,
                        principalTable: "Movies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MovieCompanies",
                columns: table => new
                {
                    MovieId = table.Column<string>(nullable: false),
                    CompanyId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MovieCompanies", x => new { x.MovieId, x.CompanyId });
                    table.ForeignKey(
                        name: "FK_MovieCompanies_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MovieCompanies_Movies_MovieId",
                        column: x => x.MovieId,
                        principalTable: "Movies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MovieImageDatas",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    MovieId = table.Column<string>(nullable: false),
                    Image = table.Column<string>(nullable: true),
                    Title = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MovieImageDatas", x => new { x.MovieId, x.Id });
                    table.ForeignKey(
                        name: "FK_MovieImageDatas_Movies_MovieId",
                        column: x => x.MovieId,
                        principalTable: "Movies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MovieSimilars",
                columns: table => new
                {
                    SimilarId = table.Column<string>(nullable: false),
                    MovieId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MovieSimilars", x => new { x.MovieId, x.SimilarId });
                    table.ForeignKey(
                        name: "FK_MovieSimilars_Movies_MovieId",
                        column: x => x.MovieId,
                        principalTable: "Movies",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MovieSimilars_Movies_SimilarId",
                        column: x => x.SimilarId,
                        principalTable: "Movies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Trailers",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    ThumbnailUrl = table.Column<string>(nullable: true),
                    LinkEmbed = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trailers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Trailers_Movies_Id",
                        column: x => x.Id,
                        principalTable: "Movies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MovieActors",
                columns: table => new
                {
                    MovieId = table.Column<string>(nullable: false),
                    PersonId = table.Column<string>(nullable: false),
                    AsCharacter = table.Column<string>(nullable: true),
                    IsStar = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MovieActors", x => new { x.MovieId, x.PersonId });
                    table.ForeignKey(
                        name: "FK_MovieActors_Movies_MovieId",
                        column: x => x.MovieId,
                        principalTable: "Movies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MovieActors_People_PersonId",
                        column: x => x.PersonId,
                        principalTable: "People",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MovieDirectors",
                columns: table => new
                {
                    MovieId = table.Column<string>(nullable: false),
                    PersonId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MovieDirectors", x => new { x.MovieId, x.PersonId });
                    table.ForeignKey(
                        name: "FK_MovieDirectors_Movies_MovieId",
                        column: x => x.MovieId,
                        principalTable: "Movies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MovieDirectors_People_PersonId",
                        column: x => x.PersonId,
                        principalTable: "People",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MovieWriters",
                columns: table => new
                {
                    MovieId = table.Column<string>(nullable: false),
                    PersonId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MovieWriters", x => new { x.MovieId, x.PersonId });
                    table.ForeignKey(
                        name: "FK_MovieWriters_Movies_MovieId",
                        column: x => x.MovieId,
                        principalTable: "Movies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MovieWriters_People_PersonId",
                        column: x => x.PersonId,
                        principalTable: "People",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Ratings",
                columns: table => new
                {
                    MovieId = table.Column<string>(nullable: false),
                    SourceId = table.Column<string>(nullable: false),
                    Id = table.Column<string>(nullable: true),
                    Score = table.Column<float>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ratings", x => new { x.MovieId, x.SourceId });
                    table.ForeignKey(
                        name: "FK_Ratings_Movies_MovieId",
                        column: x => x.MovieId,
                        principalTable: "Movies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Ratings_RatingSources_SourceId",
                        column: x => x.SourceId,
                        principalTable: "RatingSources",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MovieActors_PersonId",
                table: "MovieActors",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_MovieCompanies_CompanyId",
                table: "MovieCompanies",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_MovieDirectors_PersonId",
                table: "MovieDirectors",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_MovieSimilars_SimilarId",
                table: "MovieSimilars",
                column: "SimilarId");

            migrationBuilder.CreateIndex(
                name: "IX_MovieWriters_PersonId",
                table: "MovieWriters",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_Ratings_SourceId",
                table: "Ratings",
                column: "SourceId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BoxOffices");

            migrationBuilder.DropTable(
                name: "MovieActors");

            migrationBuilder.DropTable(
                name: "MovieCompanies");

            migrationBuilder.DropTable(
                name: "MovieDirectors");

            migrationBuilder.DropTable(
                name: "MovieImageDatas");

            migrationBuilder.DropTable(
                name: "MovieSimilars");

            migrationBuilder.DropTable(
                name: "MovieWriters");

            migrationBuilder.DropTable(
                name: "Ratings");

            migrationBuilder.DropTable(
                name: "Trailers");

            migrationBuilder.DropTable(
                name: "Companies");

            migrationBuilder.DropTable(
                name: "People");

            migrationBuilder.DropTable(
                name: "RatingSources");

            migrationBuilder.DropTable(
                name: "Movies");
        }
    }
}
