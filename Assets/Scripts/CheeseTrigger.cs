using UnityEngine;

public class CheeseTrigger : MonoBehaviour
{
    public InterfaceManager interfaceManager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Bubble")
        {
            interfaceManager.cheesesCount++;
            interfaceManager.incrementCheeses = true;
            interfaceManager.soundEffectPlayer.Play();

            gameObject.SetActive(false);
        }
    }
}