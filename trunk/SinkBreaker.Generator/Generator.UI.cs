using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SinkBreaker.Generator
{
    class GeneratorUI
    {
        static void Main(string[] args)
        {
            StringBuilder sb = new StringBuilder();
            string[] figs = new[] { "ninetile", "plus", "vline", "hline", "angle-ul", "angle-ur", "angle-dl", "angle-dr" };
            Random rand = new Random();

            sb.AppendLine("1000");

            for (int i = 0; i < 1000; i++)
            {
                sb.AppendLine(string.Format("{0} 1 1", figs[rand.Next(0, figs.Length)]));
            }

            File.WriteAllText("generatedFigures.txt", sb.ToString());
        }
    }
}
