using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System.Threading.Tasks;


public class Slide : MovableObject {

    public Sprite ImageClear;
    public Sprite ImageBlurry;

    [HideInInspector]
    public GameObject OilDropOn;

    // Start is called before the first frame update
    public override void Start () {
        
        base.Start ();

        Rotation_for_Carrying = new Vector3 (130F, 0F, 180F);

        OilDropOn = null;

    }


    public override async Task<Values_After_JointUse> use_with (GameObject _OtherObject) {

        Values_After_JointUse ResultValues = new Values_After_JointUse (false, null, false, null, false);

        if (OilDropOn == null 
            && GetComponent<MouseUI> ().Place.GetComponent<PhotonicMicroscope_SpecimenHolder> ()
            && GetComponent<MouseUI> ().Place.GetComponent<PhotonicMicroscope_SpecimenHolder> ().ControlRevolvingNosepiece.GetComponent<PhotonicMicroscope_RevolvingNosepiece> ().ApplyingImmesionOil_on_Slide_Allowed ()
            && _OtherObject.GetComponent<ImmersionOil> ()
            && _OtherObject.GetComponent<ImmersionOil> ().CapOn == null) {

            await _OtherObject.GetComponent<ImmersionOil> ().applyOilDrop_on (gameObject);

            ResultValues = new Values_After_JointUse (true, null, false, GetComponent<MouseUI> ().Place, true);
            //new interactivity for immersion oil bottle (third argument) needs to be false because it's still being carried after joint use with the slide (in build mode, where the cursor lies on the object being carried, setting the latter's interactivity to be true creates a conflict)

        }

        return ResultValues;

    }



}
