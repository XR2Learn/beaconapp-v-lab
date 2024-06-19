using UnityEngine;

public class PositionMutator: Effector, VariableListener {

    [SerializeField]
    private Vector3Variable input;

    [SerializeField]
    private Transform target;

    [SerializeField]
    private bool local;

    [SerializeField]
    private bool relative;

    public void OnEnable() {
        input.AddListener(this);
    }

    public void OnDestroy() {
        input.RemoveListener(this);
    }

    public override void Trigger() {
        Run();
    }

    public void ValueChanged() {
        Run();
    }

    protected void Run() {

        if (preconditions != null && !preconditions.Get()) {
            return;
        }

        Vector3 t = relative ? (local ? target.localPosition : target.position) : Vector2.zero;
        t += input.Get();
        if (local) {
            target.localPosition = t;
        }
        else {
            target.position = t;
        }
    }
}
