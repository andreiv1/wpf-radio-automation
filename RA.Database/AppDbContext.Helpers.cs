using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA.Database
{
    public partial class AppDbContext
    {
        /// <summary>
        /// Attaches the specified entity to the DbContext or returns the tracked entity with the same key values.
        /// This method is used to handle situations where the DbContext might already be tracking an entity with
        /// the same key values, avoiding InvalidOperationExceptions caused by attaching multiple instances of the same entity.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="entity">The entity to be attached or checked for existing tracking.</param>
        /// <returns>
        /// The attached entity if the specified entity was not already tracked, or the tracked entity with the same
        /// key values if it was already tracked.
        /// </returns>
        public TEntity AttachOrGetTrackedEntity<TEntity>(TEntity entity) where TEntity : class
        {
            var entityType = Model.FindEntityType(typeof(TEntity));
            var primaryKey = entityType.FindPrimaryKey();
            var keyValues = primaryKey.Properties
                .Select(p => p.PropertyInfo.GetValue(entity)).ToArray();

            TEntity trackedEntity = null;

            foreach (var localEntity in Set<TEntity>().Local)
            {
                var localKeyValues = primaryKey.Properties
                    .Select(p => p.PropertyInfo.GetValue(localEntity)).ToArray();

                if (keyValues.SequenceEqual(localKeyValues))
                {
                    trackedEntity = localEntity;
                    break;
                }
            }

            if (trackedEntity == null)
            {
                Set<TEntity>().Attach(entity);
                return entity;
            }
            else
            {
                return trackedEntity;
            }
        }

    }
}
