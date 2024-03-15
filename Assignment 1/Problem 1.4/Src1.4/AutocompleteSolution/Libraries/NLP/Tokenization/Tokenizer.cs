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
        private void TokenizeAndUpdateTokens(string word, List<string> tokenList)
        {
            // Tokenize the word recursively for the cases where checks at the end and beginning are made.
            List<string> tokens = new List<string>();
            
            List<string> unfinishedItem = Tokenize(tokens);
            tokenList.AddRange(unfinishedItem);
        }

        public List<string> Tokenize(List<string> listOfStrings)
        {
            List<string> tokenList = new List<string>();
            List<string> abbreviations = new List<string>{"i.e.", "e.g.", "etc.", "et al.", "vs.", "a.m.", "p.m.", "e.g.", "i.a.", "cf.", "etc.", "esp.", "i.e.", "e.g.", "ex.", "no.", "op.", "cit.", "vol.", "v.", "p.", "pp.", "dr.", "mrs.", "mr.", "ph.d.", "st.", "ave.", "rd.", "blvd.", "apt.", "fig.", "al.", "b.c.", "a.d.", "p.o.", "p.s.", "inc.", "ltd.", "co.", "est.", "jan.", "feb.", "mar.", "apr.", "jun.", "jul.", "mon.", "tue.", "wed.", "thu.", "fri.", "sat.", "sun.", "aug.", "sep.", "oct.", "nov.", "dec."
            };

            // Check if the list of strings is empty
            if (listOfStrings == null || listOfStrings.Count == 1)
            {
                return tokenList; // Return an empty token list
            }

            foreach (string word in listOfStrings)
            {
                // Checks for abbreviations
                if (abbreviations.Contains(word.ToLower()))
                {
                    tokenList.Add(word.ToLower());
                }
                // Checks for punctuations at the end of the word
                else if (word.EndsWith(".") || word.EndsWith("_") || word.EndsWith("“") || word.EndsWith("¨") || word.EndsWith("%") || word.EndsWith("&") || word.EndsWith(",") || word.EndsWith("#") || word.EndsWith("?") || word.EndsWith("*") || word.EndsWith("!") || word.EndsWith("<") || word.EndsWith(">") || word.EndsWith("+") || word.EndsWith("-") || word.EndsWith("'") || word.EndsWith(")") || word.EndsWith("\"") || word.EndsWith("$") || word.EndsWith("£") || word.EndsWith("€"))
                {
                    string wordOnly = word.Substring(0, word.Length - 1);
                    TokenizeAndUpdateTokens(wordOnly, tokenList);
                    string punctuationOnly = word.Substring(word.Length - 1);
                    tokenList.Add(punctuationOnly.ToLower());
                }
                // Checks for punctuations at the beginning of the word
                else if (word.StartsWith("(") || word.StartsWith("_") || word.StartsWith("“") || word.StartsWith("¨") || word.StartsWith(".") || word.StartsWith(",") || word.StartsWith("%") || word.StartsWith("&") || word.StartsWith("?") || word.StartsWith("#") || word.StartsWith("<") || word.StartsWith(">") || word.StartsWith("+") || word.StartsWith("-") || word.StartsWith("=") || word.StartsWith("'") || word.StartsWith("\"") || word.StartsWith("$") || word.StartsWith("£") || word.StartsWith("€") || word.StartsWith("[") || word.StartsWith("~"))
                {
                    string punctuationOnly = word.Substring(0, 1);
                    tokenList.Add(punctuationOnly.ToLower());
                    string wordOnly = word.Substring(1, word.Length - 1);
                    TokenizeAndUpdateTokens(wordOnly, tokenList);
                }
                // Checks for decimal numbers
                else if (Regex.IsMatch(word, @"\d+\.\d+"))
                {
                    tokenList.Add(word);
                }
                else
                {
                    tokenList.Add(word.ToLower());
                }
            }
            return tokenList;
        }
    }
}
