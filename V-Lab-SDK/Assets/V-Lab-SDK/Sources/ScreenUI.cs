using UnityEngine;

/**
 * A bridge between controls in a virtual world and user-driven actions on a
 * projection of the world on a screen, by means of devices typically available
 * by a desktop or laptop computer or a mobile device, such as pointing devices,
 * touchscreens, keyboards, etc.
 */
public class ScreenUI: MonoBehaviour {

    /* --- Editor properties ----------------------------------------------------------------------------------- */

    /**
     * The maximum time difference between a touch-start and a touch-end event
     * for the sequence to be considered a press event.
     */  
    [SerializeField]
    private float maxPressDuration;


    /* --- Internal fields ------------------------------------------------------------------------------------- */

    /**
     * The selected control, null if none.
     */
    private Control selected;

    /**
     * Timestamp of the last touch-start event, used to calculate time
     * difference between subsequent touch-start and touch-end events.
     */
    private float touchStartTimestamp;

    /**
     * The last-saved projection of the mouse onto the reference plane of the
     * control that was selected at the time, in world space.
     */
    private Vector3 currentRefPlanePoint;

    private Vector3 currentOrbitVector;

    /* --- Initialization and per-frame behaviour -------------------------------------------------------------- */

    /**
     * Initialization.
     */
    private void Awake() {

        // initially, no selected control...
        selected = null;

        // (unnecessarily) reset last touch-start timestamp...
        touchStartTimestamp = 0.0f;

        // (unnecessarily) reset last-saved local-space mouse point...
        currentRefPlanePoint = new Vector3();
    }

    /**
     * Per-frame behaviour.
     */
    private void Update() {

        // handle mouse button-down events...
        if (Input.GetMouseButtonDown(0)) {
            HandleMouseButtonDown();
        }

        // handle mouse button-up events...
        else if (Input.GetMouseButtonUp(0)) {
            HandleMouseButtonUp();
        }

        // handle mouse button-held events...
        if (Input.GetMouseButton(0)) {
            HandleMouseButtonHeld();
        }
    }


    /* --- Internal input event handlers ----------------------------------------------------------------------- */

    /**
     * Handles mouse button-down events. Specifically, checks if any gameobject
     * was clicked-on. If it was, checks if it is a control, that is, if it has
     * a Control component. if it has, selects it, saves the click timestamp,
     * triggers a touch-start event on it and saves the screen- and local-space
     * mouse points.
     */
    private void HandleMouseButtonDown() {

        // calculate a ray from the main camera's origin through the click point
        // on the viewport plane...
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        // check if the ray has hit any collider...
        if (Physics.Raycast(ray, out RaycastHit raycastInfo)) {

            // checks if the ray has hit a Control...
            Control control = raycastInfo.transform.GetComponent<Control>();
            if (control != null) {

                // Debug.Log($"Hit a control, name: {control.gameObject.name}, reforigin: {control.transform.parent.position}, refnormal: {control.transform.parent.up}");
                Debug.Log($"Hit a control, name: {control.gameObject.name}");

                // select the control...
                selected = control;
                
                // save the click timestamp...
                touchStartTimestamp = Time.time;

                // trigger a touch-start event on the control...
                selected.TouchStart();

                if (selected.HasCapability(Control.Capability.MOVE)) {

                    // calculate and save the ref-plane point...
                    CameraRayPlaneIntersection(out currentRefPlanePoint, selected.transform.parent.position, selected.transform.parent.up, Input.mousePosition);
                }

                else if (selected.HasCapability(Control.Capability.TURN)) {

                    Vector3 p = ray.GetPoint(raycastInfo.distance);
                    currentOrbitVector = p - selected.transform.position;

                    /*
                    Vector3 p = Input.mousePosition;
                    // p.y = Camera.main.pixelHeight - p.y;
                    p.z = Camera.main.nearClipPlane;
                    currentOrbitVector = Camera.main.ScreenToWorldPoint(p) - selected.transform.position;
                    currentOrbitVector.Normalize();
                    */
                }
            }
            else {

                // the gameobject hit is not a control...

                // Debug.Log($"Hit a non-control, name: {raycastInfo.transform.gameObject.name}");
            }
        }
        else {

            // the ray didn't hit any collider...

            // Debug.Log($"Didn't hit anything");
        }
    }

