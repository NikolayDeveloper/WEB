using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Iserv.Niis.Domain.Abstract;
using Iserv.Niis.Domain.Entities.AccountingData;
using Iserv.Niis.Domain.Entities.Dictionaries;
using Iserv.Niis.Domain.Entities.ManyToManyMappingEntities;
using Iserv.Niis.Domain.Entities.Other;
using Iserv.Niis.Domain.Entities.Settings;
using Microsoft.AspNetCore.Identity;

namespace Iserv.Niis.Domain.Entities.Security
{
    public class ApplicationUser : IdentityUser<int>, IEntity<int>
    {
        public ApplicationUser()
        {
            DocumentAccessRoles = new HashSet<DocumentAccessPermissions>();
            Stages = new HashSet<UserRouteStageRelation>();
            Icgss = new HashSet<UserIcgsRelation>();
            Ipcs = new HashSet<UserIpcRelation>();
            UserSettings = new HashSet<UserSetting>();
        }
        public string IpAddress { get; set; }
        public string XIN { get; set; }
        public string Password { get; set; }

        /// <summary>
        /// Пароль от сертефиката
        /// </summary>
        public string CertPassword { get; set; }

        /// <summary>
        /// Путь к сертефикату
        /// </summary>
        public string CertStoragePath { get; set; }

        public bool? IsArchive { get; set; }
        public bool IsVirtual { get; set; }
        public string TemplateUserName { get; set; }
        public int? DifficultyPriority { get; set; }
        public int? MaximumLoad { get; set; }
        public string Code { get; set; }
        public string NameRu { get; set; }
        public string NameEn { get; set; }
        public string NameKz { get; set; }
        public string Description { get; set; }
        public int? ExternalId { get; set; }
        public DateTimeOffset DateCreate { get; set; }
        public DateTimeOffset DateUpdate { get; set; }
        public int? DepartmentId { get; set; }
        public DicDepartment Department { get; set; }
        public int? PositionId { get; set; }
        public DicPosition Position { get; set; }
        public int? CustomerId { get; set; }
        public DicCustomer Customer { get; set; }

        public bool IsDeleted { get; set; }
        public DateTimeOffset? DeletedDate { get; set; }

        public ICollection<UserIcgsRelation> Icgss { get; set; }
        public ICollection<DocumentAccessPermissions> DocumentAccessRoles { get; set; }
        public ICollection<UserRouteStageRelation> Stages { get; set; }
        public ICollection<UserIpcRelation> Ipcs { get; set; }
        public Signature Signature { get; set; }

        /// <summary>
        /// Настройки пользователя.
        /// </summary>
        public ICollection<UserSetting> UserSettings { get; set; }

        [Timestamp]
        public byte[] Timestamp { get; set; }
    }
}