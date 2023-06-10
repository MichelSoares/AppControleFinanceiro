using Npgsql;
using System.Data;
using System.Reflection;

namespace ControleFinanceiroAPI.Util
{
    public static class SqlDBHelper
    {
        public static DataSet ExecuteDataSet(string sql, CommandType cmdType, string ConnectionString)
        {
            using (DataSet ds = new DataSet())
            using (NpgsqlConnection connStr = new NpgsqlConnection(ConnectionString))
            using (NpgsqlCommand cmd = new NpgsqlCommand(sql, connStr))
            {
                cmd.CommandType = cmdType;
                try
                {
                    cmd.Connection.Open();
                    new NpgsqlDataAdapter(cmd).Fill(ds);
                }
                catch (NpgsqlException ex)
                {
                    //log to a file or Throw a message ex.Message;
                }
                return ds;
            }
        }
        public static void Executenoquery(string sql, CommandType cmdType, string ConnectionString)
        {
            using (DataSet ds = new DataSet())
            using (NpgsqlConnection connStr = new NpgsqlConnection(ConnectionString))
            using (NpgsqlCommand cmd = new NpgsqlCommand(sql, connStr))
            {
                cmd.CommandType = cmdType;
                try
                {
                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();
                }
                catch (NpgsqlException ex)
                {
                    //log to a file or Throw a message ex.Message;
                }
            }
        }

        public static Object ExecuteNoQueryScalar(string sql, CommandType cmdType, string ConnectionString)
        {
            using (DataSet ds = new DataSet())
            using (NpgsqlConnection connStr = new NpgsqlConnection(ConnectionString))
            using (NpgsqlCommand cmd = new NpgsqlCommand(sql, connStr))
            {
                cmd.CommandType = cmdType;
                try
                {
                    cmd.Connection.Open();
                    //cmd.Connection.Dispose();
                    return cmd.ExecuteScalar();
                }
                catch (NpgsqlException ex)
                {
                    return (int)-1;
                }
            }
        }

        public static DataTable ExecuteDataTable(string sql, CommandType cmdType, String ConnectionString)
        {
            using (DataSet ds = new DataSet())
            using (NpgsqlConnection connStr = new NpgsqlConnection(ConnectionString))
            using (NpgsqlCommand cmd = new NpgsqlCommand(sql, connStr))
            {
                cmd.CommandType = cmdType;
                try
                {
                    cmd.Connection.Open();
                    new NpgsqlDataAdapter(cmd).Fill(ds);
                }
                catch (NpgsqlException ex)
                {
                    //Show a message or log a message on ex.Message
                }
                return ds.Tables[0];
            }
        }
        public static DataTable ExecuteDataTable_cmd(NpgsqlCommand sql, CommandType cmdType, String ConnectionString)
        {
            using (DataSet ds = new DataSet())
            using (NpgsqlConnection connStr = new NpgsqlConnection(ConnectionString))
            using (NpgsqlCommand cmd = sql)
            {
                sql.Connection = connStr;
                cmd.CommandType = cmdType;
                try
                {
                    cmd.Connection.Open();
                    new NpgsqlDataAdapter(cmd).Fill(ds);
                }
                catch (NpgsqlException ex)
                {
                    //Show a message or log a message on ex.Message
                }
                //Console.WriteLine(ds.Tables[0]);
                return ds.Tables[0];
            }
        }
        public static T ToObject<T>(this DataRow dataRow)
     where T : new()
        {
            T item = new T();
            foreach (DataColumn column in dataRow.Table.Columns)
            {
                if (dataRow[column] != DBNull.Value)
                {
                    PropertyInfo prop = item.GetType().GetProperty(column.ColumnName);
                    if (prop != null)
                    {
                        object result = Convert.ChangeType(dataRow[column], prop.PropertyType);
                        prop.SetValue(item, result, null);
                        continue;
                    }
                    else
                    {
                        FieldInfo fld = item.GetType().GetField(column.ColumnName);
                        if (fld != null)
                        {
                            object result = Convert.ChangeType(dataRow[column], fld.FieldType);
                            fld.SetValue(item, result);
                        }
                    }
                }
            }
            return item;
        }
    }
}
