using System;
using System.Collections.Generic;

using System.Text;

namespace KFilter
{
    /// <summary>
    /// Copyright © henryfan 2013		 
    ///Email:	henryfan@msn.com	
    ///HomePage:	http://www.ikende.com		
    ///CreateTime:	2013/2/1 20:21:16
    /// </summary>

    enum WordType
    {
        None = 1,
        Number = 2,
        EN = 4,
        CN = 8,
        KJ1 = 16,
        KJ2 = 32,
        KJ3 = 64,
        KJ4 = 128,
        KJ5 = 256,
        Other = 512
    }
}
