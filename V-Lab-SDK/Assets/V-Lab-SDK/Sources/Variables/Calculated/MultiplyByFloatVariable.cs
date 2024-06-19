using UnityEngine;

/**
 * 
 */
public class MultiplyByFloatVariable: NumericVariable, VariableListener {

    [SerializeField]
    private Variable<float> input;

    [SerializeField]
    private float by;

    protected void Awake() {
        input.AddListener(this);
    }

    public override float Get() {
        return input.Get() * by;
    }

    public override void Set(float value) {
        input.Set(value / by);
    }

    public void ValueChanged() {
        NotifyListeners();
    }
}
