using UnityEngine;
using UnityEngine.UI;

public class NeedBarUI : MonoBehaviour
{
    [SerializeField] private CorgiNeedsSystem needsSystem;
    [SerializeField] private string needId;
    [SerializeField] private Slider slider;
    [SerializeField] private Image fillImage;

    private CorgiNeed _trackedNeed;

    private void Awake()
    {
        if (slider == null)
        {
            slider = GetComponentInChildren<Slider>();
        }
    }

    private void OnEnable()
    {
        ResolveNeed();

        if (needsSystem != null)
        {
            needsSystem.NeedChanged += HandleNeedChanged;
        }

        RefreshUI();
    }

    private void OnDisable()
    {
        if (needsSystem != null)
        {
            needsSystem.NeedChanged -= HandleNeedChanged;
        }
    }

    private void HandleNeedChanged(CorgiNeed changedNeed)
    {
        if (changedNeed == _trackedNeed)
        {
            RefreshUI();
        }
    }

    private void ResolveNeed()
    {
        if (needsSystem == null || string.IsNullOrWhiteSpace(needId)) return;

        needsSystem.TryGetNeed(needId, out _trackedNeed);

        if (_trackedNeed?.Definition != null && fillImage != null)
        {
            fillImage.color = _trackedNeed.Definition.BarColor;
        }
    }

    private void RefreshUI()
    {
        if (slider == null || _trackedNeed == null) return;

        slider.minValue = 0f;
        slider.maxValue = 1f;
        slider.value = _trackedNeed.NormalizedValue;

        if (fillImage != null && _trackedNeed.Definition != null)
        {
            fillImage.color = _trackedNeed.IsCritical
                ? Color.Lerp(_trackedNeed.Definition.BarColor, Color.red, 0.5f)
                : _trackedNeed.Definition.BarColor;
        }
    }
}
