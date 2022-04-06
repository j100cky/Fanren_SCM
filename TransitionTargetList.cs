using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Transition Targets List")]

public class TransitionTargetList : ScriptableObject
{
    public List<ScenePositionPairs> pairs;
}
