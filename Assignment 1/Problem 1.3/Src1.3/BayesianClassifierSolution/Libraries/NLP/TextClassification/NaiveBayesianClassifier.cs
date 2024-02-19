using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLP.TextClassification
{
    public class NaiveBayesianClassifier
    {
        public List<Sentence> ImportData(List<Sentence> tokenizedDataSet, int classLabel)
        {
            int chosenClassLabel = classLabel; // 0 negative, 1 positive
            List<Sentence> classLabeledDataSet = new List<Sentence>();

            foreach (Sentence sentence in tokenizedDataSet) 
            {
                Sentence sentenceOfChosenClassLabel = new Sentence();
                foreach (TokenData tokenData in sentence.TokenDataList)
                { 
                    List<TokenData> tokenDataOfChosenClassLabel = new List<TokenData>();
                    if (chosenClassLabel == 0 && tokenData.Class0Count == 1)
                    {
                        tokenDataOfChosenClassLabel.Add(tokenData);
                    }
                    else if (chosenClassLabel == 1 && tokenData.Class1Count == 1)
                    {
                        tokenDataOfChosenClassLabel.Add(tokenData);
                    }
                    
                }
                classLabeledDataSet.Add(sentenceOfChosenClassLabel);
            }
            return classLabeledDataSet;
        }


    }
}
