using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Launch_Script.Executer.Commands
{
    public class EmptyCommand : ICommand
    {
        public EmptyCommand() { }

        public async Task execute()
        {
            return;
        }
    }

    internal class EmptyParser : IParser
    {
        public const string CMD = "";

        public override string command()
        {
            return EmptyParser.CMD;
        }

        public override bool cmdTokenMatches(List<string> tokens)
        {
            return tokens != null && tokens.Count == 0;
        }

        public override ICommand parse(List<string> tokens)
        {
            if (!this.cmdTokenMatches(tokens)) throw new ParserException("Unknown error occured [EmptyParser.parse]");

            return new EmptyCommand();
        }
    }
}
