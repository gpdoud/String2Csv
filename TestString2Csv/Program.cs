using System;
using System.Collections.Generic;

namespace TestString2Csv {

    class Program {

        static void Main(string[] args) {
            var sc = new String2CsvLib.String2Csv();
            var config = args[0];
            sc.LoadConfig(config);
            var infile = args[1];
            sc.ParseFile2Csv(infile);
            //var lines = System.IO.File.ReadAllLines(infile);
            //var csvLines = new List<string>();
            //foreach(var line in lines) {
            //    var csv = sc.ParseLine2Csv(line);
            //    csvLines.Add(csv);
            //}
            //var outfile = infile.Substring(0, infile.Length - 3) + "csv";
            //System.IO.File.WriteAllLines(outfile, csvLines.ToArray());
        }
    }
}
