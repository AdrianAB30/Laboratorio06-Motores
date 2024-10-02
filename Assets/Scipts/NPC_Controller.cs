using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class NPC_Controller : MonoBehaviour
{
    [SerializeField] private Animator _animation;
    [SerializeField] private SOs_DataNpc npcData;
    [SerializeField] private Transform[] pointsPatrol;
    private Transform currentPatrol;
    private Transform lastPatrol;
    private int patrolIndex = 0;
    private Vector3 movementNPC;
    [SerializeField] private GameObject dialogueMark;
    [SerializeField] private GameObject panelDialogue;
    [SerializeField] private TMP_Text textDialogue;
    private float typingTime = 0.03f;
    private bool isPlayerInRange;
    private bool dialogueStart;
    private bool npcCanMove;
    private int lineIndexDialogue;

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
        if (npcCanMove)
        {
            NpcPatrol();
            AnimationNPC();
        }
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
        if (other.CompareTag("Puntos"))
        {
            npcCanMove = true;
            _animation.SetFloat("Speed", npcData.velocity);
            StartCoroutine(DelayNPC());
            MoveNextPatrol();        
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
    private void NpcPatrol()
    {
        movementNPC = (currentPatrol.position - transform.position).normalized;
        transform.position = Vector3.MoveTowards(transform.position, currentPatrol.position, npcData.velocity * Time.deltaTime);

        if(Vector3.Distance(transform.position,currentPatrol.position) < 0.2f)
        {
            StartCoroutine(DelayNPC());
            MoveNextPatrol();
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
    private IEnumerator DelayNPC()
    {
        yield return new WaitForSeconds(npcData.patrolStopDuration);
        MoveNextPatrol();
        npcCanMove = true;
    }
}
