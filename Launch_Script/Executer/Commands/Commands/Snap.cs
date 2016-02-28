using Launch_Script.Tools;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Launch_Script.Executer.Commands
{
    public class SnapCommand : ICommand
    {
        private List<WindowsOperations.SnappingDirection> dirs;

        public SnapCommand(List<WindowsOperations.SnappingDirection> dirs)
        {
            this.dirs = dirs;
        }

        public async Task execute()
        {
            await WindowsOperations.SnapCurrentWindowClean(dirs);
        }
    }

    internal class SnapParser : IParser
    {
        public const string CMD = "snap";

        public override string command()
        {
            return SnapParser.CMD;
        }

        public override ICommand parse(List<string> tokens)
        {
            if (!this.cmdTokenMatches(tokens)) throw new ParserException("Unknown error occured [SnapParser.parse]");
            if (tokens.Count < 2) throw new ParserException("No snapping directions given!");

            List<WindowsOperations.SnappingDirection> dirs = new List<WindowsOperations.SnappingDirection>();

            for (int i = 1; i < tokens.Count; i++)
            {
                switch (tokens[i])
                {
                    case "left":
                        dirs.Add(WindowsOperations.SnappingDirection.LEFT);
                        break;
                    case "right":
                        dirs.Add(WindowsOperations.SnappingDirection.RIGHT);
                        break;
                    case "top":
                        dirs.Add(WindowsOperations.SnappingDirection.TOP);
                        break;
                    case "bottom":
                        dirs.Add(WindowsOperations.SnappingDirection.BOTTOM);
                        break;
                    default:
                        throw new ParserException("Invalid snapping direction");

                }
            }

            return new SnapCommand(dirs);
        }
    }
}
