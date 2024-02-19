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
            progressListBox.Items.Add("Total tokenized training set reviews: " + tokenizedTrainingSet.Count);
            progressListBox.Items.Add("Total training set tokens: " + counterForTokens);



           
        }


        

    }
}
