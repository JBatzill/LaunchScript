using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Launch_Script.Executer.Commands
{
    public interface ICommand
    {
        Task execute();
    }

    public class CommandExecutionException : Exception
    {
        public CommandExecutionException(string msg) : base(msg) { }
    }
}
