
using UnityEngine;
using TMPro;

public class GunSystem : MonoBehaviour
{
    public float minX,maxX;
    public float minY,maxY;

    public float _minX,_maxX;
    public float _minY,_maxY;

    [Header("Guns Stats")]
    public float timeBetweenShooting,spread, range, reloadTime, timeBetweenShots;
    public int allAmmo,magazineSize, bulletsPerTap;
    public bool allowButtonHold;
    int bulletsLeft, bulletsShot;
    public float gDamage;
    public Transform camera;
    Vector3 rot_; 

    //bools 
    bool shooting, readyToShoot, reloading;

    //Reference
    [Header("Reference")]
    public Camera fpsCam;
    public Transform attackPoint;
    public RaycastHit rayHit;
    //public LayerMask whatIsEnemy;

    //Graphics
    [Header("Graphics")]
    public GameObject bulletHoleGraphic;
    public ParticleSystem muzzleFlash;
    //public CamShake camShake;
    //public float camShakeMagnitude, camShakeDuration;
    public TextMeshProUGUI text;

    [Header("Aim")]
    public Vector3 aimT;
    public Vector3 orT;
    public bool aiming;

    private void Awake()
    {
        bulletsLeft = magazineSize;
        readyToShoot = true;

    }

    private void Start()
    {

    }

    private void Update()
    {
        MyInput();

        rot_ = camera.transform.localRotation.eulerAngles;

        //SetText
        text.SetText(bulletsLeft + " | " + allAmmo);
        if(rot_.x != 0 || rot_.y != 0 ){
            camera.transform.localRotation = Quaternion.Slerp(camera.localRotation,Quaternion.Euler(0,0,0),Time.deltaTime * 3);
        }
    }   
    private void MyInput()
    {
        if (allowButtonHold) shooting = Input.GetKey(KeyCode.Mouse0);
        else shooting = Input.GetKeyDown(KeyCode.Mouse0);

        if (Input.GetKeyDown(KeyCode.R) && bulletsLeft < magazineSize && !reloading) Reload();

        //Shoot
        if (readyToShoot && shooting && !reloading && bulletsLeft > 0){
            bulletsShot = bulletsPerTap;
            Shoot();
        }

        Vector3 target = orT;
        if (Input.GetMouseButton(1)){
            aiming = true;
            target = aimT;
        }
        else{
            aiming = false;
        }
        Vector3 wtf = Vector3.Lerp(transform.localPosition,target,Time.deltaTime*10);
        transform.localPosition = wtf;
    }
    private void Shoot()
    {
        readyToShoot = false;

        //Spread
        float x = Random.Range(-spread, spread);
        float y = Random.Range(-spread, spread);

        //Calculate Direction with Spread
        Vector3 direction = fpsCam.transform.forward + new Vector3(x,y,0);

        //RayCast
        if (Physics.Raycast(fpsCam.transform.position, direction, out rayHit, range))
        {
            Debug.Log(rayHit.collider.name);

            if (rayHit.collider.CompareTag("Enemy")){
                rayHit.collider.GetComponent<EnemyAi>().TakeDamage(gDamage);
            }
            else if(rayHit.collider.CompareTag("Ground")){
                GameObject blthl = Instantiate(bulletHoleGraphic, rayHit.point, Quaternion.LookRotation(rayHit.normal));
                Destroy(blthl,5f);
                blthl.transform.position += blthl.transform.forward / 1000;
            }
        }


        //Graphics
        muzzleFlash.Play();

        bulletsLeft--;
        bulletsShot--;

        Invoke("ResetShot", timeBetweenShooting);

        if(bulletsShot > 0 && bulletsLeft > 0)
        Invoke("Shoot", timeBetweenShots);

        if(aiming == false){
          recoil();  
        }
        else if(aiming == true){
            aimRecoil();
        }
    }
    private void ResetShot()
    {
        readyToShoot = true;
    }
    private void Reload()
    {
        reloading = true;
        Invoke("ReloadFinished", reloadTime);
    }
    private void ReloadFinished()
    {
        allAmmo += bulletsLeft;
        if(allAmmo > magazineSize){
            allAmmo -= magazineSize;
            bulletsLeft = magazineSize;
        }
        else if(allAmmo < magazineSize){
            bulletsLeft = allAmmo;
            allAmmo -= bulletsLeft;
        }
        else{
            bulletsLeft = allAmmo;
        }   
        reloading = false;
    }

    void recoil()
    {
        float recoilX = Random.Range(minX,maxX);
        float RECOILy = Random.Range(minY,maxY);
        camera.transform.localRotation = Quaternion.Euler(rot_.x - RECOILy,rot_.y + recoilX,rot_.z);
    }

    void aimRecoil()
    {
        float _recoilX = Random.Range(_minX,_maxX);
        float _RECOILy = Random.Range(_minY,_maxY);
        camera.transform.localRotation = Quaternion.Euler(rot_.x - _RECOILy,rot_.y + _recoilX,rot_.z);
    }    
}
