using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLP
{
    public class Sentence
    {
        private List<string> tokenDataList;

        public Sentence()
        {
            tokenDataList = new List<string>();  
        }

        public List<string> TokenDataList
        {
            get { return tokenDataList; }
            set { tokenDataList = value; }
        }
    }
}
