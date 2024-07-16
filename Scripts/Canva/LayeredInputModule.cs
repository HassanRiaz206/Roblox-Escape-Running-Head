using UnityEngine;
using UnityEngine.EventSystems;

public class LayeredInputModule : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public void OnPointerDown(PointerEventData eventData)
    {
        // Handle pointer down event if needed
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        // Handle pointer up event if needed
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (CameraRotation.Instance != null)
        {
            // No need to handle drag events specifically for camera rotation in this context
        }
    }
}