using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MouseCursorUguiController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
    public Texture2D cursorTexture;
    public CursorMode cursorMode = CursorMode.Auto;
    public Vector2 hotSpot = Vector2.zero;

    public void OnPointerEnter (PointerEventData eventData) {
        Debug.Log ("OnPointerEnter");
        Cursor.SetCursor (cursorTexture, hotSpot, cursorMode);
    }

    public void OnPointerExit (PointerEventData eventData) {
        Debug.Log ("OnPointerExit");
        Cursor.SetCursor (null, Vector2.zero, cursorMode);
    }
}
