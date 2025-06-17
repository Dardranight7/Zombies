using UnityEngine;

public class Door : PointableObject
{
    [Header("Configuración")]
    public Transform doorPivot; // El pivot que rota (usualmente la hoja de la puerta)
    public Transform doorHandle; // El objeto que representa el mango de la puerta
    public float minAngle = 0f; // Ángulo cerrado
    public float maxAngle = 90f; // Ángulo abierto
    public float rotationSpeed = 5f; // Sensibilidad al mover el mouse

    private bool isInteracting = false;
    private float currentAngle = 0f;

    public bool invertInput; // Invertir la dirección del movimiento del mouse

    void Update()
    {
        onClicked.AddListener(HandleDoorInteraction);
        

        if (Input.GetMouseButtonUp(0))
        {
            isInteracting = false;
        }

        HandleHandleRotation();
        if (isInteracting)
        {
            float mouseX = Input.GetAxis("Mouse X");
            if (invertInput)
                mouseX = -mouseX; // Invertir la dirección del movimiento del mouse si es necesario
            
            currentAngle += mouseX * rotationSpeed;
            currentAngle = Mathf.Clamp(currentAngle, minAngle, maxAngle);

            // Rotar la puerta alrededor de su eje Y
            doorPivot.localRotation = Quaternion.Euler(0, currentAngle, 0);
        }
    }

    public void HandleHandleRotation()
    {
        doorHandle.localRotation = Quaternion.RotateTowards(doorHandle.localRotation, Quaternion.Euler(0, 0, 90), 180 * Time.deltaTime); 
    }

    public void HandleDoorInteraction()
    {
        isInteracting = true;
    }
}
