using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class InterfaceManager : MonoBehaviour
{
    public InputsController inputsController;
    public Image blackImage;

    private void Start()
    {
        StartCoroutine(Introduction());
    }

    private IEnumerator Introduction()
    {
        Vector3 cameraPos = new(0f, 1f, 4f);
        Quaternion cameraRot = Quaternion.Euler(0f, 150, 0f);

        Camera.main.transform.SetPositionAndRotation(cameraPos, cameraRot);

        yield return new WaitForSeconds(1f);

        Color color = blackImage.color;
        color.a = 1f;

        float fadeDuration = 2f;
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
            color.a = alpha;
            blackImage.color = color;
            yield return null;
        }

        color.a = 0f;
        blackImage.color = color;

        yield return null;
    }
}