using System;
using System.Collections.Generic;
using System.Text;

namespace String2CsvLib {

    public struct Config {
        public int NbrDictionaryLookupKeys;
        public List<InputFormat> InputFormats;
    }

    public struct InputFormat {
        public int Key;
        public string Field;
        public int OutOrder;
        public int Start;
        public int Length;
        public int LeftTrim;
        public int RightTrim;
        public bool LeadZeroTrim;
        public List<Translate> Translations;
    }
    public struct Translate {
        public char From;
        public char To;
    }
}
