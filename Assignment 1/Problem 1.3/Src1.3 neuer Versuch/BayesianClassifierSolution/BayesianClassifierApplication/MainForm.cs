using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO; // For the Path class; see LoadDataSet()
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NLP;
using NLP.TextClassification;
using NLP.Tokenization;

namespace BayesianClassifierApplication
{
    public partial class MainForm : Form
    {
        private const string TEXT_FILE_FILTER = "Text files (*.txt)|*.txt";

        private TextClassificationDataSet trainingSet = null;
        private TextClassificationDataSet testSet = null;
        private List<Sentence> tokenizedTrainingSet = null;
        private List<Sentence> tokenizedTestSet = null;
        private Dictionary<string, TokenData> bagOfWords = null;
        private NaiveBayesianTextClassifier naiveBayesianTextClassifier = null;
        


        public MainForm()
        {
            InitializeComponent();
            progressListBox.Font = new Font("Consolas", 11);
        }

        private TextClassificationDataSet LoadDataSet()
        {
            TextClassificationDataSet dataSet = null;
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = TEXT_FILE_FILTER;
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    dataSet = new TextClassificationDataSet();
                    StreamReader dataReader = new StreamReader(openFileDialog.FileName);
                    while (!dataReader.EndOfStream)
                    {
                        string line = dataReader.ReadLine();
                        List<string> lineSplit = line.Split(new char[] { '\t' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                        TextClassificationDataItem item = new TextClassificationDataItem();
                        item.Text = lineSplit[0].ToLower();
                        item.ClassLabel = int.Parse(lineSplit[1]);
                        dataSet.ItemList.Add(item);
                    }
                    dataReader.Close();
                    int count0 = dataSet.ItemList.Count(i => i.ClassLabel == 0);
                    int count1 = dataSet.ItemList.Count(i => i.ClassLabel == 1);
                    string fileName = Path.GetFileName(openFileDialog.FileName); // File name without the file path.
                    progressListBox.Items.Add("Loaded data file \"" + fileName + "\" with " + count0.ToString() +
                        " negative reviews and " + count1.ToString() + " positive reviews.");
                }
            }
            return dataSet;
        }

        private void loadTrainingSetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            trainingSet = LoadDataSet();
            if ((trainingSet != null) && (testSet != null)) { tokenizeButton.Enabled = true; }
            loadTrainingSetToolStripMenuItem.Enabled = false; // To avoid accidentally reloading the training set instead of the validation set...
        }

        private void loadTestSetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            testSet = LoadDataSet();
            if ((trainingSet != null) && (testSet != null)) { tokenizeButton.Enabled = true; }
            loadTestSetToolStripMenuItem.Enabled = false;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void tokenizeButton_Click(object sender, EventArgs e)
        {
            // Tokenize trainingSet
            List<Sentence> tempTokenizedTrainingSet = new List<Sentence>();
            Tokenizer tokenizer = new Tokenizer();

            int counterForTokens = 0;
            foreach (TextClassificationDataItem review in trainingSet.ItemList)
            {
                List<TokenData> tokenizedReview = tokenizer.Tokenize(review);

                Sentence sentence = new Sentence();
                foreach (TokenData tokenData in tokenizedReview)
                {
                    sentence.TokenDataList.Add(tokenData);
                    counterForTokens++;
                }

                tempTokenizedTrainingSet.Add(sentence);
            }

            tokenizedTrainingSet = tempTokenizedTrainingSet;
            progressListBox.Items.Clear();
            progressListBox.Items.Add("Total tokenized trainingSet reviews: " + tokenizedTrainingSet.Count);
            progressListBox.Items.Add("Total trainingSet tokens: " + counterForTokens);

            

            

            // Tokenize testSet
            List<Sentence> tempTokenizedTestSet = new List<Sentence>();

            int counterForTokensTest = 0;
            foreach (TextClassificationDataItem review in testSet.ItemList)
            {
                List<TokenData> tokenizedReview = tokenizer.Tokenize(review);

                Sentence sentence = new Sentence();
                foreach (TokenData tokenData in tokenizedReview)
                {
                    sentence.TokenDataList.Add(tokenData);
                    counterForTokensTest++;
                }

                tempTokenizedTestSet.Add(sentence);
            }

            tokenizedTestSet = tempTokenizedTestSet;
            progressListBox.Items.Add("Total tokenized testSet reviews: " + tokenizedTestSet.Count);
            progressListBox.Items.Add("Total testSet tokens: " + counterForTokensTest);
        }

        private void initializeClassifierButton_Click(object sender, EventArgs e)
        {
            NaiveBayesianTextClassifier initNaiveBayesianTextClassifier = new NaiveBayesianTextClassifier();
            Dictionary<string, TokenData> initBagOfWords = initNaiveBayesianTextClassifier.Initialize(tokenizedTrainingSet);

            naiveBayesianTextClassifier = initNaiveBayesianTextClassifier;
            bagOfWords = initBagOfWords;

            progressListBox.Items.Clear();
            progressListBox.Items.Add("The bag of words consists of " + bagOfWords.Count + " unique tokens.");

        }

