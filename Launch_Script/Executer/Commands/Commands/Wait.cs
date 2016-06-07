using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Launch_Script.Executer.Commands
{
    public class WaitCommand : ICommand
    {
        private Func<Task> cmd;

        public WaitCommand(Func<Task> cmd)
        {
            this.cmd = cmd;
        }

        public async Task execute()
        {
            await cmd.Invoke();
        }

        #region WaitMethods
        #region WaitMethods.Keyboard
        internal static async Task _waitForKeyboard()
        {
            Console.Write("Enter any key to continue:");
            Console.ReadKey();
        }
        #endregion
        #endregion
    }

    internal class WaitParser : IParser
    {
        public const string CMD = "wait";

        public override string command()
        {
            return WaitParser.CMD;
        }

        public override ICommand parse(List<string> tokens)
        {
            if (!this.cmdTokenMatches(tokens)) throw new ParserException("Unknown error occured [WaitParser.parse]");
            if (tokens.Count > 2) throw new ParserException("Invalid number of arguments!");
            //if (String.IsNullOrEmpty(tokens[1])) throw new ParserException("Invalid URL!");
            //if (tokens.Count >= 3 && tokens[2] == null) throw new ParserException("Invalid shell arguments!");
            //if (tokens.Count == 4 && tokens[3] == null) throw new ParserException("Invalid shell working directory");


            return new WaitCommand(WaitCommand._waitForKeyboard);

        }
    }
}
