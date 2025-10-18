using UnityEngine;

public class OpenChestButton : MonoBehaviour
{
    public GameObject Chest;

    public void ToggleActive()
    {
        Chest.SetActive(!Chest.activeSelf);
    }
}
