using System;
using System.Collections.Generic;
using System.Text;

namespace String2CsvLib {

    public class String2Csv {

        static Dictionary<int, InputFormat> layout;

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
                fields.Add(fld);
            }
            return string.Join(',', fields);
        }
        private string Trim(string field, int LeftTrim, int RightTrim, bool LeadZeroTrim) {
            var fld = field;
            if(RightTrim > 0)
                fld = fld.Substring(0, fld.Length - RightTrim);
            if(LeftTrim > 0)
                fld = fld.Substring(LeftTrim);
            if(LeadZeroTrim)
                fld = fld.TrimStart('0');
            return fld.Trim();
        }

        public void LoadConfig(string fullpath) {
            using(var ss = System.IO.File.OpenText(fullpath)) {
                var json = ss.ReadToEnd();
                var items = Newtonsoft.Json.JsonConvert.DeserializeObject<List<InputFormat>>(json);
                LoadDictionary(items);
            }
        }
        private void LoadDictionary(List<InputFormat> items) {
            layout = new Dictionary<int, InputFormat>();
            foreach(var item in items) {
                var infmt = new InputFormat() {
                    Key = item.Key,
                    Field = item.Field,
                    Start = item.Start,
                    Length = item.Length,
                    LeftTrim = item.LeftTrim,
                    RightTrim = item.RightTrim,
                    LeadZeroTrim = item.LeadZeroTrim
                };
                layout.Add(item.Key, infmt);
            }
        }

        public String2Csv() { }
    }
}
