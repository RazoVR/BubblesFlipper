using UnityEngine;

public class MouseRotation : MonoBehaviour
{
    public InputsController inputsController;

    // Rotate
    public float rotationSpeed = 5f; 
    private float moveSpeed = 5f;
    public Transform cameraTransform;
    public Transform mouseTransform;

    // Animator
    public Animator animator;  
    public Rigidbody targetRigidbody; 
    public float speedMultiplier = 0.1f; // Multiplier for animation speed

    void FixedUpdate()
    {
        // Combine the input into a direction vector
        Vector3 inputDirection = new(inputsController.keyboardHorizontalInput, 0, inputsController.keyboardVerticalInput);

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
            Vector3 direction = (cameraForward * inputsController.keyboardVerticalInput + cameraRight * inputsController.keyboardHorizontalInput).normalized;

            // Calculate the target rotation
            Quaternion targetRotation = Quaternion.LookRotation(direction);

            // Smoothly rotate towards the target rotation
            mouseTransform.rotation = Quaternion.Slerp(mouseTransform.rotation, targetRotation, Time.deltaTime * rotationSpeed);

            // Move the object in the direction
            mouseTransform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
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