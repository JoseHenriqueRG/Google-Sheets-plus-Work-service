using System;
using System.Data.SqlClient;

namespace Jobs
{
    public class SqlHelper
    {
        public static string conStr { get; set; }

        public static SqlConnection GetConnection()
        {
            try
            {
                SqlConnection connection = new(conStr);
                return connection;
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message);
            }
        }
    }
}
