using UnityEngine;

[CreateAssetMenu(menuName = "Corgi/Need Definition")]
public class NeedDefinition : ScriptableObject
{
    [SerializeField] private string needId = "hunger";
    [SerializeField] private string displayName = "Hunger";
    [SerializeField] private Color barColor = Color.white;
    [SerializeField, Min(0f)] private float maxValue = 100f;
    [SerializeField, Min(0f)] private float startingValue = 75f;
    [SerializeField, Min(0f)] private float decayPerSecond = 1.5f;
    [SerializeField, Min(0f)] private float restorePerInteraction = 35f;
    [SerializeField, Range(0f, 1f)] private float criticalThreshold = 0.2f;

    public string NeedId => needId;
    public string DisplayName => displayName;
    public Color BarColor => barColor;
    public float MaxValue => maxValue;
    public float StartingValue => Mathf.Clamp(startingValue, 0f, maxValue);
    public float DecayPerSecond => decayPerSecond;
    public float RestorePerInteraction => restorePerInteraction;
    public float CriticalThreshold => criticalThreshold;
}
