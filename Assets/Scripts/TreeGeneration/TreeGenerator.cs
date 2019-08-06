using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
public class TreeGenerator : MonoBehaviour
{
    public static TreeGenerator instance;
    public TrieBranch mainTrie = new TrieBranch();
    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        print("Creating Trie");
        string[] englishWords = GetEnglishWords();
        for (int i = 0; i < englishWords.Length; i++)
        {
            mainTrie.AddWord(englishWords[i]);
        }
    }


    public string[] GetChildWordsFromString(string inputText)
    {
        List<string> foundAnagrams = new List<string>();
        inputText = inputText.Replace(" ", string.Empty).ToLower();
        foundAnagrams.AddRange(mainTrie.GetPossibleWordsFromCharacters(inputText, ""));

        foundAnagrams.Remove(inputText);


        foundAnagrams = foundAnagrams.Where(s => WordHasViableChildren(inputText, s)).ToList();

        return foundAnagrams.ToArray();
    }
    public string[] GetRemainingLetters(string rootWord, string[] childWords)
    {
        return childWords.Select(s => RemoveCharactersFromString(rootWord, s)).ToArray();
    }


    static string[] GetEnglishWords()
    {
        return File.ReadAllLines("Assets/Data/EnglishWords.txt");//.Where(s => s.Length > 3).ToArray();
    }
    static void SortEnglishWords()
    {
        string[] unsortedWords = File.ReadAllLines("EnglishWords.txt");
        unsortedWords = unsortedWords.OrderBy(s => s.Length).ToArray();
        File.WriteAllLines("EnglishWords.txt", unsortedWords);
    }
    public bool WordIsEndOfSentence(string word)
    {
        // return mainTrie.GetPossibleWordsFromCharacters(word, "").Any(s => GetChildWordsFromString(RemoveCharactersFromString(word, s)).Length > 0);
        return true;
    }
    bool WordHasViableChildren(string rootWord, string childWord)
    {
        string excessLetters = RemoveCharactersFromString(rootWord, childWord);

        List<string> childWords = mainTrie.GetPossibleWordsFromCharacters(excessLetters, "");

        if (excessLetters.Length == 0)
        {
            return true;
        }
        return childWords.Any(s => WordHasViableChildren(rootWord, childWord + s));
    }
    public static string RemoveCharactersFromString(string inputString, string charactersToRemove)
    {
        string inputCopy = inputString;
        for (int j = 0; j < charactersToRemove.Length; j++)
        {
            inputCopy = inputCopy.Remove(inputCopy.IndexOf(charactersToRemove[j]), 1);
        }
        return inputCopy;
    }


}
