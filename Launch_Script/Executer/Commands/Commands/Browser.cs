using Launch_Script.Tools;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Launch_Script.Executer.Commands
{
    public class BrowserCommand : ICommand
    {
        private string url;
        private bool newWindow;

        public BrowserCommand(string url, bool newWindow)
        {
            this.url = url;
            this.newWindow = newWindow;
        }

        public async Task execute()
        {
            Process proc = new Process();
            proc.StartInfo.FileName = WindowsOperations.GetSystemDefaultBrowser();
            if(this.newWindow) proc.StartInfo.Arguments = "--new-window ";
            proc.StartInfo.Arguments += url;
            proc.StartInfo.UseShellExecute = true;
            try
            {
                proc.Start();
            }
            catch (Exception ex)
            {
                throw new CommandExecutionException(ex.Message);
            }

            await Task.Delay(300);
        }
    }

    internal class BrowserParser : IParser
    {
        public const string CMD = "browser";
        public const string CMD_NEW_WINDOW = "new";

        public override string command()
        {
            return BrowserParser.CMD;
        }

        public override ICommand parse(List<string> tokens)
        {
            if (!this.cmdTokenMatches(tokens)) throw new ParserException("Unknown error occured [BrowserParser.parse]");

            string url = "";
            bool newWindow = false;


            if (tokens.Count == 1)
            {
                newWindow = true;
            }
            else if (tokens.Count == 2)
            {
                url = String.IsNullOrEmpty(tokens[1]) ? "" : tokens[1];
            } 
            else if(tokens.Count == 3)
            {
                //3 tokens only correct if second token is new window token
                if (String.IsNullOrEmpty(tokens[1]) || !tokens[1].Equals(BrowserParser.CMD_NEW_WINDOW)) throw new ParserException("Invalid arguments!");

                newWindow = true;
                url = String.IsNullOrEmpty(tokens[2]) ? "" : tokens[2];
            } 
            else if (tokens.Count > 3) throw new ParserException("Invalid number of arguments!");

            return new BrowserCommand(url, newWindow);

        }
    }
}
