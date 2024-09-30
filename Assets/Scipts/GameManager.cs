using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Button optionsButton;
    [SerializeField] private Image optionsPanel;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private CinemachineVirtualCamera cameraPlayer;
    [SerializeField] private Transform teleporter;

    public void ShowOptions()
    {
        optionsPanel.gameObject.SetActive(true);
        playerController.SetCanMove(false);
        DisableCameraControl();
    }
    public void DissapearOptions()
    {
        optionsPanel.gameObject.SetActive(false);
        playerController.SetCanMove(true);
        EnabledCameraControl();
    }
    private void EnabledCameraControl()
    {
        cameraPlayer.enabled = true;
    }
    private void DisableCameraControl()
    {
        cameraPlayer.enabled = false;
    }
}
