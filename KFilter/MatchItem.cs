using System;
using System.Collections.Generic;

using System.Text;

namespace KFilter
{
    /// <summary>
    /// Copyright © henryfan 2013		 
    ///Email:	henryfan@msn.com	
    ///HomePage:	http://www.ikende.com		
    ///CreateTime:	2013/2/1 20:22:43
    /// </summary>
    public class MatchItem : ICloneable, IComparable
    {
        public bool IsMatch;

        internal int KeyWordLength = 0;

        internal char[] KeyWords = new char[64];

        internal int[] KeyWordIndexs = new int[64];

        public char[] Data;

        public int StartIndex()
        {
            return KeyWordIndexs[0];
        }

        public int EndIndex()
        {
            return KeyWordIndexs[KeyWordLength - 1];
        }


        internal void Add(char data, int index)
        {
            KeyWords[KeyWordLength] = data;
            KeyWordIndexs[KeyWordLength] = index;
            KeyWordLength++;
        }

        public void Replace(char rdata)
        {
            for (int i = 0; i < KeyWordLength; i++)
            {
                Data[KeyWordIndexs[i]] = rdata;
            }
        }

        public void Reset()
        {
            IsMatch = false;
            KeyWordLength = 0;

        }


        public override string ToString()
        {
            return  new string(Data, KeyWordIndexs[0], KeyWordIndexs[KeyWordLength - 1] - KeyWordIndexs[0] + 1);

        }

        public object Clone()
        {
            if (IsMatch)
            {
                MatchItem item = new MatchItem();
                KeyWords.CopyTo(item.KeyWords, 0);
                KeyWordIndexs.CopyTo(item.KeyWordIndexs, 0);
                item.IsMatch = true;
                item.Data = Data;
                item.KeyWordLength = KeyWordLength;
                return item;
            }
            return null;
        }

        public int CompareTo(object obj)
        {
            return KeyWordIndexs[0].CompareTo(((MatchItem)obj).KeyWordIndexs[0]);
        }
    }
}
