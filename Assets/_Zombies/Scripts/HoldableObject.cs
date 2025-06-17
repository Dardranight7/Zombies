using UnityEngine;

public class HoldableObject : PointableObject
{
    public bool isHeld = false; // Flag to check if the object is held
    public Rigidbody rb; // Rigidbody component for physics interactions
    public float holdVelocity = 500f; // Speed at which the object moves towards the grab point
    public float angleLimit = 45f; // Limit for rotation angle
    float timeToHoldAgain = 0;
    public float releaseForce = 600f; // Force applied when the object is released with force

    private void Awake()
    {
        onClicked.AddListener(BeHolded); // Add listener for the click event to hold the object
    }

    private void OnDestroy()
    {
        onClicked.RemoveListener(BeHolded); // Remove listener for the click event to hold the object
    }

    private void Update()
    {
        if (isHeld)
        {
            if (Input.GetMouseButtonDown(0))
            {
                BeReleased();
                timeToHoldAgain = Time.time + 0.2f; // Set a cooldown time to prevent immediate re-holding
            }
            if (Input.GetMouseButtonDown(1))
            {
                BeReleasedWithForce();
                timeToHoldAgain = Time.time + 0.2f; // Set a cooldown time to prevent immediate re-holding
            }
        }
    }

    public void BeHolded()
    {
        if (isHeld || timeToHoldAgain - Time.time > 0)
            return;
        FirstPersonPointer.OnGrabObject?.Invoke(this, angleLimit); // Notify that this object is being held
        isHeld = true; // Set the held state to true
        rb.useGravity = false; // Disable gravity for the object while held
    }

    public void BeReleased()
    {
        isHeld = false; // Set the held state to false
        rb.useGravity = true; // Enable gravity for the object
        FirstPersonPointer.OnGrabObject?.Invoke(null, angleLimit); // Notify that this object is being held
    }

    public void BeReleasedWithForce()
    {
        isHeld = false; // Set the held state to false
        rb.useGravity = true; // Enable gravity for the object
        rb.AddForce(FirstPersonPointer.Instance.grabPoint.forward * releaseForce); // Add force to the object when released
        FirstPersonPointer.OnGrabObject?.Invoke(null, angleLimit); // Notify that this object is being held
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
