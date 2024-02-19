﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using NLP;
using NLP.TextClassification;
using NLP.Tokenization;

namespace PerceptronClassifierApplication
{
    public partial class MainForm : Form
    {
        // set a filter for allowed input 
        private const string TEXT_FILE_FILTER = "Text files (*.txt)|*.txt";

        private PerceptronClassifier classifier = null;
        private Vocabulary vocabulary = null;
        private TextClassificationDataSet trainingSet = null;
        private TextClassificationDataSet validationSet = null;
        private TextClassificationDataSet testSet = null;
        private List<Sentence> tokenizedTrainingSet = null;
        private List<Sentence> tokenizedValidationSet = null;
        private List<Sentence> tokenizedTestSet = null;
        private PerceptronOptimizer perceptronOptimizer = null;
        private PerceptronEvaluator trainingEvaluator = null;
        private PerceptronEvaluator validationEvaluator = null;
        private PerceptronEvaluator testEvaluator = null;
        private PerceptronClassifier perceptronClassifier = null;
        private Thread computationThread;
        private double previousValidationAccuracy = 0;
        private List<double> trainedWeights = null;

        public MainForm()
        {
            InitializeComponent();
            progressListBox.Font = new Font("Consolas", 11);

        }

        // This is a method to read in a data set. Is used for loading trainig, validation and test set.
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
                        item.Text = lineSplit[0].ToLower(); // This is the review text in lowercase spelling
                        item.ClassLabel = int.Parse(lineSplit[1]);  // This is the value 0 or 1
                        dataSet.ItemList.Add(item);
                        
                    }
                    dataReader.Close();
                    // Count0 and Count1 only here tracking amount of negative and positive reviews.
                    int count0 = dataSet.ItemList.Count(i => i.ClassLabel == 0);
                    int count1 = dataSet.ItemList.Count(i => i.ClassLabel == 1);
                    string fileName = Path.GetFileName(openFileDialog.FileName); // File name without the file path.
                    
