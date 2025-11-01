using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cap : MovableObject {
    
    // Start is called before the first frame update
    public override void Start() {        
        
        base.Start();

        Y_Offset_for_Relocation = 0.022722F;
        Y_Offset_for_Carrying = 0.01F;

    }

}
