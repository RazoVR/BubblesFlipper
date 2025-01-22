using UnityEngine;

public class BallController : MonoBehaviour
{
    // floats Speed and tiltDirection, with Rigidbody rb

    #region|Variables|

    public float defaultSpeed = 10f;
    public float currentSpeed = 0f;
    public float ballDirection;

    private Rigidbody rb;

    #endregion

    // Start and FixedUpdate

    #region|Monobehaviour|

    // Gets the RigidBody

    #region|Start|

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    #endregion

    // Moves the ball according to the given inputs, and sets the ballDirection value

    #region|FixedUpdate|

    private void FixedUpdate()
    {
        // Move the ball

        MoveBall(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        // Set the tilt

        SetBallDIrection();
    }

    #endregion

    #endregion

    // Moves the ball and sets the ballDirection value

    #region|Methods|

    // Moves the ball according to the user input

    #region|MoveBall|

    private void MoveBall(float moveHorizontal, float moveVertical)
    {
        Vector3 forward = Camera.main.transform.forward;
        Vector3 right = Camera.main.transform.right;

        // Womp womp l'axe Y

        forward.y = 0;
        right.y = 0;

        forward.Normalize();
        right.Normalize();

        Vector3 movement = (forward * moveVertical + right * moveHorizontal).normalized;

        rb.AddForce(movement * defaultSpeed);
        currentSpeed = rb.linearVelocity.magnitude;
    }

    #endregion

    // Sets the ballDirection value based on the camera view

    #region|SetBallDirection|

    private void SetBallDIrection()
    {
        // Set the camera tilt value

        Vector3 ballNormalizedVelocity = rb.linearVelocity.normalized;
        ballDirection = Vector3.Dot(Camera.main.transform.right, ballNormalizedVelocity);
    }

    #endregion

    #endregion
}