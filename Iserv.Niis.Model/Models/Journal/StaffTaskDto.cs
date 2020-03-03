namespace Iserv.Niis.Model.Models.Journal
{
    /// <summary>
    /// Представляет агрегированные данные по задачам сотрудника для отображение в дневнике в гриде эксперта
    /// </summary>
    public class StaffTaskDto
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public int Incoming { get; set; }
        public int Executed { get; set; }
        public int OnJob { get; set; }
        public int NotOnJob { get; set; }
        public int Overdue { get; set; }
        public int Outgoing { get; set; }
    }
}
