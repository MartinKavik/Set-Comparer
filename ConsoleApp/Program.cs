using System;
using System.Collections.Generic;
using SetComparer.Comparers;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {            
            // a) 
            IntSetComparer comp = new IntSetComparer();
            bool add1 = comp.AddUniqueSet("1,2,3");     // unique
            bool add2 = comp.AddUniqueSet("1,3,2");     // duplicate
            Console.WriteLine("Add1 = {0}\nAdd2 = {1}", add1, add2);

            // b)
            (new List<string>{
                "56,69,26,0",   // unique
                "3,2,1",        // 2. duplicate
                "2,156",        // unique
                "156,2",        // duplicate
                "32"            // unique
            }).ForEach(s => comp.AddUniqueSet(s));
            int nonDuplicates = comp.GetUniquesCount();
            int duplicates = comp.GetDuplicatesCount();            
            Console.WriteLine("Non-duplicates = {0}\nDuplicates = {1}", nonDuplicates, duplicates);

            // c)
            Console.WriteLine("Most frequent duplicate group: {0}", comp.GetMostFrequentSet());

            // d)
            (new List<string>{
                "56,,0",  
                null,              
                "156,ab",               
            }).ForEach(s => comp.AddUniqueSet(s));
            comp.GetInvalidInputsReport(Console.Out);
            

            Console.ReadLine();
        }
    }
}
