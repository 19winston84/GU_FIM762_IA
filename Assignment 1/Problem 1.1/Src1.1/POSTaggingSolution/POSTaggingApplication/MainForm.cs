using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NLP;
using NLP.POS;
using NLP.POS.Taggers;

namespace POSTaggingApplication
{
    public partial class MainForm : Form
        
    {
        
        // Field
        private const string TEXT_FILE_FILTER = "Text files (*.txt)|*.txt";
        private POSDataSet completeDataSet = null;
        private POSDataSet trainingDataSet = null;
        private POSDataSet testDataSet = null;
        private List<TokenData> vocabulary = null;
        private ConversionInstructions completeConversionData = null;
        private UnigramTagger unigramTagger = null;

        public MainForm()
        {
            InitializeComponent();
            resultsListBox.Font = new Font("Consolas", 11);
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void loadPOSCorpusToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = TEXT_FILE_FILTER;
                int tokenCount = 0;
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    StreamReader fileReader = new StreamReader(openFileDialog.FileName);
                    completeDataSet = new POSDataSet();
                    while (!fileReader.EndOfStream)
                    {
                        string line = fileReader.ReadLine();
                        if (line != "")
                        {
                            List<string> lineSplit = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                            List<TokenData> tokenDataList = new List<TokenData>();
                            Sentence sentence = new Sentence();
                            foreach (string lineSplitItem in lineSplit)
                            {
                                List<string> spellingAndTag = lineSplitItem.Split(new char[] { '_' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                                Token token = new Token();
                                if (spellingAndTag.Count == 2) // Needed in order to ignore the very last line that just contains "_.".
                                {
                                    token.Spelling = spellingAndTag[0].ToLower().Trim(); // Convert all words to lowercase.
                                    token.POSTag = spellingAndTag[1].Trim();
                                }
                                
                                TokenData tokenData = new TokenData(token);
                                if (token.POSTag.Length == 1 || token.POSTag[1] != '|') // A somewhat ugly fix, needed to remove some junk from the data ...
                                {
                                    tokenDataList.Add(tokenData);
                                    tokenCount++;
                                }
                            }
                            sentence.TokenDataList = tokenDataList;
                            completeDataSet.SentenceList.Add(sentence);
                        }
                    }
                    fileReader.Close();
                    resultsListBox.Items.Add("Loaded the Brown corpus with " + completeDataSet.SentenceList.Count.ToString()
                        + " sentences and " + tokenCount.ToString() + " tokens.");

                }
            }
        }

        private List<TokenData> GenerateVocabulary(POSDataSet dataSet)
        {
            List<TokenData> tmpTokenDataList = new List<TokenData>();
            foreach (Sentence sentence in dataSet.SentenceList)
            {
                foreach (TokenData tokenData in sentence.TokenDataList)
                {
                    tmpTokenDataList.Add(tokenData);
                }
            }
            // Sort in alphabetical order, then by tag (also in alphabetical order)...
            // This takes a few seconds to run: It would have been more elegant (and easy) to put the
            // computation in a separate thread, but I didn't bother to do that here, as it would make
            // the code slightly more complex. Here, it is OK that the application freezes for a few
            // seconds while it is sorting the data.
            tmpTokenDataList = tmpTokenDataList.OrderBy(t => t.Token.Spelling).ThenBy(t => t.Token.POSTag).ToList();
            // ... then merge
            List<TokenData> tokenDataList = MergeTokens(tmpTokenDataList);
            return tokenDataList;
        }

        private List<TokenData> MergeTokens(List<TokenData> unmergedDataSet)
        {
            List<TokenData> mergedDataSet = new List<TokenData>();
            if (unmergedDataSet.Count > 0)
            {
                int index = 0;
                Token currentToken = new Token();
                currentToken.Spelling = unmergedDataSet[index].Token.Spelling;
                currentToken.POSTag = unmergedDataSet[index].Token.POSTag;
                TokenData currentTokenData = new TokenData(currentToken);
                index++;
                while (index < unmergedDataSet.Count)
                {
                    Token nextToken = unmergedDataSet[index].Token;
                    if ((nextToken.Spelling == currentToken.Spelling) && (nextToken.POSTag == currentToken.POSTag))
                    {
                        currentTokenData.Count += 1;
                    }
                    else
                    {
                        mergedDataSet.Add(currentTokenData);
                        currentToken = new Token();
                        currentToken.Spelling = unmergedDataSet[index].Token.Spelling;
                        currentToken.POSTag = unmergedDataSet[index].Token.POSTag;
                        currentTokenData = new TokenData(currentToken);
                    }
                    index++;
                }
                mergedDataSet.Add(currentTokenData); // Add the final element as well ...
            }
            return mergedDataSet;
        }


