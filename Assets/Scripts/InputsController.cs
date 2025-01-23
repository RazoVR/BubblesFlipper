using UnityEngine;

public class InputsController : MonoBehaviour
{
    [HideInInspector] public float keyboardHorizontalInput;
    [HideInInspector] public float keyboardVerticalInput;
    [HideInInspector] public float mouseHorizontalInput;
    [HideInInspector] public float mouseVerticalInput;
    [HideInInspector] public float scrollInput;

    public bool playing;

    private void Start()
    {
        playing = false;
    }

    void Update()
    {
        keyboardHorizontalInput = Input.GetAxis("Horizontal");
        keyboardVerticalInput = Input.GetAxis("Vertical");

        mouseHorizontalInput = Input.GetAxis("Mouse X");
        mouseVerticalInput = Input.GetAxis("Mouse Y");

        scrollInput = Input.GetAxis("Mouse ScrollWheel");
    }
}