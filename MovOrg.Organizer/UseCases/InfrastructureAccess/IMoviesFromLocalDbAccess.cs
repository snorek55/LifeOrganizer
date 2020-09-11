﻿using MovOrg.Organizer.UseCases.DTOs;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace MovOrg.Organizer.UseCases.DbAccess
{
	public interface IMoviesFromLocalDbAccess
	{
		Task<IEnumerable<MovieListItemDto>> GetMoviesFromLocal();
	}
}