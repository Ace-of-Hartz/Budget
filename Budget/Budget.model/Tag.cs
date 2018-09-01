using System;
using System.Collections.Generic;
using System.Text;

namespace Budget.model
{
    public class Tag : dto.Tags
    {
        public Account Account { get; set; }
    }
}
