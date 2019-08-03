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

                foundAnagrams.AddRange(mainTrie.GetPossibleWordsFromCharacters(inputText, ""));


                foundAnagrams = foundAnagrams.Where(s => RemoveCharactersFromString(inputText, s).Length == 0 || mainTrie.GetPossibleWordsFromCharacters(RemoveCharactersFromString(inputText, s), "").Count > 0).ToList();

                for (int i = 0; i < foundAnagrams.Count; i++)
                {
                    Console.WriteLine(foundAnagrams[i]);

                    List<string> childWords = mainTrie.GetPossibleWordsFromCharacters(RemoveCharactersFromString(inputText, foundAnagrams[i]), "");



                    for (int j = 0; j < childWords.Count; j++)
                    {
                        Console.WriteLine("    " + childWords[j]);
                    }
                }

                anagramFindingStopwatch.Stop();
                int minutesElapsed = anagramFindingStopwatch.Elapsed.Minutes;
                Console.WriteLine("Found {0} anagrams in {1} minutes!", foundAnagrams.Count, minutesElapsed);


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
        static string RemoveCharactersFromString(string inputString, string charactersToRemove)
        {
            string inputCopy = inputString;
            for (int j = 0; j < charactersToRemove.Length; j++)
            {
                inputCopy = inputCopy.Remove(inputCopy.IndexOf(charactersToRemove[j]), 1);
            }
            return inputCopy;
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

        public List<string> GetPossibleWordsFromCharacters(string availableCharacters, string currentWord)
        {
            List<string> foundWords = new List<string>();

            if (endsWord)
            {
                foundWords.Add(currentWord);
            }
            string distinctCharacters = new string(availableCharacters.Distinct().ToArray());
            for (int i = 0; i < distinctCharacters.Length; i++)
            {
                char selectedCharacter = distinctCharacters[i];
                int indexOfFirstCharacter = availableCharacters.IndexOf(selectedCharacter);
                string restOfInput = availableCharacters.Substring(0, indexOfFirstCharacter) + availableCharacters.Substring(indexOfFirstCharacter + 1, availableCharacters.Length - (indexOfFirstCharacter + 1));
                string currentWordPrime = currentWord + selectedCharacter;



                if (ContainsChar(selectedCharacter))
                {
                    TrieBranch branchToCheck = childBranches[selectedCharacter];
                    foundWords.AddRange(branchToCheck.GetPossibleWordsFromCharacters(restOfInput, currentWordPrime));
                }

            }

            return foundWords;
        }
    }
}
