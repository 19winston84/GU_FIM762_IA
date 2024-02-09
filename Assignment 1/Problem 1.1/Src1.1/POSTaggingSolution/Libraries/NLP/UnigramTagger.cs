using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLP.POS.Taggers
{
    public class UnigramTagger : POSTagger
    {
        private Dictionary<string, string> mostFrequentTags;

        public UnigramTagger()
        {
            mostFrequentTags = new Dictionary<string, string>();
        }

        public void Train(POSDataSet trainingDataSet)
        {
            var tokenTags = new Dictionary<string, Dictionary<string, int>>();

            foreach (var sentence in trainingDataSet.Sentences)
            {
                foreach (var tokenData in sentence.TokenDataList)
                {
                    if (!tokenTags.ContainsKey(tokenData.Token.Spelling))
                        tokenTags[tokenData.Token.Spelling] = new Dictionary<string, int>();

                    if (!tokenTags[tokenData.Token.Spelling].ContainsKey(tokenData.Token.POSTag))
                        tokenTags[tokenData.Token.Spelling][tokenData.Token.POSTag] = 0;

                    tokenTags[tokenData.Token.Spelling][tokenData.Token.POSTag]++;
                }
            }

            foreach (var token in tokenTags)
            {
                var mostFrequentTag = token.Value.OrderByDescending(tag => tag.Value).First();
                mostFrequentTags[token.Key] = mostFrequentTag.Key;
            }
        }

        public override List<string> Tag(Sentence sentence)
        {
            List<string> tags = new List<string>();

            foreach (TokenData tokenData in sentence.TokenDataList)
            {
                string spelling = tokenData.Token.Spelling;
                string tag = mostFrequentTags.ContainsKey(spelling) ? mostFrequentTags[spelling] : "UNK";
                tags.Add(tag);
            }

            return tags;
        }


    }
}