        private void loadTagConversionDataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = TEXT_FILE_FILTER;
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {

                    // reads the whole universalTag file 
                    StreamReader fileReader = new StreamReader(openFileDialog.FileName);
                    ConversionInstructions completeTagConversionData = new ConversionInstructions();
                    while (!fileReader.EndOfStream)
                    {
                        // creating a string of the current line.
                        string line = fileReader.ReadLine();
                        // Process data here ...
                        if (line != "")
                        { 
                            List<string> oldTagNewTag = line.Split(new char[] { '\t' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                            TagConversionPair tagConversionPair = new TagConversionPair();
                            if (oldTagNewTag.Count == 2) // Needed in order to ignore the very last line that just contains "_.".
                            {
                                tagConversionPair.OldTag = oldTagNewTag[0].Trim(); 
                                tagConversionPair.NewTag = oldTagNewTag[1].Trim();
                            }
                            completeTagConversionData.TagConversionList.Add(tagConversionPair);
                        }
                    }
                    fileReader.Close();
                    resultsListBox.Items.Add("Loaded the Brown To Universal Tag Map and created " + completeTagConversionData.TagConversionList.Count.ToString() + " conversion pairs.");
                    completeConversionData = completeTagConversionData;
                }
                
            }

            // Keep these lines: They will activate the conversion button, provided that the
            // Brown data set has been loaded first:
            if (completeDataSet != null)
            {
                if (completeDataSet.SentenceList.Count > 0)
                {
                    convertPOSTagsButton.Enabled = true;
                }
            }
        }

        private void convertPOSTagsButton_Click(object sender, EventArgs e)
        {
            // Write code here, such that the Brown tags are mapped to the 
            // Universal tags (for the complete data set), using the representation described above 
            // After running this method, all the tokens should be assigned
            // one of the 12 Universal tags.
            // 
            // Method call: 
            // completeDataSet.ConvertPOSTags(... <suitable input, namely the tag conversion data> ...); // this you have to write ...
            
            if (completeConversionData == null || !completeConversionData.TagConversionList.Any())
            {
                resultsListBox.Items.Add("Tag conversion data has not been loaded.");
                return; // Stop further execution
            }

            // Proceed with conversion
            completeDataSet.ConvertPOSTags(completeConversionData);
            // Continue with the rest of the method...
            resultsListBox.Items.Add("The Brown tags are now converted to 12 universal tags.");


            // Next, build the vocabulary, using the 12 universal tags (this method you get for free! :) )
            vocabulary = GenerateVocabulary(completeDataSet);

            // Keep this line: It will activate the split button.
            splitDataSetButton.Enabled = true;
        }

        private void splitDataSetButton_Click(object sender, EventArgs e)
        {
            double splitFraction;
            Boolean splitFractionOK = double.TryParse(splitFractionTextBox.Text, out splitFraction);
            if (splitFractionOK && splitFraction > 0 && splitFraction <= 1)
            {
                
                var (trainingData, testData) = POSDataSet.Split(completeDataSet, splitFraction);
                
                // Activate buttons as before
                generateStatisticsButton.Enabled = true;
                generateUnigramTaggerButton.Enabled = true;

                trainingDataSet = trainingData;
                testDataSet = testData;

                resultsListBox.Items.Add("The complete data set was successfully split 80/20.");
            }
            else
            {
                resultsListBox.Items.Add("Incorrectly specified split fraction");
            }
            

        }

