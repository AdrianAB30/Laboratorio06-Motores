using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Sounds Rooms", menuName = "ScriptableObjects/Sounds Rooms", order = 0)]
public class SOs_Sounds : ScriptableObject
{
    public AudioClip soundRoom;
    public AudioClip SoundClip => soundRoom;
}
