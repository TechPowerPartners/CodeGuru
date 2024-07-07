namespace Domain.Entities
{
    public class Articles
    {
        /// <summary> Идентификатор ответа на вопрос </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Титул статьи
        /// </summary>
        public string Title { get; set; }

        /// <summary> Текст статьи </summary>
        public string Text { get; set; } = default!;

        /// <summary>
        /// Создано
        /// </summary>
        public DateTime? CreatedAt { get; set; }

        /// <summary>
        /// День последного изменение
        /// </summary>
        public DateTime? EditedDate { get; protected set; }

        /// <summary>
        /// Создатель
        /// </summary>
        public User Creator { get; set; }
        
        public void EditedAt(DateTime date)
        {
            EditedDate = date;
        }
        
    }
}
