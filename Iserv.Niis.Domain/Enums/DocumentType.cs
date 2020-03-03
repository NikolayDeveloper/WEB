namespace Iserv.Niis.Domain.Enums
{
    public enum DocumentType : byte
    {
        Incoming = 0,
        Outgoing = 1,
        Internal = 3,
        Request = 4,
        Contract = 5,
        ProtectionDoc = 6,
        None = 7,
        DocumentRequest = 8 //Документы Заявки
    }
}
