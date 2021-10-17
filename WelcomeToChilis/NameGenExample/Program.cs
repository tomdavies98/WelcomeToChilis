using NameGenerator;
using System;

namespace NameGenExample
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Generator gen = new Generator();
            
            for(int i =0; i< 10000; ++i)
            {
                var name = gen.MakeName();
                Console.WriteLine(name);
            }
            
        }
    }
}
