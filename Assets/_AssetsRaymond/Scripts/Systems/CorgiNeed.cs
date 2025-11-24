using System;
using UnityEngine;

[Serializable]
public sealed class CorgiNeed
{
    [SerializeField] private NeedDefinition definition;
    [SerializeField] private float currentValue;

    public event Action<CorgiNeed> ValueChanged;

    public NeedDefinition Definition => definition;
    public float CurrentValue => currentValue;
    public float NormalizedValue => definition != null && definition.MaxValue > 0f
        ? currentValue / definition.MaxValue
        : 0f;
    public bool IsCritical => definition != null && NormalizedValue <= definition.CriticalThreshold;
    public bool IsDepleted => currentValue <= Mathf.Epsilon;

    public void Configure(NeedDefinition definitionToUse)
    {
        definition = definitionToUse;
        currentValue = definitionToUse != null ? definitionToUse.StartingValue : 0f;
        NotifyChanged();
    }

    public void Tick(float deltaTime)
    {
        if (definition == null || deltaTime <= 0f) return;
        Modify(-definition.DecayPerSecond * deltaTime);
    }

    public void RestoreDefaultAmount()
    {
        if (definition == null) return;
        Modify(definition.RestorePerInteraction);
    }

    public void Modify(float amount)
    {
        if (definition == null || Math.Abs(amount) <= Mathf.Epsilon) return;

        float previousValue = currentValue;
        currentValue = Mathf.Clamp(currentValue + amount, 0f, definition.MaxValue);

        if (!Mathf.Approximately(previousValue, currentValue))
        {
            NotifyChanged();
        }
    }

    private void NotifyChanged()
    {
        ValueChanged?.Invoke(this);
    }
}
