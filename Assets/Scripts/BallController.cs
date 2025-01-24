using UnityEngine;

public class BallController : MonoBehaviour
{
    // floats Speed and tiltDirection, with Rigidbody rb

    #region|Variables|

    public InputsController inputsController;
    public Rigidbody rb;
    public ParticleSystem particles;
    public float defaultSpeed = 10f;
    public static float currentSpeed = 0f;
    public float ballDirection;

    #endregion

    // Start and FixedUpdate

    #region|Monobehaviour|

    // Moves the ball according to the given inputs, and sets the ballDirection value

    #region|FixedUpdate|

    private void FixedUpdate()
    {
        if (InputsController.isPlaying)
        {
            // Move the ball

            MoveBall(inputsController.keyboardHorizontalInput, inputsController.keyboardVerticalInput);

            // Set the tilt

            SetBallDIrection();

            ScaleCameraFov();
        }
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

        if (currentSpeed < 5f && particles.isPlaying)
        {
            particles.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        }

        else if (!particles.isPlaying)
        {
            ParticleSystem.EmissionModule emissionModule = particles.emission;
            emissionModule.enabled = true;
            particles.Play(true);
        }
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

    private void ScaleCameraFov()
    {
        float targetFov = Mathf.Lerp(60f, 90f, currentSpeed / defaultSpeed);
        Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, targetFov, Time.deltaTime * 5f);
    }

    #endregion
}