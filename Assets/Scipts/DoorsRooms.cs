using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DoorsRooms : MonoBehaviour
{
    [SerializeField] private AudioSource openDoor;
    [SerializeField] private AudioSource closeDoor;
    [SerializeField] private Image imageFade;
    [SerializeField] private float fadeDuration;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(FadeIn());
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
            StartCoroutine(FadeOut());
            if (!closeDoor.isPlaying) 
            {
                closeDoor.Play();
            }           
        }
    }
    private IEnumerator FadeIn()
    {
        float elapsedTime = 0f;
        Color tempColor = imageFade.color;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            tempColor.a = Mathf.Clamp01(elapsedTime / fadeDuration);
            imageFade.color = tempColor;
            yield return null;
        }
    }
    private IEnumerator FadeOut()
    {
        float elapsedTime = 0f;
        Color tempColor = imageFade.color;
        while(elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            tempColor.a = 1 - Mathf.Clamp01(elapsedTime / fadeDuration);
            imageFade.color = tempColor;
            yield return null;
        }       
    }
}
