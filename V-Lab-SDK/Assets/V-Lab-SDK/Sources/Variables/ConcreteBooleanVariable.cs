using UnityEngine;

/**
 * 
 */
public class ConcreteBooleanVariable: BooleanVariable {

    [SerializeField]
    private bool value;

    public override bool Get() {
        return value;
    }

    public override void Set(bool value) {
        this.value = value;
        NotifyListeners();
    }

    public override void Toggle() {
        Set(!Get());
    }
}