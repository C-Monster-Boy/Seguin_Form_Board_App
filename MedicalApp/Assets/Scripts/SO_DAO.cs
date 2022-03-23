using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[CreateAssetMenu(fileName = "DAO", menuName = "SO/DAO")]
public class SO_DAO : ScriptableObject
{
    /***
     * User Input
     */
    public const string FATHERS_NAME = "fathers_name";
    public const string RANK = "rank";
    public const string UNIT = "unit";
    public const string CHILD_NAME = "child_name";
    public const string AGE = "age";
    public const string DOB = "dob";
    public const string SEX = "sex";
    public const string CLASS = "class";
    public const string MOBILE_NUMBER = "mobile_number";
    /***
     * System Generated
     */

    public const string ROUND_1_TIME = "round_1_time";
    public const string ROUND_2_TIME = "round_2_time";
    public const string ROUND_3_TIME = "round_3_time";
    public const string MIN_ROUND_TIME = "min_round_time";
    public const string MENTAL_AGE = "mental_age";
    public const string IQ = "iq";

    public Dictionary<string, string> dataMap;

    public bool AddItem(string key, string value)
    {
        if(dataMap == null) Initialize();
        
        bool isValid = Validate(key, value);
        if(isValid) dataMap[key] = value;
        
        return isValid;
    }

    public void Initialize()
    {
        dataMap = new Dictionary<string, string>
        {
            [FATHERS_NAME] = "",
            [RANK] = "",
            [UNIT] = "",
            [CHILD_NAME] = "",
            [AGE] = "",
            [DOB] = "",
            [SEX] = "",
            [CLASS] = "",
            [MOBILE_NUMBER] = "",
            [ROUND_1_TIME] = "",
            [ROUND_2_TIME] = "",
            [ROUND_3_TIME] = "",
            [MIN_ROUND_TIME] = "",
            [MENTAL_AGE] = "",
            [IQ] = ""
        };
    }

    public bool ValidateUserInput()
    {
        return ValidateAge(dataMap[AGE]) && ValidateDob(dataMap[DOB]) 
            && ValidateSex(dataMap[SEX]) && ValidateClass(dataMap[CLASS])
            && ValidateMobileNumber(dataMap[MOBILE_NUMBER]);
    }

    private bool Validate(string key, string value)
    {
        Debug.Log("validating key: " + key);
        if (!dataMap.ContainsKey(key)) return false;

        switch(key)
        {
            case AGE:
                return ValidateAge(value);
            case DOB:
                return ValidateDob(value);
            case SEX:
                return ValidateSex(value);
            case CLASS:
                return ValidateClass(value);
            case MOBILE_NUMBER:
                return ValidateMobileNumber(value);
            default:
                return true;
        }
    }

    #region Validators
    private bool ValidateFathersName(string value)
    {
        return true;
    }
    private bool ValidateRank(string value)
    {
        return true;
    }
    private bool ValidateUnit(string value)
    {
        return true;
    }
    private bool ValidateAge(string value)
    {
        int age = 0;
        bool isNumeric = int.TryParse(value, out age);

        return isNumeric;
    }

    private bool ValidateDob(string value)
    {
        bool correct_num_of_spearators = (value.Length - value.Replace("/", "").Length) == 2;

        if (!correct_num_of_spearators) return false;

        string[] dob = value.Split('/');

        if (dob.Length != 3) return false;

        bool isDateANumber = int.TryParse(dob[0], out int date);
        bool isMonthANumber = int.TryParse(dob[1], out int month);
        bool isYearANumber = int.TryParse(dob[2], out int year);

        bool isNmericDob = isDateANumber && isMonthANumber && isYearANumber;

        if (!isYearANumber) return false;

        if (dataMap[AGE] == "") return true;

        DateTime now = DateTime.Now;
        try
        {
            DateTime birth = new DateTime(year, month, date);
            TimeSpan diff = now.Subtract(birth);

            int diffDays = Mathf.FloorToInt((float)(diff.TotalDays / 365.25));
            if (diffDays == int.Parse(dataMap[AGE]))
            {
                return true;
            }

            Debug.Log("DOB, AGE mismatch");
            return false;
        }
        catch
        {
            return false;
        }
        

        
    }

    private bool ValidateSex(string value)
    {
        string lower = value.ToLower();
        return lower == "male" || lower == "female" || lower == "other";
    }
    private bool ValidateClass(string value)
    {
        bool isNumeric = int.TryParse(value, out int currentClass);

        return (isNumeric && currentClass > 0 && currentClass <= 12) || value.ToLower() == "college";
    }

    private bool ValidateMobileNumber(string value)
    {
        bool isNumeric = long.TryParse(value, out _);

        return isNumeric && value.Length == 10; 
    }

    #endregion
    
    public void AppendDataToCsv()
    {
        string filePath = Application.persistentDataPath + "/seguinData.csv";

        if (!File.Exists(filePath))
        {
            string headers = "Father's Name,Rank,Unit,Child's Name,Chronological Age,DOB,Sex," +
                "Class,Mobile Number,Round 1 Time,Round 2 Time,Round 3 Time," +
                "Min Round Time,Mental Age,IQ\n";
            File.WriteAllText(filePath, headers);
        }

        File.AppendAllText(filePath, GetTestDataAsString());

    }

    private string GetTestDataAsString()
    {
        string testData = "";

        testData += dataMap[FATHERS_NAME] + ",";
        testData += dataMap[RANK] + ",";
        testData += dataMap[UNIT] + ",";
        testData += dataMap[CHILD_NAME] + ",";
        testData += dataMap[AGE] + ",";
        testData += dataMap[DOB] + ",";
        testData += dataMap[SEX] + ",";
        testData += dataMap[CLASS] + ",";
        testData += dataMap[MOBILE_NUMBER] + ",";
        testData += dataMap[ROUND_1_TIME] + ",";
        testData += dataMap[ROUND_2_TIME] + ",";
        testData += dataMap[ROUND_3_TIME] + ",";
        testData += dataMap[MIN_ROUND_TIME] + ",";
        testData += dataMap[MENTAL_AGE] + ",";
        testData += dataMap[IQ] + "\n";

        return testData;
    }





}