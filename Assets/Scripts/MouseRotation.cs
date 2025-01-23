using UnityEngine;

public class MouseRotation : MonoBehaviour
{
    // Rotate
    public float rotationSpeed = 5f; 
    private float moveSpeed = 5f;
    public Transform cameraTransform;

    // Animator
    public Animator animator;  
    public Rigidbody targetRigidbody; 
    public float speedMultiplier = 0.1f; // Multiplier for animation speed

    void FixedUpdate()
    {
        // WTF codegpt a vraiment cook

        // Rotation

        float horizontal = Input.GetAxis("Horizontal"); // A/D or Left/Right
        float vertical = Input.GetAxis("Vertical");     // W/S or Up/Down

        // Combine the input into a direction vector
        Vector3 inputDirection = new Vector3(horizontal, 0, vertical);

        if (inputDirection.sqrMagnitude > 0.01f) // Avoid rotating when there's no input
        {
            // Transform the input direction to align with the camera's forward
            Vector3 cameraForward = cameraTransform.forward;
            Vector3 cameraRight = cameraTransform.right;

            // Flatten the vectors to avoid tilting due to camera pitch
            cameraForward.y = 0;
            cameraRight.y = 0;

            // Normalize the vectors
            cameraForward.Normalize();
            cameraRight.Normalize();

            // Calculate the world-space direction based on camera orientation
            Vector3 direction = (cameraForward * vertical + cameraRight * horizontal).normalized;

            // Calculate the target rotation
            Quaternion targetRotation = Quaternion.LookRotation(direction);

            // Smoothly rotate towards the target rotation
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);

            // Move the object in the direction
            transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
        }


        // Animator

        {
            // Get the velocity magnitude of the object
            float velocity = targetRigidbody.linearVelocity.magnitude;

            // Adjust the animation speed based on the velocity
            animator.speed = Mathf.Clamp(velocity * speedMultiplier, 0.0f, 2f);
        }

    }
}