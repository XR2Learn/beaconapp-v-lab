using UnityEngine;

public class NumericToVector3Variable : Vector3Variable, VariableListener {

    [SerializeField]
    private float xValue;

    [SerializeField]
    private NumericVariable xVariable;

    [SerializeField]
    private float yValue;

    [SerializeField]
    private NumericVariable yVariable;

    [SerializeField]
    private float zValue;

    [SerializeField]
    private NumericVariable zVariable;

    public void OnEnable() {
        if (xVariable != null) {
            xVariable.AddListener(this);
        }
        if (yVariable != null) {
            yVariable.AddListener(this);
        }
        if (zVariable != null) {
            zVariable.AddListener(this);
        }
    }

    public void OnDestroy() {
        if (xVariable != null) {
            xVariable.RemoveListener(this);
        }
        if (yVariable != null) {
            yVariable.RemoveListener(this);
        }
        if (zVariable != null) {
            zVariable.RemoveListener(this);
        }
    }

    public override Vector3 Get() {
        float x = xVariable != null ? xVariable.Get() : xValue;
        float y = yVariable != null ? yVariable.Get() : yValue;
        float z = zVariable != null ? zVariable.Get() : zValue;
        return new Vector3(x, y, z);
    }

    public override void Set(Vector3 value) {
        // NOP...
    }

    public void ValueChanged() {
        NotifyListeners();
    }
}
