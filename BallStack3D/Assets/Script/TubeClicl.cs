using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TubeClicl : MonoBehaviour
{
    
    private void OnMouseUp()
    { 
        GameManager.instance.TubeLogic(this.gameObject);    
    }
   
}
