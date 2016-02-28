using Launch_Script.Executer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Launch_Script
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0) return;

            LaunchScriptExecuter executer = new LaunchScriptExecuter(args[0]);

            try
            {
                executer.parse();
                executer.executeAsync().Wait();
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                Console.Read();
            }

            //Console.Read();
        }
    }
}
