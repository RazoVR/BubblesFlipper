using UnityEngine;

public class InputsController : MonoBehaviour
{
    [HideInInspector]
    public float horizontalInput;

    [HideInInspector]
    public float verticalInput;

    [HideInInspector]
    public float scrollInput;

    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        scrollInput = Input.GetAxis("Mouse ScrollWheel");
    }
}