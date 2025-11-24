using UnityEngine;

public class GameLoop : MonoBehaviour
{
    [SerializeField] private CorgiPlayer player;
    [SerializeField] private CorgiNeedsSystem needsSystem;
    [SerializeField] private CorgiStateMachine stateMachine;

    private void Awake()
    {
        player ??= FindObjectOfType<CorgiPlayer>();
        needsSystem ??= player != null ? player.NeedsSystem : FindObjectOfType<CorgiNeedsSystem>();
        stateMachine ??= player != null ? player.StateMachine : FindObjectOfType<CorgiStateMachine>();
    }

    private void Update()
    {
        float deltaTime = Time.deltaTime;

        if (needsSystem != null)
        {
            needsSystem.Tick(deltaTime);
        }

        if (stateMachine != null)
        {
            stateMachine.Tick(deltaTime);
        }
    }
}
