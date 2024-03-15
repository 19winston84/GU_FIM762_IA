using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLP;

namespace NLP.NGrams
{
    public class NGram
    {
        public string Identifier { get; set; } // e.g. "how are you", with spaces, for a 3-gram

        public List<string> TokenList { get; set; } // e.g. {"how","are","you"}, for the 3-gram exemplified above

        public double FrequencyPerMillionInstances { get; set; }

        public int Frequency { get; set; }

        public NGram(string identifier)
        {
            Identifier = identifier.Trim();

            TokenList = identifier.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList();

            for (int i = 0; i < TokenList.Count; i++)
            {
                TokenList[i] = TokenList[i].Trim();
            }
        }
    }
}
