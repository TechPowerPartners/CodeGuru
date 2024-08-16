using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Contracts.Articles
{
    public class GetCommentsRequest
    {
        public Guid Article { get; set; }
    }
}
