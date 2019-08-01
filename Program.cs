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
            TrieBranch mainTrie = new TrieBranch();

            string[] englishWords = GetEnglishWords();
            for (int i = 0; i < englishWords.Length; i++)
            {
                mainTrie.AddWord(englishWords[i]);
            }


            while (true)
            {
                string inputText = Console.ReadLine();
                inputText = inputText.Replace(" ", string.Empty).ToLower();

                Console.WriteLine("In Trie: " + mainTrie.ContainsWord(inputText).ToString() + " In Text: " + englishWords.Contains(inputText));
            }

            // inputText = new string(inputText.OrderBy(s => (int)s).ToArray());



        }
        static string[] GetEnglishWords()
        {
            return File.ReadAllLines("EnglishWords.txt");
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
