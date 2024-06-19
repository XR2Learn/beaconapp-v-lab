using UnityEngine;

public class NumericThresholdVariable: BooleanVariable, VariableListener {

    [SerializeField]
    private NumericVariable input;

    [SerializeField]
    private float threshold;

    protected void Awake() {
        input.AddListener(this);
    }

    public override void Toggle() {
        // NOP...
    }

    public override bool Get() {
        return input.Get() > threshold;
    }

    public override void Set(bool value) {
        // NOP...
    }

    public void ValueChanged() {
        NotifyListeners();
    }
}
