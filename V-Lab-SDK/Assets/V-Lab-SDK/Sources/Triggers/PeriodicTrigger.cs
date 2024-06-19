using UnityEngine;

/**
 *
 */
public class PeriodicTrigger: MonoBehaviour {

    [SerializeField]
    private Effector target;

    [SerializeField]
    private float delay;

    [SerializeField]
    private float period;

    [SerializeField]
    private int repetitions;

    [SerializeField]
    private bool triggerOnStart;

    private float elapsed;

    private bool running;

    private int count;

    public void Awake() {
        elapsed = 0;
        count = 0;
        running = !(delay > 0.0f);
    }

    public void Start() {
        if (/*running && */triggerOnStart) {
            target.Trigger();
        }
    }

    public void Update() {
        if (repetitions == 0 || count < repetitions) {
            elapsed += Time.deltaTime;
            if (!running) {
                if (elapsed > delay) {
                    running = true;
                    elapsed = 0.0f;
                    target.Trigger();
                    count++;
                }
            }
            else if (elapsed > period) {
                elapsed = 0;
                target.Trigger();
                count++;
            }
        }
    }
}