using Launch_Script.Tools;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Launch_Script.Executer.Commands
{
    public class SettingsCommand : ICommand
    {
        private Action cmd;

        public SettingsCommand(Action cmd)
        {
            this.cmd = cmd;
        }

        public async Task execute()
        {
             cmd.Invoke();
        }

        #region settingMethods
        #region settingMethods.fileLink
        private const string FILE_LINK_EXT = ".ls";
        private const string FILE_LINK_APPNAME = "LaunchScript";
        internal static void _setFileLinkOn()
        {
            WindowsOperations.AssociateFileTypeIfNeeded(FILE_LINK_EXT, FILE_LINK_APPNAME);
        }

        internal static void _setFileLinkOff()
        {
            WindowsOperations.RemoveFileTypeAssociation(".ls", "LaunchScript");
        }
        #endregion
        #endregion
    }

    internal class SettingsParser : IParser
    {
        public const string CMD = "settings";
        public const string CMD_SETTINGS_FILELINK = "filelink";
        public const string CMD_SETTINGS_FILELINK_ON = "on";
        public const string CMD_SETTINGS_FILELINK_OFF = "off";
       
        public override string command()
        {
            return SettingsParser.CMD;
        }

        public override ICommand parse(List<string> tokens)
        {
            if (!this.cmdTokenMatches(tokens)) throw new ParserException("Unknown error occured [SettingsParser.parse]");
            if(tokens.Count <= 1) throw new ParserException("Missing arguments!");

            switch (tokens[1])
            {
                case CMD_SETTINGS_FILELINK:
                    return _setFileLink(tokens);
                default:
                    throw new ParserException("unknown argument");
                    return null;
            }
        }

        private ICommand _setFileLink(List<string> tokens)
        {
            if (tokens.Count <= 2) throw new ParserException("Missing value for " + CMD_SETTINGS_FILELINK + "!");
            if (tokens.Count > 3) throw new ParserException("Too many arguments for " + CMD_SETTINGS_FILELINK + "!");
            if (String.IsNullOrEmpty(tokens[2])) throw new ParserException("Invalid argument for \"" + CMD_SETTINGS_FILELINK + "\"");

            if (tokens[2].Equals(CMD_SETTINGS_FILELINK_ON))
            {
                return new SettingsCommand(SettingsCommand._setFileLinkOn);
            }
            else if (tokens[2].Equals(CMD_SETTINGS_FILELINK_OFF))
            {
                return new SettingsCommand(SettingsCommand._setFileLinkOff);
            }
            else throw new ParserException("invalid argument for \"" + CMD_SETTINGS_FILELINK + "\"");
        }
    }
}
