using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "QuestObject", menuName = "Scriptable Objects/QuestObject")]

[System.Serializable]
public class stats
{
    public uint level;
    public string testString;
}

public class QuestObject : ScriptableObject
{
    // Template for quests

    //Quest Name
    [SerializeField] string questName;
    //Quest Description
    [SerializeField] string questDescription;
    //Quest Requirements
    [SerializeField] List<QuestObject> requiredQuests;
    [SerializeField] stats requiredStats;
    //Quest Rewards
    [SerializeField] stats rewardStats;

    //Checks for if quest is completed
    [SerializeField] bool isCompleted; 
}
