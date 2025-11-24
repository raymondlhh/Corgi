using UnityEngine;

[DisallowMultipleComponent]
public class CorgiVisual : MonoBehaviour
{
    [SerializeField] private SkinnedMeshRenderer meshRenderer;
    [SerializeField] private string pulseProperty = "_ActionPulse";
    [SerializeField] private string moodColorProperty = "_MoodColor";
    [SerializeField] private Gradient moodGradient;
    [SerializeField, Min(0.1f)] private float pulseDuration = 0.4f;

    private MaterialPropertyBlock _propertyBlock;
    private float _pulseTimer;

    private void Awake()
    {
        meshRenderer ??= GetComponentInChildren<SkinnedMeshRenderer>();
        _propertyBlock = new MaterialPropertyBlock();
    }

    private void Update()
    {
        if (_pulseTimer > 0f)
        {
            _pulseTimer -= Time.deltaTime;
            float normalizedTime = Mathf.Clamp01(_pulseTimer / pulseDuration);
            ApplyPulse(Mathf.SmoothStep(0f, 1f, normalizedTime));
        }
        else if (_pulseTimer < 0f)
        {
            _pulseTimer = 0f;
            ApplyPulse(0f);
        }
    }

    public void PulseEmission()
    {
        if (meshRenderer == null) return;

        _pulseTimer = pulseDuration;
        ApplyPulse(1f);
    }

    public void UpdateMood(float normalizedNeed)
    {
        if (meshRenderer == null || moodGradient == null) return;

        Color moodColor = moodGradient.Evaluate(Mathf.Clamp01(normalizedNeed));

        meshRenderer.GetPropertyBlock(_propertyBlock);
        if (!string.IsNullOrEmpty(moodColorProperty))
        {
            _propertyBlock.SetColor(moodColorProperty, moodColor);
        }
        meshRenderer.SetPropertyBlock(_propertyBlock);
    }

    private void ApplyPulse(float intensity)
    {
        if (meshRenderer == null || string.IsNullOrEmpty(pulseProperty)) return;

        meshRenderer.GetPropertyBlock(_propertyBlock);
        _propertyBlock.SetFloat(pulseProperty, intensity);
        meshRenderer.SetPropertyBlock(_propertyBlock);
    }
}
