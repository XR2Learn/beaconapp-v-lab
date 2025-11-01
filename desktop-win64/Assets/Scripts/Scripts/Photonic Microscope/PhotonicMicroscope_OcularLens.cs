using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotonicMicroscope_OcularLens : Panel {

    public GameObject TwinLens;

    public override void updateZoomability (bool _NewZoomability) {

        base.updateZoomability (_NewZoomability);
        
        TwinLens.GetComponent<MouseUI> ().Zoomable = _NewZoomability;

    }

}
