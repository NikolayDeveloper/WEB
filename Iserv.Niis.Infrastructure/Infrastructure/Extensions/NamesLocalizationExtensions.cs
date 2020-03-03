using System.Threading;
using Iserv.Niis.Domain.Abstract;

namespace Iserv.Niis.Api.Infrastructure.Extensions
{
    public static class NamesLocalizationExtensions
    {
        ///// <summary>
        ///// Возвращает имя в зависимости от текущей культуры. Если значение пустое, берется Ru
        ///// </summary>
        ///// <typeparam name="TSrc"></typeparam>
        ///// <param name="selector"></param>
        ///// <returns></returns>
        //public static string LocalizedName<TSrc>(this TSrc selector) where TSrc : IHaveLocalizedNames
        //{
        //    return selector.LocalizedName(string.Empty, true);
        //}

        /// <summary>
        /// Возвращает имя в зависимости от указанной культуры
        /// </summary>
        /// <typeparam name="TSource">Источник</typeparam>
        /// <param name="selector">Сущность которую следует локализовать</param>
        /// <param name="targetLanguage">Локализация</param>
        /// <param name="useRuValueIfTargetNull">Если указанная локализация не содержит значения, использовать значение для русского языка</param>
        /// <returns></returns>
        public static string LocalizedName<TSource>(this TSource selector, string targetLanguage, bool useRuValueIfTargetNull)
            where TSource : IHaveLocalizedNames
        {
            if (selector == null)
            {
                return string.Empty;
            }
            var languageName = targetLanguage ?? Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName;
            switch (languageName)
            {
                case "en":
                    return !string.IsNullOrEmpty(selector.NameEn)
                        ? selector.NameEn
                        : (useRuValueIfTargetNull
                            ? selector.NameRu
                            : string.Empty);
                case "kk":
                    return !string.IsNullOrEmpty(selector.NameKz)
                        ? selector.NameKz
                        : (useRuValueIfTargetNull
                            ? selector.NameRu
                            : string.Empty);
                default:
                    return selector.NameRu;
            }
        }

        //public static IMappingExpression<TSource, TDestination> WithLocalizedProperty<TSource, TDestination>(this IMappingExpression<TSource, TDestination> destinationMember)
        //    where TSource : IHaveLocalizedNames where TDestination : SelectOptionDto
        //{
        //    return destinationMember.ForMember(x => x.NameValue, opt => opt.MapFrom(x => x.LocalizedName()));
        //}
    }
}
