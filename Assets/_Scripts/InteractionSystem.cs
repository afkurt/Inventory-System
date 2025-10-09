using UnityEngine;

public class InteractionSystem : MonoBehaviour
{
    public float lastInteractionTime = -999f;
    private float interactionCooldown = 1f;
    
    
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            if (Time.time - lastInteractionTime < interactionCooldown)
                return;

            lastInteractionTime = Time.time;



            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if(Physics.Raycast(ray, out RaycastHit hit)) // buray� tam anlamad�m
            {
                IInteractable interactable = hit.collider.GetComponent<IInteractable>();
                if(interactable != null)
                {
                    interactable.Interact();
                    Debug.Log("Etkile�ime ge�ildi");
                }
                else
                {
                    Debug.Log("Bo�a t�klad�m");
                }
            }
        }
    }
}