                    progressListBox.Items.Add("Loaded data file \"" + fileName + "\" with " + count0.ToString() +
                        " negative reviews and " + count1.ToString() + " positive reviews.");
                }
            }
            return dataSet; 
        }

        // Nomen est omen. Using the LoadDataSet() method from above.
        private void loadTrainingSetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            trainingSet = LoadDataSet();
            if ((trainingSet != null) && (validationSet != null) && (testSet != null)) { tokenizeButton.Enabled = true; }
            loadTrainingSetToolStripMenuItem.Enabled = false; // To avoid accidentally reloading the training set instead of the validation set...
            
        }

        // Nomen est omen. Using the LoadDataSet() method from above.
        private void loadValidationSetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            validationSet = LoadDataSet();
            if ((trainingSet != null) && (validationSet != null) && (testSet != null)) { tokenizeButton.Enabled = true; }
            loadValidationSetToolStripMenuItem.Enabled = false;
        }

        // Nomen est omen. Using the LoadDataSet() method from above.
        private void loadTestSetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            testSet = LoadDataSet();
            if ((trainingSet != null) && (validationSet != null) && (testSet != null)) { tokenizeButton.Enabled = true; }
            loadTestSetToolStripMenuItem.Enabled = false;
        }

        // Nomen est omen. Exit button in menu.
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }


        private void GenerateVocabulary(List<Sentence> onlyTrainingDataSet)
        {
            // Write a method that generates the vocabulary. Note that this
            // should ONLY be done for the training set!
            
            List<string> allWords = new List<string>();
            foreach (Sentence sentence in onlyTrainingDataSet) 
            {
                foreach (TokenData tokenData in sentence.TokenDataList)
                {
                    allWords.Add(tokenData.Token.Spelling);
                }
            }

            allWords.Sort();

            Vocabulary tempVocabulary = new Vocabulary();
            foreach (string word in allWords) 
            { 
                tempVocabulary.AddWord(word);
            }

            vocabulary = tempVocabulary;
            progressListBox.Items.Add("The vocabulary is generated on the trainingSet and contains " + vocabulary.GetCount() + " tokens.");

            
            // You must generate an instance of the Vocabulary class,
            // which you must also implement (a skeleton is available
            // in the NLP library)
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
            progressListBox.Items.Add("Total tokenized training set reviews: " + tokenizedTrainingSet.Count);
            progressListBox.Items.Add("Total training set tokens: " + counterForTokens);

            

            indexButton.Enabled = true;
            // Write code here for tokenizing the text. That is,
            // implement the Tokenize() method in the Tokenizer class.


            // First tokenize the training set:



            // Then build the vocabulary from the training set:
            GenerateVocabulary(tokenizedTrainingSet);

            // Next, tokenize the validation set:
            List<Sentence> tempTokenizedValidationSet = new List<Sentence>();

            int counterForTokensValidation = 0;
            foreach (TextClassificationDataItem review in validationSet.ItemList)
            {
                List<TokenData> tokenizedReview = tokenizer.Tokenize(review);

                Sentence sentence = new Sentence();
                foreach (TokenData tokenData in tokenizedReview)
                {
                    sentence.TokenDataList.Add(tokenData);
                    counterForTokensValidation++;
                }

                tempTokenizedValidationSet.Add(sentence);
            }

            tokenizedValidationSet = tempTokenizedValidationSet;
            progressListBox.Items.Add("Total tokenized validation set reviews: " + tokenizedValidationSet.Count);
            progressListBox.Items.Add("Total validation set tokens: " + counterForTokensValidation);

            // Add code here ..

            // Finally, tokenize the test set:

            // Add code here:
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
            progressListBox.Items.Add("Total tokenized test set reviews: " + tokenizedTestSet.Count);
            progressListBox.Items.Add("Total test set tokens: " + counterForTokensTest);

            //
        }

        private void indexButton_Click(object sender, EventArgs e)
        {
            // Write code here for indexing the data sets to generate

            // Index the training set, using the *training* set vocabulary (see above)

            int trainingCounter = 0;
            foreach (Sentence sentence in tokenizedTrainingSet)
            {
                List<int> sentenceAsVocabularyIndexes = new List<int>();
                foreach (TokenData tokenData in sentence.TokenDataList)
                {
                    string word = tokenData.Token.Spelling;
                       
                    int vocabularyIndex = vocabulary.GetIndex(word);
                    tokenData.SetTheIndex(vocabularyIndex);
                    sentenceAsVocabularyIndexes.Add(vocabularyIndex);
                }
                trainingSet.ItemList[trainingCounter].ReviewAsVocabularyIndexes = sentenceAsVocabularyIndexes;
                trainingCounter++;
            }

            
            // Index the validation set, using the *training* set vocabulary (see above)
            // Assign index = -1 if a given word does not exist in the training set.

            int validationCounter = 0;
            foreach (Sentence sentence in tokenizedValidationSet)
            {
                List<int> sentenceAsVocabularyIndexes = new List<int>();
                foreach (TokenData tokenData in sentence.TokenDataList)
                {
                    string word = tokenData.Token.Spelling;

                    int vocabularyIndex = vocabulary.GetIndex(word);
                    tokenData.SetTheIndex(vocabularyIndex);
                    sentenceAsVocabularyIndexes.Add(vocabularyIndex);
                }
                validationSet.ItemList[validationCounter].ReviewAsVocabularyIndexes = sentenceAsVocabularyIndexes;
                validationCounter++;
            }

            // Index the test set, using the *training* set vocabulary (see above)
            // Assign index = -1 if a given word does not exist in the training set.
            int testCounter = 0;
            foreach (Sentence sentence in tokenizedTestSet)
            {
                List<int> sentenceAsVocabularyIndexes = new List<int>();
                foreach (TokenData tokenData in sentence.TokenDataList)
                {
                    string word = tokenData.Token.Spelling;

                    int vocabularyIndex = vocabulary.GetIndex(word);
                    tokenData.SetTheIndex(vocabularyIndex);
                    sentenceAsVocabularyIndexes.Add(vocabularyIndex);
                }
                testSet.ItemList[testCounter].ReviewAsVocabularyIndexes = sentenceAsVocabularyIndexes;
                testCounter++;
            }
            
            progressListBox.Items.Clear();
            progressListBox.Items.Add("The Indexing is finished now.");
            initializeOptimizerButton.Enabled = true;
        }

        private void initializeOptimizerButton_Click(object sender, EventArgs e)
        {
            PerceptronClassifier classifier = new PerceptronClassifier();
            PerceptronOptimizer optimizer = new PerceptronOptimizer(trainingSet, vocabulary);
            PerceptronEvaluator trainEvaluator = new PerceptronEvaluator(trainingSet, vocabulary);
            PerceptronEvaluator valiEvaluator = new PerceptronEvaluator(validationSet, vocabulary);
            PerceptronEvaluator tesEvaluator = new PerceptronEvaluator(testSet, vocabulary);

            perceptronClassifier = classifier;
            perceptronOptimizer = optimizer;
            trainingEvaluator = trainEvaluator;
            validationEvaluator = valiEvaluator;
            testEvaluator = tesEvaluator;

            
            int numberOfFeatures = vocabulary.GetLastIndex(); 
            progressListBox.Items.Clear();
            progressListBox.Items.Add("The number of features is: " +  numberOfFeatures + "The last index of vocabulary is " + vocabulary.GetLastIndex());
            
            perceptronClassifier.Initialize(numberOfFeatures);
            perceptronOptimizer.Weights = perceptronClassifier.WeightList;

            
            progressListBox.Items.Add("The perceptron optimizer is initialized and ready to go.");





            // Write code here for initializing a perceptron optimizer, which
            // you must also write (i.e. a class called PerceptronOptimizer).
            // Moreover, as mentioned in the assignment text,
            // it might be a good idea to define an evaluator class (e.g. PerceptronEvaluator)
            // You should place both classes in the TextClassification folder in the NLP library.

            startOptimizerButton.Enabled = true;
        }



        // Flag to signal stop the optimization
        private volatile bool shouldStop = false; 
        
        private void ShowProgress(string progressInformation)
        {
            progressListBox.Items.Add(progressInformation);
        }
        private void startOptimizerButton_Click(object sender, EventArgs e)
        {
            startOptimizerButton.Enabled = false;
            progressListBox.Items.Clear();
            progressListBox.Items.Add("Starting");

            computationThread = new Thread(new ThreadStart(() => ComputationLoop()));
            computationThread.Start();

            stopOptimizerButton.Enabled = true; // Enable the stop button

        }
        private void ComputationLoop()
        {
            int firstSave = 1;
            int epoch = 0;
            while (!shouldStop) // Continue until stop signal is received
            {
                perceptronOptimizer.Optimize();
                trainingEvaluator.Weights = perceptronOptimizer.Weights;
                trainingEvaluator.Evaluate();

                validationEvaluator.Weights = perceptronOptimizer.Weights;
                validationEvaluator.Evaluate();

                if (epoch % 10 == 0)
                {
                    
                    string trainingAccuracyFormatted = trainingEvaluator.Accuracy.ToString("F6") + "%";
                    string validationAccuracyFormatted = validationEvaluator.Accuracy.ToString("F6") + "%";

                    string progressInformation = $"Epoch {epoch}: " +
                                                  $"Training Set Accuracy: {trainingAccuracyFormatted} | " +
                                                  $"Validation Set Accuracy: {validationAccuracyFormatted}";

                    ThreadSafeShowProgress(progressInformation);
                }
                epoch++;

                
                if (epoch > firstSave) 
                { 
                    if (previousValidationAccuracy < validationEvaluator.Accuracy)
                    {
                        previousValidationAccuracy = validationEvaluator.Accuracy;
                        trainedWeights = perceptronOptimizer.Weights;
                    }
                }
            }
            ThreadSafeHandleDone();
        }

        //ComputationLoop() with data saver
        /*private void ComputationLoop()
        {
            int firstSave = 1;
            int epoch = 0;
            string csvFilePath = "/Users/fredriktsitje/Documents/Github/GU_FIM762_IA/Assignment 1/Problem 1.2/accuracyData.csv"; // Path to your CSV file

            // Create or append to the CSV file
            using (StreamWriter sw = File.AppendText(csvFilePath))
            {
                while (!shouldStop) // Continue until stop signal is received
                {
                    perceptronOptimizer.Optimize();
                    trainingEvaluator.Weights = perceptronOptimizer.Weights;
                    trainingEvaluator.Evaluate();

                    validationEvaluator.Weights = perceptronOptimizer.Weights;
                    validationEvaluator.Evaluate();

                    testEvaluator.Weights = perceptronOptimizer.Weights;
                    testEvaluator.Evaluate();

                    // Write accuracy values to the CSV file
                    string accuracyData = $"{epoch},{trainingEvaluator.Accuracy},{validationEvaluator.Accuracy}, {testEvaluator.Accuracy}";
                    sw.WriteLine(accuracyData);

                    if (epoch % 10 == 0)
                    {
                        string trainingAccuracyFormatted = trainingEvaluator.Accuracy.ToString("F6") + "%";
                        string validationAccuracyFormatted = validationEvaluator.Accuracy.ToString("F6") + "%";

                        string progressInformation = $"Epoch {epoch}: " +
                                                      $"Training Set Accuracy: {trainingAccuracyFormatted} | " +
                                                      $"Validation Set Accuracy: {validationAccuracyFormatted}";

                        ThreadSafeShowProgress(progressInformation);
                    }
                    epoch++;

                    if (epoch > firstSave)
                    {
                        if (previousValidationAccuracy < validationEvaluator.Accuracy)
                        {
                            previousValidationAccuracy = validationEvaluator.Accuracy;
                            trainedWeights = perceptronOptimizer.Weights;
                        }
                    }
                }
            }

            ThreadSafeHandleDone();
        }*/

        private void ThreadSafeHandleDone()
        {
            if (InvokeRequired)
            {
                BeginInvoke(new MethodInvoker(() => { HandleDone(); }));
            }
            else
            {
                HandleDone();
            }
        }
        private void ThreadSafeShowProgress( string progressInformation)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new MethodInvoker(() => { ShowProgress(progressInformation); }));
            }
            else
            { 
               ShowProgress(progressInformation);
            }
        }
        private void HandleDone()
        {
            startOptimizerButton.Enabled = true; // Enable start button
            stopOptimizerButton.Enabled = false; // Disable stop button
            progressListBox.Items.Add("Optimization stopped.");
        }
        private void stopOptimizerButton_Click(object sender, EventArgs e)
        {
            stopOptimizerButton.Enabled = false;
            shouldStop = true; // Signal stop to computation loop

            testEvaluator.Weights = trainedWeights;
            testEvaluator.Evaluate();
            // Stop the optimizer here.
            string testAccuracyFormatted = testEvaluator.Accuracy.ToString("F2") + "%";
            string progressInformation = $"Test Set Accuracy: {testAccuracyFormatted} ";
            progressListBox.Items.Add(progressInformation);


            // For simplicity (even though one may perhaps resume the optimizer), at this
            // point, evaluate the best classifier (= best validation performance) over
            // the *test* set, and print the accuracy to the screen (in a thread-safe
            // manner, and with proper (clear) formatting).

            stopOptimizerButton.Enabled = true; // A bit ugly, should wait for the
            // optimizer to actually stop, but that's OK, it will stop quickly.
        }
    }
}