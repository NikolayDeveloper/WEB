using System;
using System.Collections.Generic;
using System.Linq;
using Iserv.Niis.DataAccess.EntityFramework;
using Iserv.Niis.ExternalServices.Features.Abstractions;
using Iserv.Niis.ExternalServices.Features.IntegrationIntelEGov.Models;
using MediatR;

namespace Iserv.Niis.ExternalServices.Features.IntegrationIntelEGov.Feature
{
    /// <summary>
    /// Медиатор загрузки зарегистрированных патентов по ИИНу заявителя
    /// </summary>
    public class GetCustomerPatentInfo
    {
        /// <summary>
        /// Методы медиатора, реализация отсутствует так как в этой части интеграции нет проверок и кодов ответов
        /// </summary>
        #region NotUsed

        public class Query : IRequest<List<CustomerPatentInfo>>
        {
            public string Argument { get; set; }
            public List<CustomerPatentInfo> Result { get; set; }
        }

        public class QueryPreLogging : AbstractPreLogging<Query>
        {
            public override void Logging(Query message)
            {
               
            }
        }

        public class QueryValidator : AbstractCommonValidate<Query>
        {
            public override void Validate(Query message)
            {
            }
        }

        public class QueryPostLogging : AbstractPostLogging<Query>
        {
            public override void Logging(Query message)
            {
            }
        }

        public class QueryException : AbstractionExceptionHandler<Query, List<CustomerPatentInfo>>
        {
            public override List<CustomerPatentInfo> GetExceptionResult(Query message, Exception ex)
            {
                return null;
            }
        }

        #endregion

        /// <summary>
        /// Загрузка зарегистрированных патентов по ИИНу заявителя
        /// </summary>
        public class QueryHandler : IRequestHandler<Query, List<CustomerPatentInfo>>
        {
            private readonly NiisWebContext _niisContext;

            public QueryHandler(NiisWebContext niisContext)
            {
                _niisContext = niisContext;
            }

            /// <summary>
            /// Метод загрузки, разбит на подзапросы для оптимизации поиска
            /// </summary>
            /// <param name="message"></param>
            /// <returns></returns>
            public List<CustomerPatentInfo> Handle(Query message)
            {
                //Получение роли Заявитель
                var roleId = _niisContext.DicCustomerRoles.First(d => d.Code == "1").Id;

                //Получение всех заявителей из патентов
                var pdc =
                    (from cust in _niisContext.ProtectionDocCustomers
                     where cust.CustomerRoleId == roleId && cust.Customer.Xin == message.Argument
                     select cust.ProtectionDocId).ToList();

                //Получение самих патентов
                message.Result = (
                    from pd in _niisContext.ProtectionDocs
                    where pdc.Contains(pd.Id)
                    select new CustomerPatentInfo
                    {
                        NameRu = pd.NameRu,
                        NameEn = pd.NameEn,
                        NameKz = pd.NameKz,
                        CertificateNumber = pd.GosNumber,
                        ValidityDate = pd.ValidDate.GetValueOrDefault().Date,
                        PatentId = pd.Barcode,
                        PatentTypeId = pd.Type.ExternalId ?? pd.TypeId
                    }).ToList();
                
                return message.Result;
            }
        }

    }
}