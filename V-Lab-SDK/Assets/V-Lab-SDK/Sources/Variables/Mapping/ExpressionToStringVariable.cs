using UnityEngine;

public class ExpressionToStringVariable: StringVariable, VariableListener {

    [SerializeField]
    private Variable[] inputs;

    [SerializeField]
    private string expression;

    public void Awake() {
        foreach (Variable input in inputs) {
            input.AddListener(this);
        }
    }

    public void Start() {
        NotifyListeners();
    }

    public override string Get() {
        return string.Format(expression, inputs);
    }

    public override void Set(string value) {
        // NOP...
    }

    public void ValueChanged() {
        NotifyListeners();
    }
}
