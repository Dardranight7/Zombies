using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class PointableObject : MonoBehaviour
{
    public UnityEvent onClicked; // UnityEvent to handle pointer events
    public UnityEvent<bool> onClickToToggle;
    bool lastState = false; // Track the last state for toggling
    public void OnPointerClick()
    {
        // Handle pointer click event here if needed
        Debug.Log("PointableObject clicked: " + gameObject.name);
        onClicked?.Invoke(); // Invoke the UnityEvent when clicked
        onClickToToggle?.Invoke(lastState); // Toggle the state
        lastState = !lastState; // Update the last state
    }
}
