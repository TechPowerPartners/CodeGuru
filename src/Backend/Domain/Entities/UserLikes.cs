using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities;
public class UserLikes
{
    [Key]
    public Guid Guid { get; set; }

    public Guid ArticleId { get; set; }
    public Guid UserId { get; set; }
}
