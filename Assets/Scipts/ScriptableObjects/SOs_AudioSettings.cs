using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Audio Settings", menuName = "ScriptableObjectsAudio/Audio Settings", order = 1)]

public class SOs_AudioSettings : ScriptableObject
{
    public float masterVolume = 1;
    public float musicVolume = 1;
    public float sfxVolume = 1;
}
