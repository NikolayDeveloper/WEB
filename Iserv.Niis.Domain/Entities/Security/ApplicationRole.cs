using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Iserv.Niis.Domain.Abstract;
using Iserv.Niis.Domain.Constants;
using Iserv.Niis.Domain.Entities.ManyToManyMappingEntities;
using Microsoft.AspNetCore.Identity;

namespace Iserv.Niis.Domain.Entities.Security
{
    public class ApplicationRole : IdentityRole<int>, IEntity<int>, IHaveLocalizedNames
    {
        public ApplicationRole()
        {
            Stages = new HashSet<RoleRouteStageRelation>();
            ProtectionDocTypes = new HashSet<RoleProtectionDocTypeRelation>();
        }
        public string Code { get; set; }
        public string NameRu { get; set; }
        public string NameEn { get; set; }
        public string NameKz { get; set; }
        public int? ExternalId { get; set; }

        [Timestamp]
        public byte[] Timestamp { get; set; }
        [NotMapped]
        public bool CanSelectMktu =>
            Name == KeyFor.Role.ExpertOfFull_InEssenceExpertise
            || Name == KeyFor.Role.ExpertOfPreliminary_FormalExpertise;
        public DateTimeOffset DateCreate { get; set; }
        public DateTimeOffset DateUpdate { get; set; }
        public ICollection<RoleRouteStageRelation> Stages { get; set; }
        public ICollection<RoleProtectionDocTypeRelation> ProtectionDocTypes { get; set; }

        public override string ToString()
        {
            return Code;
        }
    }
}