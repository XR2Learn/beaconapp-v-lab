using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotonicMicroscope_FocusKnob : Knob {

    public GameObject ControlMicroscope;
    public GameObject ControlStageBase;

    [HideInInspector]
    public float Angle;

    public enum FocusKnobTypes {
        Coarse,
        Fine
    }

    public FocusKnobTypes FocusKnobType;

    float Multiplier;

    void setAngle (float _angle) {
        Angle = _angle;
        transform.localEulerAngles = new Vector3 (transform.localEulerAngles.x, transform.localEulerAngles.y, Angle);
    }


    public void Restore_to_OriginalRotation () {
        setAngle (0F);
    }

    // Start is called before the first frame update
    public override void Start () {

        PivotAxis = Axes.Y_Axis;
        RotationAxis = Axes.Z_Axis;

        da = 4F;

        if (FocusKnobType == FocusKnobTypes.Coarse)
            Multiplier = 1F;
        else
            Multiplier = 0.005F;

        setAngle (0F);

    }

    public override void rotate (int _direction) {

        if (ControlMicroscope.GetComponent<PhotonicMicroscope> ().Cable_State == connected) {

            if (_direction < 0F && 
                ControlStageBase.GetComponent<PhotonicMicroscope_StageBase>().Height < PhotonicMicroscope_StageBase.MaxHeight) {
                setAngle (Angle - da);
                ControlStageBase.GetComponent<PhotonicMicroscope_StageBase> ().move (da, Multiplier);
            } else if (_direction > 0F &&
                ControlStageBase.GetComponent<PhotonicMicroscope_StageBase> ().Height >
                PhotonicMicroscope_StageBase.MinHeight) {
                setAngle (Angle + da);
                ControlStageBase.GetComponent<PhotonicMicroscope_StageBase> ().move (-da, Multiplier);
            }

        }

    }

}
