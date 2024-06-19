using UnityEngine;

/**
 * @todo Derive from Effector.
 */
public class LightIntensityMutator: Effector, VariableListener {

    [SerializeField]
    private NumericVariable input;

    [SerializeField]
    private Light target;

    protected void OnEnable() {
        input.AddListener(this);
    }

    protected void OnDestroy() {
        input.RemoveListener(this);
    }

    protected void Run() {
        target.intensity = input.Get();
    }

    protected void Start() {
        Run();
    }

    public void ValueChanged() {
        Run();
    }

    public override void Trigger() {
        Run();
    }
}
