using System;
using System.Collections.Generic;

using System.Text;

namespace KFilter
{
    /// <summary>
    /// Copyright © henryfan 2013		 
    ///Email:	henryfan@msn.com	
    ///HomePage:	http://www.ikende.com		
    ///CreateTime:	2013/2/1 20:22:20
    /// </summary>
    class CharItem
    {
        public CharItem(char key)
        {
            Key = key;

        }

        public WordType WordType = WordType.None;

        public bool Eof;

        public int MaxBlank = 5;

        public char Key;

        public Dictionary<char, CharItem> Items = new Dictionary<char, CharItem>();

        private List<CharItem> LstItems = new List<CharItem>(8);

        public void Add(string value, int start, WordItem wi)
        {

            if (start < value.Length)
            {
                char key = Utils.Cast(value[start]);

                CharItem item = null;
                if (!Items.TryGetValue(key, out item))
                {
                    item = new CharItem(key);
                    item.WordType |= wi.Type;
                    item.MaxBlank = MaxBlank;
                    Items.Add(key, item);
                    LstItems.Add(item);
                }
                if (start + 1 == value.Length)
                {
                    item.Eof = true;
                }
                else
                {
                    item.Add(value, start + 1, wi);
                }

            }
           

        }



        public void Match(char[] data, int start, MatchItem result)
        {
            BlankType = KFilter.WordType.None;
            int blank = MaxBlank;
            int count = LstItems.Count;
        BETIN:
            if (start >= data.Length)
            {
                result.IsMatch = false;
                return;
            }
            char key = Utils.Cast(data[start]);
            CharItem item = null;

            if (count ==1)
            {

                if (LstItems[0].Key == key)
                    item = LstItems[0];
                  
            }
            else if (count == 2)
            {
                if (LstItems[0].Key == key)
                    item = LstItems[0];
                else
                {
                    if (LstItems[1].Key == key)
                        item = LstItems[1];
                }
            }
            else
            {
                Items.TryGetValue(key, out item);
            }

            if (item != null)
            {
                result.IsMatch = true;
                result.Add(key, start);

                if (!item.Eof)
                {
                    item.Match(data, start + 1, result);
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
                    start++;
                    goto BETIN;
                }
                result.IsMatch = false;
            }
        }

        private WordType BlankType = WordType.None;

    }
}
