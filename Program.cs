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

            /*  string inputText = Console.ReadLine();
             inputText = inputText.Replace(" ", string.Empty).ToLower();
             inputText = new string(inputText.OrderBy(s => (int)s).ToArray());*/

            Trie mainTrie = new Trie();

            string[] englishWords = GetEnglishWords();
            for (int i = 0; i < englishWords.Length; i++)
            {
                mainTrie.AddWord(englishWords[i]);
            }


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
    }

}
