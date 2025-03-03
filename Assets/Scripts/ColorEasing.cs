using UnityEngine;

public class ColorEasing : MonoBehaviour
{
    [SerializeField] private Renderer objectRenderer; // Assign the object's Renderer in the Inspector
    [SerializeField] private Color targetColor = Color.red; // Color to ease into
    [SerializeField] private float easeDuration = 2f; // Time to ease in/out
    [SerializeField] private float holdDuration = 1f; // Time to hold the target color

    private Color originalColor; // The starting color of the object
    private float timer; // Tracks time for transitions
    public bool easingIn = false; // Whether we're easing into the color
    private bool holding = false; // Whether we're holding the target color

    private void Start()
    {
        // Get the original color from the object's material
        if (objectRenderer == null)
        {
            objectRenderer = GetComponent<Renderer>();
        }

        if (objectRenderer != null)
        {
            originalColor = objectRenderer.material.color;
        }
        else
        {
            Debug.LogError("Renderer not assigned or found!");
        }
    }

    private void Update()
    {
        if (objectRenderer == null) return;

        // Update the timer
        timer += Time.deltaTime;

        if (easingIn)
        {
            timer = 0;
            // Easing into the target color
            float t = Mathf.Clamp01(timer / easeDuration);
            objectRenderer.material.color = Color.Lerp(originalColor, targetColor, EaseInOutQuad(t));

            if (t >= 1f)
            {
                easingIn = false;
                holding = true;
                timer = 0f;
            }
        }
        else if (holding)
        {
            // Holding the target color
            if (timer >= holdDuration)
            {
                holding = false;
                timer = 0f;
            }
        }
        else
        {
            // Easing back to the original color
            float t = Mathf.Clamp01(timer / easeDuration);
            objectRenderer.material.color = Color.Lerp(targetColor, originalColor, EaseInOutQuad(t));

            if (t >= 1f)
            {
                enabled = false; // Disable the script when the transition is complete
            }
        }
    }

    // Easing function for smoother transitions
    private float EaseInOutQuad(float t)
    {
        return t < 0.5f ? 2f * t * t : 1f - Mathf.Pow(-2f * t + 2f, 2f) / 2f;
    }
}
