using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance { get; private set; }

    [Header("Movement Settings")]
    public float moveSpeed = 5f;

    [Header("References")]
    public Animator animator;

    private Rigidbody rb;
    private InputManager inputActions;
    private Vector2 movementInput;
    private Transform camTransform;

    private void Awake()
    {
        Instance = this;
        inputActions = new InputManager();
    }

    private void OnEnable()
    {
        inputActions.Player.Enable();
        inputActions.Player.Move.performed += OnMovePerformed;
        inputActions.Player.Move.canceled += OnMoveCanceled;
    }

    private void OnDisable()
    {
        inputActions.Player.Move.performed -= OnMovePerformed;
        inputActions.Player.Move.canceled -= OnMoveCanceled;
        inputActions.Player.Disable();
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        camTransform = Camera.main.transform;
    }

    private void FixedUpdate()
    {
        // Obtener forward y right de la cámara, aplanados en el plano XZ
        Vector3 camForward = camTransform.forward;
        Vector3 camRight = camTransform.right;
        camForward.y = 0f;
        camRight.y = 0f;
        camForward.Normalize();
        camRight.Normalize();

        // Movimiento relativo a cámara, solo en XZ
        Vector3 moveDirection = camForward * movementInput.y + camRight * movementInput.x;
        moveDirection.Normalize(); // Asegura que no supere la magnitud 1 al moverse en diagonal
        Vector3 velocity = moveDirection * moveSpeed;

        // Aplicar la velocidad en XZ, mantener la Y actual
        Vector3 currentVelocity = rb.linearVelocity;
        rb.linearVelocity = new Vector3(velocity.x, currentVelocity.y, velocity.z);

        // Animación de velocidad
        if (animator != null)
        {
            float planarSpeed = new Vector2(rb.linearVelocity.x, rb.linearVelocity.z).magnitude;
            animator.SetFloat("Speed", planarSpeed);
        }

    }

    private void OnMovePerformed(InputAction.CallbackContext context)
    {
        movementInput = context.ReadValue<Vector2>();
    }

    private void OnMoveCanceled(InputAction.CallbackContext context)
    {
        movementInput = Vector2.zero;
    }
}
