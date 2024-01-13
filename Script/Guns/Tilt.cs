using UnityEngine;

public class Sway : MonoBehaviour
{
    [Header("Rotation")]
    public float rotationAmount = 4f;
    public float maxRotationAmount = 5f;
    public float smoothRotation = 12f;
    [Header("Aim Rotation")]
    public float aimRotationAmount = 2f;
    public float aimMaxRotationAmount = 2.5f;
    //public float aimSmoothRotation = 6f;
    
    [Space]
    public bool rotationX = true, rotationY = true, rotationZ = true;

    private Vector3 initialPosition;
    private Quaternion initialRotation;

    public GunSystem gun;

    private float InputX, InputY;

    private void Start()
    {
        initialPosition = transform.localPosition;
        initialRotation = transform.localRotation;
    }

    private void Update()
    {
        CalculateSway();
        if(gun.aiming == false){
            TiltSway();
        }
        if(gun.aiming == true){ 
            aimTiltSway();
        }
    }

    private void CalculateSway()
    {
        InputX = Input.GetAxis("Mouse X");
        InputY = Input.GetAxis("Mouse Y");
    }

    private void TiltSway()
    {
        float tiltY = Mathf.Clamp(InputX * rotationAmount, -maxRotationAmount, maxRotationAmount);
        float tiltX = Mathf.Clamp(InputY * rotationAmount, -maxRotationAmount, maxRotationAmount);

        Quaternion finalRotation = Quaternion.Euler(new Vector3(rotationX ? -tiltX : 0f, rotationY ? tiltY : 0f, rotationZ ? tiltY : 0f));
            
        transform.localRotation = Quaternion.Slerp(transform.localRotation, finalRotation * initialRotation, smoothRotation * Time.deltaTime);
    }

    private void aimTiltSway()
    {
        float _tiltY = Mathf.Clamp(InputX * aimRotationAmount, -aimMaxRotationAmount, aimMaxRotationAmount);
        float _tiltX = Mathf.Clamp(InputY * aimRotationAmount, -aimMaxRotationAmount, aimMaxRotationAmount);

        Quaternion _finalRotation = Quaternion.Euler(new Vector3(rotationX ? -_tiltX : 0f, rotationY ? _tiltY : 0f, rotationZ ? _tiltY : 0f));
            
        transform.localRotation = Quaternion.Slerp(transform.localRotation, _finalRotation * initialRotation, smoothRotation * Time.deltaTime);
    }
}