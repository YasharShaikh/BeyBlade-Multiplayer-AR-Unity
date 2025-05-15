using UnityEngine;

public class MovementController : MonoBehaviour
{
    [SerializeField] private Joystick joystick;
    [SerializeField] private float speed = 5f;
    [SerializeField] private float maxVelocityChange = 10f;
    [SerializeField] private float tiltAmount = 10.0f;
    private Vector3 velocityVector = Vector3.zero;
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        joystick = GetComponentInChildren<Joystick>();
        rb.freezeRotation = true; // Prevent rigidbody from rotating due to physics
    }

    private void Update()
    {
        float _xMovementInput = joystick.Horizontal;
        float _zMovementInput = joystick.Vertical;

        Vector3 _movementHorizontal = transform.right * _xMovementInput;
        Vector3 _movementVertical = transform.forward * _zMovementInput;

        Vector3 _movementVelocityVector = (_movementHorizontal + _movementVertical).normalized * speed;

        Move(_movementVelocityVector); 
    }

    private void FixedUpdate()
    {
        if (velocityVector != Vector3.zero)
        {
            Vector3 velocity = rb.linearVelocity;
            Vector3 velocityChange = velocityVector - velocity;

            // Clamp change to avoid sudden jerks
            velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
            velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
            velocityChange.y = 0f;

            rb.AddForce(velocityChange, ForceMode.VelocityChange); 
        }

        transform.rotation = Quaternion.Euler(joystick.Vertical* speed * tiltAmount,0.0f,(-1*joystick.Horizontal * speed * tiltAmount));
    }

    private void Move(Vector3 movementVelocityVector)
    {
        velocityVector = movementVelocityVector;
    }
}
