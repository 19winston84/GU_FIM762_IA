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
using NLP.NGrams;
using NLP.POS;
using NLP.POS.Taggers;
using NLP.TextClassification;
using NLP.Tokenization;

namespace AutocompleteApplication
{
    public partial class MainForm : Form
    {
        private const string TEXT_FILE_FILTER = "Text files (*.txt)|*.txt";
        private POSDataSet completeDataSet = null;
        private List<Sentence> tokenizedCompleteDataSet = null;
        private Dictionary<string, NGram> unigramDictionary = null;
        private Dictionary<string, NGram> bigramDictionary = null;
        private Dictionary<string, NGram> trigramDictionary = null;

        public MainForm()
        {
            InitializeComponent();
            progressListBox.Font = new Font("Consolas", 12);
        }

        // Write this method (and, of course, all other relevant methods, placed in
        // appropriately named classes, placed in a suitable folder.
        //
        // Note: To add a folder, right-click on the project in the Solution Explorer,
        // (e.g., NLP), then select Add - New Folder. 
        // Do NOT add folders externally (outside Visual Studio).
        //
        // Here, no class labels are needed. Instead you simply
        // need to read text and then tokenize it, after which you can generate
        // the n-grams (for n = 1,2, and 3).
        private void loadDataSetToolStripMenuItem_Click(object sender, EventArgs e)
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
                        if (!string.IsNullOrWhiteSpace(line))
                        {
                            List<string> lineSplit = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                            List<string> tokenList = new List<string>();
                            Sentence sentence = new Sentence();

                            foreach (string token in lineSplit)
                            {
                                tokenList.Add(token);
                                tokenCount++;
                            }
                            sentence.TokenDataList = tokenList;
                            completeDataSet.SentenceList.Add(sentence);
                        }
                    }
                    fileReader.Close();
                    progressListBox.Items.Add("Loaded the text data with " + completeDataSet.SentenceList.Count.ToString()
                        + " sentences and " + tokenCount.ToString() + " tokens.");
                }
            }
        }


        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void TokenizeButton_Click(object sender, EventArgs e)
        {
            progressListBox.Items.Clear();
            List<Sentence> tempTokenizedCompleteDataSet = new List<Sentence>();
            Tokenizer tokenizer = new Tokenizer();

            int counterForTokens = 0;
            foreach (Sentence review in completeDataSet.SentenceList)
            {
                List<string> tokenizedReview = tokenizer.Tokenize(review.TokenDataList);

                Sentence sentence = new Sentence();
                foreach (string token in tokenizedReview)
                {
                    sentence.TokenDataList.Add(token);
                    counterForTokens++;
                }

                tempTokenizedCompleteDataSet.Add(sentence);
            }

            tokenizedCompleteDataSet = tempTokenizedCompleteDataSet;
            progressListBox.Items.Add("Total tokenized training set reviews: " + tokenizedCompleteDataSet.Count);
            progressListBox.Items.Add("Total training set tokens: " + counterForTokens);
            progressListBox.Items.Add("");
        }

        private void GenerateUnigramsButton_Click(object sender, EventArgs e)
        {
            Dictionary<string, NGram> UnigramDictionary = new Dictionary<string, NGram>();

            foreach (Sentence review in tokenizedCompleteDataSet)
            {
                foreach (string token in review.TokenDataList)
                {
                    NGram unigram = new NGram(token);

                    if (UnigramDictionary.ContainsKey(token))
                    {
                        UnigramDictionary[token].Frequency++;
                    }
                    else
                    {
                        unigram.Frequency++;
                        UnigramDictionary.Add(token, unigram);

                    }
                }
            }

            foreach (KeyValuePair<string, NGram> entry in UnigramDictionary)
            {
                NGram unigram = entry.Value;
                int frequency = unigram.Frequency;
                unigram.FrequencyPerMillionInstances = (double)frequency / (1000000);
            }

            UnigramDictionary = UnigramDictionary.OrderBy(x => x.Key).ToDictionary(x => x.Key, x => x.Value);
            unigramDictionary = UnigramDictionary;
            progressListBox.Items.Add($"There are {unigramDictionary.Count} unique unigrams in the alphabetically sorted dictionary.");
        }

        private void GenerateBigramsButton_Click(object sender, EventArgs e)
        {
            Dictionary<string, NGram> BigramDictionary = new Dictionary<string, NGram>();

            foreach (Sentence review in tokenizedCompleteDataSet)
            {
                List<string> tokens = review.TokenDataList;
                for (int i = 0; i < tokens.Count - 1; i++)
                {
                    string currentToken = tokens[i];
                    string nextToken = tokens[i + 1];
                    string newToken = $"{currentToken} {nextToken}";

                    NGram bigram = new NGram(newToken);

                    if (BigramDictionary.ContainsKey(newToken))
                    {
                        BigramDictionary[newToken].Frequency++;
                    }
                    else
                    {
                        bigram.Frequency++;
                        BigramDictionary.Add(newToken, bigram);
                    }
                }
            }

            foreach (KeyValuePair<string, NGram> entry in BigramDictionary)
            {
                NGram bigram = entry.Value;
                int frequency = bigram.Frequency;
                bigram.FrequencyPerMillionInstances = (double) frequency / (1000000);
            }

            BigramDictionary = BigramDictionary.OrderBy(x => x.Key).ToDictionary(x => x.Key, x => x.Value);
            bigramDictionary = BigramDictionary;
            progressListBox.Items.Add($"There are {bigramDictionary.Count} unique bigrams in the alphabetically sorted dictionary.");
        }

        private void GenerateTrigramButton_Click(object sender, EventArgs e)
        {
            Dictionary<string, NGram> TrigramDictionary = new Dictionary<string, NGram>();

            foreach (Sentence review in tokenizedCompleteDataSet)
            {
                List<string> tokens = review.TokenDataList;
                for (int i = 0; i < tokens.Count - 2; i++)
                {
                    string currentToken = tokens[i];
                    string nextToken = tokens[i + 1];
                    string nextnextToken = tokens[i + 2];
                    string newToken = $"{currentToken} {nextToken} {nextnextToken}";

                    NGram bigram = new NGram(newToken);

                    if (TrigramDictionary.ContainsKey(newToken))
                    {
                        TrigramDictionary[newToken].Frequency++;
                    }
                    else
                    {
                        bigram.Frequency++;
                        TrigramDictionary.Add(newToken, bigram);
                    }
                }
            }

            foreach (KeyValuePair<string, NGram> entry in TrigramDictionary)
            {
                NGram trigram = entry.Value;
                int frequency = trigram.Frequency;
                trigram.FrequencyPerMillionInstances = (double)frequency / (1000000);
            }
            
            TrigramDictionary = TrigramDictionary.OrderBy(x => x.Key).ToDictionary(x => x.Key, x => x.Value);
            trigramDictionary = TrigramDictionary;
            progressListBox.Items.Add($"There are {trigramDictionary.Count} unique bigrams in the alphabetically sorted dictionary.");
        }

        private void InputBox_TextChanged(object sender, EventArgs e)
        {
            string input = InputBox.Text;
            if (!input.Contains(" "))
            {
                input = input.Trim();
                
                var options = trigramDictionary.Where(pair => pair.Key.StartsWith(".") && pair.Value.TokenList[1] == input)
                                                    .OrderByDescending(pair => pair.Value.FrequencyPerMillionInstances)
                                                    .Select(pair => pair.Value.TokenList[2])
                                                    .Take(3)
                                                    .ToList();

                OptionBox.Items.Clear();

                if (options != null)
                {
                    foreach (string option in options)
                    {
                        OptionBox.Items.Add(option);
                    }
                }
                else
                {
                    var options1 = bigramDictionary.Where(pair => pair.Key.StartsWith("."))
                                                        .OrderByDescending(pair => pair.Value.FrequencyPerMillionInstances)
                                                        .Select(pair => pair.Value.TokenList[1])
                                                        .Take(3)
                                                        .ToList();

                    OptionBox.Items.Clear();

                    foreach (string option in options1)
                    {
                        OptionBox.Items.Add(option);
                    }
                }
                   
            }
            else
            {
                string[] inputWords = input.Split(' ');
                string longerInput = $"{inputWords[inputWords.Length - 2]} {inputWords[inputWords.Length - 1]}";
                string[] splitInput = longerInput.Split(' ');
                var options = trigramDictionary.Where(pair => pair.Value.TokenList[0] == splitInput[0] && pair.Value.TokenList[1] == splitInput[1])
                                                .OrderByDescending(pair => pair.Value.FrequencyPerMillionInstances)
                                                .Select(pair => pair.Value.TokenList[2])
                                                .Take(3)
                                                .ToList();

                OptionBox.Items.Clear();

                if (options != null)
                {
                    foreach (string option in options)
                    {
                        OptionBox.Items.Add(option);
                    }
                }
                else
                {
                    var options1 = bigramDictionary.Where(pair => pair.Value.TokenList[0] == splitInput[1])
                                                    .OrderByDescending(pair => pair.Value.FrequencyPerMillionInstances)
                                                    .Select(pair => pair.Value.TokenList[1])
                                                    .Take(3)
                                                    .ToList();

                    OptionBox.Items.Clear();

                    foreach (string option in options1)
                    {
                        OptionBox.Items.Add(option);
                    }
                }
            
            }
           
        }

      
        private void OptionBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            string option = InputBox.Text;
        }

        private void InputBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Tab)
            {
                if (OptionBox.Items.Count > 0)
                {
                    string selectedOption = OptionBox.Items[0].ToString();

                    InputBox.Text += " " + selectedOption;

                    InputBox.SelectionStart = InputBox.Text.Length;
                    InputBox.SelectionLength = 0;

                    e.Handled = true;
                }
            }
        }
    }
}
