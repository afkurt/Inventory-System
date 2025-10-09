using UnityEngine;

public class CursorManager : MonoBehaviour
{
    public Texture2D newCursor;
    void Start()
    {
        Cursor.SetCursor(newCursor, Vector2.zero, CursorMode.Auto);
    }

    
}
