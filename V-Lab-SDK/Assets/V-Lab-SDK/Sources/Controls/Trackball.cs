using UnityEngine;

/**
 * 
 */
[RequireComponent(typeof(Collider))]
public class Trackball: Control {

    [SerializeField]
    private float step;

    [SerializeField]
    private bool useXAxis;

    [SerializeField]
    private bool useYAxis;

    [SerializeField]
    private float xRange;

    [SerializeField]
    private float yRange;

    [SerializeField]
    private bool restrictedOnX;

    [SerializeField]
    private float horizontalSpan;

    [SerializeField]
    private bool restrictedOnY;

    [SerializeField]
    private float verticalSpan;


    private float x, y;

    public Trackball() {
        SetCapabilities(new Capability[] { Capability.TURN });
    }

    public override float GetMoveStep() {
        return step;
    }

    public void Start() {
        x = y = 0;
        AdjustHandlePosition();
    }

    Vector3 vn, vnh, vnv, vo, voh, vov;
    float anh, anv, aoh, aov, adh, adv;

    public override void Turn(Vector3 newOrbitVector, Vector3 oldOrbitVector) {

        base.Turn(newOrbitVector, oldOrbitVector);

        Vector3 vn = transform.parent.InverseTransformDirection(newOrbitVector);
        Vector3 vo = transform.parent.InverseTransformDirection(oldOrbitVector);

        vnh = new Vector3(vn.x, 0, vn.z);
        vnv = new Vector3(0, vn.y, vn.z);

        voh = new Vector3(vo.x, 0, vo.z);
        vov = new Vector3(0, vo.y, vo.z);

        Quaternion rh = Quaternion.FromToRotation(voh.normalized, vnh.normalized);
        Quaternion rv = Quaternion.FromToRotation(vov.normalized, vnv.normalized);

        transform.rotation = rh  * transform.rotation * rv;
    }

    public void OnGUI() {
        
        Vector3 r = transform.localEulerAngles;
        
        GUILayout.BeginArea(new Rect(10, 10, 300, 200), GUI.skin.box);
        GUILayout.BeginVertical();
        GUILayout.Label($"value\tx:{x:0.00}\ty:{y:0.00}");
        GUILayout.Label($"local\tx:{r.x:0.00}\ty:{r.y:0.00}\tz:{r.z:0.00}");
        GUILayout.Label($"angle\th:{anh:0.00}\tv:{anv:0.00}");
        GUILayout.Label($"delta\th:{adh:0.00}\tv:{adv:0.00}");
        GUILayout.Label($"orbit\tv:{vn}");
        GUILayout.Label($"vrtcl\tv:{vnv}");
        GUILayout.Label($"hrznt\tv:{vnh}");
        GUILayout.EndVertical();
        GUILayout.EndArea();
    }

    public void OnDrawGizmos() {
        Vector3 p = transform.parent.position;
        Gizmos.color = Color.green;
        Gizmos.DrawLine(p, p + transform.parent.TransformDirection(vn.normalized));
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(p, p + transform.parent.TransformDirection(vnh).normalized);
        Gizmos.DrawLine(p, p + transform.parent.forward.normalized);
        Gizmos.color = Color.red;
        Gizmos.DrawLine(p, p + transform.parent.TransformDirection(vnv).normalized);
        Gizmos.DrawLine(p, p + transform.parent.up.normalized);
    }

    public override void OnValueChanged() {
        base.OnValueChanged();
        AdjustHandlePosition();
    }

    // yAxisSpan

    protected void AdjustHandlePosition() {
    }
}
