using System;
using Iserv.Niis.Domain.Enums;
using Iserv.Niis.Model.Models.Calendar;

namespace Iserv.Niis.Business.Abstract
{
    public interface ICalendarProvider
    {
        /// <summary>
        /// Метод для получения даты публикации  согласно дням публикаций в производственном календаре
        /// </summary>
        /// <param name="registerDate"></param>
        /// <returns></returns>
        DateTimeOffset GetPublicationDate(DateTimeOffset registerDate);
        /// <summary>
        /// Метод для получени предыдущего периода публикаций согласно дням публикаций в производственном календаре
        /// </summary>
        /// <param name="publicationDate"></param>
        /// <returns></returns>
        PublicationRange GetPreviousPublicationRange(DateTimeOffset publicationDate);
        /// <summary>
        /// Метод для получения даты выполнения услуги согласно рабочим дням в производственном календаре
        /// </summary>
        /// <param name="fromDate"></param>
        /// <param name="expirationType">Тип истечения</param>
        /// <param name="expirationValue"></param>
        /// <returns></returns>
        DateTimeOffset GetExecutionDate(DateTimeOffset fromDate, ExpirationType expirationType, short expirationValue);

        /// <summary>
        /// Метод получения даты готовности для передачи на формальную экспертизу
        /// </summary>
        /// <param name="fromDate">Начальная дата</param>
        /// <returns>Дата готовности для передачи на полную экспертизу</returns>
        DateTimeOffset GetFormalExaminationDate(DateTimeOffset fromDate);

        /// <summary>
        /// Метод получения даты готовности для передачи на полную экспертизу
        /// </summary>
        /// <param name="fromDate">Начальная дата</param>
        /// <returns>Дата готовности для передачи на полную экспертизу</returns>
        DateTimeOffset GetFullExaminationDate(DateTimeOffset fromDate);

        /// <summary>
        /// Метод получения даты готовности для передачи на в Госреестр
        /// </summary>
        /// <param name="fromDate"></param>
        /// <returns></returns>
        DateTimeOffset GetTransferToGosreestrDate(DateTimeOffset fromDate);

        /// <summary>
        /// Является ли выходным днем
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        bool IsHoliday(DateTimeOffset date);
    }
}