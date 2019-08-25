using System;
using System.Collections.Generic;
using System.Text;

namespace String2CsvLib {

    public struct InputFormat {
        public int Key;
        public string Field;
        public int Start;
        public int Length;
        public int LeftTrim;
        public int RightTrim;
        public bool LeadZeroTrim;
    }
}
