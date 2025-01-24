using UnityEngine;

public class FinishHandling : MonoBehaviour
{
    public InterfaceManager interfaceManager;

    private void OnTriggerEnter(Collider other)
    {
        interfaceManager.StopChrono();
        interfaceManager.cameraImage.sprite = interfaceManager.restartImage;
        interfaceManager.cameraImage.color = Color.white;
    }
}
