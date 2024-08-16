using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities;

public class UserComment
{
    [Key]
    public Guid Guid { get; set; }
    public Guid UserId { get; set; }
    public Guid Article { get; set; }
    public string Comment { get; set; }
    public DateTime Created { get; set; } = DateTime.Now;

}