        private void RunClassifierButton_Click(object sender, EventArgs e)
        {
            double count0 = trainingSet.ItemList.Count(i => i.ClassLabel == 0);
            double count1 = trainingSet.ItemList.Count(i => i.ClassLabel == 1);
            double class0LabelProb = count0/(count0+count1);
            double class1LabelProb = count1/(count0+count1);

            double class0Denominator1 = bagOfWords.Count;
            double class1Denominator1 = bagOfWords.Count;

            foreach (KeyValuePair<string, TokenData> kvp in bagOfWords)
            {
                TokenData tokenData = kvp.Value;
                class0Denominator1 += tokenData.Class0Count;
                class1Denominator1 += tokenData.Class1Count;
                
            }

            double class0Denominator = class0Denominator1;
            double class1Denominator = class1Denominator1;
        
            int correctPrediction = 0;
            int tempIterator = 0;
            foreach (Sentence review in tokenizedTestSet)
            {
                int predictedClassLabel = naiveBayesianTextClassifier.Classify(review, bagOfWords, class0Denominator, class1Denominator, class0LabelProb,class1LabelProb);
                int actualClassLabel = testSet.ItemList[tempIterator].ClassLabel; // Ugly, but made it work
                tempIterator++;
                if (predictedClassLabel == actualClassLabel)
                {
                    correctPrediction++;
                }
            }

            double totalPredictions = tokenizedTestSet.Count;
            double Accuracy = (double) correctPrediction/ totalPredictions;

            progressListBox.Items.Clear();
            progressListBox.Items.Add($"The accuracy of the naive bayesian text classifier is {Accuracy}");
        }

        private void runAnalysisForBButton_Click(object sender, EventArgs e)
        {
            progressListBox.Items.Clear ();
            double count0 = trainingSet.ItemList.Count(i => i.ClassLabel == 0);
            double count1 = trainingSet.ItemList.Count(i => i.ClassLabel == 1);
            // P(label)
            double probabilityLabel0 = count0 / (count0 + count1);
            double probabilityLabel1 = count1 / (count0 + count1);

            // Count total
            double totalInstancesOfLabel0 = 0;
            double totalInstancesOfLabel1 = 0;

            foreach (KeyValuePair<string, TokenData> kvp in bagOfWords)
            {
                TokenData tokenData = kvp.Value;
                totalInstancesOfLabel0 += tokenData.Class0Count;
                totalInstancesOfLabel1 += tokenData.Class1Count;
            }

            
            string[] wordsToCheck = "friendly,perfectly,horrible,poor".Split(',');
            
            // P(label|word) = P(word|label)P(label)/P(word)
            foreach (string word in wordsToCheck)
            {
                
                double instancesOfWordWithLabel0 = bagOfWords[word].Class0Count;
                double instancesOfWordWithLabel1 = bagOfWords[word].Class1Count;

                // P(word|label)
                double probabilityOfWordGivenLabel0 = (instancesOfWordWithLabel0) / (totalInstancesOfLabel0 );
                double probabilityOfWordGivenLabel1 = (instancesOfWordWithLabel1) / (totalInstancesOfLabel1 );

                // P(word) 
                double totalCountOfWord = instancesOfWordWithLabel0 + instancesOfWordWithLabel1;
                double totalCountOfTokens = totalInstancesOfLabel0 + totalInstancesOfLabel1;
                double probabilityOfWord = (totalCountOfWord) / totalCountOfTokens;
                Console.WriteLine($"{probabilityOfWord}");

                // P(label0|word) 
                double probabilityLabel0GivenWord = (probabilityOfWordGivenLabel0 * probabilityLabel0) / probabilityOfWord;
                double probabilityLabel1GivenWord = (probabilityOfWordGivenLabel1 * probabilityLabel1) / probabilityOfWord;
                double probabilitySum = probabilityLabel0GivenWord + probabilityLabel1GivenWord;
                
                
                
                probabilityLabel0GivenWord *= 1 / probabilitySum;
                probabilityLabel1GivenWord *= 1 / probabilitySum;
                

                progressListBox.Items.Add($"P(negative|{word}) = {probabilityLabel0GivenWord}");
                progressListBox.Items.Add($"P(positive|{word}) = {probabilityLabel1GivenWord}");
                progressListBox.Items.Add(" ");
                
                
            }
        }

