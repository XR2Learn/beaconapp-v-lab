using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;


public class PhotonicMicroscope_SpecimenHolder : InteractableObject {

    public GameObject ControlStage;
    public GameObject ControlClipController;
    public GameObject ControlRevolvingNosepiece;

    GameObject SpecimenOn;


    void setAttachedSpecimen (GameObject _Specimen) {

        SpecimenOn = _Specimen;

        ControlStage.GetComponent<PhotonicMicroscope_Stage> ().setImage_for_Specimen (_Specimen);

    }


    // Start is called before the first frame update
    public override void Start () {

        SpecimenOn = null;

    }

    public override async Task<Values_After_JointUse> use_with (GameObject _OtherObject) {

        Values_After_JointUse ResultValues = new Values_After_JointUse (false, null, false, null, false);

        if (SpecimenOn == null && _OtherObject.GetComponent<Slide> ()) {



            ResultValues = new Values_After_JointUse (true, gameObject, true, null, true);
            //it needs to be placed BEFORE placeIn because in placeIn, the specimen becomes child of the specimen holder, and changing an object's interactivity (here specimen holder's) affects the interactivity of its children

            await openClipController_and_place (_OtherObject);

            setAttachedSpecimen (_OtherObject);
            
            ControlRevolvingNosepiece.GetComponent<PhotonicMicroscope_RevolvingNosepiece>().addCollidableObject_to_List(_OtherObject);

        }

        return ResultValues;

    }



    async Task openClipController_and_place (GameObject _Slide) {

        _Slide.transform.parent = transform;
        _Slide.transform.localPosition = new Vector3 (-0.0226F, -0.0021F, 0.11F);
        _Slide.transform.localEulerAngles = Vector3.zero;

        while (ControlClipController.transform.localEulerAngles.y < 8.683F) {
            ControlClipController.transform.Rotate (0F, 0.3F, 0F);
            await Task.Delay (1);
        }

        ControlClipController.transform.localEulerAngles = new Vector3 (0F, 8.683F, 0F);

        await Task.Delay (10);

        _Slide.transform.localPosition = new Vector3 (-0.0226F, -0.0021F, 0.0062F);

    }


    void closeClipController () {

        ControlClipController.transform.localEulerAngles = Vector3.zero;

    }

    void placeIn (GameObject _OtherObject) {



    }

    public override void evacuate (GameObject _Object) {

        setAttachedSpecimen (null);
        
        _Object.GetComponent<MouseUI> ().Place = null;

        _Object.transform.parent = null;

        closeClipController ();

        GetComponent<MouseUI> ().setInteractivity (true);

        ControlRevolvingNosepiece.GetComponent<PhotonicMicroscope_RevolvingNosepiece> ().removeCollidableObject_from_List (_Object);

    }


    public void setActivation_of_SpecimenMovement (bool _ActivationStatus) {

        if (SpecimenOn != null)
            SpecimenOn.GetComponent<BoxCollider> ().enabled = _ActivationStatus;
    
    }


    public bool HasSpecimen_with_ImmersionOil () {
     
        if (SpecimenOn != null && SpecimenOn.GetComponent<Slide> ().OilDropOn != null)
            return true;
        else
            return false;

    }

    public void updateInteractivity_of_Self_and_Slide (bool _Collision_with_Lens) {

        GetComponent<MouseUI> ().TemporarilyInaccessible = _Collision_with_Lens;

        if (SpecimenOn != null)
            SpecimenOn.GetComponent<MouseUI> ().TemporarilyInaccessible = _Collision_with_Lens;

    }

}
