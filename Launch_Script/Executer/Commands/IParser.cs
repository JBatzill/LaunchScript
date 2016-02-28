using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Launch_Script.Executer.Commands
{
    public abstract class IParser 
    {
        public abstract ICommand parse(List<string> tokens);

        public abstract string command();

        public virtual bool cmdTokenMatches(List<string> tokens)
        {
            return tokens.Count > 0 && !String.IsNullOrEmpty(tokens[0]) && this.command().Equals(tokens[0]);
        }
    }
}
