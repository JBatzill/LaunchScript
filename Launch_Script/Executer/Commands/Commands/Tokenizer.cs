using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Launch_Script.Executer.Commands
{
    public static class Tokenizer
    {
        private struct Token
        {
            /// <summary>
            /// value of token
            /// </summary>
            public string val;
            /// <summary>
            /// start index of token
            /// </summary>
            public int start;
            /// <summary>
            /// length of token
            /// </summary>
            public int length;
            /// <summary>
            /// index of first char after token
            /// </summary>
            public int end;
            /// <summary>
            /// end of line
            /// </summary>
            public bool EOL;
        }

        public static List<String> tokenize(string line)
        {
            //trim line
            line = line.Trim();
            //if there are no tokens, return an empty list
            if(line.Length == 0) return new List<string>();

            
            //get tokens
            List<string> tokens = new List<string>();
            int cIdx = 0;
            Token t;
            do
            {
                t = getNextToken(line, cIdx);
                tokens.Add(t.val);
                cIdx = t.end;
            } while (!t.EOL);

            return tokens;
        }

        private static Token getNextToken(string line, int start)
        {
            //skip spaces
            while (start < line.Length && line[start] == ' ') start++;
            //if there are no chars left, stop tokenizing
            if (start == line.Length) throw new TokenizerException("Unknown error occured [Tokenizer.getNextToken]");

            //check if token which contains spaces or not
            if (line.StartsWith(Commands.CommentParser.CMD))
            {
                return new Token { val = line.Substring(start, Commands.CommentParser.CMD.Length), start = start, length = Commands.CommentParser.CMD.Length, end = line.Length, EOL = true };
            }
            else if (line[start] == '"')
            {
                int end = ++start;
                while (end < line.Length && (line[end] != '"' || line[end - 1] == '|')) end++;

                if (end == line.Length) throw new TokenizerException("There is no matching ending for \"");

                return new Token() { val = line.Substring(start, end - start), start = start, length = end - start, end=end+1, EOL = (end == line.Length - 1) };
            }
            else
            {
                int end = start + 1;
                while (end < line.Length && line[end] != ' ') end++;

                return new Token() { val = line.Substring(start, end - start), start = start, length = end - start, end = end,EOL = (end == line.Length) };
            }
        }
    }

    public class TokenizerException : Exception {
        public TokenizerException(string msg) : base(msg) { }
    }
}
