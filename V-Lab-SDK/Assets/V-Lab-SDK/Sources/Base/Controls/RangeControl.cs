using UnityEngine;

public abstract class RangeControl: Control, VariableListener {

    [SerializeField]
    private NumericVariable value;

    public abstract bool IsBounded();

    public abstract float GetUpperBound();

    public abstract float GetLowerBound();

    public float GetRange() {
        return IsBounded() ? GetUpperBound() - GetLowerBound() : float.MaxValue;
    }

    public virtual void Increase() {
        float v = value.Get() + GetMoveStep();
        if (v > GetUpperBound()) {
            v = GetUpperBound();
        }
        value.Set(v);
    }

    public virtual void Decrease() {
        float v = value.Get() - GetMoveStep();
        if (v < GetLowerBound()) {
            v = GetLowerBound();
        }
        value.Set(v);
    }

    public void SetValue(float v) {
        if (v > GetUpperBound()) {
            v = GetUpperBound();
        }
        else if (v < GetLowerBound()) {
            v = GetLowerBound();
        }
        value.Set(v);
    }

    public float GetValue() {
        return value.Get();
    }

    public virtual void Awake() {
        value.AddListener(this);
    }

    public void ValueChanged() {
        OnValueChanged();
    }
}
