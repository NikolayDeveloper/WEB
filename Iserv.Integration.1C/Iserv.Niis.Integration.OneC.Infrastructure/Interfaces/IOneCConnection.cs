using System;

namespace Iserv.Niis.Integration.OneC.Infrastructure.Interfaces
{
    /// <summary>
    /// Интерфейс для подключения к 1C.
    /// </summary>
    public interface IOneCConnection : IDisposable
    {
        #region Свойства
        /// <summary>
        /// Строка подключения к 1C.
        /// </summary>
        string ConnectionString { get; set; }

        /// <summary>
        /// COMConnector для подключения к 1C.
        /// </summary>
        string ProgId { get; set; }
        #endregion

        /// <summary>
        /// Открывает соединение с базой данных с параметрами свойств, указанными в <see cref="ConnectionString"/>.
        /// </summary>
        /// <returns>Соединение с 1C.</returns>
        dynamic Open();

        /// <summary>
        /// Закрывает соединение с базой данных. Это предпочтительный метод закрытия любого открытого соединения.
        /// </summary>
        void Close();
    }
}