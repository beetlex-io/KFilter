using System;
using System.Collections.Generic;

using System.Text;

namespace KFilter
{
    /// <summary>
    /// Copyright © henryfan 2013		 
    ///Email:	henryfan@msn.com	
    ///HomePage:	http://www.ikende.com		
    ///CreateTime:	2013/2/1 20:22:02
    /// </summary>
    class WordItem
    {
        public WordItem(string value)
        {
            Value = value;
            Type = Utils.GetWordType(value);
        }
        public string Value;
        public WordType Type;
    }
}
