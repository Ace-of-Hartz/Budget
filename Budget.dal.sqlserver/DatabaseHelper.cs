using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Budget.dal.sqlserver
{
    public static class DatabaseHelper
    {
        public static IEnumerable<T> GetDtos<T>(this SqlDataReader sqlDataReader)
        {
            while (sqlDataReader.Read())
            {
                yield return sqlDataReader.GetDto<T>();
            }
        }

        private static T GetDto<T>(this SqlDataReader sqlDataReader)
        {
                var newObj = Activator.CreateInstance<T>();
                PropertyInfo[] propertyInfos = newObj.GetType().GetProperties();
                for (int i = 0; i < propertyInfos.Length; ++i)
                {
                    int columnIndex = sqlDataReader.GetOrdinal(propertyInfos[i].Name);
                    var propertyValue = sqlDataReader.IsDBNull(columnIndex) ? null : sqlDataReader.GetValue(columnIndex);

                    propertyInfos[i].SetValue(newObj, propertyValue);
                }
                return newObj;
        }
    }
}
