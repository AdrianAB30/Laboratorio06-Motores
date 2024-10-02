using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class NPC_Controller : MonoBehaviour
{
    [SerializeField] private SOs_DataNpc npcData;
    [SerializeField] private GameObject dialogueMark;
    [SerializeField] private GameObject panelDialogue;
    [SerializeField] private TMP_Text textDialogue;
    private float typingTime = 0.03f;
    private bool isPlayerInRange;
    private bool dialogueStart;
    private int lineIndexDialogue;
    public bool IsDialogueActive => dialogueStart;

    private void Update()
    {
        if(isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            if(!dialogueStart)
            {
                OnInteract();
            }
            else if(textDialogue.text == npcData.dialogueLines[lineIndexDialogue])
            {
                NextDialogue();
            }
            else
            {
                StopAllCoroutines();
                textDialogue.text = npcData.dialogueLines[lineIndexDialogue];
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
            dialogueMark.SetActive(true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            dialogueMark.SetActive(false);
        }
    }
    public void OnInteract()
    {
        dialogueStart = true;
        panelDialogue.SetActive(true);
        dialogueMark.SetActive(false);
        lineIndexDialogue = 0;
        StartCoroutine(ShowLine());
    }
    private void NextDialogue()
    {
        lineIndexDialogue++;
        if(lineIndexDialogue < npcData.dialogueLines.Length)
        {
            StartCoroutine(ShowLine());
        }
        else
        {
            dialogueStart=false;
            panelDialogue.SetActive(false);
            dialogueMark.SetActive(true);
        }
    }
    private IEnumerator ShowLine()
    {
        textDialogue.text = string.Empty;
        for (int i = 0; i < npcData.dialogueLines[lineIndexDialogue].Length; i++)
        {
            char ch = npcData.dialogueLines[lineIndexDialogue][i];
            textDialogue.text += ch;
            yield return new WaitForSeconds(typingTime);
        }
    }
}
