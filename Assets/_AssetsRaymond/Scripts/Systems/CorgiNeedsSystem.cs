using System;
using System.Collections.Generic;
using UnityEngine;

public class CorgiNeedsSystem : MonoBehaviour
{
    [SerializeField] private List<NeedDefinition> needDefinitions = new();

    private readonly Dictionary<string, CorgiNeed> _needsById = new(StringComparer.OrdinalIgnoreCase);

    public event Action<CorgiNeed> NeedChanged;
    public event Action<CorgiNeed> NeedDepleted;

    public IReadOnlyDictionary<string, CorgiNeed> Needs => _needsById;

    private void Awake()
    {
        BuildNeedsDictionary();
    }

    public void Tick(float deltaTime)
    {
        if (_needsById.Count == 0) return;

        foreach (CorgiNeed need in _needsById.Values)
        {
            need.Tick(deltaTime);
        }
    }

    public bool TryGetNeed(string needId, out CorgiNeed need)
    {
        return _needsById.TryGetValue(needId, out need);
    }

    public void RestoreNeed(string needId, float amount)
    {
        if (!_needsById.TryGetValue(needId, out CorgiNeed need)) return;

        need.Modify(amount);
    }

    public void RestoreNeedDefault(string needId)
    {
        if (!_needsById.TryGetValue(needId, out CorgiNeed need)) return;

        need.RestoreDefaultAmount();
    }

    private void BuildNeedsDictionary()
    {
        foreach (CorgiNeed existingNeed in _needsById.Values)
        {
            existingNeed.ValueChanged -= HandleNeedChanged;
        }

        _needsById.Clear();

        foreach (NeedDefinition definition in needDefinitions)
        {
            if (definition == null || string.IsNullOrWhiteSpace(definition.NeedId)) continue;

            if (_needsById.ContainsKey(definition.NeedId))
            {
                Debug.LogWarning($"Duplicate need id detected: {definition.NeedId}. Only the first instance will be used.", this);
                continue;
            }

            var need = new CorgiNeed();
            need.Configure(definition);
            need.ValueChanged += HandleNeedChanged;

            _needsById.Add(definition.NeedId, need);
        }
    }

    private void HandleNeedChanged(CorgiNeed need)
    {
        NeedChanged?.Invoke(need);

        if (need.IsDepleted)
        {
            NeedDepleted?.Invoke(need);
        }
    }
}
