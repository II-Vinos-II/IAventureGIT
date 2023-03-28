using UnityEngine;

public class AimTargetRotation : MonoBehaviour
{
    [SerializeField] private Transform hand;
    [SerializeField] private float angleOffset;

    void Update() {
        transform.LookAt(hand);
        transform.Rotate(0, angleOffset, 0);
    }
}
