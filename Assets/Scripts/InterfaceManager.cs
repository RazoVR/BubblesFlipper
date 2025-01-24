using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InterfaceManager : MonoBehaviour
{
    [Header("Scripts References")]
    public InputsController inputsController;
    public CameraController cameraController;
    public MouseRotation mouseRotation;
    public CheesesHandler cheesesHandler;

    [Header("Player References")]
    public Transform bubble;
    public Rigidbody rb;

    [Header("Audio Sources")]
    public AudioSource musicPlayer;
    public AudioSource soundEffectPlayer;

    [Header("User Interface Variables")]
    public Image cameraImage;
    public Slider sensitivitySlider;
    public Button backButton;
    public Button startButton;
    public Button helpButton;
    public Button exitButton;
    public TMP_Text sensitivityText;
    public TMP_Text chronoText;
    public TMP_Text counterText;
    public TMP_Text speedometerText;
    public TMP_Text startText;
    public TMP_Text helpText;
    public TMP_Text exitText;

    [Header("Sprites References")]
    public Sprite helpImage;
    public Sprite number3;
    public Sprite number2;
    public Sprite number1;
    public Sprite goText;

    [HideInInspector]
    public int cheesesCount = 0;

    [HideInInspector]
    public bool incrementCheeses = false;

    private float chronoElapsedTime = 0f;

    private void Start()
    {
        Application.targetFrameRate = 120;

        if (!InputsController.isPlaying)
        {
            StartCoroutine(Introduction());
        }

        else
        {
            InputsController.isPlaying = false;
            cameraImage.color = Color.clear;

            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;

            Camera.main.transform.localPosition = new(0f, 0.3f, -cameraController.distance);
            Camera.main.transform.localRotation = new(0, 0, 0, 1);
            cameraController.ResetCameraValues();

            mouseRotation.animator.speed = 0f;
            mouseRotation.mouseTransform.localRotation = new(0, 0, 0, 1);
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;

            startButton.image.color = Color.clear;
            helpButton.image.color = Color.clear;
            exitButton.image.color = Color.clear;

            startText.color = Color.clear;
            helpText.color = Color.clear;
            exitText.color = Color.clear;

            StartCoroutine(StartCountdown());
        }
    }

    private void Update()
    {
        if (InputsController.isPlaying && inputsController.keyboardRestartKey)
        {
            Restart();
        }

        if (InputsController.isPlaying && inputsController.keyboardEscapeKey)
        {
            InputsController.isPlaying = false;

            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;

            Restart();
        }

        if (incrementCheeses)
        {
            UpdateCheesesCount();
        }
    }

    private void FixedUpdate()
    {
        //  Le bon pedometer :3
        speedometerText.text = $"{Math.Round(BallController.currentSpeed, 2)} cm per second";
    }

    private IEnumerator Introduction()
    {
        sensitivitySlider.value = cameraController.speed;
        sensitivityText.text = $"Mouse sensitivity: {cameraController.speed}";

        Vector3 cameraPos = new(0f, 1f, 4f);
        Quaternion cameraRot = Quaternion.Euler(0f, 150, 0f);

        Camera.main.transform.localPosition = cameraPos;
        Camera.main.transform.localRotation = cameraRot;

        yield return new WaitForSeconds(1f);

        StartCoroutine(FadeImage(cameraImage, null, null, 0f, 1f));

        yield return new WaitForSeconds(1f);

        cameraImage.raycastTarget = false;
    }

    public void StartButtonClick()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        StartCoroutine(PlayGameCoroutine());
    }

    public void HelpButtonClick()
    {
        cameraImage.sprite = helpImage;
        cameraImage.color = Color.white;
        backButton.transform.parent.gameObject.SetActive(true);
    }

    public void ExitButtonClick()
    {
        #if UNITY_EDITOR

        UnityEditor.EditorApplication.isPlaying = false;

        #else

        Application.Quit();

        #endif
    }

    public void SliderValueChanged(float value)
    {
        cameraController.speed = Mathf.RoundToInt(value);
        sensitivitySlider.value = cameraController.speed;
        sensitivityText.text = $"Mouse sensitivity: {cameraController.speed}";
    }

    public void BackButtonClick()
    {
        cameraImage.color = Color.clear;
        backButton.transform.parent.gameObject.SetActive(false);
    }

    private IEnumerator PlayGameCoroutine()
    {
        Vector3 startPosition = Camera.main.transform.localPosition;
        Quaternion startRotation = Camera.main.transform.localRotation;

        Vector3 targetPosition = new(0f, 0.3f, -5f);
        Quaternion targetRotation = Quaternion.identity;

        float startSpeed = mouseRotation.animator.speed;

        StartCoroutine(StartCountdown());

        float duration = 3f;
        float elapsedTime = 0f;

        // Animate the camera position and rotation movement, and slow the mouse animator

        while (elapsedTime < duration)
        {
            if (elapsedTime > 1f && startButton.image.color.a == 1f)
            {
                StartCoroutine(FadeImage(null, startButton, null, 0f, 1f));
                StartCoroutine(FadeImage(null, helpButton, null, 0f, 1f));
                StartCoroutine(FadeImage(null, exitButton, null, 0f, 1f));

                StartCoroutine(FadeImage(null, null, startText, 0f, 1f));
                StartCoroutine(FadeImage(null, null, helpText, 0f, 1f));
                StartCoroutine(FadeImage(null, null, exitText, 0f, 1f));
            }

            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;

            // OutExpo animation

            t = 1f - Mathf.Pow(2f, -10f * t);

            mouseRotation.animator.speed = Mathf.Lerp(startSpeed, 0f, t);

            Camera.main.transform.localPosition = Vector3.Lerp(startPosition, targetPosition, t);
            Camera.main.transform.localRotation = Quaternion.Slerp(startRotation, targetRotation, t);

            yield return null;
        }

        // Validate

        mouseRotation.animator.speed = 0f;

        Camera.main.transform.localPosition = targetPosition;
        Camera.main.transform.localRotation = targetRotation;
    }

    private IEnumerator FadeImage(Image targetImage, Button targetButton, TMP_Text targetText, float desiredAlpha, float fadeDuration)
    {
        Color color = default;

        if (targetImage != null)
        {
            color = targetImage.color;
        }

        if (targetButton != null)
        {
            color = targetButton.image.color;
        }

        if (targetText != null)
        {
            color = targetText.color;
        }

        float originalAlpha = color.a;
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(originalAlpha, desiredAlpha, elapsedTime / fadeDuration);
            color.a = alpha;

            if (targetImage != null)
            {
                targetImage.color = color;
            }

            if (targetButton != null)
            {
                targetButton.image.color = color;
            }

            if (targetText != null)
            {
                targetText.color = color;
            }

            yield return null;
        }

        color.a = 0f;

        if (targetImage != null)
        {
            targetImage.color = color;
        }

        if (targetButton != null)
        {
            targetButton.image.color = color;
        }

        if (targetText != null)
        {
            targetText.color = color;
        }
    }

    public void StartChrono()
    {
        inputsController.chronoIsRunning = true;
        counterText.color = Color.white;
        chronoText.color = Color.white;
        speedometerText.color = Color.white;
        StartCoroutine(UpdateChrono());
    }

    public void StopChrono()
    {
        inputsController.chronoIsRunning = false;
    }

    public void ResetChrono()
    {
        chronoElapsedTime = 0f;
        chronoText.text = "00:00:000";
    }

    private IEnumerator UpdateChrono()
    {
        while (inputsController.chronoIsRunning)
        {
            chronoElapsedTime += Time.deltaTime;

            int minutes = Mathf.FloorToInt(chronoElapsedTime / 60f);
            int seconds = Mathf.FloorToInt(chronoElapsedTime % 60f);
            int milliseconds = Mathf.FloorToInt((chronoElapsedTime * 1000f) % 1000f);

            chronoText.text = $"{minutes:00}:{seconds:00}:{milliseconds:000}";

            yield return null;
        }
    }

    private void Restart()
    {
        SceneManager.LoadScene(0);
    }

    private IEnumerator StartCountdown()
    {
        cameraImage.sprite = number3;
        cameraImage.color = new(1f, 1f, 1f, 0.25f);

        float duration = 3f;
        float elapsedTime = 0f;

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

            yield return null;
        }

        cameraImage.sprite = goText;
        InputsController.isPlaying = true;
        musicPlayer.Play();

        StartCoroutine(FadeImage(cameraImage, null, null, 0f, 1f));
        StartChrono();
    }

    private void UpdateCheesesCount()
    {
        counterText.text = $"{cheesesCount}/20 Cheeses";
    }
}