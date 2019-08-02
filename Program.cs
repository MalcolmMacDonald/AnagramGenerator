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

                Stopwatch anagramFindingStopwatch = new Stopwatch();
                anagramFindingStopwatch.Start();
                List<string> foundAnagrams = new List<string>();


                foreach (char[] permutation in inputText.GetPermutations())
                {
                    string permutationText = new string(permutation);
                    string[] foundSubWords = FindWordsInTrieBranch(mainTrie, permutationText);
                    foundSubWords = foundSubWords.Except(foundAnagrams).ToArray();
                    for (int j = 0; j < foundSubWords.Length; j++)
                    {
                        foundAnagrams.Add(foundSubWords[j]);
                        Console.WriteLine(foundSubWords[j]);
                    }
                }

                anagramFindingStopwatch.Stop();
                int minutesElapsed = anagramFindingStopwatch.Elapsed.Minutes;
                Console.WriteLine("Found {0} anagrams in {1} minutes!", foundAnagrams.Count, minutesElapsed);


                /* List<string> inputPermutations = PermuteString(inputText);

                for (int i = 0; i < inputPermutations.Count; i++)
                {
                    string thisPerumatation = inputPermutations[i];
                    inputPermutations.RemoveAll(s => s == thisPerumatation);
                    inputPermutations.Insert(i, thisPerumatation);
                }


                for (int i = 0; i < inputPermutations.Count; i++)
                {*/

                // Console.WriteLine("Variations of " + inputText + " found: ");

                //}


            }
        }
        static string[] GetEnglishWords()
        {
            return File.ReadAllLines("EnglishWords.txt").Where(s => s.Length > 3).ToArray();
        }
        static void SortEnglishWords()
        {
            string[] unsortedWords = File.ReadAllLines("EnglishWords.txt");
            unsortedWords = unsortedWords.OrderBy(s => s.Length).ToArray();
            File.WriteAllLines("EnglishWords.txt", unsortedWords);
        }

        static List<string> PermuteString(string input)
        {
            List<string> foundPermutations = new List<string>();

            if (input.Length == 1)
            {
                foundPermutations.Add(input);
                return foundPermutations;
            }

            for (int i = 0; i < input.Length; i++)
            {
                char selectedCharacter = input[i];
                string restOfInput = input.Substring(0, i) + input.Substring(i + 1, input.Length - (i + 1));

                List<string> childPermutations = PermuteString(restOfInput);
                for (int j = 0; j < childPermutations.Count; j++)
                {
                    foundPermutations.Add(selectedCharacter + childPermutations[j]);
                }
            }

            return foundPermutations;
        }


        static string[] FindWordsInTrieBranch(TrieBranch branch, string input)
        {

            List<string> foundWords = new List<string>();
            List<string> wordEndings = new List<string>();
            for (int i = 0; i < input.Length; i++)
            {
                string wordToCheck = input.Substring(0, input.Length - i);
                if (branch.ContainsWord(wordToCheck))
                {
                    foundWords.Add(wordToCheck);

                    string newWordEnding = input.Substring(wordToCheck.Length, input.Length - wordToCheck.Length);
                    wordEndings.Add(newWordEnding);
                }
            }
            List<string> returnedWords = new List<string>();
            for (int i = 0; i < foundWords.Count; i++)
            {
                string[] wordEndingsSubWords = FindWordsInTrieBranch(branch, wordEndings[i]);
                for (int j = 0; j < wordEndingsSubWords.Length; j++)
                {
                    returnedWords.Add(foundWords[i] + " " + wordEndingsSubWords[j]);
                }
            }
            foundWords = foundWords.Concat(returnedWords).ToList();
            foundWords = foundWords.Where(s => s.Replace(" ", "").Length == input.Length).ToList();
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
