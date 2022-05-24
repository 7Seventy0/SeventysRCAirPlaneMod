using UnityEngine;
using System.Collections;
using Photon.Pun;

public class BulletClass : MonoBehaviour
{
    public float damage;
   // public NetworkHandler networkHandler;

    void Start()
    {
        GetComponent<BoxCollider>().isTrigger = true;
    }

    void OnTriggerEnter(Collider collider)
    {
        if(collider.GetComponent<PhotonView>() != null)
        {
            Debug.Log("JUST HIT " + collider.name);
          //  networkHandler.SendPlayerHitEvent(damage, collider.GetComponent<PhotonView>().ControllerActorNr);
            //networkHandler.InstantiateHitParticles(transform.position.x, transform.position.y, transform.position.z);
        }
    }

}