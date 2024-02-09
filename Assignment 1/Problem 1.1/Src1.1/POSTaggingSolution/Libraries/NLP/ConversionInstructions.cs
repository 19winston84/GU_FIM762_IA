using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLP
{
    public class ConversionInstructions
    {
        // Field
        private List<TagConversionPair> tagConversionList;

        // Constructor
        public ConversionInstructions()
        {
            tagConversionList = new List<TagConversionPair>();
        }

        // Property. Allows for access of Field.
        public List<TagConversionPair> TagConversionList
        {
            get { return tagConversionList; }
            set { tagConversionList = value; }
        }
    }
}
