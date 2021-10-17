using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace NameGenerator
{
    public class Generator
    {
        private Random _rand = new Random();
        public string[] Adjectives { get; set; }
        public string[] Animals { get; set; }
        public string[] Colours { get; set; }
        public string[] Nouns { get; set; }

        public Generator()
        {
            LoadData();
        }

        private void LoadData()
        {
            Adjectives = File.ReadAllLines(Directory.GetCurrentDirectory() + "\\Datasets\\adjectives.txt");
            Animals = File.ReadAllLines(Directory.GetCurrentDirectory() + "\\Datasets\\animals.txt");
            Colours = File.ReadAllLines(Directory.GetCurrentDirectory() + "\\Datasets\\colours.txt");
            Nouns = File.ReadAllLines(Directory.GetCurrentDirectory() + "\\Datasets\\nouns.txt");
        }

        private string FormatWord(string word)
        {
            if(word.Contains(" "))
            {
                var words = word.Split(' ');
                var concatonate = "";
                foreach(var entry in words)
                {
                    if(entry.Length > 1)
                    {
                        concatonate += char.ToUpper(entry[0]) + entry.Substring(1);
                    }
                }
                word = concatonate;

                return word;
            }
            else
            {
                if (word.Length > 1)
                {
                    return char.ToUpper(word[0]) + word.Substring(1);
                }

                return char.ToUpper(word[0]).ToString();
            }
        }

        private string GetRandomColour()
        {
            return FormatWord(Colours[_rand.Next(Colours.Length)]);
        }

        private string GetRandomAdjective()
        {
            return FormatWord(Adjectives[_rand.Next(Adjectives.Length)]);
        }

        private string GetRandomAnimal()
        {
            return FormatWord(Animals[_rand.Next(Animals.Length)]);
        }

        private string GetRandomNoun()
        {
            return FormatWord(Nouns[_rand.Next(Nouns.Length)]);
        }

        private string GetRandomNumber()
        {
            return _rand.Next(1000).ToString();
        }

        public string MakeName()
        {
            //Chance of adding colour
            var name = "";
            var colourChance = _rand.Next(0, 10);

            if (colourChance >= 8)
                name += GetRandomColour();

            name += GetRandomAdjective();

            var animalChance = _rand.Next(0, 10);

            if(animalChance > 5)
            {
                name += GetRandomAnimal();
            }
            else
            {
                name += GetRandomNoun();
            }

            name += GetRandomNumber();

            return name;
        }




    }
}
