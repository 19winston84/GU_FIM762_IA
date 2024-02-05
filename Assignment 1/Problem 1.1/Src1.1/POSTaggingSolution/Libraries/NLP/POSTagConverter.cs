using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLP
{
    public class POSTagConverter(string oldTag)
    {
        private List<Sentence> sentenceList;

        public POSDataSet()
        {
            sentenceList = new List<Sentence>();
        }

        public List<Sentence> SentenceList
        {
            get { return sentenceList; }
            set { sentenceList = value; }
        }
    }
}