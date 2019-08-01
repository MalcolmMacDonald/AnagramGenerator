using System;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.IO;
using System.Linq;
using System.Diagnostics;
namespace AnagramGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            //SortEnglishWords();
            TrieBranch mainTrie = new TrieBranch();
            Console.WriteLine("Creating Trie");
            string[] englishWords = GetEnglishWords();
            for (int i = 0; i < englishWords.Length; i++)
            {
                mainTrie.AddWord(englishWords[i]);
            }

            Console.WriteLine("Trie Created");
            while (true)
            {
                string inputText = Console.ReadLine();
                inputText = inputText.Replace(" ", string.Empty).ToLower();

                string[] variationsOfWord = FindWordsInTrieBranch(mainTrie, inputText);

                Console.WriteLine("Variations of " + inputText + " found: ");
                for (int i = 0; i < variationsOfWord.Length; i++)
                {
                    int variationLength = variationsOfWord[i].Length;
                    string[] variationsFoundOfVariations = FindWordsInTrieBranch(mainTrie, inputText.Substring(variationLength, inputText.Length - variationLength));

                    if (variationsFoundOfVariations.Length > 0)
                    {
                        for (int j = 0; j < variationsFoundOfVariations.Length; j++)
                        {
                            Console.WriteLine(variationsOfWord[i] + " " + variationsFoundOfVariations[j]);
                        }
                    }
                    else
                    {
                        Console.WriteLine(variationsOfWord[i]);
                    }
                }
            }
        }
        static string[] GetEnglishWords()
        {
            return File.ReadAllLines("EnglishWords.txt");
        }
        static void SortEnglishWords()
        {
            string[] unsortedWords = File.ReadAllLines("EnglishWords.txt");
            unsortedWords = unsortedWords.OrderBy(s => s.Length).ToArray();
            File.WriteAllLines("EnglishWords.txt", unsortedWords);
        }

        static string[] FindWordsInTrieBranch(TrieBranch branch, string input)
        {
            List<string> foundWords = new List<string>();
            for (int i = 0; i < input.Length; i++)
            {
                string wordToCheck = input.Substring(0, input.Length - i);
                if (branch.ContainsWord(wordToCheck))
                {
                    foundWords.Add(wordToCheck);
                }
            }
            return foundWords.ToArray();
        }
    }


    class TrieBranch
    {
        char thisChar;
        public Dictionary<char, TrieBranch> childBranches = new Dictionary<char, TrieBranch>();
        public bool endsWord;

        public TrieBranch(char _thisChar = ' ')
        {
            thisChar = _thisChar;
        }

        public bool ContainsChar(char input)
        {
            return childBranches.ContainsKey(input);
        }

        public TrieBranch AddBranch(char newChar)
        {
            childBranches.Add(newChar, new TrieBranch(newChar));
            return childBranches[newChar];
        }

        public bool ContainsWord(string word)
        {
            if (word.Length < 1)
            {
                return endsWord;
            }

            char firstCharacter = word[0];

            string tempWord = word.Remove(0, 1);

            if (!ContainsChar(firstCharacter))
            {
                return false;
            }
            else
            {
                return childBranches[firstCharacter].ContainsWord(tempWord);
            }
        }

        public void AddWord(string word)
        {
            if (word.Length < 1)
            {
                endsWord = true;
                return;
            }

            string tempWord = word;

            char firstCharacter = tempWord[0];
            tempWord = tempWord.Remove(0, 1);
            if (ContainsChar(firstCharacter))
            {
                childBranches[firstCharacter].AddWord(tempWord);
            }
            else
            {
                AddBranch(firstCharacter).AddWord(tempWord);
            }
        }
    }
}
