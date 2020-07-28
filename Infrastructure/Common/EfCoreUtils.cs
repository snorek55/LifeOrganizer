using Microsoft.EntityFrameworkCore;

using Organizers.Common.Domain;
using Organizers.Common.UseCases;
using Organizers.MovOrg.Domain;

using System;
using System.Collections.Generic;
using System.Reflection;

namespace Infrastructure.Common
{
	public static class EfCoreUtils
	{
		public static void UpdateManyToManyLinkAndEntities<T>(IEnumerable<T> disconnectedLinksEnumerable, DbContext dbContext) where T : class
		{
			var linkType = typeof(T);
			Type movieType = typeof(Movie);
			Type entityType = null;
			PropertyInfo movieIdPropertyInfo = linkType.GetProperty("MovieId");
			PropertyInfo entityIdPropertyInfo = null;
			PropertyInfo entityPropertyInfo = null;
			PropertyInfo linkListMoviePropertyInfo = null;

			//Busco las propiedades de la entidad, las de la pelicula ya las conozco
			foreach (var prop in linkType.GetProperties())
			{
				if (prop.Name.Equals("MovieId") || prop.Name.Equals("Movie"))
					continue;

				if (prop.Name.EndsWith("Id"))
					entityIdPropertyInfo = linkType.GetProperty(prop.Name);
				else
				{
					entityType = prop.PropertyType;
					entityPropertyInfo = linkType.GetProperty(prop.Name);
				}
			}
			//Busco la propiedad de la lista en la entidad de peliculas
			var entityListType = typeof(List<>).MakeGenericType(linkType);
			foreach (var prop in movieType.GetProperties())
			{
				if (prop.PropertyType.Equals(entityListType))
					linkListMoviePropertyInfo = prop;
			}

			//Guards
			//TODO: improve guards
			if (entityType == null || linkType.GetProperties().Length != 4) throw new ArgumentException("Error while trying to get types of many-to-many link entities");

			//TODO: extension method for dbcontext
			var linkDbSet = dbContext.GetType().GetMethod("Set").MakeGenericMethod(linkType).Invoke(dbContext, null);
			var movieDbSet = dbContext.GetType().GetMethod("Set").MakeGenericMethod(movieType).Invoke(dbContext, null);
			var entityDbSet = dbContext.GetType().GetMethod("Set").MakeGenericMethod(entityType).Invoke(dbContext, null);

			var linkDbSetFindMethod = linkDbSet.GetType().GetMethod("Find");
			var movieDbSetFindMethod = movieDbSet.GetType().GetMethod("Find");
			var entityDbSetFindMethod = entityDbSet.GetType().GetMethod("Find");

			//Por cada link desconnectado, miro si existe y si no existe, miro si existen cada uno de los componentes y los creo si son necesarios asi como el link
			foreach (var link in disconnectedLinksEnumerable)
			{
				var movieId = (string)movieIdPropertyInfo.GetValue(link);
				var entityId = (string)entityIdPropertyInfo.GetValue(link);
				var existingLink = linkDbSetFindMethod.Invoke(linkDbSet, new object[] { new object[] { movieId, entityId } });
				if (existingLink != null)
					continue;
				UpdateEntity((Entity)entityPropertyInfo.GetValue(link), dbContext);
				var existingEntity = entityDbSetFindMethod.Invoke(entityDbSet, new object[] { new object[] { entityId } });
				if (existingEntity == null) throw new RepositoryException("Entity not added correctly when updating many to many link");
				var existingMovie = movieDbSetFindMethod.Invoke(movieDbSet, new object[] { new object[] { movieId } });
				if (existingMovie == null) throw new RepositoryException("Movie not found when updating many to many link");

				var newLink = Activator.CreateInstance(linkType);
				linkType.GetProperty("MovieId").SetValue(newLink, movieId);
				linkType.GetProperty("Movie").SetValue(newLink, existingMovie);
				entityIdPropertyInfo.SetValue(newLink, entityId);
				entityPropertyInfo.SetValue(newLink, existingEntity);
				var existingMovieLinkList = linkListMoviePropertyInfo.GetValue(existingMovie);
				typeof(List<>).MakeGenericType(linkType).GetMethod("Add").Invoke(existingMovieLinkList, new object[] { newLink });
			}
		}

		//TODO: https://stackoverflow.com/questions/16153047/net-invoke-async-method-and-await
		public static void UpdateEntity(Entity entity, DbContext dbContext)
		{
			var superEntityType = entity.GetType();
			var dbSet = dbContext.GetType().GetMethod("Set").MakeGenericMethod(superEntityType).Invoke(dbContext, null);
			var parameterArray = new object[] { new object[] { entity.Id } };
			var dbsetType = dbSet.GetType();
			var findMethod = dbsetType.GetMethod("Find");

			var existingEntity = findMethod.Invoke(dbSet, parameterArray);
			if (existingEntity == null)
			{
				var addMethod = dbsetType.GetMethod("Add");
				addMethod.Invoke(dbSet, new object[] { entity });
			}
		}
	}
}