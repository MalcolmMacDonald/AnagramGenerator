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
                mainTrie.AddWord(mainTrie, englishWords[i]);
            }


            /*   while (true)
              {
                  string inputText = Console.ReadLine();
                  inputText = inputText.Replace(" ", string.Empty).ToLower();

                  Console.WriteLine("In Trie: " + mainTrie.ContainsWord(inputText).ToString());
              }*/

            // inputText = new string(inputText.OrderBy(s => (int)s).ToArray());

            Stopwatch trieStopwatch = new Stopwatch();
            trieStopwatch.Start();

            for (int i = 0; i < englishWords.Length; i++)
            {
                mainTrie.ContainsWord(englishWords[i]);
            }

            trieStopwatch.Stop();

            Console.WriteLine("Trie Seconds: " + trieStopwatch.Elapsed.Milliseconds);

            Stopwatch linqStopwatch = new Stopwatch();

            linqStopwatch.Start();
            for (int i = 0; i < englishWords.Length; i++)
            {
                englishWords.Contains(englishWords[i]);
            }
            linqStopwatch.Stop();
            Console.WriteLine("Linq Seconds: " + linqStopwatch.Elapsed.Milliseconds);

            Console.ReadKey();

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
        public void AddWord(TrieBranch branch, string word)
        {

            string tempWord = word;

            while (tempWord.Length > 0)
            {

                char currentChar = tempWord[0];
                tempWord = tempWord.Remove(0, 1);

                if (branch.ContainsChar(currentChar))
                {
                    branch = branch.childBranches[currentChar];
                }
                else
                {
                    branch = branch.AddBranch(currentChar);
                    branch.AddWord(branch, tempWord);
                }
            }
            branch.endsWord = true;
        }
    }

}
