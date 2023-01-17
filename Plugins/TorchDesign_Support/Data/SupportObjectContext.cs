using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using Nop.Core;
using Nop.Data;
using Nop.Data.Initializers;
using System.Data.Common;
using System.Data;
using System.Linq;
using System.Text;
using Nop.Core.Domain.Customers;
using System.Data.Entity.ModelConfiguration;
using System.Reflection;
using Nop.Core.Domain.Orders;
using System.IO;
using System.Web.Hosting;
using System.Data.SqlClient;
using Nop.Core.Data;
namespace Nop.Plugin.Widgets.TorchDesign_Support.Data
{
    /// <summary>
    /// Object context
    /// </summary>
    public class SupportObjectContext : DbContext, IDbContext
    {
		 public SupportObjectContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {
            //((IObjectContextAdapter) this).ObjectContext.ContextOptions.LazyLoadingEnabled = true;
        }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

			  Database.SetInitializer<SupportObjectContext>(null);

			  modelBuilder.Configurations.Add(new SupportCategoryMap());
			  modelBuilder.Configurations.Add(new SupportTopicMap());
			  modelBuilder.Configurations.Add(new SupportTopicProductCategoryMap());
			  modelBuilder.Configurations.Add(new SupportTopicRelatedProductMap());
			  modelBuilder.Configurations.Add(new SupportTopicStepMap());
              modelBuilder.Configurations.Add(new SupportCategoryProductCategoryMap());
              //modelBuilder.Configurations.Add(new SupportTopicStepMappingMap());
			  modelBuilder.Configurations.Add(new SupportTopicSupportCategoryMap());
			  modelBuilder.Configurations.Add(new SupportDownloadMap());
			  modelBuilder.Configurations.Add(new SupportDownloadProductCategoryMap());
			  modelBuilder.Configurations.Add(new SupportDownloadRelatedProductMap());
			  modelBuilder.Configurations.Add(new SupportVideoMap());
			  modelBuilder.Configurations.Add(new SupportVideoProductCategoryMap());
			  modelBuilder.Configurations.Add(new SupportVideoRelatedProductMap());
        
				//modelBuilder.Configurations.Add(new FAQ_CategoryRecordMap());
				//disable EdmMetadata generation
				//modelBuilder.Conventions.Remove<IncludeMetadataConvention>();
			 
			 // modelBuilder.Entity<RewardPointsHistory>().HasRequired(p => p.UsedWithOrder).WithOptional();
			  //modelBuilder.Entity<RewardPointsHistory>().Ignore(p => p.UsedWithOrder);
			  //modelBuilder.Ignore<RewardPointsHistory>();

			  //unable to determine the principal end of an association between the types 
			  //'Nop.Core.Domain.Orders.Order' and 'Nop.Core.Domain.Customers.RewardPointsHistory'. 
			  //	The principal end of this association must be explicitly configured
			  //using either the relationship fluent API or data annotations.
				//modelBuilder.Ignore<Order>();
				//modelBuilder.Ignore<RewardPointsHistory>();

			  //var typesToRegister = Assembly.GetExecutingAssembly().GetTypes()
			  //.Where(type => !String.IsNullOrEmpty(type.Namespace))
			  //.Where(type => type.BaseType != null && type.BaseType.IsGenericType && type.BaseType.GetGenericTypeDefinition() == typeof(EntityTypeConfiguration<>));
			  //foreach (var type in typesToRegister)
			  //{
			  //	dynamic configurationInstance = Activator.CreateInstance(type);
			  //	modelBuilder.Configurations.Add(configurationInstance);
			  //}
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

        protected virtual string[] ParseCommands(string filePath, bool throwExceptionIfNonExists)
        {
            if (!File.Exists(filePath))
            {
                if (throwExceptionIfNonExists)
                    throw new ArgumentException(string.Format("Specified file doesn't exist - {0}", filePath));
                else
                    return new string[0];
            }


            var statements = new List<string>();
            using (var stream = File.OpenRead(filePath))
            using (var reader = new StreamReader(stream))
            {
                var statement = "";
                while ((statement = ReadNextStatementFromStream(reader)) != null)
                {
                    statements.Add(statement);
                }
            }

            return statements.ToArray();
        }


        protected virtual string ReadNextStatementFromStream(StreamReader reader)
        {
            var sb = new StringBuilder();

            string lineOfText;

            while (true)
            {
                lineOfText = reader.ReadLine();
                if (lineOfText == null)
                {
                    if (sb.Length > 0)
                        return sb.ToString();
                    else
                        return null;
                }

                if (lineOfText.TrimEnd().ToUpper() == "GO")
                    break;

                sb.Append(lineOfText + Environment.NewLine);
            }

            return sb.ToString();
        }
        /// <summary>
        /// Install
        /// </summary>
        public void Install()
        {
              //Database.SetInitializer<SupportObjectContext>(null);
              //var dbScript = CreateDatabaseScript();
              //Database.ExecuteSqlCommand(dbScript);
              //SaveChanges();

            var tablesToValidate = new[] { "TD_SupportTopic,TD_SupportTopicProductCategoryMapping,TD_SupportTopicRelatedProduct,TD_SupportTopicStep,TD_SupportTopicSupportCategory,TD_SupportTopicSupportCategoryMapping,TD_SupportDownloadMapping,TD_SupportDownloadProductCategory,TD_SupportDownloadRelatedProduct,TD_SupportVideo,TD_SupportVideoProductCategory,TD_SupportVideoRelatedProduct,TD_SupportCategoryProductCategoryMapping" };

            //custom commands (stored proedures, indexes)
            var customCommands = new List<string>();
            customCommands.AddRange(ParseCommands(HostingEnvironment.MapPath("~/Plugins/Widgets.TorchDesign_Support/Content/SupportSearch.sql"), false));

            //Check if table is exists or not.
            var initializer = new CreateTablesIfNotExist<SupportObjectContext>(tablesToValidate, customCommands.ToArray());
            //Create table if not exists
            initializer.InitializeDatabase(((SupportObjectContext)this));

           
  }

        /// <summary>
        /// Uninstall
        /// </summary>
        public void Uninstall()
        {
            //drop the table

            this.DropPluginTable("TD_SupportCategoryProductCategoryMapping");
            this.DropPluginTable("TD_SupportDownloadProductCategory");
            this.DropPluginTable("TD_SupportDownloadRelatedProduct");          
            this.DropPluginTable("TD_SupportTopicProductCategoryMapping");
            this.DropPluginTable("TD_SupportTopicRelatedProduct");
            this.DropPluginTable("TD_SupportTopicStep");
            this.DropPluginTable("TD_SupportTopicSupportCategoryMapping");
            this.DropPluginTable("TD_SupportVideoProductCategory");
            this.DropPluginTable("TD_SupportVideoRelatedProduct");
            this.DropPluginTable("TD_SupportDownloadMapping");
            this.DropPluginTable("TD_SupportTopic");
            this.DropPluginTable("TD_SupportTopicSupportCategory");
            this.DropPluginTable("TD_SupportVideo");

            var strCon = new DataSettingsManager().LoadSettings().DataConnectionString;
            SqlConnection con = new SqlConnection(strCon);
            con.Open();
            SqlCommand cmd = new SqlCommand("DROP PROCEDURE SupportSearch", con);
            cmd.ExecuteNonQuery();
            con.Close();
         
			  
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
            return this.Database.SqlQuery<TElement>(sql, parameters).ToList();
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