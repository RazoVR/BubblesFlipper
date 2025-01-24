using UnityEngine;

public class CheeseTrigger : MonoBehaviour
{
    private void OnTriggerEnter()
    {
         gameObject.SetActive(false);
    }
}