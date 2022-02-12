using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Items.Database
{
    public class DbContext
    {
        private readonly IConfiguration _configuration;

        public DbContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IDbConnection GetDbConnection() => new SqlConnection(_configuration.GetConnectionString("DapperConnection"));

        public IDbConnection CheckDbExisitingConnection() => new SqlConnection(_configuration.GetConnectionString("CheckDatabaseExistsConnection"));
        
    
    }

}