        private void generateStatisticsButton_Click(object sender, EventArgs e)
        {
            resultsListBox.Items.Clear(); // Clears the listBox before adding new information.

            // Initialize a dictionary to count instances of each POS tag
            Dictionary<string, int> posTagCounts = new Dictionary<string, int>();
            // Initialize a dictionary to map each word to its set of unique POS tags
            Dictionary<string, HashSet<string>> wordToTags = new Dictionary<string, HashSet<string>>();
            int totalCount = 0;

            foreach (Sentence sentence in trainingDataSet.Sentences)
            {
                totalCount += sentence.TokenDataList.Count;
                foreach (TokenData tokenData in sentence.TokenDataList)
                {
                    string word = tokenData.Token.Spelling.ToLower(); // Normalize word to lowercase
                    string posTag = tokenData.Token.POSTag;

                    // Update posTagCounts
                    if (posTagCounts.ContainsKey(posTag))
                    {
                        posTagCounts[posTag]++;
                    }
                    else
                    {
                        posTagCounts.Add(posTag, 1);
                    }

                    // Update wordToTags
                    if (wordToTags.ContainsKey(word))
                    {
                        wordToTags[word].Add(posTag);
                    }
                    else
                    {
                        wordToTags[word] = new HashSet<string> { posTag };
                    }
                }
            }

            // Count how many words are associated with 1, 2, 3, ... different POS tags
            Dictionary<int, int> tagsToWordCount = new Dictionary<int, int>();
            foreach (var entry in wordToTags)
            {
                int numberOfTags = entry.Value.Count;
                if (tagsToWordCount.ContainsKey(numberOfTags))
                {
                    tagsToWordCount[numberOfTags]++;
                }
                else
                {
                    tagsToWordCount.Add(numberOfTags, 1);
                }
            }

            // Determine the maximum length for alignment
            int maxPosTagLength = posTagCounts.Keys.Any() ? posTagCounts.Keys.Max(tag => tag.Length) : 0;
            int maxCountLength = posTagCounts.Values.Any() ? posTagCounts.Values.Max().ToString().Length : 0;

            // Display total count summary with alignment
            string totalCountLine = $"Total tokens:".PadRight(maxPosTagLength + maxCountLength + 4) + $"{totalCount}";
            resultsListBox.Items.Add(totalCountLine);

            // Display the counts and proportions for each POS tag, aligned
            foreach (var posTagCount in posTagCounts)
            {
                double proportion = (double)posTagCount.Value / totalCount;
                string posTagPadded = posTagCount.Key.PadRight(maxPosTagLength);
                string countPadded = posTagCount.Value.ToString().PadRight(maxCountLength);
                resultsListBox.Items.Add($"{posTagPadded}: {countPadded} ({proportion:P2})");
            }

            // New Section: Display the fraction of words associated with different numbers of POS tags
            resultsListBox.Items.Add(string.Empty); // Add an empty line for better readability
            resultsListBox.Items.Add("Words by number of associated POS tags:");
            foreach (var tagCountEntry in tagsToWordCount.OrderBy(tagCount => tagCount.Key))
            {
                double fraction = (double)tagCountEntry.Value / wordToTags.Count;
                resultsListBox.Items.Add($"{tagCountEntry.Key} tags: {tagCountEntry.Value} words ({fraction:P2})");
            }

            // Initialize a list to store words with 6 tokens
            List<string> wordsWithSixTokens = new List<string>();

            // Iterate over the wordToTags dictionary
            foreach (var entry in wordToTags)
            {
                // Check if the set of POS tags for the word contains 6 elements
                if (entry.Value.Count == 6)
                {
                    // Add the word to the list
                    wordsWithSixTokens.Add(entry.Key);
                }
            }

            // Output the list of words with 6 tokens
            foreach (string word in wordsWithSixTokens)
            {
                resultsListBox.Items.Add($"The word with six tokens is {word}");
            }

        }

        private void generateUnigramTaggerButton_Click(object sender, EventArgs e)
        {
            var unigramTaggerBaby = new UnigramTagger();
            unigramTaggerBaby.Train(trainingDataSet);

            // Assuming you have a way to store the unigramTagger for later use
            // For example, if you have a field in your class to store it:
            // this.unigramTagger = unigramTagger;

            resultsListBox.Items.Clear();
            resultsListBox.Items.Add("Unigram tagger has been generated and trained.");

            // Keep this line: It will activate the evaluation button for the unigram tagger
            runUnigramTaggerButton.Enabled = true;

            // Write code here for generating a unigram tagger, again using the *training* set;
            // Here, you *should* Define a class Unigram tagger derived from (inheriting) the base class
            // POSTagger in the NLP library included in this solution.

            // For the actual tagging (once the unigram tagger has been generated)
            // you must override the Tag() method in the base class (POSTagger)).

            // Note that, for most POS taggers, it matters whether or not a word is
            // (say) the first word or the last word of a sentence, but not for the
            // unigram tagger, so it is easy to write the Tag() method - it need not
            // take into account the position of the word in the sentence.

            // Keep this line: It will activate the evaluation button for the unigram tagger
            runUnigramTaggerButton.Enabled = true;
            unigramTagger = unigramTaggerBaby;
        }

        private void runUnigramTaggerButton_Click(object sender, EventArgs e)
        {
            resultsListBox.Items.Clear(); // Clear the list box before displaying new results

            if (testDataSet == null || unigramTagger == null)
            {
                resultsListBox.Items.Add("Test data set or Unigram tagger is not initialized.");
            }

            int correctPredictions = 0;
            int totalPredictions = 0;
            foreach (var sentence in testDataSet.Sentences)
            {
                
                List<string> predictedTags = unigramTagger.Tag(sentence);
                for (int i = 0; i < sentence.TokenDataList.Count; i++)
                {
                    totalPredictions++;

                    // Check if the predicted tag matches the actual tag
                    if (predictedTags[i] == sentence.TokenDataList[i].Token.POSTag)
                    {
                        correctPredictions++;
                    }
                }

                
            }

            if (totalPredictions > 0)
            {
                // Calculate accuracy
                double accuracy = (double)correctPredictions / totalPredictions;

                // Display accuracy
                resultsListBox.Items.Add($"The unigram accuracy is {accuracy:P2}");
            }
            else
            {
                resultsListBox.Items.Add("No predictions were made.");
            }
        }


    }
}
