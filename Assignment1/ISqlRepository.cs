using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace BankingApplication
{
    interface ISqlRepository<T>
    {
        public void Insert(T entity);
        public void Update(T entity);
        public T SelectById(int id);
        public List<T> SelectAll(int id);
    }
}
