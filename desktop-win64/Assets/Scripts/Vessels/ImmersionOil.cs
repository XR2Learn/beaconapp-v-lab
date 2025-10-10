using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;


public class ImmersionOil : Bottle {

    public GameObject OilDropPrototype;

    // Start is called before the first frame update
    public override void Start () {
        
        base.Start ();

        Y_Offset_for_Relocation = 0.021522F;

        Rotation_for_Carrying = new Vector3 (0F, 120F, 0F);

        LocalPosition_for_Cap = new Vector3 (0F, 0.0225F, 0F);

    }

    public override async Task<Values_After_JointUse> use_with (GameObject _OtherObject) {

        Values_After_JointUse ResultValues = new Values_After_JointUse (false, null, false, null, false);

        if (CapOn == null && _OtherObject.GetComponent<MouseUI> ().Label == Labels.ImmersionOilCap) {

            placeOn (_OtherObject);

            setCapOn (_OtherObject);

            ResultValues = new Values_After_JointUse (true, gameObject, true, null, true);

        }

        return ResultValues;

    }

    public async Task applyOilDrop_on(GameObject _Slide) {

        transform.SetParent (_Slide.transform);

        //place oil bottle horizontally
        transform.localPosition = new Vector3 (0.00703F, 0.00539F, 0.01619F);
        transform.localEulerAngles = new Vector3 (45.212F, 306.247F, 103.937F);

        await Task.Delay (100);
        
        //create drop on slide
        _Slide.GetComponent<Slide> ().OilDropOn = Instantiate (OilDropPrototype.gameObject, _Slide.transform);

        _Slide.GetComponent<Slide> ().OilDropOn.transform.localPosition = new Vector3 (0.00058F, 0.00033F, 0F);
        _Slide.GetComponent<Slide> ().OilDropOn.transform.localEulerAngles = Vector3.zero;
        _Slide.GetComponent<Slide> ().OilDropOn.transform.localScale = Vector3.one;

        await Task.Delay (1000);

    }


}
