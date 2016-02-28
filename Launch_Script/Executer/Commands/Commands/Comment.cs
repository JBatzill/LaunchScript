using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Launch_Script.Executer.Commands
{
    public class Comment : ICommand
    {
        public async Task execute()
        {
            return;
        }
    }

    internal class CommentParser : IParser
    {
        public const string CMD = "#";

        public override string command()
        {
            return CommentParser.CMD;
        }

        public override ICommand parse(List<string> tokens)
        {
            if (!this.cmdTokenMatches(tokens)) throw new ParserException("Unknown error occured [CommentParser.parse]");

            return new Comment();
        }
    }
}
