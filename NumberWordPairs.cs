using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script is used to expressed numbers from arabic numbers into chinese characters. 
//For example, this is used by the OneSkillbookPanelScript to show the level of each skillbook.
[CreateAssetMenu(menuName = "Number to Word Pair")]
public class NumberWordPairs : ScriptableObject
{
    [SerializeField] List<int> numbers;
    [SerializeField] List<string> words;
    public Dictionary<int, string> numberWordPairs;

    public void MakeDictionary()
    {
        numberWordPairs = new Dictionary<int, string>();
        for(int i = 0; i < numbers.Count; i++)
        {
            numberWordPairs.Add(numbers[i], words[i]);
        }
    }

    public string GetString(int number)
    {

        return numberWordPairs[number];
    }

}
