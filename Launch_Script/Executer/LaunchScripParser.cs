using Launch_Script.Executer.Commands;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Launch_Script.Executer
{
    internal static class LaunchScriptParser
    {
        private static List<IParser> PARSER = new List<IParser>() { new BrowserParser(), new CommentParser(), new LaunchParser(), new SnapParser(), new SettingsParser(), new DelayParser(),
                                                                    new EmptyParser(), new TypeParser(), new WaitParser() };   

        public static List<ICommand> parse(string file)
        {
            StreamReader sr = new StreamReader(file);

            List<ICommand> commands = new List<ICommand>();

            int lineNumber = 1;
            while (!sr.EndOfStream)
            {
                string line = sr.ReadLine();
                List<string> tokens;

                try {
                    tokens = Tokenizer.tokenize(line);
                } catch(Exception ex) {
                    throw ParserException.CREATE_MESSAGE(file, lineNumber, line, "_", ex.Message);
                }

                bool handled = false;
                foreach (IParser parser in LaunchScriptParser.PARSER)
                {
                    if(parser.cmdTokenMatches(tokens)) 
                    {
                        try
                        {
                            commands.Add(parser.parse(tokens));
                        }
                        catch (Exception ex)
                        {
                            throw ParserException.CREATE_MESSAGE(file, lineNumber, line, parser.command(), ex.Message);
                        }

                        handled = true;
                        break;
                    }
                }

                if (!handled) throw ParserException.CREATE_MESSAGE(file, lineNumber, line, "UNKNOWN", "unknown command!");

                lineNumber++;
            }

            return commands;
        }
    }

    public class ParserException : Exception
    {
        public static ParserException CREATE_MESSAGE(string file, int lineNumber, string line, string cmd, string error)
        {
            string msg = "Error in script \"" + file + "\" at line " + lineNumber.ToString() + ":\n" +
                            "Input: " + line + "\n" +
                            "Command: " + cmd + "\n" +
                            "Error: " + error + "\n";
            return new ParserException(msg);
        }

        public ParserException(string msg) : base(msg) { }
    }
}
