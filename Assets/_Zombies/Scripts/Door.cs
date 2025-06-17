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

    public AudioSource audioSource; // Fuente de audio para reproducir sonidos
    public AudioClip openSound; // Sonido al abrir la puerta
    public AudioClip closeSound; // Sonido al cerrar la puerta


    private void Awake()
    {
        onClicked.AddListener(HandleDoorInteraction);
    }

    private void OnDestroy()
    {
        onClicked.RemoveAllListeners(); // Eliminar todos los listeners al destruir el objeto
    }

    void Update()
    {   

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

            if (maxAngle - currentAngle <= 2 && closeIsPlayed == false) // Si la puerta está casi cerrada y el sonido está reproduciéndose
            {
                audioSource.PlayOneShot(closeSound); // Reproducir sonido de cierre
                closeIsPlayed = true; // Marcar que el sonido de cierre ya se ha reproducido
            }
            if (maxAngle - currentAngle > 6 )
            {
                closeIsPlayed = false; // Reiniciar la variable si la puerta se abre de nuevo
            }
        }
    }

    bool closeIsPlayed = false; // Variable para verificar si el sonido de cierre ya se ha reproducido

    public void HandleHandleRotation()
    {
        if (isInteracting)
        {
            doorHandle.localRotation = Quaternion.RotateTowards(doorHandle.localRotation, Quaternion.Euler(0, 0, 45), 180 * Time.deltaTime); 
        }
        else
        {
            doorHandle.localRotation = Quaternion.RotateTowards(doorHandle.localRotation, Quaternion.Euler(0, 0, 0), 180 * Time.deltaTime);
        }
    }

    public void HandleDoorInteraction()
    {
        // Verificar si el jugador está lo suficientemente cerca de la puerta para interactuar
        if ((PlayerController.Instance.transform.position - transform.position).sqrMagnitude < 3f * 3f)
        {
            isInteracting = true;
            audioSource.pitch = Random.Range(0.9f, 1.1f); // Ajustar el pitch aleatoriamente
            audioSource.PlayOneShot(openSound); // Reproducir sonido de apertura
        }
    }
}
