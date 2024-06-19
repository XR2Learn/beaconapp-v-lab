using UnityEngine;

/**
 * @todo Derive from Effector.
 */
public class GameObjectActivator: MonoBehaviour, VariableListener {

    [SerializeField]
    private BooleanVariable input;

    [SerializeField]
    private GameObject target;

    protected void Awake() {
        input.AddListener(this);
    }

    protected void Start() {
        target.SetActive(input.Get());
    }

    public void ValueChanged() {
        target.SetActive(input.Get());
    }
}
