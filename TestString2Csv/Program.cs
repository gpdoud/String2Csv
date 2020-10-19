using String2CsvLib;
using System;
using System.Collections.Generic;

namespace TestString2Csv {

    class Program {

        static void Run(string[] args) { 
            var sc = new String2CsvLib.String2Csv();
            var config = args[0];
            sc.LoadConfig(config);
            LoadFieldRequiredDictionary(sc);
            var infile = args[1];
            sc.ParseFile2Csv(infile);
        }
        static void LoadFieldRequiredDictionary(String2CsvLib.String2Csv sc) {
            //sc.FieldRequired.Add("5130045", "company_code", false);
            //sc.FieldRequired.Add("1154015", "company_code", false);
            //sc.FieldRequired.Add("5130050", "company_code", false);
            //sc.FieldRequired.Add("5130045", "tran_date_yyyy", false);
            //sc.FieldRequired.Add("1154015", "tran_date_yyyy", false);
            //sc.FieldRequired.Add("5130050", "tran_date_yyyy", false);
        }
        static void Main(string[] args) {
            Run(args);
        }
    }
}
