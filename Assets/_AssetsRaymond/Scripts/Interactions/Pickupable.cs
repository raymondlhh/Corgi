using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Pickupable : MonoBehaviour
{
    [SerializeField] private Rigidbody rigidBody;
    [SerializeField] private Transform visualRoot;

    public bool IsHeld { get; private set; }

    private void Awake()
    {
        rigidBody ??= GetComponent<Rigidbody>();
        visualRoot ??= transform;
    }

    public void Pickup(Transform parent)
    {
        if (parent == null) return;

        IsHeld = true;

        if (rigidBody != null)
        {
            rigidBody.isKinematic = true;
            rigidBody.detectCollisions = false;
        }

        transform.SetParent(parent);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
    }

    public void Drop(Vector3 impulse)
    {
        IsHeld = false;

        transform.SetParent(null);

        if (rigidBody == null) return;

        rigidBody.isKinematic = false;
        rigidBody.detectCollisions = true;
        rigidBody.linearVelocity = Vector3.zero;
        rigidBody.angularVelocity = Vector3.zero;

        if (impulse.sqrMagnitude > 0.001f)
        {
            rigidBody.AddForce(impulse, ForceMode.Impulse);
        }
    }
}
