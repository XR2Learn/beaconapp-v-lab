using UnityEngine;

/**
 * 
 */
[RequireComponent(typeof(Collider))]
public class Knob: RangeControl {

    [SerializeField]
    private float upperBound;

    [SerializeField]
    private float lowerBound;

    [SerializeField]
    private float step;

    /// <summary>
    /// Internal value, non-quantized and internally-managed, to deal with known
    /// issues with rotation math in Unity.
    /// </summary>
    private float v;

    public Knob() {
        SetCapabilities(new Capability[] {Capability.TURN});
    }

    public override bool IsBounded() {
        return true;
    }

    public override float GetUpperBound() {
        return upperBound;
    }

    public override float GetLowerBound() {
        return lowerBound;
    }

    public override float GetMoveStep() {
        return step;
    }

    public void Start() {
        v = GetLowerBound();
        AdjustHandleRotation();
    }

    public override void Turn(Vector3 newOrbitVector, Vector3 oldOrbitVector) {

        Vector3 a = transform.InverseTransformVector(oldOrbitVector);
        Vector3 b = transform.InverseTransformVector(newOrbitVector);
        a.y = b.y = 0;
        a.Normalize();
        b.Normalize();

        float da = Vector3.SignedAngle(b, a, -transform.up);
        
        float dv = da * GetRange() / 360;

        v += dv;

        float nv = v;

        if (step > 0.000000000001f) {
            nv = ((int) (nv / step)) * step;
        }

        if (nv < GetLowerBound()) {
            nv = GetLowerBound();
        }

        if (nv > GetUpperBound()) {
            nv = GetUpperBound();
        }

        SetValue(nv);
    }

    public override void OnValueChanged() {
        base.OnValueChanged();
        AdjustHandleRotation();
    }

    protected void AdjustHandleRotation() {

        float a = (GetValue() - GetLowerBound()) / GetRange() * 360.0f;

        transform.localRotation = Quaternion.AngleAxis(a, Vector3.up);
    }
}
