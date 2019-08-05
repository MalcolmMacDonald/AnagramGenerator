using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
public class TreeGenerator : MonoBehaviour
{
    TrieBranch mainTrie = new TrieBranch();

    // Start is called before the first frame update
    void Start()
    {
        //SortEnglishWords();
        print("Creating Trie");
        string[] englishWords = GetEnglishWords();
        for (int i = 0; i < englishWords.Length; i++)
        {
            mainTrie.AddWord(englishWords[i]);
        }




    }


    public string[] GetChildWordsFromString(string inputText)
    {
        inputText = inputText.Replace(" ", string.Empty).ToLower();
        List<string> foundAnagrams = new List<string>();

        foundAnagrams.AddRange(mainTrie.GetPossibleWordsFromCharacters(inputText, ""));


        foundAnagrams = foundAnagrams.Where(s => RemoveCharactersFromString(inputText, s).Length == 0 || mainTrie.GetPossibleWordsFromCharacters(RemoveCharactersFromString(inputText, s), "").Count > 0).ToList();
        foundAnagrams.Remove(inputText);
        List<string> childWords = new List<string>();
        childWords.AddRange(foundAnagrams);
        for (int i = 0; i < foundAnagrams.Count; i++)
        {
            print(foundAnagrams[i]);

            //   childWords.AddRange(mainTrie.GetPossibleWordsFromCharacters(RemoveCharactersFromString(inputText, foundAnagrams[i]), ""));
        }
        return childWords.ToArray();
    }


    static string[] GetEnglishWords()
    {
        return File.ReadAllLines("Assets/Data/EnglishWords.txt").Where(s => s.Length > 3).ToArray();
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
