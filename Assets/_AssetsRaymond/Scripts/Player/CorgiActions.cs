using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class CorgiActions : MonoBehaviour
{
    private static readonly int BarkTrigger = Animator.StringToHash("Bark");
    private static readonly int SitBool = Animator.StringToHash("IsSitting");
    private static readonly int RollTrigger = Animator.StringToHash("Roll");
    private static readonly int SleepBool = Animator.StringToHash("IsSleeping");
    private static readonly int EatTrigger = Animator.StringToHash("Eat");

    [Header("Audio")]
    [SerializeField] private AudioClip barkClip;
    [SerializeField] private AudioClip eatClip;
    [SerializeField] private AudioClip rollClip;

    [Header("Needs")]
    [SerializeField] private string hungerNeedId = "hunger";
    [SerializeField] private string energyNeedId = "energy";

    [Header("Pickup")]
    [SerializeField, Min(0f)] private float dropImpulse = 3f;

    private AudioSource _audioSource;
    private CorgiPlayer _player;
    private Pickupable _heldItem;

    public bool IsHoldingItem => _heldItem != null;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void Configure(CorgiPlayer player)
    {
        _player = player;
    }

    public void Bark()
    {
        if (_player?.Animator == null) return;

        _player.Animator.SetTrigger(BarkTrigger);
        PlayClip(barkClip);
        _player.Visual?.PulseEmission();
    }

    public void Sit()
    {
        if (_player?.Animator == null) return;

        bool newValue = !_player.Animator.GetBool(SitBool);
        _player.Animator.SetBool(SitBool, newValue);

        if (newValue)
        {
            _player.StateMachine?.ChangeState<IdleState>();
        }
    }

    public void Roll()
    {
        if (_player?.Animator == null) return;

        _player.Animator.SetTrigger(RollTrigger);
        PlayClip(rollClip);
        _player.Visual?.PulseEmission();
    }

    public void Eat()
    {
        if (_player?.Animator == null) return;

        _player.Animator.SetTrigger(EatTrigger);
        PlayClip(eatClip);

        if (!string.IsNullOrWhiteSpace(hungerNeedId))
        {
            _player.NeedsSystem?.RestoreNeedDefault(hungerNeedId);
        }
    }

    public void Sleep()
    {
        if (_player?.Animator == null) return;

        _player.Animator.SetBool(SleepBool, true);
        _player.StateMachine?.ChangeState<SleepingState>();

        if (!string.IsNullOrWhiteSpace(energyNeedId))
        {
            _player.NeedsSystem?.RestoreNeedDefault(energyNeedId);
        }
    }

    public void WakeUp()
    {
        if (_player?.Animator == null) return;

        _player.Animator.SetBool(SleepBool, false);
        _player.StateMachine?.ChangeState<IdleState>();
    }

    public void Pickup(Pickupable pickupable)
    {
        if (pickupable == null || _player?.MouthAnchor == null) return;

        if (_heldItem == pickupable)
        {
            Drop();
            return;
        }

        if (_heldItem != null)
        {
            Drop();
        }

        _heldItem = pickupable;
        pickupable.Pickup(_player.MouthAnchor);
    }

    public void Drop()
    {
        if (_heldItem == null) return;

        Vector3 impulse = _player != null
            ? _player.transform.forward * dropImpulse
            : Vector3.forward * dropImpulse;

        _heldItem.Drop(impulse);
        _heldItem = null;
    }

    private void PlayClip(AudioClip clip)
    {
        if (_audioSource == null || clip == null) return;

        _audioSource.pitch = Random.Range(0.95f, 1.05f);
        _audioSource.PlayOneShot(clip);
    }
}
