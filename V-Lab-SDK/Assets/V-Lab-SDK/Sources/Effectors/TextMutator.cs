using TMPro;
using UnityEngine;

/**
 * @todo Derive from Effector.
 */
public class TextMutator: Effector, VariableListener {

    [SerializeField]
    private StringVariable input;

    [SerializeField]
    private TMP_Text target;

    protected void Awake() {
        input.AddListener(this);
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

    protected void Run() {
        target.text = input.Get();
    }
}
