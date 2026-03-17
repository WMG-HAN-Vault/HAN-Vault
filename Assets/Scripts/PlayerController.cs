using EasyPeasyFirstPersonController;
using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public bool IsMakingSound()
    {
        FirstPersonController firstPersonController = GetComponent<FirstPersonController>();

        Debug.Log(firstPersonController.IsCrouching());

        return !firstPersonController.IsCrouching();
    }
}
