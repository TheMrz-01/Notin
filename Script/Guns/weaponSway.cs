using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class weaponSway: MonoBehaviour
{
    public float intensity;
    public float smoother;

    public float amount;
    public float maxAmount;
    public float smoothAmount;

    PlayerMovement player;

    private float mouseX, mouseY;

    Quaternion origin_rot;
    Vector3 origin_t;

    void Start()
    {
        origin_rot = transform.localRotation;
        origin_t = transform.localPosition;
    }   
    void Update()
    {
        updateSway();
        /*if(player.moving == true){
            gunBoobing();
        }
        else if(player.moving == false){
            idle();
        }*/
    }

    void updateSway()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        Quaternion adj_x = Quaternion.AngleAxis(-intensity * mouseX,Vector3.up);
        Quaternion adj_y = Quaternion.AngleAxis(intensity * mouseY,Vector3.right);
        Quaternion target_rotation = origin_rot * adj_x * adj_y;

        transform.localRotation = Quaternion.Lerp(transform.localRotation,target_rotation,Time.deltaTime * smoother);
    }

    void gunBoobing()
    {
        
    }

    /*void idle()
    {
        float moveX = Mathf.Clamp(amount, -maxAmount, maxAmount);
        float moveY = Mathf.Clamp(amount, -maxAmount, maxAmount);

        Vector3 finalPosition = new Vector3(moveX, moveY, 0);

        transform.localPosition = Vector3.Lerp(, Time.deltaTime * smoothAmount);
    }*/
}