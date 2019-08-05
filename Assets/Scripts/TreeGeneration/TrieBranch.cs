using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TrieBranch
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
