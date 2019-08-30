# String2CsvLib
Project to extract fields from a fixed format string or a file of fixed strings

## JSON configuration file format

The configuration file requires a definition for each field that is to be extracted from each input line

| Parameter    | Type  | Required | Description |
| ---          | ---   | ---      | ---         |
| Key          | int   | Yes      | Defines unique identifier for fields in input file |
| Field        | str   | No       | Description of field (not used at runtime) |
| OutOrder     | int   | Yes      | Defines the order that fields will be in CSV file
| Start        | int   | Yes      | Starting position of field in input |
| Length       | int   | Yes      | Number of chars of the input |
| LeftTrim     | int   | No       | Remove the first _n_ chars from the field |
| RightTrim    | int   | No       | Remove the last _n_ chars from the field |
| LeadZeroTrim | bool  | No       | Remove leading zeros from field (like number fields) |
| Translations | array | No       | Collection of from/to char substitutions |
|   From       | str   | No       | String in field to be replaced |
|   To         | str   | No       | String to replace 'From' value |

Example configuration file

```
[
  {
    "key": 1,
    "field": "code",
    "outorder": 2,
    "start": 0,
    "length": 5,
    "lefttrim": 0,
    "righttrim": 0,
    "leadzerotrim": false
  },
  {
    "key": 2,
    "field": "account",
    "outorder": 1,
    "start": 18,
    "length": 10,
    "lefttrim": 3,
    "righttrim": 0,
    "leadzerotrim": false
  },
  {
    "key": 3,
    "field": "amount",
    "outorder": 4,
    "start": 106,
    "length": 15,
    "lefttrim": 0,
    "righttrim": 0,
    "leadzerotrim": true
  },
  {
    "key": 4,
    "field": "operator",
    "outorder": 3,
    "start": 121,
    "length": 1,
    "lefttrim": 0,
    "righttrim": 0,
    "leadzerotrim": false,
    "translations": [
      { "from": "+", "to": "Plus" },
      { "from": "-", "to": "Minus" }
    ]
  }
]
```

Explaination of config

Key: 1

This is a basic configuration wherea substring is extracted from each line and leading or trailing blanks are removed. The OutOrder of 2 means this will be the second field in the output file.

Key: 2

This basic configuration includes Key 1 plus the LeftTrim means 3 characters will be trimmed from the beginning of the value. This can occus when something like an account number has some fixed number of leading zeros. The OutOrder of 1 means this will be the first field in the output file.

Key: 3

This configuration includes the LeadZeroTrim item that strips all the leading zeros from the beginning of a field. This can be used to remove leading zeros that are not needed in a currency field. The OutOrder of 4 means this will be the fourth field in the output file.

Key: 4

This configuration includes Translations which will substitute a string for every occurrance of another string. Translations are an array of individual translations. In the example, the translation will change a plus sign (+) to the letter 'D' and a minus sign (-) to the letter 'C'. The OutOrder of 3 means this will be the third field in the output file.

## Field Required Dictionary

The system includes a dictionary that can be loaded such that any individual field can be excluded from the output based on one or more other field values. This dictionary is meant to be loaded by a class that can get this information from another source.

The first step is to get access to the FieldRequired property which is a class that allows loading and retrieving from the dictionary. To load the dictionary, call the Add method and pass the Key fields made up of the eas_account number and the particular fields name (from the Field value in the config.json file). The third parameter is a boolean indicating whether the field should (true) or should not (false) be included in the output.

### Example

The following code will load the FieldRequired dictionary so that the company_code field is not put in the output if the eas_number is 5130045.

    var sc = new String2Csv();
    sc.FieldRequired.Add("5130045", "company_code", false);