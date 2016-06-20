using System;
using System.Collections.Generic;

using System.Text;

namespace KFilter
{
    /// <summary>
    /// Copyright © henryfan 2013		 
    ///Email:	henryfan@msn.com	
    ///HomePage:	http://www.ikende.com		
    ///CreateTime:	2013/2/2 20:16:01
    /// </summary>
    class Rule
    {
        public char[] Data = new char[1024 * 512];

        public int[] Indes = new int[1024 * 512];

        const int BLANK = 10;

        public int Count;

        public int mLastIndex = -10;

        public void Add(char[] data, int index)
        {
            if (data[index] != '\0' && Utils.IsOtherType(data[index]))
            {
                if (index - mLastIndex == 2)
                {
                    if (Count > 0 && index - Indes[Count - 1] > BLANK)
                    {
                        Data[Count] = '\0';
                        Indes[Count] = 0;
                        Count++;
                    }
                    Data[Count] = data[index - 1];
                    Indes[Count] = index - 1;
                    Count++;
                    mLastIndex = -10;
                }
                else if (index - mLastIndex == 3)
                {
                    if (Count > 0 && index - Indes[Count - 1] > BLANK)
                    {
                        Data[Count] = '\0';
                        Indes[Count] = 0;
                        Count++;
                    }
                    Data[Count] = data[index - 2];
                    Indes[Count] = index - 2;
                    Count++;
                    Data[Count] = data[index - 1];
                    Indes[Count] = index - 1;
                    Count++;
                    mLastIndex = -10;
                }
                else
                {

                    mLastIndex = index;
                }

            }

        }

        public void Reset()
        {
            mLastIndex = -10;
            Count = 0;
        }
    }


}
