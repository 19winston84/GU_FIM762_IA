using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLP
{
    public class Token
    {
        // get; and set; allows for setting a value but also to ask for the value.
        public string Spelling { get; set; }

        public string POSTag { get; set; }
    }
}
