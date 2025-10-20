using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestManager : MonoBehaviour, IInteractable
{
    public InventorySlot[] ChestSlots;
    public GameObject ChestUI;
    public Animator ChestAnimator;
    public Animator ChestUIAnimator;
    public AudioManager AudioManager;
    

    public bool isChestOpen = false;

    private void Awake()
    {
        ChestUIAnimator = ChestUI.GetComponent<Animator>();
        ChestAnimator = GetComponent<Animator>();    
        AudioManager = AudioManager.Instance;
    }
    public void Interact()
    {
        if (isChestOpen)
        {
            ChestUIAnimator.SetTrigger("Close");
            ChestAnimator.SetTrigger("Close");
            AudioManager.PlaySound(AudioManager.ChestClosingClip);
            isChestOpen = false;

        }
        else
        {
            AudioManager.PlaySound(AudioManager.ChestOpeningClip);
            ChestUI.SetActive(true);
            ChestUIAnimator.SetTrigger("Open");
            ChestAnimator.SetTrigger("Open");
            StartCoroutine(WaitForAnimation());
        }
    }

    private IEnumerator WaitForAnimation()
    {
        yield return new WaitForSeconds(1f);
        isChestOpen = true;
    }
}
