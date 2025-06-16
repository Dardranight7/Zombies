using UnityEngine;

public class HoldableObject : PointableObject
{
    public bool isHeld = false; // Flag to check if the object is held
    public Rigidbody rb; // Rigidbody component for physics interactions
    public float holdVelocity = 500f; // Speed at which the object moves towards the grab point

    private void Awake()
    {
        onClickToToggle.AddListener(state =>
        {
            if (state)
            {
                BeHolded();
            }
            else
            {
                BeReleased();
            }
        }); // Add listener for click to toggle hold state
    }

    public void BeHolded()
    {
        FirstPersonPointer.OnGrabObject?.Invoke(this); // Notify that this object is being held
        isHeld = true; // Set the held state to true
        rb.useGravity = false; // Disable gravity for the object while held
    }

    public void BeReleased()
    {
        isHeld = false; // Set the held state to false
        rb.useGravity = true; // Enable gravity for the object
        FirstPersonPointer.OnGrabObject?.Invoke(null); // Notify that this object is being held
    }

    public void FixedUpdate()
    {
        if (isHeld)
        {
            UpdatePositionWhenHeldUsingPhysics(); // Update position when held using physics
        }
    }

    //This function is called when the object is held, add a force to rigidbody to move towards grab point.
    public void UpdatePositionWhenHeldUsingPhysics()
    {   
        rb.linearVelocity = (FirstPersonPointer.Instance.grabPoint.position - transform.position) * holdVelocity * Time.fixedDeltaTime; // Adjust the multiplier as needed for speed
        rb.rotation = Quaternion.RotateTowards(transform.rotation, FirstPersonPointer.Instance.grabPoint.rotation, 360 * Time.fixedDeltaTime); // Rotate towards the grab point
    }
}
