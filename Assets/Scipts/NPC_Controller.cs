using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.InputSystem;

public class NPC_Controller : MonoBehaviour
{
    [Header("NPC Data")]
    [SerializeField] private Animator _animation;
    [SerializeField] private SOs_DataNpc npcData;
    private Vector3 movementNPC;
    [Header("Patrol Objects and Data")]
    [SerializeField] private Transform[] pointsPatrol;
    [SerializeField] private Transform playerTransform;
    private Transform currentPatrol;
    private Transform lastPatrol;
    private int patrolIndex = 0;
    [Header("UI Dialogue")]
    [SerializeField] private GameObject dialogueMark;
    [SerializeField] private GameObject panelDialogue;
    [SerializeField] private TMP_Text textDialogue;
    [Header("Dialogue Data")]
    private float typingTime = 0.03f;
    private int lineIndexDialogue;
    [Header("Booleanos")]
    private bool isPlayerInRange;
    private bool dialogueStart;
    private bool npcCanMove = true;

    private void Awake()
    {
        _animation = GetComponent<Animator>();
    }
    private void Start()
    {
        currentPatrol = pointsPatrol[patrolIndex];
        transform.position = currentPatrol.position;
        lastPatrol = currentPatrol;
    }
    private void Update()
    {
        if (!dialogueStart && npcCanMove)
        {
            NpcPatrol();
            AnimationNPC();
        }
        else if (dialogueStart)
        {
            RotateNpcToPlayer();
            _animation.SetFloat("Speed", 0f);
        }      
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Puntos"))
        {
            npcCanMove = false;
            _animation.SetFloat("Speed",0f);
            StartCoroutine(DelayNPC());    
        }
        else if (other.CompareTag("Player"))
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
    public void OnInteractWithPlayer(InputAction.CallbackContext context) 
    { 
        if(context.performed && isPlayerInRange)
        {
            if (!dialogueStart)
            {
                StartDialogue();
            }
            else if (textDialogue.text == npcData.dialogueLines[lineIndexDialogue])
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
    private void NpcPatrol()
    {
        movementNPC = (currentPatrol.position - transform.position).normalized;
        RotatePatrol();
        transform.position = Vector3.MoveTowards(transform.position, currentPatrol.position, npcData.velocity * Time.deltaTime);

        if(Vector3.Distance(transform.position,currentPatrol.position) < 0.2f)
        {
            StartCoroutine(DelayNPC());
            npcCanMove = false;
        }
    }
    private void MoveNextPatrol()
    {
        lastPatrol = currentPatrol;
        patrolIndex = (patrolIndex + 1) % pointsPatrol.Length;
        currentPatrol = pointsPatrol[patrolIndex];
    }
    private void RotatePatrol()
    {
        Vector3 target = (currentPatrol.position - transform.position).normalized;
        if (target != Vector3.zero)
        {
            Quaternion rotation = Quaternion.LookRotation(target);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 6f);
        }
    }
    private void RotateNpcToPlayer()
    {
        Vector3 directionToPlayer = (playerTransform.position - transform.position).normalized;
        if (directionToPlayer != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(directionToPlayer);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 6f);
        }
    }
    private void AnimationNPC()
    {
        if (npcCanMove)
        {
            float movementVelocityNpc = npcData.velocity;
            _animation.SetFloat("Speed", movementVelocityNpc);
        }
        else
        {
            _animation.SetFloat("Speed", 0f);
        }
    }
    private void StartDialogue()
    {
        dialogueStart = true;
        panelDialogue.SetActive(true);
        dialogueMark.SetActive(false);
        npcCanMove = false;
        _animation.SetBool("isTalking", true);
        RotateNpcToPlayer();
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
            _animation.SetBool("isTalking",false);
            _animation.SetFloat("Speed", npcData.velocity);
            npcCanMove = true;
            MoveNextPatrol();
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
    private IEnumerator DelayNPC()
    {
        npcCanMove = false;
        _animation.SetFloat("Speed", 0f);
        yield return new WaitForSeconds(npcData.patrolStopDuration);
        MoveNextPatrol();
        _animation.SetFloat("Speed", npcData.velocity);
        npcCanMove =true;
    }
}
