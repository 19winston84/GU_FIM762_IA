using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLP.TextClassification
{
    public class PerceptronEvaluator
    {
        public PerceptronClassifier perceptronClassifier = new PerceptronClassifier();
        public Vocabulary Vocabulary { get; set; }
        public TextClassificationDataSet DataSet { get; set; }
        public List<double> Weights {  get; set; }
        public double Accuracy { get; set; }
        public PerceptronEvaluator(TextClassificationDataSet dataSet, Vocabulary vocabulary) 
        {
            DataSet = dataSet;
            Vocabulary = vocabulary;
            Weights = null;
            Accuracy = 0;
        }

        public void Evaluate()
        {
            int correctCounter = 0;
            int totalCounter = 0;
            foreach (TextClassificationDataItem review in DataSet.ItemList)
            {
                totalCounter++;
                perceptronClassifier.WeightList = Weights;
                int output = perceptronClassifier.Classify(review.ReviewAsVocabularyIndexes);
                int target = review.ClassLabel;

                if (output == target)
                {
                    correctCounter++;
                }
            }
             Accuracy = (double)correctCounter / totalCounter;
        }

        public void EvaluateAndStoreExamples(string outputFile)
        {
            int correctCounter = 0;
            int totalCounter = 0;

            StringBuilder matchingExamples = new StringBuilder();
            StringBuilder mismatchingExamples = new StringBuilder();

            foreach (TextClassificationDataItem review in DataSet.ItemList)
            {
                totalCounter++;
                perceptronClassifier.WeightList = Weights;
                int output = perceptronClassifier.Classify(review.ReviewAsVocabularyIndexes);
                int target = review.ClassLabel;

                if (output == target)
                {
                    correctCounter++;
                    // Store matching example
                    if (correctCounter <= 5)
                        matchingExamples.AppendLine(review.Text);
                }
                else
                {
                    // Store mismatching example
                    if (totalCounter - correctCounter <= 5)
                        mismatchingExamples.AppendLine(review.Text);
                }
            }

            // Calculate accuracy
            Accuracy = (double)correctCounter / totalCounter;

            // Write examples to CSV
            using (StreamWriter writer = new StreamWriter(outputFile))
            {
                writer.WriteLine("Matching Examples (output == target)");
                writer.WriteLine(matchingExamples.ToString());

                writer.WriteLine("\nMismatching Examples (output != target)");
                writer.WriteLine(mismatchingExamples.ToString());
            }
        }

    }

}
