using System.Collections;
using System.Collections.Generic;
//using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PhotonicMicroscope_RevolvingNosepiece : RevolvingKnob {

    public GameObject ControlStageBase;
    public GameObject ControlMicroscopingUI;

    public GameObject ControlObjectiveLens4X;
    public GameObject ControlObjectiveLens10X;
    public GameObject ControlObjectiveLens40X;
    public GameObject ControlObjectiveLens100X;

    const float AnglePrecision = 0.1F;
    const float r = 0.1F * AnglePrecision;

    [HideInInspector]
    public int Focus;

    int ForbiddenDirection;

    bool CollisionBefore;
    public bool Collision_from_StageBaseMovement;

    void setFocus (int _Focus) {

        Focus = _Focus;

        ControlStageBase.GetComponent<PhotonicMicroscope_StageBase> ().updateFocus (Focus);
        ControlMicroscopingUI.GetComponent<PhotonicMicroscope_MicroscopingUI> ().updateFocus (Focus);

    }


    // Start is called before the first frame update
    public override void Start () {

        MouseMovementAxis = Axes.X_Axis;
        RotationAxis = Axes.Y_Axis;

        da = -6F;

        setFocus (10);

        ForbiddenDirection = 0; //needs to be 0 so as not to coincide with left (-1) or right (-1)

        CollisionBefore = false;

        Collision_from_StageBaseMovement = false;

    }



    public override void applyRotation (int _Direction) {

        //checks collision of objective lenses with stage and specimen holder
        //if collision, blocks movement to this direction
        if (ObjectiveLensCollision ()) {

            if (!CollisionBefore) {

                ForbiddenDirection = _Direction;

                CollisionBefore = true;

            }

        } else {

            ForbiddenDirection = 0;

            CollisionBefore = false;

        }

        if (!Collision_from_StageBaseMovement && ForbiddenDirection != _Direction) {

            setRotation (_Direction * da);

            ControlMicroscopingUI.GetComponent<PhotonicMicroscope_MicroscopingUI> ().setVisibility_of_SpecimenImage (false);

        }

    }


    public override void doneRotating () {

        float Angle = transform.localRotation.eulerAngles.y;

        Angle = MathFunctions.EquivalentPositiveAngle_of (Angle);

        float NearestAngle = -1000F; //just a symbolic value; not to be used later on
        int NewFocus = 0; //just a symbolic value, too

        float FarthestAngle = -1000F;
        int AlternativeFocus = 0;

        if (Angle > 0F && Angle < 90F) {

            if (Mathf.Abs (Angle - 0F) < Mathf.Abs (Angle - 90F)) {

                NearestAngle = 0F;
                NewFocus = 4;

                FarthestAngle = 90F;
                AlternativeFocus = 10;

            } else {

                NearestAngle = 90F;
                NewFocus = 10;

                FarthestAngle = 0F;
                AlternativeFocus = 4;

            }

        } else if (Angle > 90F && Angle < 180F) {

            if (Mathf.Abs (Angle - 90F) < Mathf.Abs (Angle - 180F)) {

                NearestAngle = 90F;
                NewFocus = 10;

                FarthestAngle = 180F;
                AlternativeFocus = 40;

            } else {

                NearestAngle = 180F;
                NewFocus = 40;

                FarthestAngle = 90F;
                AlternativeFocus = 10;

            }

        } else if (Angle > 180F && Angle < 270F) {

            if (Mathf.Abs (Angle - 180F) < Mathf.Abs (Angle - 270F)) {

                NearestAngle = 180F;
                NewFocus = 40;

                FarthestAngle = 270F;
                AlternativeFocus = 100;

            } else {

                NearestAngle = 270F;
                NewFocus = 100;

                FarthestAngle = 180F;
                AlternativeFocus = 40;

            }
        } else if (Angle > 270F && Angle < 360F) {

            if (Mathf.Abs (Angle - 270F) < Mathf.Abs (Angle - 360F)) {

                NearestAngle = 270F;
                NewFocus = 100;

                FarthestAngle = 360F;
                AlternativeFocus = 4;

            } else {

                NearestAngle = 360F;
                NewFocus = 4;

                FarthestAngle = 270F;
                AlternativeFocus = 100;

            }

        }

        setFocus (NewFocus);

        float Direction = -Mathf.Sign (NearestAngle - Angle);

        bool OppositePivotNeeded = false;

        while (!MathFunctions.ApproximateEquality_of (MathFunctions.EquivalentPositiveAngle_of (transform.localEulerAngles.y), NearestAngle, AnglePrecision)) {

            transform.Rotate (0F, -r * Direction, 0F);

            if (ObjectiveLensCollision () && Direction == (int)ForbiddenDirection) {

                OppositePivotNeeded = true;

                break;

            }

        }

        if (OppositePivotNeeded) {

            bool OppositePivotNeededAgain = false;

            while (!MathFunctions.ApproximateEquality_of (MathFunctions.EquivalentPositiveAngle_of (transform.localEulerAngles.y), FarthestAngle, AnglePrecision)) {

                transform.Rotate (0F, r * Direction, 0F);

                if (ObjectiveLensCollision () && Direction == -(int)ForbiddenDirection) {//handling the rare exception when it had instantly moved to the permitted direction and then collided, misidentifying that way the direction as forbidden and therefore moving wrongly to the forbidden direction

                    OppositePivotNeededAgain = true;

                    break;

                }

            }

            setFocus(AlternativeFocus);

            if (OppositePivotNeededAgain) {

                while (!MathFunctions.ApproximateEquality_of (MathFunctions.EquivalentPositiveAngle_of (transform.localEulerAngles.y), NearestAngle, AnglePrecision)) {

                    transform.Rotate (0F, -r * Direction, 0F);

                }

                setFocus (NewFocus);

            }

        }

        ControlMicroscopingUI.GetComponent<PhotonicMicroscope_MicroscopingUI> ().setVisibility_of_SpecimenImage (true);

    }



    public bool ObjectiveLensCollision () {

        return ControlObjectiveLens40X.GetComponent<CollisionSensitive> ().CollisionDetected || ControlObjectiveLens100X.GetComponent<CollisionSensitive> ().CollisionDetected;
        //4X and 10X don't collide with the stage, no matter what its height, so they are not taken into account.

    }

    public void addCollidableObject_to_List (GameObject _Slide) {
        ControlObjectiveLens40X.GetComponent<CollisionSensitive> ().Collidables.Add (_Slide);
        ControlObjectiveLens100X.GetComponent<CollisionSensitive> ().Collidables.Add (_Slide);
    }

    public void removeCollidableObject_from_List (GameObject _Slide) {
        ControlObjectiveLens40X.GetComponent<CollisionSensitive> ().Collidables.Remove (_Slide);
        ControlObjectiveLens100X.GetComponent<CollisionSensitive> ().Collidables.Add (_Slide);
    }


    public bool ApplyingImmesionOil_on_Slide_Allowed () {

        if (!ObjectiveLensCollision () && ControlStageBase.GetComponent<PhotonicMicroscope_StageBase> ().Height <= ControlStageBase.GetComponent<PhotonicMicroscope_StageBase> ().MaximumHeight_for_ImmersionOil)
            return true;
        else 
            return false;
    
    }


}