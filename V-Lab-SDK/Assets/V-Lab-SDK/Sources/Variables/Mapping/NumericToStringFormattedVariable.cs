using UnityEngine;

public class NumericVariableStringFormatter: StringVariable, VariableListener {

    [SerializeField]
    private NumericVariable input;

    [SerializeField]
    private string format;

    public void Awake() {
            input.AddListener(this);
    }

    public void Start() {
        NotifyListeners();
    }

    public override string Get() {
        return input.Get().ToString(format);
    }

    public override void Set(string value) {
        // NOP...
    }

    public void ValueChanged() {
        NotifyListeners();
    }
}
