using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using Nop.Core;
using Nop.Data;
using Nop.Plugin.Widgets.TorchDesign_StoreLocator.Domain;
using Nop.Data.Initializers;
using System.Data.Common;
using System.Data;
using System.Linq;
using System.Text;
namespace Nop.Plugin.Widgets.TorchDesign_StoreLocator.Data
{
    /// <summary>
    /// Object context
    /// </summary>
    public class StoreLocatorObjctContext : DbContext, IDbContext
    {
        public StoreLocatorObjctContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {
            //((IObjectContextAdapter) this).ObjectContext.ContextOptions.LazyLoadingEnabled = true;
        }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
          
         
            modelBuilder.Configurations.Add(new StoreLocatorRecordMap());
        
            //modelBuilder.Configurations.Add(new FAQ_CategoryRecordMap());
            //disable EdmMetadata generation
            //modelBuilder.Conventions.Remove<IncludeMetadataConvention>();
         base.OnModelCreating(modelBuilder);
        }

        public string CreateDatabaseScript()
        {
            return ((IObjectContextAdapter)this).ObjectContext.CreateDatabaseScript();
        }

        public new IDbSet<TEntity> Set<TEntity>() where TEntity : BaseEntity
        {
            return base.Set<TEntity>();
        }

        /// <summary>
        /// Install
        /// </summary>
        public void Install()
        {

            ////create the table
            //var dbScript = CreateDatabaseScript();
            //Database.ExecuteSqlCommand(dbScript);
            //SaveChanges();

            //pass table names to ensure that we have already passed table exists
            var tablesToValidate = new[] {"Td_StoreLocator"};

            //custom commands (stored proedures, indexes)
            var customCommands = new List<string>();

            //Check if table is exists or not.
            var initializer = new CreateTablesIfNotExist<StoreLocatorObjctContext>(tablesToValidate, customCommands.ToArray());
            //Create table if not exists
            initializer.InitializeDatabase(((StoreLocatorObjctContext)this));
            
            ////create the table
            //var dbScript = CreateDatabaseScript();
            //Database.ExecuteSqlCommand(dbScript);
            //SaveChanges();
            // create stored procedures
            StringBuilder GetStoresByLatLanWithMiles = new StringBuilder();
            GetStoresByLatLanWithMiles.Append("CREATE PROCEDURE [dbo].[GetStoresByLatLanWithMiles](@Longitude decimal(10, 7) = null,@Latitude decimal(10, 7) = null,@Miles decimal(10, 7) = null,@StoreType nvarchar(20) = null)")
                .AppendLine("AS")
                .AppendLine("BEGIN")
                .AppendLine("if (@StoreType = 'Lowe''s')")
                .AppendLine("Begin")
                .AppendLine("SELECT Id, StoreName,StoreNumber,StoreType,[Address], City, Region, CountryCode, PostalCode, PhoneNumber, Latitude, Longitude,dbo.CalculateDistance(Longitude, Latitude, @Longitude,@Latitude ) AS DistanceFromAddress FROM TD_StoreLocator WHERE dbo.CalculateDistance(Longitude, Latitude, @Longitude,@Latitude) < @Miles And StoreType='Lowe''s' ORDER BY DistanceFromAddress")
                .AppendLine("End")
                .AppendLine("Else")
                .AppendLine("Begin")
                .AppendLine("SELECT Id, StoreName,StoreNumber,StoreType,[Address], City, Region, CountryCode, PostalCode, PhoneNumber, Latitude, Longitude,dbo.CalculateDistance(Longitude, Latitude, @Longitude,@Latitude ) AS DistanceFromAddress FROM TD_StoreLocator WHERE dbo.CalculateDistance(Longitude, Latitude, @Longitude,@Latitude) < @Miles ORDER BY DistanceFromAddress")
                .AppendLine("End")
                .AppendLine("END");
         
            Database.ExecuteSqlCommand(GetStoresByLatLanWithMiles.ToString());
  }

        /// <summary>
        /// Uninstall
        /// </summary>
        public void Uninstall()
        {
            //drop the table
            this.DropPluginTable("Td_StoreLocator");
            //this.DropPluginTable("FAQ_Category");
        }

