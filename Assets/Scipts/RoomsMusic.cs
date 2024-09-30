using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomsMusic : MonoBehaviour
{
    [SerializeField] private SOs_Sounds soundsRooms;
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            audioSource.clip = soundsRooms.SoundClip;
            audioSource.Play();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            audioSource.clip = soundsRooms.SoundClip;
            audioSource.Stop();
        }
    }
}
