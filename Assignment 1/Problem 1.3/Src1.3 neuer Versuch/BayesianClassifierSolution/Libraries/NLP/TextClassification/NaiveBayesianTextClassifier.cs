using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace NLP.TextClassification
{
    public class NaiveBayesianTextClassifier
    {
        public Dictionary<string,TokenData> Initialize(List<Sentence> tokenizedTrainingSet)
        {
            Dictionary<string, TokenData> bagOfWords = new Dictionary<string, TokenData>();
            foreach (Sentence sentence in tokenizedTrainingSet)
            {
                foreach (TokenData tokenData in sentence.TokenDataList)
                {
                    if (bagOfWords.ContainsKey(tokenData.Token.Spelling))
                    {
                        bagOfWords[tokenData.Token.Spelling].Class1Count += tokenData.Class1Count;
                        bagOfWords[tokenData.Token.Spelling].Class0Count += tokenData.Class0Count;
                    }
                    else
                    {
                        bagOfWords.Add(tokenData.Token.Spelling, tokenData);
                    }
                }

            }

            return bagOfWords;
            
        }

        public int Classify(Sentence tokenizedReview, Dictionary<string,TokenData> bagOfWords, double class0Denominator, double class1Denominator, double class0LabelProb, double class1LabelProb)
        {
            // Implement the caluclations to find the class Label.
            double class0LabelProbability = Math.Log(class0LabelProb);
            double class1LabelProbability = Math.Log(class1LabelProb);
            foreach (TokenData tokenData in tokenizedReview.TokenDataList)
            {
                if (bagOfWords.ContainsKey(tokenData.Token.Spelling))
                {
                    double class0LabelCounts = bagOfWords[tokenData.Token.Spelling].Class0Count;
                    double class0TokenProb = (class0LabelCounts + 1) / class0Denominator;
                    class0LabelProbability += Math.Log(class0TokenProb);
                    


                    double class1LabelCounts = bagOfWords[tokenData.Token.Spelling].Class1Count;
                    double class1TokenProb = (class1LabelCounts + 1) / class1Denominator;
                    class1LabelProbability += Math.Log(class1TokenProb);
                    

                }
                

            }
            if (class0LabelProbability > class1LabelProbability)
            {
                return 0;
            }
            else
            {
                return 1;
            }
        }
    }
}
