using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestManager : MonoBehaviour, IInteractable
{
    public InventorySlot[] ChestSlots;
    public GameObject ChestUI;
    public Animator ChestAnimator;
    public Animator ChestUIAnimator;

    public bool isChestOpen = false;

    private void Awake()
    {
        ChestUIAnimator = ChestUI.GetComponent<Animator>();
        ChestAnimator = GetComponent<Animator>();    
    }
    public void Interact()
    {
        if (isChestOpen)
        {
            ChestUIAnimator.SetTrigger("Close");
            ChestAnimator.SetTrigger("Close");
            AudioManager.Instance.PlaySound(AudioManager.Instance.ChestClosingClip);
            isChestOpen = false;
            //StartCoroutine(DisableAfterAnimation(1f));

        }
        else
        {
            AudioManager.Instance.PlaySound(AudioManager.Instance.ChestOpeningClip);
            ChestUI.SetActive(true);
            isChestOpen = true;
            ChestUIAnimator.SetTrigger("Open");
            ChestAnimator.SetTrigger("Open");
            
        }
    }
    private IEnumerator DisableAfterAnimation(float delay)
    {
        yield return new WaitForSeconds(delay);
        ChestUI.SetActive(false);
    }

    

}
