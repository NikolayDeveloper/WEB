using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Iserv.Niis.Domain.Abstract;

namespace Iserv.Niis.Domain.Entities.Dictionaries
{
    public class DicBeneficiaryType: DictionaryEntity<int>, IReference, IHaveConcurrencyToken
    {
    }
}
