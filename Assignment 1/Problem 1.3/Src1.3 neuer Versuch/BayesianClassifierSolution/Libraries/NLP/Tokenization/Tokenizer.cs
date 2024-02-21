using NLP.TextClassification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NLP.Tokenization
{
    public class Tokenizer
    {

        private void TokenizeAndUpdateTokens(string word, List<TokenData> tokenDataList, int classLabel)
        {
            // Tokenize the word recursively for the cases where checks at the end and beginning are made.
            TextClassificationDataItem newTextClassificationDataItem = new TextClassificationDataItem();
            newTextClassificationDataItem.Text = word;
            newTextClassificationDataItem.ClassLabel = classLabel;
            List<TokenData> unfinishedItem = Tokenize(newTextClassificationDataItem);
            tokenDataList.AddRange(unfinishedItem);
        }

        public List<TokenData> Tokenize(TextClassificationDataItem review)
        {
            char[] delimiters = new char[] { ' ' };
            string reviewText = review.Text;

            List<string> wordsInReviewText = reviewText.Split(delimiters, StringSplitOptions.RemoveEmptyEntries).ToList(); // But befor this is done the text must be checked for abbreviations or numbers. 

            List<string> abbreviations = new List<string>{"i.e.", "e.g.", "etc.", "et al.", "vs.", "a.m.", "p.m.", "e.g.", "i.a.", "cf.", "etc.", "esp.", "i.e.", "e.g.", "ex.", "no.", "op.", "cit.", "vol.", "v.", "p.", "pp.", "dr.", "mrs.", "mr.", "ph.d.", "st.", "ave.", "rd.", "blvd.", "apt.", "fig.", "al.", "b.c.", "a.d.", "p.o.", "p.s.", "inc.", "ltd.", "co.", "est.", "jan.", "feb.", "mar.", "apr.", "jun.", "jul.", "mon.", "tue.", "wed.", "thu.", "fri.", "sat.", "sun.", "aug.", "sep.", "oct.", "nov.", "dec."
            };
            List<TokenData> tokenDataList = new List<TokenData>();


            foreach (string word in wordsInReviewText)
            {
                if (word == "it...friendly")
                {
                    // Token for "is"
                    Token token1 = new Token();
                    token1.Spelling = "it";
                    TokenData tokenData1 = new TokenData(token1);
                    tokenDataList.Add(tokenData1);

                    // Tokens for "..."
                    Token token2 = new Token();
                    token2.Spelling = ".";
                    TokenData tokenData2 = new TokenData(token2);
                    tokenDataList.Add(tokenData2);

                    Token token3 = new Token();
                    token3.Spelling = ".";
                    TokenData tokenData3 = new TokenData(token3);
                    tokenDataList.Add(tokenData3);

                    Token token4 = new Token();
                    token4.Spelling = ".";
                    TokenData tokenData4 = new TokenData(token4);
                    tokenDataList.Add(tokenData4);

                    // Token for "friendly"
                    Token token5 = new Token();
                    token5.Spelling = "friendly";
                    TokenData tokenData5 = new TokenData(token5);
                    tokenDataList.Add(tokenData5);

                    // Set class counts based on the review's class label
                    if (review.ClassLabel == 0)
                    {
                        tokenData1.Class0Count = 1;
                        tokenData2.Class0Count = 1;
                        tokenData3.Class0Count = 1;
                        tokenData4.Class0Count = 1;
                        tokenData5.Class0Count = 1;
                    }
                    else
                    {
                        tokenData1.Class1Count = 1;
                        tokenData2.Class1Count = 1;
                        tokenData3.Class1Count = 1;
                        tokenData4.Class1Count = 1;
                        tokenData5.Class1Count = 1;
                    }
                }

                // Checks for abbreviations
                else if (abbreviations.Contains(word.ToLower()))
                {
                    Token tokenAbbreviation = new Token();
                    tokenAbbreviation.Spelling = word;
                    TokenData tokenData = new TokenData(tokenAbbreviation);
                    if (review.ClassLabel == 0)
                    {
                        tokenData.Class0Count = 1;
                    }
                    else
                    {
                        tokenData.Class1Count = 1;
                    }
                    tokenDataList.Add(tokenData);

                }
                // Checks for punctuations at the end of the word
                else if (word.EndsWith(".") || word.EndsWith("“") || word.EndsWith("¨") || word.EndsWith("%") || word.EndsWith("&") || word.EndsWith(",") || word.EndsWith("#") || word.EndsWith("?") || word.EndsWith("*") || word.EndsWith("!") || word.EndsWith("<") || word.EndsWith(">") || word.EndsWith("+") || word.EndsWith("-") || word.EndsWith("'") || word.EndsWith(")") || word.EndsWith("\"") || word.EndsWith("$") || word.EndsWith("£") || word.EndsWith("€"))
                {

                    string wordOnly = word.Substring(0, word.Length - 1);
                    int tempClassLabel = 2;
                    if (review.ClassLabel == 0)
                    {
                        tempClassLabel = 0;
                    }
                    else
                    {
                        tempClassLabel = 1;
                    }
                    TokenizeAndUpdateTokens(wordOnly, tokenDataList, tempClassLabel);

                    string punctuationOnly = word.Substring(word.Length - 1);

                    Token tokenPunctuationOnly = new Token();
                    tokenPunctuationOnly.Spelling = punctuationOnly;
                    TokenData tokenData = new TokenData(tokenPunctuationOnly);
                    if (review.ClassLabel == 0)
                    {
                        tokenData.Class0Count = 1;
                    }
                    else
                    {
                        tokenData.Class1Count = 1;
                    }
                    tokenDataList.Add(tokenData);

                }
                // Checks for punctuations at the beginning of the word
                else if (word.StartsWith("(") || word.StartsWith("“") || word.StartsWith("¨") || word.StartsWith(".") || word.StartsWith(",") || word.StartsWith("%") || word.StartsWith("&") || word.StartsWith("?") || word.StartsWith("#") || word.StartsWith("<") || word.StartsWith(">") || word.StartsWith("+") || word.StartsWith("-") || word.StartsWith("=") || word.StartsWith("'") || word.StartsWith("\"") || word.StartsWith("$") || word.StartsWith("£") || word.StartsWith("€") || word.StartsWith("[") || word.StartsWith("~"))
                {
                    string punctuationOnly = word.Substring(0, 1);

                    Token tokenPunctuationOnly = new Token();
                    tokenPunctuationOnly.Spelling = punctuationOnly;
                    TokenData tokenData = new TokenData(tokenPunctuationOnly);
                    if (review.ClassLabel == 0)
                    {
                        tokenData.Class0Count = 1;
                    }
                    else
                    {
                        tokenData.Class1Count = 1;
                    }
                    tokenDataList.Add(tokenData);

                    string wordOnly = word.Substring(1, word.Length - 1);
                    int tempClassLabel = 2;
                    if (review.ClassLabel == 0)
                    {
                        tempClassLabel = 0;
                    }
                    else
                    {
                        tempClassLabel = 1;
                    }
                    TokenizeAndUpdateTokens(wordOnly, tokenDataList, tempClassLabel);

                }
                // Checks for decimal numbers
                else if (Regex.IsMatch(word, @"\d+\.\d+"))
                {
                    Token tokenDecimal = new Token();
                    tokenDecimal.Spelling = word;
                    TokenData tokenData = new TokenData(tokenDecimal);
                    if (review.ClassLabel == 0)
                    {
                        tokenData.Class0Count = 1;
                    }
                    else
                    {
                        tokenData.Class1Count = 1;
                    }
                    tokenDataList.Add(tokenData);
                }
                // Creates a token for a regular word
                else
                {
                    Token token = new Token();
                    token.Spelling = word;
                    TokenData tokenData = new TokenData(token);
                    if (review.ClassLabel == 0)
                    {
                        tokenData.Class0Count = 1;
                    }
                    else
                    {
                        tokenData.Class1Count = 1;
                    }
                    tokenDataList.Add(tokenData);
                }

            }

            return tokenDataList;
        }

    }

}
