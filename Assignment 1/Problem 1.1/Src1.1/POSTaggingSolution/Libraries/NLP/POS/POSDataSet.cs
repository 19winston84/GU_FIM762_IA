using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLP.POS
{
    public class POSDataSet
    {
        // Field
        private List<Sentence> sentenceList;


        // Constructor
        public POSDataSet()
        {
            sentenceList = new List<Sentence>();
        }

        public POSDataSet(List<Sentence> sentences)
        {
            Sentences = sentences;
        }

        // Property. Allows for access of Field.
        public List<Sentence> SentenceList
        {
            get { return sentenceList; }
            set { sentenceList = value; }
        }
        public List<Sentence> Sentences { get; private set; }

        // Method
        public void ConvertPOSTags(ConversionInstructions conversionInstructions)
        {
            foreach (Sentence sentence in sentenceList) 
            {
                foreach (TokenData tokenData in sentence.TokenDataList)
                {
                    TagConversionPair conversionPair = conversionInstructions.TagConversionList.Find(pair => pair.OldTag.Equals(tokenData.Token.POSTag, StringComparison.OrdinalIgnoreCase));

                    // If a conversion is found, update the token's POS tag
                    if (conversionPair != null)
                    {
                        tokenData.Token.POSTag = conversionPair.NewTag;
                    }
                }
            }

        }

        // Static Method
        public static (POSDataSet trainingSet, POSDataSet testSet) Split(POSDataSet completeDataSet, double splitFraction)
        {
            int splitIndex = (int)(completeDataSet.SentenceList.Count * splitFraction);

            // Assuming Sentences is a property that holds the dataset sentences
            var trainingSentences = completeDataSet.SentenceList.Take(splitIndex).ToList();
            var testSentences = completeDataSet.SentenceList.Skip(splitIndex).ToList();

            POSDataSet trainingSet = new POSDataSet(trainingSentences);
            POSDataSet testSet = new POSDataSet(testSentences);

            return (trainingSet, testSet);
        }
    }
}
