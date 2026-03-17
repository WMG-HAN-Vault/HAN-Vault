using EasyPeasyFirstPersonController;
using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public bool IsMakingSound()
    {
        FirstPersonController firstPersonController = GetComponent<FirstPersonController>();

        return !firstPersonController.IsCrouching();
    }
}