        /// <summary>
        /// Execute stores procedure and load a list of entities at the end
        /// </summary>
        /// <typeparam name="TEntity">Entity type</typeparam>
        /// <param name="commandText">Command text</param>
        /// <param name="parameters">Parameters</param>
        /// <returns>Entities</returns>
        public IList<TEntity> ExecuteStoredProcedureList<TEntity>(string commandText, params object[] parameters) where TEntity : BaseEntity, new()
        {
            bool hasOutputParameters = false;
            if (parameters != null)
            {
                foreach (var p in parameters)
                {
                    var outputP = p as DbParameter;
                    if (outputP == null)
                        continue;

                    if (outputP.Direction == ParameterDirection.InputOutput ||
                        outputP.Direction == ParameterDirection.Output)
                        hasOutputParameters = true;
                }
            }

            var context = ((IObjectContextAdapter)(this)).ObjectContext;
            //if (hasOutputParameters)
            //{
            //    //no output parameters
            //    //var result = this.Database.SqlQuery<TEntity>(commandText, parameters).ToList();
            //    //for (int i = 0; i < result.Count; i++)
            //    //    result[i] = AttachEntityToContext(result[i]);

            //    //return result;

            //    var result = context.ExecuteStoreQuery<TEntity>(commandText, parameters).ToList();
            //    foreach (var entity in result)
            //        Set<TEntity>().Attach(entity);
            //    return result;
            //}
            //else
            //{

                //var connection = context.Connection;
                var connection = this.Database.Connection;
                //Don't close the connection after command execution


                //open the connection for use
                if (connection.State == ConnectionState.Closed)
                    connection.Open();
                //create a command object
                using (var cmd = connection.CreateCommand())
                {
                    //command to execute
                    cmd.CommandText = commandText;
                    cmd.CommandType = CommandType.StoredProcedure;

                    // move parameters to command object
                    if (parameters != null)
                        foreach (var p in parameters)
                            cmd.Parameters.Add(p);

                    //database call
                    var reader = cmd.ExecuteReader();
                    //return reader.DataReaderToObjectList<TEntity>();
                    var result = context.Translate<TEntity>(reader).ToList();
                    for (int i = 0; i < result.Count; i++)
                        result[i] = AttachEntityToContext(result[i]);
                    //close up the reader, we're done saving results
                    reader.Close();
                    return result;
            //    }

            }
        }

        protected virtual TEntity AttachEntityToContext<TEntity>(TEntity entity) where TEntity : BaseEntity, new()
        {
            //little hack here until Entity Framework really supports stored procedures
            //otherwise, navigation properties of loaded entities are not loaded until an entity is attached to the context
            var alreadyAttached = Set<TEntity>().Local.Where(x => x.Id == entity.Id).FirstOrDefault();
            if (alreadyAttached == null)
            {
                //attach new entity
                Set<TEntity>().Attach(entity);
                return entity;
            }
            else
            {
                //entity is already loaded.
                return alreadyAttached;
            }
        }
        /// <summary>
        /// Creates a raw SQL query that will return elements of the given generic type.  The type can be any type that has properties that match the names of the columns returned from the query, or can be a simple primitive type. The type does not have to be an entity type. The results of this query are never tracked by the context even if the type of object returned is an entity type.
        /// </summary>
        /// <typeparam name="TElement">The type of object returned by the query.</typeparam>
        /// <param name="sql">The SQL query string.</param>
        /// <param name="parameters">The parameters to apply to the SQL query string.</param>
        /// <returns>Result</returns>
        public IEnumerable<TElement> SqlQuery<TElement>(string sql, params object[] parameters)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Executes the given DDL/DML command against the database.
        /// </summary>
        /// <param name="sql">The command string</param>
        /// <param name="doNotEnsureTransaction">false - the transaction creation is not ensured; true - the transaction creation is ensured.</param>
        /// <param name="timeout">Timeout value, in seconds. A null value indicates that the default value of the underlying provider will be used</param>
        /// <param name="parameters">The parameters to apply to the command string.</param>
        /// <returns>The result returned by the database after executing the command.</returns>
        public int ExecuteSqlCommand(string sql, bool doNotEnsureTransaction = false, int? timeout = null, params object[] parameters)
        {
            throw new NotImplementedException();
        }
    }
}