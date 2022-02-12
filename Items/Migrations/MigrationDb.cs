using Dapper;
using Items.Database;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Items.Migrations
{
    public class MigrationDb
    {
        private readonly DbContext _dbContext;
        private readonly IDbConnection checkDbConnection;
        private readonly IDbConnection dbConnection;
        public MigrationDb(DbContext dbContext)
        {
            _dbContext = dbContext;
            checkDbConnection = _dbContext.CheckDbExisitingConnection();
            dbConnection = _dbContext.GetDbConnection();
        }

        public void MigrateDatabase()
        {
            var query = "SELECT * FROM sys.databases WHERE name = 'DbItem'";

            var databases = checkDbConnection.Query(query);
			if (!databases.Any())
			{
				checkDbConnection.Execute("CREATE DATABASE DbItem");

				dbConnection.Execute(@"CREATE PROCEDURE SELECTREGIONSBYHIERARCHY
										AS
										BEGIN
											  WITH recursiveHierarchyRegions(Id,CategoryName,
												CategoryValue, 
												ParentId,Parents)
												AS (
												SELECT Id, 
												CategoryName,
												CategoryValue, 
												ParentId, 
												cast('LinkTo:' as VARCHAR(3000)) as Parents		
												FROM Regions AS Area
													WHERE ParentId IS NULL        
		
												UNION ALL

												SELECT r.Id, 
												r.CategoryName,
												r.CategoryValue, 
												r.ParentId,
												cast(recursiveHierarchyRegions.Parents + '>>>'+ recursiveHierarchyRegions.CategoryValue  as VARCHAR(3000))
													FROM Regions r, recursiveHierarchyRegions 
													where r.ParentId = recursiveHierarchyRegions.Id
											)

												SELECT Id,CategoryName,CategoryValue,ParentId,SUBSTRING(Parents,11,LEN(Parents)) as Parents
												FROM RecursiveHierarchyRegions
										END");

				dbConnection.Execute(@"CREATE PROCEDURE PaginationOrder 
											  @offset INT, @count INT
										AS
										BEGIN
    										SELECT * FROM Orders
											ORDER BY Id asc
											OFFSET @offset ROWS FETCH NEXT @count ROWS ONLY;
										END");

			}

            checkDbConnection.Close();
            dbConnection.Close();
        }
    }
}
