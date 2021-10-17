using RedditMemeManager;
using System;

namespace ReditMemeCollectorExample
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var memePath = MemeManager.GetMeme(@"C:\Users\tomda\Desktop\Memes");
            var success = MemeManager.DeleteMeme(memePath);

            if(success)
            {
                Console.WriteLine($"Meme deleted: {memePath}");
            }
            else 
            {
                Console.WriteLine("Could not delete meme...");
            }

            Console.ReadLine();
        }
    }
}
