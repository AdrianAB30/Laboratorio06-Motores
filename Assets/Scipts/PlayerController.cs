using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Player Data")]
    private Rigidbody myRBD;
    private Animator myAnimator;
    private AudioSource audioSource;
    private Vector2 movement;
    private bool canMove = true;
    [SerializeField] private float speed;
    [SerializeField] NPC_Controller currentNPC;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        myRBD = GetComponent<Rigidbody>();
        myAnimator = GetComponent<Animator>();
    }
    private void FixedUpdate()
    {
        if (canMove)
        {
            myRBD.velocity = new Vector3(movement.x, myRBD.velocity.y, movement.y);
            AudioWalking();
        }
    }
    private void Update()
    {
        if (canMove)
        {
            RotatePlayer();
            AnimationWalk();
        }
    }
    private void RotatePlayer()
    {
        if (movement.y < 0)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else if (movement.y > 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }
    public void OnMovement(InputAction.CallbackContext context)
    {
        if (canMove)
        {
            movement = context.ReadValue<Vector2>() * speed;
        }
    }
    public void AudioWalking()
    {
        if (movement.magnitude > 0 && canMove)
        {
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }
        else
        {
            if (audioSource.isPlaying)
            {
                audioSource.Stop();
            }
        }
    }
    private void AnimationWalk()
    {
        myAnimator.SetFloat("VelX", movement.x);
        myAnimator.SetFloat("VelY", movement.y);
    }
    public void SetCanMove(bool value)
    {
        canMove = value;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Teleporter"))
        {
            SceneManager.LoadScene("Nivel2");
        }
        else if (other.CompareTag("Teleporter2"))
        {
            SceneManager.LoadScene("Nivel1");
        }
        else if (other.CompareTag("NPC"))
        {
            currentNPC = other.GetComponent<NPC_Controller>();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("NPC"))
        {
            currentNPC = null; 
        }
    } 
    public void OnInteractWithNPC(InputAction.CallbackContext context)
    {
        if(context.performed && currentNPC != null)
        {
            currentNPC.OnInteractWithPlayer(context);
        }
    }
}
