using System;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.IO;
using System.Linq;
namespace AnagramGenerator
{
    class Program
    {
        static void Main(string[] args)
        {

            Trie mainTrie = new Trie();

            string[] englishWords = GetEnglishWords();
            for (int i = 0; i < englishWords.Length; i++)
            {
                mainTrie.AddWord(englishWords[i]);
            }


            while (true)
            {
                string inputText = Console.ReadLine();
                inputText = inputText.Replace(" ", string.Empty).ToLower();

                Console.WriteLine(mainTrie.ContainsWord(inputText).ToString());
            }

            // inputText = new string(inputText.OrderBy(s => (int)s).ToArray());





            // Console.WriteLine(inputText);






            //    List<string> inputPermutations = new List<string>();




        }
        static string[] GetEnglishWords()
        {
            return File.ReadAllLines("EnglishWords.txt");
        }


    }


    class Trie
    {
        TrieBranch rootBranch = new TrieBranch();

        public void AddWord(string word)
        {

            string tempWord = word;
            TrieBranch thisBranch = rootBranch;

            while (tempWord.Length > 0)
            {

                char currentChar = tempWord[0];
                tempWord = tempWord.Remove(0, 1);

                if (thisBranch.ContainsChar(currentChar))
                {
                    thisBranch = thisBranch.childBranches[currentChar];
                }
                else
                {
                    thisBranch = thisBranch.AddBranch(currentChar);
                }
            }
        }
        public bool ContainsWord(string word)
        {
            return rootBranch.ContainsWord(word);
        }
    }

    class TrieBranch
    {
        char thisChar;
        public Dictionary<char, TrieBranch> childBranches = new Dictionary<char, TrieBranch>();


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
                return true;
            }

            char firstLetter = word[0];

            string tempWord = word.Remove(0, 1);

            if (!childBranches.ContainsKey(firstLetter))
            {
                return false;
            }
            else
            {
                return childBranches[firstLetter].ContainsWord(tempWord);
            }
        }
    }

}
