using UnityEngine;

public class CorgiCameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset = new(0f, 3.5f, -5f);
    [SerializeField, Min(0f)] private float followSmoothTime = 0.15f;
    [SerializeField, Min(0f)] private float rotateSmoothSpeed = 8f;
    [SerializeField, Range(0f, 1f)] private float lookAheadFactor = 0.2f;

    private Vector3 _followVelocity;

    private void LateUpdate()
    {
        if (target == null) return;

        Vector3 desiredPosition = target.position + target.TransformVector(offset);

        transform.position = Vector3.SmoothDamp(
            transform.position,
            desiredPosition,
            ref _followVelocity,
            followSmoothTime);

        Vector3 lookDirection = target.forward;
        if (lookDirection.sqrMagnitude < 0.01f)
        {
            lookDirection = (target.position - transform.position).normalized;
        }

        Vector3 lookTarget = target.position + lookDirection * lookAheadFactor;
        Quaternion targetRotation = Quaternion.LookRotation((lookTarget - transform.position).normalized, Vector3.up);

        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotateSmoothSpeed * Time.deltaTime);
    }
}