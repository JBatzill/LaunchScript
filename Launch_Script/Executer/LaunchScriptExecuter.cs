using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Launch_Script.Tools;
using Launch_Script.Executer.Commands;

namespace Launch_Script.Executer
{
    /// <summary>
    /// regestry path to app name: HKEY_CLASSES_ROOT\Extensions\ContractId\Windows.Protocol\PackageId
    /// </summary>
    public class LaunchScriptExecuter
    {
        public string file { private set; get; }
        private List<ICommand> commands;

        public LaunchScriptExecuter(string file)
        {
            this.file = file;
        }

        public void parse()
        {
            this.commands = LaunchScriptParser.parse(file);
        }

        public async Task executeAsync()
        {
            foreach(ICommand cmd in commands) 
            {
                await cmd.execute();
            }
        }
    }
}
