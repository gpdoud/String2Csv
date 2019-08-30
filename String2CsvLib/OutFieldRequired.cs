using System;
using System.Collections.Generic;
using System.Text;

namespace String2CsvLib {

    public struct Key {
        public string EasAccount { get; set; }
        public string Field { get; set; }
    }

    public class FieldRequired {

        private Dictionary<Key, bool> fieldRequired = new Dictionary<Key, bool>();

        public bool IsRequired(Key reqKey) {
            if(fieldRequired.ContainsKey(reqKey))
                return fieldRequired[reqKey];
            else
                return true;
        }
        public void Add(string eas_account, string fieldname, bool isRequired) {
            Key key = new Key { EasAccount = eas_account, Field = fieldname };
            fieldRequired.Add(key, isRequired);
        }

    }
}
