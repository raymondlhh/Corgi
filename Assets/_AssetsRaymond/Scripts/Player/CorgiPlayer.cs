using UnityEngine;

public class CorgiPlayer : MonoBehaviour
{
    [field: SerializeField] public CorgiMovement Movement { get; private set; }
    [field: SerializeField] public CorgiActions Actions { get; private set; }
    [field: SerializeField] public CorgiNeedsSystem NeedsSystem { get; private set; }
    [field: SerializeField] public Animator Animator { get; private set; }
    [field: SerializeField] public Transform MouthAnchor { get; private set; }
    [field: SerializeField] public CorgiInteractionDetector InteractionDetector { get; private set; }
    [field: SerializeField] public CorgiStateMachine StateMachine { get; private set; }
    [field: SerializeField] public CorgiVisual Visual { get; private set; }

    public Vector3 Position => transform.position;

    private void Awake()
    {
        ResolveDependencies();
    }

    private void OnEnable()
    {
        if (NeedsSystem != null)
        {
            NeedsSystem.NeedChanged += HandleNeedChanged;
        }
    }

    private void OnDisable()
    {
        if (NeedsSystem != null)
        {
            NeedsSystem.NeedChanged -= HandleNeedChanged;
        }
    }

    private void ResolveDependencies()
    {
        Movement ??= GetComponent<CorgiMovement>();
        Actions ??= GetComponent<CorgiActions>();
        NeedsSystem ??= GetComponent<CorgiNeedsSystem>();
        Animator ??= GetComponentInChildren<Animator>();
        InteractionDetector ??= GetComponentInChildren<CorgiInteractionDetector>();
        StateMachine ??= GetComponent<CorgiStateMachine>();
        Visual ??= GetComponentInChildren<CorgiVisual>();

        if (Actions != null)
        {
            Actions.Configure(this);
        }

        if (StateMachine != null)
        {
            StateMachine.Configure(this);
        }
    }

    public bool TryGetNeed(string needId, out CorgiNeed need)
    {
        need = null;
        return NeedsSystem != null && NeedsSystem.TryGetNeed(needId, out need);
    }

    private void HandleNeedChanged(CorgiNeed _)
    {
        if (NeedsSystem == null || Visual == null) return;

        float total = 0f;
        int count = 0;

        foreach (CorgiNeed need in NeedsSystem.Needs.Values)
        {
            total += need.NormalizedValue;
            count++;
        }

        if (count > 0)
        {
            Visual.UpdateMood(total / count);
        }
    }
}
