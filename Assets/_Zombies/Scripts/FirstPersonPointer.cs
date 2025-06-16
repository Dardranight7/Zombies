using System;
using UnityEngine;
using UnityEngine.UI;

public class FirstPersonPointer : MonoBehaviour
{
    public static System.Action<PointableObject> OnObjectPointed;
    public static System.Action<InspectionableObject> OnInspectionateObject;
    public static System.Action<HoldableObject> OnGrabObject;
    public Transform grabPoint; // Transform where the object will be held
    public static FirstPersonPointer Instance { get; private set; }
    private PointableObject currentPointedObject;
    public Image pointerImage; // UI Image to represent the pointer

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked; // Lock the cursor to the center of the screen
        Cursor.visible = false; // Hide the cursor
        Instance = this; // Set the singleton instance
    }

    private void Update()
    {
        RayCastInMiddleOfScreen(); // Cast a ray from the center of the screen
        //Function to handle input for clicking on the pointed object
        if (Input.GetMouseButtonDown(0)) // Left mouse button click
        {
            if (currentPointedObject != null)
            {
                currentPointedObject.OnPointerClick(); // Invoke the click event on the pointed object
            }
        }
        RotateGrabPoint(); // Handle rotation of the grab point
    }

    // function to rotate the grab point, allowing the player to rotate the object they are holding 45 degrees to the left or right using input E and Q respectively.
    public void RotateGrabPoint()
    {
        if (Input.GetKey(KeyCode.E)) // Rotate right
        {
            RotateGrabPointRight();
        }
        else if (Input.GetKey(KeyCode.Q)) // Rotate left
        {
            RotateGrabPointLeft();
        }
    }

    private void RotateGrabPointLeft()
    {
        // limit the rotation to 45 degrees to the left in interpolation
        grabPoint.localRotation = Quaternion.RotateTowards(grabPoint.localRotation, Quaternion.Euler(0, 45f, 0), 360 * Time.deltaTime);
    }

    private void RotateGrabPointRight()
    {
        // limit the rotation to 45 degrees to the right in interpolation
        grabPoint.localRotation = Quaternion.RotateTowards(grabPoint.localRotation, Quaternion.Euler(0, -45f, 0), 360 * Time.deltaTime);
    }

    public void RayCastInMiddleOfScreen()
    {
        Ray ray = Camera.main.ScreenPointToRay(new Vector2(Screen.width / 2, Screen.height / 2));
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100f)) // Adjust the distance as needed
        {
            PointableObject pointableObject = hit.collider.GetComponent<PointableObject>();
            if (pointableObject != null)
            {
                OnObjectPointed?.Invoke(pointableObject);
            }
            else
            {
                OnObjectPointed?.Invoke(null);
            }
        }
        else
        {
            OnObjectPointed?.Invoke(null);
        }
    }

    private void Start()
    {
        OnObjectPointed += HandleObjectPointed;
    }

    private void OnDestroy()
    {
        OnObjectPointed -= HandleObjectPointed;
    }

    public void HandleObjectPointed(PointableObject pointableObject)
    {
        // Handle the object pointed event here
        if (pointableObject == null)
        {
            if (pointerImage != null)
            {
                currentPointedObject = null;
                // Cancela cualquier tween activo en este objeto
                LeanTween.cancel(pointerImage.gameObject);
                pointerImage.transform.LeanScale(new Vector3(1, 1, 1), 0.1f);
            }
        }
        else
        {
            // If the object is not null, update the pointer image and scale it and difrentiate it from the previous one
            if (currentPointedObject != pointableObject)
            {
                Debug.Log("Pointed at: " + pointableObject.name); 
                currentPointedObject = pointableObject;
                if (pointerImage != null)
                {
                    // Cancela cualquier tween activo en este objeto
                    LeanTween.cancel(pointerImage.gameObject);
                    pointerImage.transform.LeanScale(new Vector3(3, 3, 3),0.1f);
                }
            }
        }
    }
}
