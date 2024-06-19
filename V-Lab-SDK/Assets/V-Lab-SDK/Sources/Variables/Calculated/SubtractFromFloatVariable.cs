using UnityEngine;

/**
 * 
 */
public class SubtractFromFloatVariable: NumericVariable, VariableListener {

    [SerializeField]
    private NumericVariable input;

    [SerializeField]
    private float from;

    protected void Awake() {
        input.AddListener(this);
    }

    public override float Get() {
        return from - input.Get();
    }

    public override void Set(float value) {
        input.Set(from - value);
    }

    public void ValueChanged() {
        NotifyListeners();
    }
}
