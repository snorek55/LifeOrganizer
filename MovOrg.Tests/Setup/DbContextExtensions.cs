﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

using System;
using System.Collections.Generic;
using System.Linq;

namespace MovOrg.Tests.Setup
{
	//https://github.com/JonPSmith/EfCoreInAction/blob/Chapter09/Test/EfHelpers/EfOptionsHelper.cs
	public static class DbContextExtensions
	{
		//NOTE: This will not handle a circular relationship: e.g. EntityA->EntityB->EntityA
		public static IEnumerable<string>
			GetTableNamesInOrderForWipe //#A
			(this DbContext context,
			 int maxDepth = 10, params Type[] excludeTypes) //#B
		{
			var allEntities = context.Model
				.GetEntityTypes()
				.Where(x => !excludeTypes.Contains(x.ClrType))
				.ToList(); //#C

			ThrowExceptionIfCannotWipeSelfRef(allEntities); //#D

			var principalsDict = allEntities             //#E
				.SelectMany(x => x.GetForeignKeys()
					.Select(y => y.PrincipalEntityType)).Distinct()
				.ToDictionary(k => k, v => //#F
					v.GetForeignKeys()
						.Where(y => y.PrincipalEntityType != v) //#G
						.Select(y => y.PrincipalEntityType).ToList()); //#H

			var result = allEntities //#I
				.Where(x => !principalsDict.ContainsKey(x)) //#I
				.ToList(); //#I

			/************************************************************************
			#A This method looks at the relationships and returns the tables names in the right order to wipe all their rows without incurring a foreign key delete constraint
			#B You can exclude entity classes that you need to handle yourself, for instance - any references that only contain circular references
			#C This gets the IEntityType for all the entities, other than those that were excluded. This contains the information on how each table is built, with its relationships
			#D This contains a check for the hierarchical (entity that references itself) case where an entity refers to itself - if the delete behavior of this foreign key is set to restrict then you cannot simply delete all the rows in one go
			#E I extract all the principal entities from the entities we are considering ...
			#F ... And put them in a dictionary, with the IEntityType being the key
			#G ... I remove any self reference links as these are automatically handled
			#H ... And the PrincipalEntityType being the value
			#I I start the list of entities to delete by putting all the dependant entities first, as I must delete the rows in these tables first, and the order doesn't matter
			************************************************************/

			var reversePrincipals = new List<IEntityType>(); //#A
			int depth = 0; //#B
			while (principalsDict.Keys.Any()) //#C
			{
				foreach (var principalNoLinks in
					principalsDict
						.Where(x => !x.Value.Any()).ToList())//#D
				{
					reversePrincipals.Add(principalNoLinks.Key);//#E
					principalsDict
						.Remove(principalNoLinks.Key);//#F
					foreach (var removeLink in
						principalsDict.Where(x =>
							x.Value.Contains(principalNoLinks.Key)))//#G
					{
						removeLink.Value
							.Remove(principalNoLinks.Key);//#H
					}
				}
				if (++depth >= maxDepth) //#I
					ThrowExceptionMaxDepthReached(
						principalsDict.Keys.ToList(), depth);
			}
			reversePrincipals.Reverse();//#J
			result.AddRange(reversePrincipals);//#K
			return result.Select(FormTableNameWithSchema);//#L
		}

		/************************************************************************
		#A I am going to produce a list of principal entities in the reverse order that they should have all rows wiped in them
		#B I keep a count of the times I have been round the loop trying to resolve the the relationships
		#C While there are entities with links to other entities I need to keep going round
		#D Now loop through all the relationships that don't have a link to another principal (or that link has already been marked as wiped)
		#E I mark the entity for deletion - this list is in reverse order to the order of table row wiping
		#F I remove it from the dictionary so that it isn't looked at again
		#G I look for every reference to this principal entity in other entity''s links ...
		#H ... and remove the reference to that entity from any existing dependants still in the dictionary
		#I If I have overstepped the depth limit I throw an exception, with information on what entities had still to be processed. This can happen for certain circular references.
		#J When I get to here I have the list of entities in the reverse order to how I should wipe them, so I reverse the list
		#K I now produce combined list with the dependants at the front and the principals at the back in the right order
		#L Finally I return a collection of table names, with a optional schema, in the right order
		* ***********************************************************************/

		public static void WipeAllDataFromDatabase(this DbContext context,
			int maxDepth = 10, params Type[] excludeTypes)
		{
			foreach (var tableName in
				context.GetTableNamesInOrderForWipe(maxDepth, excludeTypes))
			{
				var commandString = $"DELETE FROM {tableName}";
				context.Database
					.ExecuteSqlRaw(commandString);
			}
		}

		//------------------------------------------------
		//private methods

		private static string FormTableNameWithSchema(IEntityType entityType)
		{
			return "[" + (entityType.GetSchema() == null
					   ? ""
					   : entityType.GetSchema() + "].[")
				   + entityType.GetTableName() + "]";
		}

		private static void ThrowExceptionIfCannotWipeSelfRef(List<IEntityType> allEntities)
		{
			var cannotWipes = allEntities
				.SelectMany(x => x.GetForeignKeys()
					.Where(y => y.PrincipalEntityType == x
								&& (y.DeleteBehavior == DeleteBehavior.Restrict
								 || y.DeleteBehavior == DeleteBehavior.ClientSetNull)))
				.ToList(); //#B
			if (cannotWipes.Any())
				throw new InvalidOperationException(
					"You cannot delete all the rows in one go in entity(s): " +
					string.Join(", ", cannotWipes.Select(x => x.DeclaringEntityType.Name)));
		}

		private static void ThrowExceptionMaxDepthReached(List<IEntityType> principalsDictKeys, int maxDepth)
		{
			throw new InvalidOperationException(
				$"It looked to a depth of {maxDepth} and didn't finish. Possible circular reference?\nentity(s) left: " +
				string.Join(", ", principalsDictKeys.Select(x => x.ClrType.Name)));
		}
	}
}