using UnityEngine;

/**
 * 
 */
public class ConcreteNumericVariable: NumericVariable {

    [SerializeField]
    private float value;

    public override float Get() {
        return value;
    }

    public override void Set(float value) {
        this.value = value;
        NotifyListeners();
    }
}
