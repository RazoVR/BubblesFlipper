using UnityEngine;

public class CameraController : MonoBehaviour
{
    // BallController and Transform references, with float variables

    #region|Variables|

    // References

    public BallController ballController;
    public Transform target;

    // Public floats

    public float distance = 5.0f;
    public float xSpeed = 120.0f;
    public float ySpeed = 120.0f;
    public float yMinLimit = -20f;
    public float yMaxLimit = 80f;
    public float distanceMin = 3f;
    public float distanceMax = 10f;

    // Private floats

    private float x = 0.0f;
    private float y = 0.0f;

    #endregion

    // Start and LateUpdate

    #region|Monobehaviour|

    // Stores the default x and y transform.eulerAngles values

    #region|Start|

    // Stores the default x and y transform.eulerAngles values

    void Start()
    {
        GetAnglesValues();
    }

    #endregion

    // Sets the position and the rotation of the camera

    #region|LateUpdate|

    void LateUpdate()
    {
        if (target)
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
        Vector3 angles = transform.eulerAngles;
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
        x += Input.GetAxis("Mouse X") * xSpeed * Time.deltaTime;
        y -= Input.GetAxis("Mouse Y") * ySpeed * Time.deltaTime;

        y = ClampAngle(y, yMinLimit, yMaxLimit);

        Quaternion rotation = Quaternion.Euler(y, x, ballController.ballDirection * ballController.currentSpeed / 5);
        distance = Mathf.Clamp(distance - Input.GetAxis("Mouse ScrollWheel") * 5, distanceMin, distanceMax);

        if (Physics.Linecast(target.position, transform.position, out RaycastHit hit))
        {
            distance -= hit.distance;
        }

        Vector3 negDistance = new(0.0f, 0.0f, -distance);
        Vector3 position = rotation * negDistance + target.position;

        transform.SetPositionAndRotation(position, rotation);
    }

    #endregion

    #endregion
}