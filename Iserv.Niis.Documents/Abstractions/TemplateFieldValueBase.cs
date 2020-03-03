using System;
using System.Collections.Generic;
using Iserv.Niis.DataAccess.EntityFramework;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; ;

namespace Iserv.Niis.Documents.Abstractions
{
    public abstract class TemplateFieldValueBase : ITemplateFieldValue
    {
        protected readonly IExecutor Executor;

        protected TemplateFieldValueBase(IExecutor executor)
        {
            Executor = executor;
        }

        public dynamic Get(Dictionary<string, object> parameters)
        {
            CheckRequired(parameters);

            return GetInternal(parameters);
        }

        protected abstract IEnumerable<string> RequiredParameters();

        protected abstract dynamic GetInternal(Dictionary<string, object> parameters);


        /// <summary>
        ///     Проверяет обязательные параметры
        /// </summary>
        /// <param name="parameters"></param>
        private void CheckRequired(Dictionary<string, object> parameters)
        {
            foreach (var key in RequiredParameters())
                if (!parameters.ContainsKey(key))
                    throw new ArgumentNullException();
        }
    }
}