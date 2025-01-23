using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InterfaceManager : MonoBehaviour
{
    public InputsController inputsController;
    public MouseRotation mouseRotation;

    public Image cameraImage;
    public Button backButton;
    public TMP_Text chronoText;

    public Sprite helpImage;
    public Sprite number3;
    public Sprite number2;
    public Sprite number1;
    public Sprite goText;

    private bool isRunning = false;
    private float chronoElapsedTime = 0f;

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

        Color color = cameraImage.color;
        color.a = 1f;

        float fadeDuration = 1f;
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
            color.a = alpha;

            cameraImage.color = color;

            yield return null;
        }

        color.a = 0f;
        cameraImage.color = color;
        cameraImage.raycastTarget = false;

        yield return null;
    }

    public void StartButtonClick()
    {
        StartCoroutine(PlayGameCoroutine());
    }

    public void HelpButtonClick()
    {
        cameraImage.sprite = helpImage;
        cameraImage.color = Color.white;
        backButton.gameObject.SetActive(true);
    }

    public void ExitButtonClick()
    {
        #if UNITY_EDITOR

        UnityEditor.EditorApplication.isPlaying = false;

        #else

        Application.Quit();

        #endif
    }

    public void BackButtonClick()
    {
        cameraImage.color = Color.clear;
        backButton.gameObject.SetActive(false);
    }

    private IEnumerator PlayGameCoroutine()
    {
        cameraImage.sprite = number3;
        cameraImage.color = new(1f, 1f, 1f, 0.25f);

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

            if (elapsedTime > 1f && elapsedTime < 2f)
            {
                cameraImage.sprite = number2;
            }

            else if (elapsedTime > 2f && elapsedTime < 3f)
            {
                cameraImage.sprite = number1;
            }

            float t = elapsedTime / duration;

            // OutExpo animation

            t = 1f - Mathf.Pow(2f, -10f * t);

            mouseRotation.animator.speed = Mathf.Lerp(startSpeed, 0f, t);
            Camera.main.transform.SetPositionAndRotation(Vector3.Lerp(startPosition, targetPosition, t), Quaternion.Slerp(startRotation, targetRotation, t));

            yield return null;
        }

        // Validate

        cameraImage.sprite = goText;
        mouseRotation.animator.speed = 0f;
        Camera.main.transform.SetPositionAndRotation(targetPosition, targetRotation);

        inputsController.isPlaying = true;
        chronoText.gameObject.SetActive(true);

        StartChrono();

        // And fade out the cameraImage

        Color color = cameraImage.color;
        color.a = 0.25f;

        float fadeDuration = 1f;
        elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(0.25f, 0f, elapsedTime / fadeDuration);
            color.a = alpha;

            cameraImage.color = color;

            yield return null;
        }

        color.a = 0f;
        cameraImage.color = color;
    }

    public void StartChrono()
    {
        isRunning = true;
        StartCoroutine(UpdateChrono());
    }

    public void StopChrono()
    {
        isRunning = false;
    }

    public void ResetChrono()
    {
        chronoElapsedTime = 0f;
        chronoText.text = "00:00:000";
    }

    private IEnumerator UpdateChrono()
    {
        while (isRunning)
        {
            chronoElapsedTime += Time.deltaTime;

            int minutes = Mathf.FloorToInt(chronoElapsedTime / 60f);
            int seconds = Mathf.FloorToInt(chronoElapsedTime % 60f);
            int milliseconds = Mathf.FloorToInt((chronoElapsedTime * 1000f) % 1000f);

            chronoText.text = $"{minutes:00}:{seconds:00}:{milliseconds:000}";

            yield return null;
        }
    }
}