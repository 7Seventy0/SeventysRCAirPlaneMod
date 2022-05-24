using UnityEngine;
using System.Collections;
using Photon.Pun;
using UnityEngine.InputSystem;

public class NetworkedPlane : MonoBehaviour
{
    public float ID;
    public float controllerID;
    PhotonView controller;

    //NetworkHandler networkHandler;

    TMPro.TMP_Text NameTagText;
    void Start()
    {
        gameObject.layer = 11;
       gameObject.GetComponent<BoxCollider>().isTrigger = true;
       // networkHandler = GameObject.FindObjectOfType<NetworkHandler>();
        transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        Debug.Log(controllerID);
        foreach(PhotonView view in FindObjectsOfType<PhotonView>())
        {
            if(view.ViewID == controllerID)
            {
                controller = view;
            }
            
        }

        NameTagText = GetComponentInChildren<TMPro.TMP_Text>();
        NameTagText.text = controller.Controller.NickName;
        InvokeRepeating("SlowUpdate", 0, 0.3f);
        Debug.Log("A new Networked plane has been SETUP by " + controller.Controller.NickName + " with the ID " + ID + " | the cause of this action was : A PLAYER SPAWNING A PLANE");
    }

    float hitCoolDown = 0.2f;
    float nextHit;
    void OnTriggerEnter(Collider collider)
    {
        Debug.Log(controller.Controller.NickName + "'s Plane Just Had A Collision!");
        if (Time.time > nextHit)
        {
            if (collider.GetComponent<BulletClass>() != null)
            {
                Debug.Log(controller.Controller.NickName + "'s Plane Just Took Damage (damage = " + collider.GetComponent<BulletClass>().damage + " )");
               // networkHandler.SendHitEvent(collider.GetComponent<BulletClass>().damage, controller.ControllerActorNr);
             //   networkHandler.InstantiateHitParticles(transform.position.x, transform.position.y, transform.position.z);
                nextHit = Time.time + hitCoolDown;
            }
            else
            {
               // networkHandler.SendHitEvent(3f, controller.ControllerActorNr);
              //  networkHandler.InstantiateHitParticles(transform.position.x, transform.position.y, transform.position.z);
                nextHit = Time.time + hitCoolDown;
            }

        }
    }

    void Update()
    {

    }

    public void GiveMeMyID(float idnumber)
    {
        ID = idnumber;
    }

    void SlowUpdate()
    {
        
        if (controller == null)
        {
            Debug.Log("A new Networked plane has been DESTROYED by " + controller.Controller.NickName + " with the ID " + ID + " | the cause of this action was : PLAYER HAS LEFT THE SERVER OR VRrig WAS LOST");
            DestroyMe();
          
        }
        
    }

    public void DestroyMe()
    {
        Destroy(gameObject);
        return;
    }
}