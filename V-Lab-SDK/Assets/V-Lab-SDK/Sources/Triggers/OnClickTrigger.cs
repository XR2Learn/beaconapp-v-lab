using UnityEngine;

/**
 *
 */
[RequireComponent(typeof(Collider))]
public class OnClickTrigger: MonoBehaviour {

    [SerializeField]
    private Effector target;

    public void OnMouseUpAsButton() {
        target.Trigger();
    }
}