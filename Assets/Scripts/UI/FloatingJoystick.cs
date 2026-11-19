using UnityEngine;
using UnityEngine.EventSystems;

public class FloatingJoystick : MonoBehaviour,
    IPointerDownHandler,
    IDragHandler,
    IPointerUpHandler
{
    [SerializeField] private RectTransform background;
    [SerializeField] private RectTransform handle;
    [SerializeField] private float joystickRadius = 100f;

    public Vector2 InputDirection { get; private set; }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnDrag(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 position;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            background,
            eventData.position,
            eventData.pressEventCamera,
            out position
        );

        InputDirection = Vector2.ClampMagnitude(
            position / joystickRadius, 1f
        );

        handle.anchoredPosition = InputDirection * joystickRadius;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        InputDirection = Vector2.zero;
        handle.anchoredPosition = Vector2.zero;
    }
}