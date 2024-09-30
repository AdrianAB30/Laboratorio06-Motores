using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorsRooms : MonoBehaviour
{
    [SerializeField] private AudioSource openDoor;
    [SerializeField] private AudioSource closeDoor;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!openDoor.isPlaying) 
            {
                openDoor.Play();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!closeDoor.isPlaying) 
            {
                closeDoor.Play();
            }
        }
    }
}
