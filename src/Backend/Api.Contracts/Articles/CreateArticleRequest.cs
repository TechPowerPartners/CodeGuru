using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Contracts.Articles
{
    public class CreateArticleRequest
    {
        /// <summary>
        /// Титул статьи
        /// </summary>
        public string Title { get; set; }

        /// <summary> Текст статьи </summary>
        public string Text { get; set; } = default!;

        /// <summary>
        /// Создано
        /// </summary>
        public DateTime? CreatedAt { get; protected set; }


        
        public bool CheckIfNull()
        {
            if (Title.Length < 1) return false;
            if (Text.Length < 1) return false;
            return true;
        }
        
    }
}
