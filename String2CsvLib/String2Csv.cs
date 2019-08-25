using System;
using System.Collections.Generic;
using System.Text;

namespace String2CsvLib {

    public class String2Csv {

        static SortedDictionary<int, InputFormat> layout;

        public string Parse2Csv(string line) {
            var fields = new List<string>();
            foreach(var key in layout.Keys) {
                var inf = layout[key];
                if(inf.Start + inf.Length > line.Length) {
                    fields.Add(string.Empty);
                    continue;
                }
                var fld = line.Substring(inf.Start, inf.Length);
                fld = Trim(fld, inf.LeftTrim, inf.RightTrim, inf.LeadZeroTrim);
                fld = Translate(fld, inf.Translations);
                fields.Add(fld);
            }
            return string.Join(',', fields);
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

        public void LoadConfig(string fullpath) {
            using(var ss = System.IO.File.OpenText(fullpath)) {
                var json = ss.ReadToEnd();
                var items = Newtonsoft.Json.JsonConvert.DeserializeObject<List<InputFormat>>(json);
                LoadDictionary(items);
            }
        }
        private void LoadDictionary(List<InputFormat> items) {
            layout = new SortedDictionary<int, InputFormat>();
            foreach(var item in items) {
                var infmt = new InputFormat() {
                    Key = item.Key,
                    Field = item.Field,
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

        public String2Csv() { }
    }
}
