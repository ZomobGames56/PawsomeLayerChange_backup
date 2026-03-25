using UnityEngine;

public interface IHitAble
{
    public void ApplyKnoackBackForce(Vector3 knockBackDirection, float impactForce, float ImpactMultiplier);
}
