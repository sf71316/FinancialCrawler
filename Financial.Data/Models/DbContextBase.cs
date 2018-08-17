using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace Financial.Data.Models
{
    public abstract class DbContextBase
    {
        protected FinancialContext _Context;
        public DbContextBase(string ConnectionString)
        {
            DbContextOptionsBuilder<FinancialContext> optionsBuilder =
                                new DbContextOptionsBuilder<FinancialContext>();
            optionsBuilder.UseSqlServer(ConnectionString);
            _Context = new FinancialContext(optionsBuilder.Options);
        }
        public DbConnection Connection
        {
            get
            {
                return this._Context.Database.GetDbConnection();
            }
        }
    }
}
