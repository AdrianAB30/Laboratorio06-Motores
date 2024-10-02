using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NPCData", menuName = "ScriptableObjectNPC/New NPC Data")]
public class SOs_DataNpc : ScriptableObject
{
    public float velocity;
    public float interactionPlayerDuration;
    public float patrolStopDuration;
    [TextArea(4,6)] public string[] dialogueLines;
}
