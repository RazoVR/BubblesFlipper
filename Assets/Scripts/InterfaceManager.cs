using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class InterfaceManager : MonoBehaviour
{
    public InputsController inputsController;
    public MouseRotation mouseRotation;
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

            if (blackImage.color.a < 0.5f)
            {
                blackImage.raycastTarget = false;
            }

            yield return null;
        }

        color.a = 0f;
        blackImage.color = color;

        yield return null;
    }

    public void StartButtonClick()
    {
        StartCoroutine(PlayGameCoroutine());
    }

    private IEnumerator PlayGameCoroutine()
    {
        Camera.main.transform.GetPositionAndRotation(out Vector3 startPosition, out Quaternion startRotation);
        float startSpeed = mouseRotation.animator.speed;

        Vector3 targetPosition = new(0f, 0.75f, -5f);
        Quaternion targetRotation = Quaternion.identity;

        float duration = 3f;
        float elapsedTime = 0f;

        // Animate the camera position and rotation movement, and slow the mouse animator

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;

            // OutExpo animation

            t = 1f - Mathf.Pow(2f, -10f * t);

            mouseRotation.animator.speed = Mathf.Lerp(startSpeed, 0f, t);
            Camera.main.transform.SetPositionAndRotation(Vector3.Lerp(startPosition, targetPosition, t), Quaternion.Slerp(startRotation, targetRotation, t));

            yield return null;
        }

        // Validate

        mouseRotation.animator.speed = 0f;
        Camera.main.transform.SetPositionAndRotation(targetPosition, targetRotation);

        inputsController.playing = true;
    }
}