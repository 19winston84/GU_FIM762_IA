using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLP
{
    public class TokenData
    {
        public Token Token { get; set; }    
        public int Count { get; set; }  
        public int Class0Count { get; set; }    
        public int Class1Count { get; set; }    

        public TokenData(Token token)
        {
            Token = token;
            Count = 1;
        }
    }
}
