using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Iserv.Niis.Domain.Helpers;

namespace Iserv.Niis.Model.Models.BibliographicData
{
    public class ChangesDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Owner.Type OwnerType { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
        public ChangeType ChangeType { get; set; }
    }

    public enum ChangeType
    {
        DeclarantName = 0,
        DeclarantAddress = 1,
        AddresseeAddress = 2,
        DeclarantNameEn = 3,
        DeclarantAddressEn = 4,
        AddresseeAddressEn = 5,
        DeclarantNameKz = 6,
        DeclarantAddressKz = 7,
        AddresseeAddressKz = 8,
        Image = 9,
        Icgs = 10,
        Everything = 11,
        PatentAttorney = 12,
        Addressee = 13,
        Declarant = 14
    }
}
