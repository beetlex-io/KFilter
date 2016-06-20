using System;
using System.Collections.Generic;

using System.Text;

namespace KFilter
{
    /// <summary>
    /// Copyright © henryfan 2013		 
    ///Email:	henryfan@msn.com	
    ///HomePage:	http://www.ikende.com		
    ///CreateTime:	2013/2/1 20:21:40
    /// </summary>
    class CharGroup
    {
        public char Key;

        public WordType WordType = WordType.None;

        public CharGroup(char key)
        {
            Key = key;
        }

        public int MaxBlank = 5;

        public Dictionary<char, CharItem> Items = new Dictionary<char, CharItem>();

        private List<CharItem> LstItems = new List<CharItem>(8);

        public void Add(string value)
        {
            int index = 1;
            char key = Utils.Cast(value[index]);

            WordItem wi = new WordItem(value);
            WordType = WordType | wi.Type;

            CharItem item = null;
            if (!Items.TryGetValue(key, out item))
            {
                item = new CharItem(key);
                item.MaxBlank = MaxBlank;
                item.WordType |= wi.Type;
                Items.Add(key, item);
                LstItems.Add(item);
            }
            if (index + 1 == value.Length)
            {
                item.Eof = true;
            }
            else
            {
                item.Add(value, index + 1, wi);
            }

        }

        [ThreadStatic]
        private static MatchItem mMatchItem;

        public static MatchItem MatchItem
        {
            get
            {
                if (mMatchItem == null)
                    mMatchItem = new MatchItem();
                mMatchItem.Reset();
                return mMatchItem;
            }


        }

        public MatchItem Match(char[] data, int index)
        {
            MatchItem result = MatchItem;
            BlankType = KFilter.WordType.None;
            result.Add(Key, index);
            int count = LstItems.Count;
            int blank = MaxBlank;
        BETIN:
            index++;
            if (index < data.Length)
            {
                char key = Utils.Cast(data[index]);
                CharItem item = null;
                if (count < 4)
                {
                    for (int i = 0; i < count; i++)
                    {
                        item = LstItems[i];
                        if (item.Key == key)
                            break;
                        else
                            item = null;
                    }
                }
                else
                    Items.TryGetValue(key, out item);
                if (item != null)
                {

                    result.Data = data;
                    result.IsMatch = true;
                    result.Add(key, index);

                    if (!item.Eof)
                    {
                        item.Match(data, index + 1, result);
                    }
                }
                else
                {
                    if (blank > 0 && (WordType & Utils.GetWordTypeWithChar(key)) == 0 && key !='\0')
                    {
                        WordType chartype = Utils.GetWordTypeWithChar(key);
                        if (BlankType == KFilter.WordType.None)
                        {
                            BlankType = chartype;
                        }
                        else
                        {
                            if (BlankType != chartype)
                                blank--;
                            BlankType = chartype;
                        }
                        goto BETIN;
                    }
                }
            }
            Rule(result, data);
           
            return result;
        }
        private WordType BlankType = WordType.None;

        private void Rule(MatchItem result, char[] data)
        {
            int cindex = result.StartIndex();
            WordType ctype = Utils.GetWordTypeWithChar(data[cindex]);
            if (ctype == WordType.EN || ctype == WordType.Number)
            {
                if (cindex > 1 && ctype == Utils.GetWordTypeWithChar(data[cindex - 1]))
                    result.IsMatch = false;
            }
            cindex = result.EndIndex();
            ctype = Utils.GetWordTypeWithChar(data[cindex]);
            if (ctype == WordType.EN || ctype == WordType.Number)
            {
                if (cindex < data.Length - 1 && ctype == Utils.GetWordTypeWithChar(data[cindex + 1]))
                    result.IsMatch = false;
            }
        }
    }
}
