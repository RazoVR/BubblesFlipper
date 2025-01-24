using UnityEngine;

public class CameraController : MonoBehaviour
{
    // BallController and Transform references, with float variables

    #region|Variables|

    // References

    public InputsController inputsController;
    public BallController ballController;
    public Transform target;
    public GameObject retopo;
    public GameObject deform;

    // Public floats

    public float distance = 5.0f;
    public float xSpeed = 120.0f;
    public float ySpeed = 120.0f;
    public float yMinLimit = -20f;
    public float yMaxLimit = 80f;
    public float distanceMin = 3f;
    public float distanceMax = 10f;

    // Private floats

    private float x = 180f;
    private float y = 0f;
    public float targetDistance = 5f;

    #endregion

    // Start and LateUpdate

    #region|Monobehaviour|

    // Stores the default x and y transform.eulerAngles values

    #region|Start|

    // Stores the default x and y transform.eulerAngles values

    void Start()
    {
        if (inputsController.isPlaying)
        {
            targetDistance = distance;

            GetAnglesValues();
        }
    }

    #endregion

    // Sets the position and the rotation of the camera

    #region|LateUpdate|

    void LateUpdate()
    {
        if (inputsController.isPlaying && target)
        {
            SetCameraValues();
        }
    }

    #endregion

    #endregion

    // Basic camera controls

    #region|Methods|

    // Stores the default x and y transform.eulerAngles values

    #region|GetAnglesValues|

    private void GetAnglesValues()
    {
        Vector3 angles = Camera.main.transform.eulerAngles;
        x = angles.y;
        y = angles.x;
    }

    #endregion

    // Clamp a given angle by the given minimum and minimum values

    #region|ClampAngle|

    public static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360f)
        {
            angle += 360f;
        }

        if (angle > 360f)
        {
            angle -= 360f;
        }

        return Mathf.Clamp(angle, min, max);
    }

    #endregion

    // Sets the position and the rotation of the camera

    #region|SetCameraValues|

    private void SetCameraValues()
    {
        // Prepare the camera x and y rotation angles

        x += inputsController.mouseHorizontalInput * xSpeed * Time.deltaTime;
        y -= inputsController.mouseVerticalInput * ySpeed * Time.deltaTime;

        // Clamp the camera y angle

        y = ClampAngle(y, yMinLimit, yMaxLimit);

        // Change the target distance based on the scroll wheel input value

        targetDistance = Mathf.Clamp(targetDistance - inputsController.scrollInput * 5, distanceMin, distanceMax);

        // Set the camera rotation

        Quaternion rotation = Quaternion.Euler(y, x, ballController.ballDirection * BallController.currentSpeed / 5);

        // Set the desired position (ignoring obstacles and walls)

        Vector3 desiredPosition = target.position - (rotation * Vector3.forward * targetDistance);

        // Check if there are walls or obstacles behind the camera and adjust the distance if so

        if (Physics.Linecast(target.position, desiredPosition, out RaycastHit hit))
        {
            float adjustedDistance = hit.distance - 2f;
            distance = Mathf.Clamp(adjustedDistance, distanceMin, targetDistance);
        }

        // If no obstacle is detected, set the correct distance slowly

        else
        {
            distance = Mathf.Lerp(distance, targetDistance, Time.deltaTime * 5);
        }

        // Calculate and set the final position and rotation of the camera

        Vector3 negDistance = new(0.0f, 0.0f, -distance);
        Vector3 position = rotation * negDistance + target.position;

        if (distance < 1f)
        {
            retopo.SetActive(false);
            deform.SetActive(false);
        }

        else if (!retopo.activeSelf)
        {
            retopo.SetActive(true);
            deform.SetActive(true);
        }

        Camera.main.transform.SetPositionAndRotation(position, rotation);
    }


    #endregion

    // Reset the rotation of the camera

    #region|ResetCameraValues|

    public void ResetCameraValues()
    {
        x = 180f;
        y = 0f;
    }

    #endregion

    #endregion
}