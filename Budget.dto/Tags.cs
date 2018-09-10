using System;
using System.Collections.Generic;
using System.Text;

namespace Budget.dto
{
    public class Tags
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public string Tag { get; set; }
        public string Description { get; set; }
    }
}
