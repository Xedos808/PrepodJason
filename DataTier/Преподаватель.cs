

namespace DataTier
{
    public class Преподаватель
    {
        public string ФИО { get; set; } = string.Empty;
        public string Должность { get; set; } = string.Empty;
        public string Кафедра { get; set; } = string.Empty;
        public decimal Зарплата { get; set; }

        public string ПредставлениеПреподавателя => $"{ФИО} | {Должность} | {Кафедра} | {Зарплата}₽";
    }
}
