using UnityEngine;

public class CheeseTrigger : MonoBehaviour
{
    public InterfaceManager interfaceManager;

    private void OnTriggerEnter()
    {
        interfaceManager.cheesesCount++;
        interfaceManager.incrementCheeses = true;
        interfaceManager.soundEffectPlayer.Play();

        gameObject.SetActive(false);
    }
}