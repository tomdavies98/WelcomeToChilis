using System;
using DSharpPlus.VoiceNext;
namespace WelcomeToChilis
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            var bot = new Bot();
            bot.RunAsync().GetAwaiter().GetResult();
        }
    }
}
