using System;
using System.Linq;
using System.Reflection;
using Iserv.Niis.Business.Abstract;
using Iserv.Niis.Domain.Abstract;
using Iserv.Niis.Exceptions;

namespace Iserv.Niis.Business.Implementations
{
    public class DicTypeResolver : IDicTypeResolver
    {
        public Type Resolve(string dicTypeStr)
        {
            var dicType = Assembly.GetAssembly(typeof(DictionaryEntity<>)).GetTypes()
                .SingleOrDefault(x => x.Name.Equals(dicTypeStr, StringComparison.InvariantCultureIgnoreCase));

            if (dicType == null)
                throw new DictionaryNameException();

            return dicType;
        }
    }
}