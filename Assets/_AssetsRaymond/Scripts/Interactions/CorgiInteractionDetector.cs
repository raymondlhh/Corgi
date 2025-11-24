using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class CorgiInteractionDetector : MonoBehaviour
{
    [SerializeField, Range(-1f, 1f)] private float forwardDotThreshold = -0.2f;

    private readonly List<IInteractable> _candidates = new();

    public IInteractable CurrentInteractable { get; private set; }

    public event System.Action<IInteractable> CurrentInteractableChanged;

    private void Awake()
    {
        if (TryGetComponent(out Collider collider))
        {
            collider.isTrigger = true;
        }
    }

    private void Update()
    {
        EvaluateCurrent();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent(out IInteractable interactable)) return;

        if (_candidates.Contains(interactable)) return;

        _candidates.Add(interactable);
        EvaluateCurrent();
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.TryGetComponent(out IInteractable interactable)) return;

        if (_candidates.Remove(interactable))
        {
            EvaluateCurrent();
        }
    }

    private void EvaluateCurrent()
    {
        IInteractable bestCandidate = null;
        float bestScore = float.MinValue;

        foreach (IInteractable candidate in _candidates)
        {
            if (candidate is not Component component) continue;

            Vector3 toTarget = component.transform.position - transform.position;
            if (toTarget.sqrMagnitude <= Mathf.Epsilon) continue;

            toTarget.Normalize();
            float dot = Vector3.Dot(transform.forward, toTarget);

            if (dot < forwardDotThreshold) continue;

            if (dot > bestScore)
            {
                bestScore = dot;
                bestCandidate = candidate;
            }
        }

        SetCurrent(bestCandidate);
    }

    private void SetCurrent(IInteractable interactable)
    {
        if (CurrentInteractable == interactable) return;

        CurrentInteractable = interactable;
        CurrentInteractableChanged?.Invoke(CurrentInteractable);
    }
}
