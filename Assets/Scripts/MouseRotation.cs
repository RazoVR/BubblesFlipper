using UnityEngine;

public class MouseRotation : MonoBehaviour
{
    public float rotationSpeed = 5f; // Speed of rotation
    private float moveSpeed = 5f;     // Speed of movement

    void FixedUpdate()
    {
        // WTF codegpt a vraiment cook

        float horizontal = Input.GetAxis("Horizontal"); 
        float vertical = Input.GetAxis("Vertical");    

        // Combine the input into a direction vector
        Vector3 direction = new Vector3(horizontal, 0, vertical);

        if (direction.sqrMagnitude > 0.01f) // Avoid rotating when there's no input
        {
            // Calculate the target rotation
            Quaternion targetRotation = Quaternion.LookRotation(direction);

            // Smoothly rotate towards the target rotation
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }

        // Move the object in the current forward direction
        transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
    }
}