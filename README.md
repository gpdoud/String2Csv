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

This is a basic configuration wherea substring is extracted from each line and leading or trailing blanks are removed.

Key: 2

This basic configuration includes Key 1 plus the LeftTrim means 3 characters will be trimmed from the beginning of the value. This can occus when something like an account number has some fixed number of leading zeros.

Key: 3

This configuration includes the LeadZeroTrim item that strips all the leading zeros from the beginning of a field. This can be used to remove leading zeros that are not needed in a currency field.

Key: 4

This configuration includes Translations which will substitute a string for every occurrance of another string. Translations are an array of individual translations. In the example, the translation will change a plus sign (+) to the letter 'D' and a minus sign (-) to the letter 'C'.