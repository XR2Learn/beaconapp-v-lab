using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotonicMicroscope_StageBase : MonoBehaviour {

    public GameObject ControlCondenser;
    public GameObject ControlCondenserKnob;
    public GameObject ControlMicroscopingUI;


    public const float MaxHeight = 0.2170F;
    public const float MinHeight = 0.1768353F;

    public const float HeightToAngleRatio = 0.00008F;

    [HideInInspector]
    public float Height;

    [HideInInspector]
    public float PerfectHeight_for_Focus;
    [HideInInspector]
    public float MaximumHeight_for_ImmersionOil;

    int Focus;

//  float Coefficient;


    public static readonly Dictionary<int, float> PerfectHeight_for_Focusing = 
        new Dictionary<int, float> {
            {4, 0.1950F},
            {10, 0.200F},
            {40, 0.210F},
            {100, 0.2106763F}
    };

    public static readonly Dictionary<int, float> MaximumHeight_for_ApplyingImmersionOil =
        new Dictionary<int, float> {
            {4, 21028F},
            {10, 2058F},
            {40, 0.20484F},
            {100, 0.19588F}
    };


    void setHeight (float _Height) {

        Height = _Height;

        transform.localPosition =
            new Vector3 (transform.localPosition.x, Height, transform.localPosition.z);

        float Factor = Mathf.Log (Mathf.Abs (Height - PerfectHeight_for_Focus) + 1,
            LogarithmicBase.ObjectiveLens_Focus[Focus]);

        ControlMicroscopingUI.GetComponent<PhotonicMicroscope_MicroscopingUI> ().updateStageBaseBlurring (Factor);

    }


    public void Restore_to_OriginalHeight () {
        setHeight (MinHeight);
    }

    // Start is called before the first frame update
    void Start () {
        setHeight (MinHeight);
    }

    public void move (float _da, float _Multiplier) {

        float dh = _Multiplier * _da * HeightToAngleRatio;

        float increment = 0F;

        if (dh > 0F && Height + dh > MaxHeight) {
            increment = MaxHeight - Height;
            setHeight (MaxHeight);        
        } else if (dh < 0F && Height + dh < MinHeight) {
            increment = MinHeight - Height;
            setHeight (MinHeight);
        } else {
            increment = dh;
            setHeight (Height + dh);
        }

        if (_da < 0F &&
            ControlCondenser.GetComponent<PhotonicMicroscope_Condenser> ().Height < PhotonicMicroscope_Condenser.OriginalMinHeight &&
            ControlCondenser.GetComponent<PhotonicMicroscope_Condenser> ().Height < ControlCondenser.GetComponent<PhotonicMicroscope_Condenser> ().MinHeight)
            ControlCondenserKnob.GetComponent<PhotonicMicroscope_CondenserKnob> ().applyRotation (_da);

        ControlCondenser.GetComponent<PhotonicMicroscope_Condenser> ().MinHeight -= increment;

    }

    public void updateFocus (int _Focus) {

        Focus = _Focus;

        updatePerfectHeight_for_Focus ();
        updateMaximumHeight_for_ImmersionOil ();
    }

    void updatePerfectHeight_for_Focus () {

        PerfectHeight_for_Focus = PerfectHeight_for_Focusing[Focus];

        setHeight (Height); //necessary, for the redefinition of Factor in microscoping

    }


    void updateMaximumHeight_for_ImmersionOil () {

        MaximumHeight_for_ImmersionOil = MaximumHeight_for_ApplyingImmersionOil[Focus];
    
    }


}