    /**
     * Handles mouse button-up events. Specifically, if a control is selected,
     * triggers a touch-end event on it, checks if the touch duration is less
     * than the max press duration and triggers a press event if not,
     * (unnecessarily) resets the touch-start timestamp and deselects the
     * control...
     */
    private void HandleMouseButtonUp() {

        // check if a control is selected...
        if (selected != null) {

            // trigger a touch-end event on the control...
            selected.TouchEnd();

            // calculate the touch duration, that is, the time difference
            // between the touch-start timestamp and the current time...
            float duration = Time.time - touchStartTimestamp;

            // check if the control can be pressed and if the touch duration is
            // less than the max press duration...
            if (selected.HasCapability(Control.Capability.PRESS) && duration < maxPressDuration) {
                
                // trigger a press event on the control...
                selected.Press(duration);
            }

            // reset the touch-start timestamp...
            touchStartTimestamp = 0.0f;

            // deselect the control...
            selected = null;
        }
    }

    /**
     * Invoked on every frame the mouse button is down. If a control is
     * selected, it calculates the click point's coordinates in the control's
     * coordinate system, calculates the drag (offset) vector and, if the drag
     * vector's magnitude is larger than the selected control's half-step (to
     * properly account for movement in any direction), saves the new click
     * point and triggers a movement event on the control passing the calculated
     * drag vector and the new click point as the movement's destination.
     */
    private void HandleMouseButtonHeld() {

        // check if a control is selected...
        if (selected != null) {

            // the selected control is being dragged-on...

            if (selected.FindCapability(Control.Capability.MOVE) != -1) {

                // calculate and the new click point in the control's coordinate
                // system...
                CameraRayPlaneIntersection(out Vector3 newRefPlanePoint, selected.transform.parent.position, selected.transform.parent.up, Input.mousePosition);

                // calculate the drag vector (in the control's coordinate system)...
                Vector3 drag = selected.transform.parent.InverseTransformVector(newRefPlanePoint - currentRefPlanePoint);

                // check if the drag vector indicates a movement larger than half
                // the control's step (so that the control can be correctly centered
                // at an adjacent position in any direction)...
                if (Vector3.Magnitude(drag) > selected.GetMoveStep() / 2) {

                    // Debug.Log($"Drag on {selected.gameObject.name}, dragv: {drag}, dragm: {drag.magnitude}, step: {selected.GetStep()}");

                    // trigger a movement event on the selected control passing the
                    // calculated drag vector and the new click point as the
                    // movement's destination...
                    selected.Move(drag, newRefPlanePoint);

                    // save the new point on the control's ref plane, thus
                    // zeroing the accumulated drag and permitting the
                    // identification of a new movement step when the
                    // accumulated drag exceeds the control's half step again...
                    currentRefPlanePoint = newRefPlanePoint;
                }
            }

            else if (selected.FindCapability(Control.Capability.TURN) != -1) {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit raycastInfo)) {
                    if (raycastInfo.collider.transform == selected.transform) {
                        Vector3 p = ray.GetPoint(raycastInfo.distance);
                        Vector3 newOrbitVector = p - selected.transform.position;
                        selected.Turn(newOrbitVector, currentOrbitVector);
                        currentOrbitVector = newOrbitVector;
                    }
                }
            }
        }
    }

    /* --- Helpers --------------------------------------------------------------------------------------------- */

    /**
     * Calculates the intersection point between a ray originating from the main
     * camera's origin and passing through the specified rayPoint and a plane
     * defined by the specified planeOrigin point and planeNormal normal, if
     * any. All arguments and the calculated point in world coordinates.
     */
    public static bool CameraRayPlaneIntersection(out Vector3 hitPoint, Vector3 planeOrigin, Vector3 planeNormal, Vector3 rayPoint) {

        // calculate a ray from the main camera's origin and passing through
        // rayPoint...
        Ray ray = Camera.main.ScreenPointToRay(rayPoint);

        // calculate a plane passing through planeOrigin and with a planeNormal
        // normal...
        Plane plane = new Plane(planeNormal, planeOrigin);

        // check if the ray intersects with the plane...
        if (plane.Raycast(ray, out float d)) {

            // calculate the intersection point on the ray based on its distance
            // from the ray's origin...
            hitPoint = ray.GetPoint(d);

            // indicate that an intersection point was found...
            return true;
        }
        else {

            // reset the intersection point argument's value (for no real
            // reason)...
            hitPoint = default(Vector3);

            // indicate that an intersection point was, alas, not found...
            return false;
        }
    }


    public void OnDrawGizmos() {
        if (selected != null) {
            // Gizmos.DrawLine(selected.transform.position, selected.transform.position + currentOrbitVector);
        }
    }
}
