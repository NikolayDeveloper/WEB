using System;
using Iserv.Niis.Integration.OneC.Infrastructure.Classes;
using Iserv.Niis.Integration.OneC.Infrastructure.Interfaces;
using Microsoft.Extensions.DependencyInjection;

using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; ; 

namespace Iserv.Niis.Integration.OneC.Infrastructure
{
    /// <summary>
    /// Контекст окружения интеграции с 1C.
    /// </summary>
    public class NiisOneCIntegrationAmbientContext
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="serviceProvider">Определяет механизм извлечения объекта обслуживания (объекта, обеспечивающего настраиваемую поддержку других объектов).</param>
        public NiisOneCIntegrationAmbientContext(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _current = this;
        }

        #region Локальные переменные
        /// <summary>
        /// Определяет механизм извлечения объекта обслуживания (объекта, обеспечивающего настраиваемую поддержку других объектов).
        /// </summary>
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// Контекст окружения интеграции с 1C.
        /// </summary>
        private static NiisOneCIntegrationAmbientContext _current;
        #endregion

        #region Свойства
        /// <summary>
        /// Контекст окружения интеграции с 1C.
        /// </summary>
        public static NiisOneCIntegrationAmbientContext Current
        {
            get
            {
                if (_current == null)
                    throw new Exception($"{nameof(NiisOneCIntegrationAmbientContext)} current is null");

                return _current;
            }
        }

        /// <summary>
        /// Интерфейс для подключения к 1C.
        /// </summary>
        public IOneCConnection OneCConnection => _serviceProvider.GetService<IOneCConnection>();

        /// <summary>
        /// Представляет набор команд возвращающих команды / запросы для выполнения.
        /// </summary>
        //public IExecutor Executor => _serviceProvider.GetRequiredService<IExecutor>();
        public IExecutor Executor => _serviceProvider.GetRequiredService<IExecutor>();
        #endregion
    }
}
