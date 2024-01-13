using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gunBobbing : MonoBehaviour
{
    Vector3 or_P;
    public PlayerMovement player;
    public Vector3 whe1,whe2;

    void Start()
    {
        or_P = transform.localPosition;        
    }

    void Update()
    {
        if(player.moving == true){
            print("ok");
            transform.localPosition = Vector3.Lerp(or_P,whe1,10);
            transform.localPosition = Vector3.Lerp(or_P,whe2,10);
        }
        else if(player.moving == false){
            print("oke");
            transform.localPosition = Vector3.Lerp(transform.localPosition,or_P,10);
        }
    }
}
