using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Launch_Script.Executer.Commands
{
    public class LaunchCommand : ICommand
    {
        private string url;
        private string domain;
        private string arguments;

        public LaunchCommand(string url, string arguments, string domain)
        {
            this.url = url;
            this.domain = domain;
            this.arguments = arguments;
        }

        public async Task execute()
        {
            Process proc = new Process();
            proc.StartInfo.FileName = this.url;
            proc.StartInfo.UseShellExecute = true;
            proc.StartInfo.WorkingDirectory = this.domain;
            proc.StartInfo.Arguments = this.arguments;
            try
            {
                proc.Start();

                //while (!proc.Responding) await Task.Delay(50);
            }
            catch (Exception ex)
            {
                throw new CommandExecutionException(ex.Message);
            }

            await Task.Delay(300);
        }
    }

    internal class LaunchParser : IParser {
        public const string CMD = "launch";

        public override string command()
        {
            return LaunchParser.CMD;
        }

        public override ICommand parse(List<string> tokens)
        {
            if (!this.cmdTokenMatches(tokens)) throw new ParserException("Unknown error occured [LaunchParser.parse]");
            if (tokens.Count < 2 || tokens.Count > 4) throw new ParserException("Invalid number of arguments!");
            if (String.IsNullOrEmpty(tokens[1])) throw new ParserException("Invalid URL!");
            if (tokens.Count >= 3 && tokens[2] == null) throw new ParserException("Invalid shell arguments!");
            if (tokens.Count == 4 && tokens[3] == null) throw new ParserException("Invalid shell working directory");

            string url = tokens[1];
            string args = (tokens.Count >= 3 ? tokens[2] : "");
            string domain = (tokens.Count == 4 ? tokens[3] : "");

            return new LaunchCommand(url, args, domain);
            
        }
    }
}
