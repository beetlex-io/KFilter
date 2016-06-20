using System;
using System.Collections.Generic;

using System.Text;
using Microsoft.VisualBasic;

namespace KFilter
{
    /// <summary>
    /// Copyright © henryfan 2013		 
    ///Email:	henryfan@msn.com	
    ///HomePage:	http://www.ikende.com		
    ///CreateTime:	2013/2/1 20:20:55
    /// </summary>
    class Utils
    {
        public static WordType GetWordType(string value)
        {
            WordType type = WordType.None;
            foreach (char item in value)
            {
                type = type | GetWordTypeWithChar(item);
            }
            return type;
        }

        static char[] mCastMapper = new char[char.MaxValue];

        static WordType[] mCharType = new WordType[char.MaxValue];

        static WordType[] mNonCharType = new WordType[char.MaxValue];

        static int[] mOtherType = new int[char.MaxValue];

        public static bool IsOtherType(char value)
        {
            int result = mOtherType[value];
            if (result != 0)
                return result == 1;
            WordType type = GetWordTypeWithChar(value);
            if (type == WordType.KJ1 || type == WordType.Other)
                result = 1;
            else
                result = 2;
            mOtherType[value] = result;
            return result == 1;
        }

        public static WordType GetWordTypeWithChar(char value)
        {
            WordType result = mCharType[value];
            if (result > WordType.None)
                return result;
            if (value >= 0x4e00 && value <= 0x9fa5)
            {
                result = WordType.CN;

            }
            else if ((value >= 65 && value <= 90) || (value >= 97 && value <= 122) || (value >= 65313 && value <= 65338) || (value >= 65345 && value <= 65370))
            {
                result = WordType.EN;
            }
            else if ((value >= '0' && value <= '9') || (value >= '０' && value <= '９'))
            {
                result = WordType.Number;
            }
            else if (value >= 0x2E80 && value <= 0x33FF)
            {
                result = WordType.KJ1;
            }
            else if (value >= 0x3400 && value <= 0x4DFF)
            {
                result = WordType.KJ2;
            }
            else if (value >= 0xA000 && value <= 0xA4FF)
            {
                result = WordType.KJ3;
            }
            else if (value >= 0xAC00 && value <= 0xD7FF)
            {
                result = WordType.KJ4;
            }
            else if (value >= 0xF900 && value <= 0xFAFF)
            {
                //F900～FAFFh
                result = WordType.KJ5;
            }
            else
            {
                result = WordType.Other;
            }
            mCharType[value] = result;
            return result;
        }

        public static char Cast(char value)
        {
            if (value == '\0')
                return value;
            char result = mCastMapper[value];
            if (result != '\0')
                return result;
            if (value >= 0x4e00 && value <= 0x9fa5)
            {

                string str = value.ToString();
                string fstr = Microsoft.VisualBasic.Strings.StrConv(str, VbStrConv.SimplifiedChinese);
                result = fstr[0];

            }
            else if (value >= 65 && value <= 90)
            {
                result = value;
            }
            else if (value >= 97 && value <= 122)
            {
                result = (char)(value - 32);
            }
            else if (value >= 65313 && value <= 65338)
            {
                result = (char)(value - 65248);
            }
            else if (value >= 65345 && value <= 65370)
            {
                result = (char)(value - 65280);
            }
            else if (value >= '0' && value <= '9')
            {
                result = value;
            }
            else if (value >= '０' && value <= '９')
            {
                result = (char)(value - 65248);
            }
            else
            {
                result = value;
            }
            mCastMapper[value] = result;
            return result;
        }



        public static List<char> GetLike(char value)
        {
            char[] result;
            if (value >= 0x4e00 && value <= 0x9fa5)
            {
                string str = value.ToString();
                string fstr = Microsoft.VisualBasic.Strings.StrConv(str, VbStrConv.TraditionalChinese);
                if (str == fstr)
                    result = Microsoft.VisualBasic.Strings.StrConv(str, VbStrConv.SimplifiedChinese).ToCharArray();
                result = fstr.ToCharArray();

            }
            else if (value >= 65 && value <= 90)
            {
                result = new char[] { (char)(value + 32), (char)(value + 65248), (char)(value + 65280) };
            }
            else if (value >= 97 && value <= 122)
            {
                result = new char[] { (char)(value - 32), (char)(value + 65248 - 32), (char)(value + 65280 - 32) };
            }
            else if (value >= 65313 && value <= 65338)
            {
                result = new char[] { (char)(value + 32), (char)(value - 65248), (char)(value - 65216) };
            }
            else if (value >= 65345 && value <= 65370)
            {
                result = new char[] { (char)(value - 32), (char)(value - 65280), (char)(value - 65248) };
            }
            else if (value >= '0' && value <= '9')
            {
                result = new char[] { (char)(value + 65248) };
            }
            else if (value >= '０' && value <= '９')
            {
                result = new char[] { (char)(value - 65248) };
            }
            else
            {
                result = new char[0];
            }
            if (result.Length == 1 && result[0] == value)
                result = new char[0];
            List<char> items = new List<char>();
            items.AddRange(result);
            return items;
        }
    }
}
