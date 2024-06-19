using UnityEngine;

/**
 * 
 */
[RequireComponent(typeof(Collider))]
public class Slider: RangeControl {

    [SerializeField]
    private float upperBound;

    [SerializeField]
    private float lowerBound;

    [SerializeField]
    private float step;

    [SerializeField]
    private float slideRange;

    public Slider() {
        SetCapabilities(new Capability[] {Capability.MOVE});
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
        AdjustHandlePosition();
    }

    public override void Move(Vector3 offset, Vector3 destination) {

        base.Move(offset, destination);

        destination = transform.parent.InverseTransformPoint(destination);

        float value = (destination.x + slideRange / 2.0f) / slideRange * GetRange();

        value = Mathf.Floor(value / GetMoveStep()) * GetMoveStep();

        SetValue(value + GetLowerBound());
    }

    public override void OnValueChanged() {
        base.OnValueChanged();
        AdjustHandlePosition();
    }

    protected void AdjustHandlePosition() {
        transform.localPosition = Vector3.Lerp(
            new Vector3(-slideRange / 2, 0, 0),
            new Vector3(slideRange / 2, 0, 0),
            (GetValue() - GetLowerBound()) / GetRange()
        );
    }
}
