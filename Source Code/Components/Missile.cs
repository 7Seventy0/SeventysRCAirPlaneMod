using UnityEngine;
using System.Collections;
using UnityEngine.XR;
using UnityEngine.InputSystem;

public class Missile : MonoBehaviour
{


    public float ID;

    Rigidbody rb;
    AudioSource audioSource;
    GameObject camera;
    bool leftStickClick;
    bool leftSec;
    bool control;
    //NetworkHandler network;
    public GameObject Bullet;

    public float health = 5;

    GameObject leftTurret;
    GameObject rightTurret;

    void Start()
    {
        gameObject.layer = 0;
        ID = Random.Range(1, 9999);
        transform.localScale = new Vector3(0.03f, 0.03f, 0.03f);
        transform.localEulerAngles = new Vector3(-90f,0,0f);
        rb = gameObject.AddComponent<Rigidbody>();
        rb.drag = 3;
        audioSource = GameObject.Find("AudioLoop").GetComponent<AudioSource>();
        audioSource.Play();
        camera = GameObject.Find("GorillaMissileCamera");

        leftTurret = GameObject.Find("TurretL");
        rightTurret = GameObject.Find("TurretR");
        
        rb.useGravity = false;
      //  network = GameObject.Find("Player").GetComponent<NetworkHandler>();

        InvokeRepeating("SyncUpdate", 0, 0.1f);
    }
    private readonly XRNode lNode = XRNode.LeftHand;
    private readonly XRNode rNode = XRNode.RightHand;

    float coolDown = 1;
    float nextAction;


    float leftFireCoolDown = 0.2f;
    float leftNextFire;

    float rightFireCoolDown = 0.2f;
    float rightNextFire;


    bool leftTrigger;
    bool rightTrigger;
    
    void SyncUpdate()
    {
      //  network.SendPosAndRotData(transform.position.x, transform.position.y, transform.position.z, transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z, ID);
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
    }
    public void Heal(float amount)
    {
        health += amount;
    }

    void Update()
    {
        camera.GetComponent<Camera>().nearClipPlane = 0.0001f;
        camera.GetComponent<Camera>().farClipPlane = 5000f;
        rb.AddForce(rb.transform.forward / 15, ForceMode.Impulse);
        
       if(health <= 0)
        {
            DestroyMe();
        }

        if(Time.time > nextAction)
        {
            InputDevices.GetDeviceAtXRNode(lNode).TryGetFeatureValue(UnityEngine.XR.CommonUsages.primary2DAxisClick, out leftStickClick);

            if (leftStickClick)
            {
                control = !control;
                nextAction = Time.time + coolDown;
            }
        }

        InputDevices.GetDeviceAtXRNode(lNode).TryGetFeatureValue(UnityEngine.XR.CommonUsages.secondaryButton, out leftSec);
        if (leftSec || Keyboard.current.f10Key.wasPressedThisFrame)
        {
            DestroyMe();
        }


        if (control)
        {
            camera.SetActive(true);
            transform.rotation = Quaternion.Lerp(transform.rotation, camera.transform.rotation, 0.05f);

            if (Time.time > leftNextFire)
            {
                InputDevices.GetDeviceAtXRNode(lNode).TryGetFeatureValue(UnityEngine.XR.CommonUsages.triggerButton, out leftTrigger);
                if (leftTrigger)
                {
                    Shot("left");
                    leftNextFire = Time.time + leftFireCoolDown;
                }
                
                
            }
            if (Time.time > rightNextFire)
            {
                InputDevices.GetDeviceAtXRNode(rNode).TryGetFeatureValue(UnityEngine.XR.CommonUsages.triggerButton, out rightTrigger);
                if (rightTrigger)
                {
                    Shot("right");
                    rightNextFire = Time.time + rightFireCoolDown;
                }
                
            }
            bool rightStick;
            InputDevices.GetDeviceAtXRNode(rNode).TryGetFeatureValue(UnityEngine.XR.CommonUsages.primary2DAxisClick, out rightStick);
            if (rightStick)
            {
                rb.AddForce(rb.transform.forward / 6, ForceMode.Impulse);

            }
        }
        else
        {
            camera.SetActive(false);
        }
        


    }


    void DestroyMe()
    {
        Debug.Log("Destroyed Local Plane");
      //  network.DestroyPlaneByID(ID);
      //  network.InstantiateDeathParticles(transform.position.x, transform.position.y, transform.position.y);
        Destroy(gameObject);
        return;
    }
    void Shot(string whatSide)
    {
        
        if (whatSide == "left")
        {
            leftTurret.GetComponent<AudioSource>().pitch = Random.Range(3.4f, 4);
            leftTurret.GetComponent<AudioSource>().Play();
            GameObject bullet = Instantiate(Bullet, leftTurret.transform.position, transform.rotation);
            bullet.AddComponent<Rigidbody>();
            bullet.GetComponent<Rigidbody>()
                .AddForce(bullet.transform.forward * 20, ForceMode.Impulse);
            bullet.GetComponent<Rigidbody>().useGravity = false;
            bullet.AddComponent<BulletClass>().damage = 1;
          //  bullet.GetComponent<BulletClass>().networkHandler = network;
            bullet.AddComponent<DestroyerClass>();
            bullet.layer = 11;

           // network.SpawnBullet(leftTurret.transform.position.x, leftTurret.transform.position.y, leftTurret.transform.position.z, transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z, transform.forward.x, transform.forward.y, transform.forward.z);
        }
        if (whatSide == "right")
        {
            
            rightTurret.GetComponent<AudioSource>().pitch = Random.Range(3.4f, 4);
            rightTurret.GetComponent<AudioSource>().Play();
            GameObject bullet = Instantiate(Bullet, rightTurret.transform.position, transform.rotation);
            bullet.AddComponent<Rigidbody>();
            bullet.GetComponent<Rigidbody>()
                .AddForce(bullet.transform.forward * 20, ForceMode.Impulse);
            bullet.GetComponent<Rigidbody>().useGravity = false;
            bullet.AddComponent<BulletClass>().damage = 1;
           // bullet.GetComponent<BulletClass>().networkHandler = network;
            bullet.AddComponent<DestroyerClass>();
            bullet.layer = 11;

         //   network.SpawnBullet(rightTurret.transform.position.x, rightTurret.transform.position.y, rightTurret.transform.position.z, transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z, transform.forward.x, transform.forward.y, transform.forward.z);



        }
    }
}