using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace String2CsvLib {

    public class String2Csv {

        Config config;
        SortedDictionary<int, InputFormat> layout = null;
        FieldRequired _FieldRequired = new FieldRequired();

        // provide ability to load from another class
        public FieldRequired FieldRequired { get { return _FieldRequired; } }

        /// <summary>
        /// Parses an file of fixed-format text lines into a file of 
        /// CSV lines based on the config.json file. If not OutFilePath
        /// is provided, the InFilePath is used with an extension of csv
        /// </summary>
        /// <param name="InFilePath">File of fixed-format text lines</param>
        /// <param name="OutFilePath">File of csv lines</param>
        public void ParseFile2Csv(string InFilePath, string OutFilePath = null) {
            // if dictionary is not loaded, throw exception
            if(layout == null) throw new Exception("Configuration not loaded. Call 'LoadConfig()'.");
            var inFile = InFilePath;
            // set outfile to infile plus 'CSV' if not provided
            var outFile = OutFilePath ?? InFilePath.Substring(0, InFilePath.Length - 3) + "csv";
            // check that infile exists or throw exception
            if(!System.IO.File.Exists(inFile)) throw new FileNotFoundException(InFilePath);
            // get all the lines from the infile
            var lines = System.IO.File.ReadAllLines(inFile);
            // collection for csv
            var csvList = new List<string>(lines.Length);
            // add the parsed csv lines to collection
            foreach(var line in lines) {
                csvList.Add(ParseLine2Csv(line));
            }
            // write csv lines to outfile
            System.IO.File.WriteAllLines(outFile, csvList.ToArray());
        }
        /// <summary>
        /// Parses a string into a csv string based on the config.json.
        /// If the line is too short, remaining fields return an empty string.
        /// </summary>
        /// <param name="line">A string</param>
        /// <returns>A string with fields comma separated</returns>
        public string ParseLine2Csv(string line) {
            // collection to hold individual fields
            var fields = new SortedDictionary<int, string>();
            // create a dictionary keyed by the field name with
            // the value being the outOrder field. this so that
            // the field can be blanked out if it is not required
            var eas_account = string.Empty;
            // iterate through the field descriptions layout dictionary 
            // returning keys in sorted order.
            // each key points field description on what to extract from
            // the line and how to massage the data.
            foreach(var key in layout.Keys) {
                var inf = layout[key];
                // if the line is too short, remaining fields return empty string
                if(inf.Start + inf.Length > line.Length) {
                    fields.Add(inf.OutOrder, string.Empty);
                    continue;
                }
                // extract the field
                var fld = line.Substring(inf.Start, inf.Length);
                // trim the field of blanks. optionally trim the leading and/or 
                // trailing characters including zeros for numbers
                fld = Trim(fld, inf.LeftTrim, inf.RightTrim, inf.LeadZeroTrim);
                // optionally perform multiple string replacement from x to y
                fld = Translate(fld, inf.Translations);
                // if this field is the eas_account, save it
                if(inf.Field.Equals("eas_account")) {
                    eas_account = fld;
                }
                // check if this field is required
                if(!FieldRequired.IsRequired(new Key { EasAccount = eas_account, Field = inf.Field })) {
                    fld = string.Empty;
                }
                // add to collection
                fields.Add(inf.OutOrder, fld);
            }
            // the entire line has been extracted. Check whether some fields
            // are not required and should be changed to a empty string
            
            // return the collection as a string of comma separated fields 
            return string.Join(',', fields.Values);
        }
        private string Trim(string field, int LeftTrim, int RightTrim, bool LeadZeroTrim) {
            var fld = field.Trim();
            if(RightTrim > 0)
                fld = fld.Substring(0, fld.Length - RightTrim);
            if(LeftTrim > 0)
                fld = fld.Substring(LeftTrim);
            if(LeadZeroTrim)
                fld = fld.TrimStart('0');
            return fld;
        }
        private string Translate(string field, List<Translate> translations) {
            if(translations == null)
                return field;
            var fld = field;
            foreach(var trans in translations) {
                fld = fld.Replace(trans.From, trans.To);
            }
            return fld;
        }
        /// <summary>
        /// Loads the json configuration file into the layout sorted dictionary.
        /// </summary>
        /// <param name="fullpath">A file of fields specs in json format.</param>
        public void LoadConfig(string fullpath) {
            using(var ss = System.IO.File.OpenText(fullpath)) {
                // read the contents of the json config file
                var json = ss.ReadToEnd();
                // turn the text into a json object with a collection of field definitions
                var config = Newtonsoft.Json.JsonConvert.DeserializeObject<Config>(json);
                // load the field definitions into the layout sorted dictionary
                LoadDictionary(config);
            }
        }
        /// <summary>
        /// Loads a field spec into the sorted dictionary
        /// </summary>
        /// <param name="items">A field spec in json format.</param>
        private void LoadDictionary(Config cfg) {
            config = new Config();
            config.NbrDictionaryLookupKeys = cfg.NbrDictionaryLookupKeys;
            config.InputFormats = cfg.InputFormats;
            layout = new SortedDictionary<int, InputFormat>();
            foreach(var item in config.InputFormats) {
                var infmt = new InputFormat() {
                    Key = item.Key,
                    Field = item.Field,
                    OutOrder = item.OutOrder,
                    Start = item.Start,
                    Length = item.Length,
                    LeftTrim = item.LeftTrim,
                    RightTrim = item.RightTrim,
                    LeadZeroTrim = item.LeadZeroTrim,
                    Translations = item.Translations
                };
                layout.Add(item.Key, infmt);
            }
        }

        public String2Csv() {

        }
    }
}
