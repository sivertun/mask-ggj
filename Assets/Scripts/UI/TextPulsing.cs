using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TextPulsing : MonoBehaviour
{
    private TextMeshProUGUI textComponent;
    public float pulseSpeed = 1.0f; // Speed of the pulse
    public float minAlpha = 0.2f;
    public float maxAlpha = 1.0f;

    void Start()
    {
        textComponent = GetComponent<TextMeshProUGUI>(); // Get the component
        if (textComponent != null)
        {
            StartCoroutine(PulseColor());
        }
    }

    IEnumerator PulseColor()
    {
        while (true) // Loop indefinitely
        {
            // Calculate a value between 0 and 1 using a sine wave
            float alpha = Mathf.Lerp(minAlpha, maxAlpha, (Mathf.Sin(Time.time * pulseSpeed) + 1f) / 2f);
            
            // Apply the new alpha to the text color
            Color color = textComponent.color;
            color.a = alpha;
            textComponent.color = color;

            yield return null; // Wait until the next frame
        }
    }
}