using CommandLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class Options
    {
        [Option('r', "requisicoes", Default = 1, Required = false, HelpText = "Esse parâmetro é a quantidade de requisições.")]
        public int requisicoes { get; set; }

        [Option('h', "host", Default = "localhost", Required = false, HelpText = "Esse parâmetro é o host do endpoint xyz")]
        public string host { get; set; }
    }
}