        private void RunAnalysisCButton_Click(object sender, EventArgs e)
        {
            double count0 = trainingSet.ItemList.Count(i => i.ClassLabel == 0);
            double count1 = trainingSet.ItemList.Count(i => i.ClassLabel == 1);
            double class0LabelProb = count0 / (count0 + count1);
            double class1LabelProb = count1 / (count0 + count1);

            double class0Denominator = bagOfWords.Count;
            double class1Denominator = bagOfWords.Count;

            foreach (KeyValuePair<string, TokenData> kvp in bagOfWords)
            {
                TokenData tokenData = kvp.Value;
                class0Denominator += tokenData.Class0Count;
                class1Denominator += tokenData.Class1Count;
            }

            int truePositiveTraining = 0;
            int falsePositiveTraining = 0;
            int trueNegativeTraining = 0;
            int falseNegativeTraining = 0;
            

            int tempIterator = 0;
            foreach (Sentence review in tokenizedTrainingSet)
            {
                int predictedClassLabel = naiveBayesianTextClassifier.Classify(review, bagOfWords, class0Denominator, class1Denominator, class0LabelProb, class1LabelProb);
                int actualClassLabel = trainingSet.ItemList[tempIterator].ClassLabel; // Ugly, but made it work
                tempIterator++;
                if (predictedClassLabel == actualClassLabel && actualClassLabel == 1)
                {
                    truePositiveTraining++;
                }
                else if (predictedClassLabel == actualClassLabel && actualClassLabel == 0) 
                {
                    trueNegativeTraining++;   
                }
                else if (predictedClassLabel != actualClassLabel && actualClassLabel == 1)
                {
                    falseNegativeTraining++;
                }
                else if (predictedClassLabel != actualClassLabel && actualClassLabel == 0)
                {
                    falsePositiveTraining++;
                }
            }

            double precision = (double)(truePositiveTraining) / (truePositiveTraining + falsePositiveTraining);
            double recall = (double)(truePositiveTraining) / (truePositiveTraining + falseNegativeTraining);
            double accuracy = (double)(truePositiveTraining+trueNegativeTraining) / (truePositiveTraining + falsePositiveTraining+trueNegativeTraining+falseNegativeTraining);
            double fOne = (double)(2 * precision * recall)/(precision + recall);

            progressListBox.Items.Clear();

            string title = "Analysis on the trainingSet";
            progressListBox.Items.Add(title);

            string[] metrics = { "Precision", "Recall", "Accuracy", "F1 Score" };
            double[] values = { precision, recall, accuracy, fOne };

            for (int i = 0; i < metrics.Length; i++)
            {
                string output = string.Format("{0,-10}: {1:F4}", metrics[i], values[i]);
                progressListBox.Items.Add(output);
            }

            // Now for testSet

            double countTest0 = testSet.ItemList.Count(i => i.ClassLabel == 0);
            double countTest1 = testSet.ItemList.Count(i => i.ClassLabel == 1);
            double testClass0LabelProb = countTest0 / (countTest0 + countTest1);
            double testClass1LabelProb = countTest1 / (countTest0 + countTest1);

            double testClass0Denominator = bagOfWords.Count;
            double testClass1Denominator = bagOfWords.Count;

            foreach (KeyValuePair<string, TokenData> kvp in bagOfWords)
            {
                TokenData tokenData = kvp.Value;
                testClass0Denominator += tokenData.Class0Count;
                testClass1Denominator += tokenData.Class1Count;
            }

            int truePositiveTest = 0;
            int falsePositiveTest = 0;
            int trueNegativeTest = 0;
            int falseNegativeTest = 0;


            int tempIteratorTest = 0;
            foreach (Sentence review in tokenizedTestSet)
            {
                int predictedClassLabel = naiveBayesianTextClassifier.Classify(review, bagOfWords, testClass0Denominator, testClass1Denominator, testClass0LabelProb, testClass1LabelProb);
                int actualClassLabel = testSet.ItemList[tempIteratorTest].ClassLabel; // Ugly, but made it work
                tempIteratorTest++;
                if (predictedClassLabel == actualClassLabel && actualClassLabel == 1)
                {
                    truePositiveTest++;
                }
                else if (predictedClassLabel == actualClassLabel && actualClassLabel == 0)
                {
                    trueNegativeTest++;
                }
                else if (predictedClassLabel != actualClassLabel && actualClassLabel == 1)
                {
                    falseNegativeTest++;
                }
                else if (predictedClassLabel != actualClassLabel && actualClassLabel == 0)
                {
                    falsePositiveTest++;
                }
            }

            double precisionTest = (double)(truePositiveTest) / (truePositiveTest + falsePositiveTest);
            double recallTest = (double)(truePositiveTest) / (truePositiveTest + falseNegativeTest);
            double accuracyTest = (double)(truePositiveTest + trueNegativeTest) / (truePositiveTest + falsePositiveTest + trueNegativeTest + falseNegativeTest);
            double fOneTest = (double)(2 * precisionTest * recallTest) / (precisionTest + recallTest);

            progressListBox.Items.Add("");
            progressListBox.Items.Add("");


            string titleTest = "Analysis on the testSet";
            progressListBox.Items.Add(titleTest);

            string[] metricsTest = { "Precision", "Recall", "Accuracy", "F1 Score" };
            double[] valuesTest = { precisionTest, recallTest, accuracyTest, fOneTest };

            for (int i = 0; i < metricsTest.Length; i++)
            {
                string outputTest = string.Format("{0,-10}: {1:F4}", metricsTest[i], valuesTest[i]);
                progressListBox.Items.Add(outputTest);
            }

        }
    }
}
