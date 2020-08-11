using Common.Domain;
using Common.UseCases;

using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Reflection;

namespace Common.Infrastructure
{
	public static class EfCoreUtils
	{
		private struct LinkPropsInfo
		{
			public PropertyInfo id;
			public PropertyInfo entity;
		}

		public static void UpdateManyToManyLinkAndEntities<T>(IEnumerable<T> disconnectedLinksEnumerable, DbContext dbContext, Type mainType, Type dependentType) where T : class
		{
			var linkType = typeof(T);
			LinkPropsInfo mainPropsInfo = FindLinkEntityPropInfo(linkType, mainType);
			LinkPropsInfo dependentPropsInfo = FindLinkEntityPropInfo(linkType, dependentType);
			PropertyInfo mainListLinkPropInfo = FindListPropInMainEntity(linkType, mainType);

			//Guards: make sure class has only navigational properties (that is, Movie,MovieId,Entity,EntityId)
			if (mainListLinkPropInfo == null ||
				mainPropsInfo.entity == null ||
				mainPropsInfo.id == null ||
				dependentPropsInfo.entity == null ||
				dependentPropsInfo.id == null ||
				linkType.GetProperties().Length != 4) throw new ArgumentException("Error while trying to get types of many-to-many link entities. Could not find entity type.");

			dynamic linkDbSet = dbContext.GetDbSetWithReflection(linkType);
			dynamic mainDbSet = dbContext.GetDbSetWithReflection(mainType);
			dynamic dependentDbSet = dbContext.GetDbSetWithReflection(dependentPropsInfo.entity.PropertyType);

			//Por cada link desconnectado, miro si existe y si no existe, miro si existen cada uno de los componentes y los creo si son necesarios asi como el link
			foreach (var link in disconnectedLinksEnumerable)
			{
				var mainId = (string)mainPropsInfo.id.GetValue(link);
				var dependentId = (string)dependentPropsInfo.id.GetValue(link);
				var existingLink = linkDbSet.Find(new object[] { mainId, dependentId });
				if (existingLink != null)
					continue;
				UpdateEntity((Entity)dependentPropsInfo.entity.GetValue(link), dbContext);
				var existingDependent = dependentDbSet.Find(new object[] { dependentId });
				if (existingDependent == null) throw new RepositoryException("Entity not added correctly when updating many to many link");
				var existingMain = mainDbSet.Find(new object[] { mainId });
				if (existingMain == null) throw new RepositoryException("Movie not found when updating many to many link");

				var newLink = Activator.CreateInstance(linkType);
				mainPropsInfo.id.SetValue(newLink, mainId);
				mainPropsInfo.entity.SetValue(newLink, existingMain);
				dependentPropsInfo.id.SetValue(newLink, dependentId);
				dependentPropsInfo.entity.SetValue(newLink, existingDependent);
				var existingMovieLinkList = mainListLinkPropInfo.GetValue(existingMain);
				mainListLinkPropInfo.PropertyType.GetMethod("Add").Invoke(existingMovieLinkList, new object[] { newLink });
			}
		}

		private static PropertyInfo FindListPropInMainEntity(Type linkType, Type mainType)
		{
			var entityListType = typeof(List<>).MakeGenericType(linkType);
			foreach (var prop in mainType.GetProperties())
			{
				if (prop.PropertyType.Equals(entityListType))
					return prop;
			}

			return null;
		}

		private static LinkPropsInfo FindLinkEntityPropInfo(Type linkType, Type entityType)
		{
			LinkPropsInfo linkEntityPropsInfo = new LinkPropsInfo();

			foreach (var prop in linkType.GetProperties())
			{
				if (prop.PropertyType == entityType)
					linkEntityPropsInfo.entity = prop;
				else if (prop.PropertyType != entityType && prop.Name.Contains(entityType.Name) && prop.Name.Substring(prop.Name.Length - 2).Equals("Id"))
					linkEntityPropsInfo.id = linkType.GetProperty(prop.Name);
			}

			return linkEntityPropsInfo;
		}

		public static void UpdateEntity(Entity entity, DbContext dbContext)
		{
			var superEntityType = entity.GetType();
			dynamic dbSet = dbContext.GetDbSetWithReflection(superEntityType);
			var dbsetType = dbSet.GetType();
			var existingEntity = dbSet.Find(new object[] { entity.Id });

			if (existingEntity == null)
			{
				var addMethod = dbsetType.GetMethod("Add");
				addMethod.Invoke(dbSet, new object[] { entity });
			}
		}
	}
}