using System;
using System.Runtime.InteropServices;
using Iserv.Niis.DataBridge.Implementations;
using Iserv.Niis.Integration.OneC.Infrastructure.Interfaces;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;

namespace Iserv.Niis.Integration.OneC.Infrastructure.Queries
{
    /// <summary>
    /// Базовый запрос для 1C.
    /// </summary>
    public abstract class BaseOneCQuery : BaseQuery
    {
        /// <summary>
        /// Создает интерфейс для подключения к 1C.
        /// </summary>
        /// <returns>Интерфейс для подключения к 1C.</returns>
        protected IOneCConnection CreateOneCConnection()
        {
            return NiisOneCIntegrationAmbientContext.Current.OneCConnection;
        }

        /// <summary>
        /// Освобождает COM объект.
        /// </summary>
        protected void ReleaseComObject(ref dynamic comObject)
        {
            if (comObject == null)
                return;

            Marshal.ReleaseComObject(comObject);
            comObject = null;

            GC.Collect();
            GC.WaitForPendingFinalizers();
        }
    }
}
