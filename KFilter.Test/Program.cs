using System;
using System.Collections.Generic;
using System.Text;
using KFilter;

namespace KD.KFilter.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            Keyword kw = new Keyword();
            kw.Add(Resource1.KEY.Split('\r', '\n'));
           
         
            string value = "java";
         
            using (System.IO.StreamReader reader = new System.IO.StreamReader("test.txt", Encoding.UTF8))
            {
                value = reader.ReadToEnd();
            }

            IList<MatchItem> items = kw.Matchs(value);
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            sw.Reset();
            sw.Start();
            items = kw.Matchs(value);
            sw.Stop();
            foreach (MatchItem item in items)
            {
                Console.WriteLine("{0}[start:{1} end:{2}]",item,item.StartIndex(),item.EndIndex() );
            }
            Console.WriteLine("Matchs:{0}", items.Count);
            Console.WriteLine("Text Length:{0} use Time:{1}ms", value.Length, sw.Elapsed.TotalMilliseconds);




            Console.Read();
        }
    }















}
