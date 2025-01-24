using UnityEngine;

public class FinishHandling : MonoBehaviour
{
    public InterfaceManager interfaceManager;

    private void OnTriggerEnter(Collider other)
    {
        interfaceManager.StopChrono();
    }
}
