﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLP
{
    public class Sentence
    {
        // Field. Fields are used to store data.
        private List<TokenData> tokenDataList;

        // Constructor
        public Sentence()
        {
            tokenDataList = new List<TokenData>();  
        }

        // Property. Used to expose the Field to public access. 
        public List<TokenData> TokenDataList
        {
            get { return tokenDataList; }
            set { tokenDataList = value; }
        }
    }
}
