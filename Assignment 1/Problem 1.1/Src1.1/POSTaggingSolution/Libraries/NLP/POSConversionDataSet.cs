using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLP
{
    public class POSConversionDataSet()
    {
        private List<TagPair> tagPairList;

        public POSConversionDataSet()
        {
            tagPairList = new List<TagPair>();
        }

        public List<TagPair> TagPairList
        {
            get { return tagPairList; }
            set { tagPairList = value; }
        }
    }
}