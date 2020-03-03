using System;
using System.Runtime.InteropServices;
using Iserv.Niis.Exceptions;
using Iserv.Niis.Integration.OneC.Infrastructure.Interfaces;

namespace Iserv.Niis.Integration.OneC.Infrastructure.Classes
{
    /// <summary>
    /// Класс осуществляющий подключение к 1C.
    /// </summary>
    public class OneCConnection : IOneCConnection
    {
        #region Конструктор
        /// <summary>
        /// Конструктор.
        /// </summary>
        public OneCConnection()
        {

        }

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="connectionString">Строка подключения к 1C.</param>
        public OneCConnection(string connectionString)
        {
            ConnectionString = connectionString;
        }
        #endregion

        #region Деструктор
        /// <summary>
        /// Деструктор.
        /// </summary>
        ~OneCConnection()
        {
            Dispose(false);
        }
        #endregion

        #region Локальные переменные
        /// <summary>
        /// Освобождены ресурсы или нет.
        /// </summary>
        private bool _isDisposed = false;

        /// <summary>
        /// COM соединение.
        /// </summary>
        private dynamic _comConnector;

        /// <summary>
        /// Соединение с 1C.
        /// </summary>
        private dynamic _comConnection;
        #endregion

        #region Свойства
        /// <summary>
        /// Строка подключения к 1C.
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        /// COMConnector для подключения к 1C.
        /// </summary>
        public string ProgId { get; set; } = "V83.COMConnector";
        #endregion

        #region Public методы
        /// <summary>
        /// Открывает соединение с базой данных с параметрами свойств, указанными в <see cref="ConnectionString"/>.
        /// </summary>
        public dynamic Open()
        {
            if (ProgId == null)
                throw new ArgumentNullException(nameof(ProgId));

            if (string.IsNullOrEmpty(ProgId))
                throw new ArgumentException(nameof(ProgId));

            InitializeComConnector();
            InitializeConnection();

            return _comConnection;
        }

        /// <summary>
        /// Закрывает соединение с базой данных. Это предпочтительный метод закрытия любого открытого соединения.
        /// </summary>
        public void Close()
        {
            ReleaseComObjects();
        }

        /// <summary>
        /// Выполняет высвобождение неуправляемых ресурсов.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }
        #endregion

        /// <summary>
        /// Выполняет высвобождение управляемых и неуправляемых ресурсов. 
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (_isDisposed)
                return;

            if (disposing)
            {
                ReleaseComObjects();
            }

            //Освобождаем неуправляемые объекты
            _isDisposed = true;
        }

        /// <summary>
        /// Освобождает COM объекты.
        /// </summary>
        private void ReleaseComObjects()
        {
            if (_comConnection != null)
            {
                Marshal.ReleaseComObject(_comConnection);
                _comConnection = null;
            }

            if (_comConnector != null)
            {
                Marshal.ReleaseComObject(_comConnector);
                _comConnector = null;
            }

            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        /// <summary>
        /// Инициализирует COM соединение.
        /// </summary>
        private void InitializeComConnector()
        {
            try
            {
                _comConnector = Activator.CreateInstance(Type.GetTypeFromProgID(ProgId));
            }
            catch (Exception exception)
            {
                ReleaseComObjects();

                throw new ComException(ComExceptionType.CannotCreateComConnectorInstance, "Cannot create com connector instance.", exception);
            }
        }

        /// <summary>
        /// Инициализирует соединение с 1C.
        /// </summary>
        private void InitializeConnection()
        {
            try
            {
                _comConnection = _comConnector.Connect(ConnectionString);
            }
            catch (Exception exception)
            {
                ReleaseComObjects();

                throw new ComException(ComExceptionType.CannotConnectTo1CDatabase, "Cannot connect to 1C database.", exception);
            }
        }
    }
}
