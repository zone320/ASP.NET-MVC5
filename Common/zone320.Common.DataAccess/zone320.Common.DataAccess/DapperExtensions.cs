using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace zone320.Common.DataAccess
{
    public static class DapperExtensions
    {
        /// <summary>
        /// Converts an enumerable set to a Dapper table value parameter
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerable"></param>
        /// <param name="typeName"></param>
        /// <param name="orderedColumnNames"></param>
        /// <returns></returns>
        public static SqlMapper.ICustomQueryParameter AsTableValuedParameter<T>(this IEnumerable<T> enumerable, string typeName, IEnumerable<string> orderedColumnNames = null)
        {
            var dataTable = new DataTable();
            if (typeof(T).IsValueType || typeof(T).FullName.Equals("System.String"))
            {
                dataTable.Columns.Add(orderedColumnNames == null ? "NONAME" : orderedColumnNames.First(), typeof(T));
                foreach (T obj in enumerable)
                {
                    dataTable.Rows.Add(obj);
                }
            }
            else
            {
                PropertyInfo[] properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
                PropertyInfo[] readableProperties = properties.Where(w => w.CanRead).ToArray();
                if (readableProperties.Length > 1 && orderedColumnNames == null)
                {
                    throw new ArgumentException("Ordered list of column names must be provided when TVP contains more than one column");
                }

                var columnNames = (orderedColumnNames ?? readableProperties.Select(s => s.Name)).ToArray();
                foreach (string name in columnNames)
                {
                    var propertyType = readableProperties.Single(s => s.Name.Equals(name)).PropertyType;
                    dataTable.Columns.Add(name, Nullable.GetUnderlyingType(propertyType) ?? propertyType);
                }

                foreach (T obj in enumerable)
                {
                    dataTable.Rows.Add(columnNames.Select(s => readableProperties.Single(s2 => s2.Name.Equals(s)).GetValue(obj)).ToArray());
                }
            }

            return dataTable.AsTableValuedParameter(typeName);
        }
    }
}