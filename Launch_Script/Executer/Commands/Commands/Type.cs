using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Launch_Script.Executer.Commands
{
    public class TypeCommand : ICommand
    {
        private List<string> keys;

        public TypeCommand(List<string> keys)
        {
            this.keys = keys;
        }

        public async Task execute()
        {
            foreach (string key in keys)
            {
                await Task.Run(() => System.Windows.Forms.SendKeys.SendWait(key));
            }
        }
    }

    internal class TypeParser : IParser
    {
        public const string CMD = "type";

        public override string command()
        {
            return TypeParser.CMD;
        }

        public override ICommand parse(List<string> tokens)
        {
            if (!this.cmdTokenMatches(tokens)) throw new ParserException("Unknown error occured [TypeParser.parse]");
            if (tokens.Count < 2) throw new ParserException("Invalid number of arguments!");

            List<string> keys = new List<string>();

            for (int i = 1; i < tokens.Count; i++)
            {
                if (String.IsNullOrEmpty(tokens[i])) throw new ParserException("No empty keys allowed!");
                keys.Add(tokens[i]);
            }

            return new TypeCommand(keys);
        }
    }
}
