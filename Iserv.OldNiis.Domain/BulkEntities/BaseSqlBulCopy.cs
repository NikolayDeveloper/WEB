using System;
using System.Data;
using System.Data.SqlClient;

namespace Iserv.OldNiis.Domain.BulkEntities
{
    /// <summary>
    /// Базовый класс для массовых вставок в таблицы.
    /// </summary>
    public class BaseSqlBulCopy
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="sqlConnectionString">Строка соединения с бд.</param>
        /// <param name="tableName">Имя таблицы.</param>
        public BaseSqlBulCopy(string sqlConnectionString, string tableName)
        {
            SqlConnectionString = sqlConnectionString;
            TableName = tableName;
        }

        #region Свойства
        /// <summary>
        /// Строка соединения с бд.
        /// </summary>
        public string SqlConnectionString { get; protected set; }

        /// <summary>
        /// Имя таблицы.
        /// </summary>
        public string TableName { get; protected set; }

        /// <summary>
        /// Схема колонок.
        /// </summary>
        protected DataColumn[] Structure { get; set; }

        #region Массовая выгрузка данных в таблицу
        /// <summary>
        /// Массовая выгрузка данных в таблицу
        /// </summary>
        public SqlBulkCopy SqlBulkCopy { get; internal set; }
        #endregion

        /// <summary>
        /// Таблица с данными записываемыми на сервер.
        /// </summary>
        public DataTable DataTable { get; protected set; }
        #endregion

        /// <summary>
        /// Осуществляет запись в БД.
        /// </summary>
        /// <returns></returns>
        public bool SqlBulkCopyWriteToServer()
        {
            if (SqlBulkCopy == null || DataTable == null)
                return false;

            try
            {
                SqlBulkCopy.WriteToServer(DataTable);
            }
            catch(Exception ex)
            {
                throw new NotSupportedException("Ошибка инсерта", ex);
            }

            InitializeDataTable();

            GC.Collect();

            return true;
        }

        #region Инициализация таблицы
        /// <summary>
        /// Инициализация таблицы
        /// </summary>
        /// <returns>Представляет одну таблицу с данными в памяти</returns>
        public void InitializeDataTable()
        {
            DataTable = new DataTable(TableName);

            foreach (var dataColumn in Structure)
            {
                var newColumn = new DataColumn(dataColumn.ColumnName, dataColumn.DataType)
                {
                    DefaultValue = dataColumn.DefaultValue
                };

                DataTable.Columns.Add(newColumn);
            }
        }
        #endregion

        #region Формирует объект <see cref="SqlBulkCopy"/> и <see cref="DataTable"/>
        /// <summary>
        /// Формирует объект <see cref="SqlBulkCopy"/> и <see cref="DataTable"/>
        /// </summary>
        public void InitializeSqlBulkCopy()
        {
            SqlBulkCopy = new SqlBulkCopy(SqlConnectionString, SqlBulkCopyOptions.KeepNulls)
            {
                DestinationTableName = TableName
            };

            foreach (var dataColumn in Structure)
            {
                SqlBulkCopy.ColumnMappings.Add(dataColumn.ColumnName, dataColumn.ColumnName);
            }
        }
        #endregion
    }
}
