﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLP
{
    public class Vocabulary
    {
        // Write this class - it should contain a data structure
        // that identifies all the words in the vocabulary via
        // an index. You can either use an alphabetically sorted
        // list or an instance of the Dictionary class (or
        // something else...)

        public Dictionary<string, int> WordIndex { get; set; }
        private int Count = 0;
        private int IndexCount = 0;

        public Vocabulary() 
        { 
            WordIndex = new Dictionary<string, int>();
        }

        public void AddWord(string Word) 
        {
            if (!WordIndex.ContainsKey(Word))
            {
                WordIndex.Add(Word, IndexCount);
                Count++;
                IndexCount++;

            }
        }

        public string GetString(int Index)
        {
            foreach (var pair in WordIndex)
            {
                if (pair.Value == Index)
                {
                    return pair.Key;
                }
            }
            return null; 
        }

        public int GetIndex(string Word)
        {
            if (WordIndex.ContainsKey(Word))
            {
                return WordIndex[Word];
            }
            else
            {
                return -1;
            }
        }

        public int GetCount()
        { 
            return Count; 
        }
        public int GetLastIndex()
        {
            return IndexCount;
        }
    }
}
