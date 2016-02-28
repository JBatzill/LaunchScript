using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Launch_Script.Executer.Commands
{
    public class DelayCommand : ICommand
    {
        private int ms;

        public DelayCommand(int ms)
        {
            this.ms = ms;
        }

        public async Task execute()
        {
            await Task.Delay(ms);
        }
    }

    internal class DelayParser : IParser
    {
        public const string CMD = "delay";

        public override string command()
        {
            return DelayParser.CMD;
        }

        public override ICommand parse(List<string> tokens)
        {
            if (!this.cmdTokenMatches(tokens)) throw new ParserException("Unknown error occured [DelayParser.parse]");
            if (tokens.Count < 2 || tokens.Count > 2) throw new ParserException("Invalid numer of arguments!");

            int delay;
            if (String.IsNullOrEmpty(tokens[1]) || Int32.TryParse(tokens[1], out delay) == false || delay < 0 || delay > 60000) throw new ParserException("Invalid delay time!");
            

            return new DelayCommand(delay);

        }
    }
}
