using System;
using System.Collections.Generic;

using System.Text;

namespace KFilter
{
    /// <summary>
    /// Copyright © henryfan 2013		 
    ///Email:	henryfan@msn.com	
    ///HomePage:	http://www.ikende.com		
    ///CreateTime:	2013/2/1 20:20:08
    /// </summary>
    public class Keyword
    {
        public Keyword()
        {
            MaxBlank = 5;
            ReplaceData = '*';
        }

        private CharGroup[] mGroup = new CharGroup[char.MaxValue];

      

        public bool IsMatch(string value)
        {
            if (string.IsNullOrEmpty(value))
                return false;
            char[] data = value.ToCharArray();
            IList<MatchItem> items = OnMatchs(data, data.Length,true);
            return items.Count > 0;
        }

        public MatchItem Match(string value)
        {
            if (string.IsNullOrEmpty(value))
                return null;
            char[] data = value.ToCharArray();
            IList<MatchItem> items = OnMatchs(data, data.Length,true);
            if (items.Count > 0)
                return items[0];
            return null;
        }

        [ThreadStatic]
        static Rule mRule;

        internal static Rule Rule1
        {
            get
            {
                if (mRule == null)
                    mRule = new Rule();
                mRule.Reset();
                return mRule;
            }
        }

        private IList<MatchItem> OnMatchs(char[] data, int length, bool matchFirst = false)
        {
            IList<MatchItem> result = new List<MatchItem>();
            Rule rule = Rule1;
            int index = 0;
            while (index < length)
            {
                CharGroup group = mGroup[Utils.Cast(data[index])];
                if (group != null)
                {
                    MatchItem item = group.Match(data, index);
                    if (item.IsMatch)
                    {
                        result.Add((MatchItem)item.Clone());
                        index = item.EndIndex() + 1;
                        rule.mLastIndex = -10;
                        if (matchFirst)
                            return result;
                    }
                    else
                    {
                        rule.Add(data, index);
                        index++;
                    }
                }
                else
                {
                    rule.Add(data, index);
                    index++;
                }
            }
            if (rule.Count > 2)
            {
                OnRule(data, rule, result);

            }
            return result;
        }

        private void OnRule(char[] source, Rule rule, IList<MatchItem> result)
        {
            IList<MatchItem> ruleitems = OnMatchs(rule.Data, rule.Count);
            for (int i = 0; i < ruleitems.Count; i++)
            {
                MatchItem ritem = ruleitems[i];
                MatchItem mitem = new MatchItem();
                mitem.IsMatch = true;
                mitem.Data = source;
                for (int k = 0; k < ritem.KeyWordLength; k++)
                {
                    int index = rule.Indes[ritem.KeyWordIndexs[k]];
                    mitem.Add(source[index], index);
                }

                result.Add(mitem);
            }
        }

        public IList<MatchItem> Matchs(string value)
        {
            if (string.IsNullOrEmpty(value))
                return new List<MatchItem>();
            char[] data = value.ToCharArray();
            IList<MatchItem> result = OnMatchs(data, data.Length);
           
            return result;

        }

        public string Replace(string value)
        {
            if (string.IsNullOrEmpty(value))
                return value;
            char[] data = value.ToCharArray();
            IList<MatchItem> items = OnMatchs(data, data.Length);
            for (int i = 0; i < items.Count; i++)
            {
                items[i].Replace(ReplaceData);
            }
            return new string(data);
        }

        public int MaxBlank
        {
            get;
            set;
        }

        public char ReplaceData
        {
            get;
            set;
        }
        public void Clear()
        {
            for (int i = 0; i < char.MaxValue; i++)
            {
                mGroup[i] = null;
            }
        }
        private void OnAdd(string item)
        {
            
            char key = Utils.Cast(item[0]);


            CharGroup group = mGroup[key];
            if (group == null)
            {
                group = new CharGroup(key);
                mGroup[key] = group;
            }
            group.Add(item);
        }

        public void Add(params string[] values)
        {
            lock (this)
            {
                if (values != null)
                {
                    foreach (string item in values)
                    {
                        if (!string.IsNullOrEmpty(item) && item.Length > 1 && item.Length < 64)
                        {

                            WordType type = Utils.GetWordType(item);
                            string value = "";
                            foreach (char c in item)
                            {
                                if (Utils.GetWordTypeWithChar(c) != WordType.Other)
                                {
                                    value += c;
                                }
                                else
                                {
                                    if (type == WordType.EN && c == ' ')
                                    {
                                        value += c;
                                    }
                                }
                            }
                            if (value.Length > 1)
                            {
                                OnAdd(value);
                            }
                        }
                    }
                }
            }
        }

    }

    

   

  

  

   

   
}
