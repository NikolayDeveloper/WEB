namespace Iserv.Niis.Documents.Enums
{
    /// <summary>
    /// Позиция колонтитульных QR-кодов.
    /// </summary>
    public enum QrCodePosition : byte
    {
        /// <summary>
        /// У документа нет QR-кода в колонтитулах.
        /// </summary>
        None,
        /// <summary>
        /// QR-код находится наверху.
        /// </summary>
        Header,
        /// <summary>
        /// QR-код находится внизу.
        /// </summary>
        Footer
    }
}
