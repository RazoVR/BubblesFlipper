using System.Collections;
using UnityEngine;

public class CheesesHandler : MonoBehaviour
{
    public Transform[] cheesesTransforms;
    public float bounceHeight = 0.5f;
    public float bounceSpeed = 2.0f;
    public float rotationSpeed = 50.0f;

    public void Start()
    {
        foreach (Transform cheeseTransform in cheesesTransforms)
        {
            if (cheeseTransform.gameObject.activeSelf)
            {
                StartCoroutine(AnimateCheese(cheeseTransform));
            }
        }
    }

    private IEnumerator AnimateCheese(Transform cheeseTransform)
    {
        float initialY = cheeseTransform.position.y;

        while (cheeseTransform.gameObject.activeSelf)
        {
            float newY = initialY + Mathf.Sin(Time.time * bounceSpeed) * bounceHeight;

            Vector3 newPosition = new(cheeseTransform.position.x, newY, cheeseTransform.position.z);
            cheeseTransform.position = newPosition;

            cheeseTransform.Rotate(Vector3.up, rotationSpeed);

            yield return null;
        }
    }

    public void ResetCheeses()
    {
        foreach (Transform cheeseTransform in cheesesTransforms)
        {
            cheeseTransform.gameObject.SetActive(true);
            StartCoroutine(AnimateCheese(cheeseTransform));
        }
    }
}