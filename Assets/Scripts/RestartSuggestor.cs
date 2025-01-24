using UnityEngine;

public class RestartSuggestor : MonoBehaviour
{
    public InterfaceManager interfaceManager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Bubble")
        {
            interfaceManager.cameraImage.sprite = interfaceManager.restartImage;
            interfaceManager.cameraImage.color = Color.white;
        }
    }
}
