using Microsoft.SqlServer.Server;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Iserv.Niis.Utils
{
    public class TableValuedParameterBuilder
    {
        readonly string _typeName;
        readonly SqlMetaData[] _columns;
        readonly List<SqlDataRecord> _rows;

        public TableValuedParameterBuilder(string typeName, params SqlMetaData[] columns)
        {
            _typeName = typeName;
            _columns = columns;
            _rows = new List<SqlDataRecord>();
        }

        public TableValuedParameterBuilder AddRow(params object[] fieldValues)
        {
            var row = new SqlDataRecord(_columns);
            row.SetValues(fieldValues);
            _rows.Add(row);
            return this;
        }

        public SqlParameter CreateParameter(string name)
        {
            return new SqlParameter
            {
                ParameterName = name,
                Value = _rows.Any() ? _rows : null,
                TypeName = _typeName,
                SqlDbType = SqlDbType.Structured
            };
        }
    }
}