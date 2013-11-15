using System;
using System.Collections.Generic;
using SinkBreaker.Common;

namespace SinkBreaker.UI
{
    class ConsoleUI
    {
        static void Main(string[] args)
        {
            BathroomGraph bathroom = new BathroomGraph(500, 500);

            #region ManualTests
            //string[] figs = new[] {"ninetile", "plus", "vline", "hline", "angle-ul", "angle-ur", "angle-dl", "angle-dr" };
            //DateTime start = DateTime.UtcNow;
            //Random rand = new Random();
            //for (int i = 0; i < 1000; i++)
            //{
            //    bathroom.PlaceFigure(figs[rand.Next(0, figs.Length)], "1", "1");
            //}

            //DateTime stop = DateTime.UtcNow;
            //Console.WriteLine(bathroom.ReturnOutput());
            //Console.WriteLine((stop - start).TotalSeconds);
            #endregion

            int figuresCount = int.Parse(Console.ReadLine());
            for (int figure = 0; figure < figuresCount; figure++)
            {
                string[] inputLine = Console.ReadLine().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                bathroom.PlaceFigure(inputLine[0], inputLine[1], inputLine[2]);
            }

            Console.WriteLine(bathroom.ReturnOutput());
        }
    }
}
