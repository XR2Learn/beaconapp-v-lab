using UnityEngine;

/**
 * 
 */
public class BooleanToIntVariable: IntegerVariable, VariableListener {

    [SerializeField]
    private BooleanVariable input;

    protected void Awake() {
        input.AddListener(this);
    }

    public override int Get() {
        return input.Get() ? 0 : 1;
    }

    public override void Set(int value) {
        input.Set(value == 0);
    }

    public void ValueChanged() {
        NotifyListeners();
    }
}
