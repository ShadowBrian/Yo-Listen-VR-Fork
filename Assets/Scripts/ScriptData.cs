using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New DialogueScript", menuName = "Dialogue Script")]
public class ScriptData : ScriptableObject
{
    public List<string> textToRead;
    public ScriptData next;
}
