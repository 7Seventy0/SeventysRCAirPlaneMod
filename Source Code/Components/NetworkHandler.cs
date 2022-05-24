//using UnityEngine;
//using System.Collections;
//using Photon.Pun;
//using ExitGames.Client.Photon;
//using Photon.Realtime;
//using UnityEngine.InputSystem;

//public class NetworkHandler : MonoBehaviour
//{
//    const byte SPAWN_CODE = 23;
//    const byte POSANDROT_CODE = 4;
//    const byte HIT_CODE = 10;
//    const byte DESTROY_CODE = 8;
//    const byte HITPARTICLE_CODE = 67;
//    const byte DEATHPARTICLE_CODE = 63;
//    const byte TEST_CODE = 34;
//    const byte BULLET_SPAWN_CODE = 132;
//    const byte PLAYER_HIT_CODE = 124;
//    private object[] spawnData;
//    private object[] hitData;
//    private object[] playerHitData;
//    private object[] hitParticleData;
//    private object[] posAndRotData;
//    private object[] destroyData;
//    private object[] testData;
//    private object[] deathParticleData;
//    private object[] bulletData;
//    public GameObject plane;
//    public GameObject hitParticle;
//    public GameObject deathParticle;
//    public GameObject bulletToSpawn;
//    void Start()
//    {
//        gameObject.AddComponent<PhotonView>().ViewID = 234;

//    }

//    // Update is called once per frame
//    void Update()
//    {

//    }

//    public void InstantiateHitParticles(float xpos, float ypos, float zpos)
//    {
//        hitParticleData = new object[] {xpos, ypos, zpos};
//        PhotonNetwork.RaiseEvent(HITPARTICLE_CODE, hitParticleData,  new RaiseEventOptions { Receivers = ReceiverGroup.All }, SendOptions.SendUnreliable);
//    }
//    public void InstantiateDeathParticles(float xpos, float ypos, float zpos)
//    {
//        deathParticleData = new object[] { xpos, ypos, zpos };
//        PhotonNetwork.RaiseEvent(DEATHPARTICLE_CODE, deathParticleData, new RaiseEventOptions { Receivers = ReceiverGroup.All }, SendOptions.SendReliable);
//    }

//    public void SendIDAndSpawn(float ID)
//    {
//        float cID = GorillaParent.instance.GetComponentsInChildren<VRRig>()[0].gameObject.GetComponent<PhotonView>().ViewID;
//        spawnData = new object[] {ID,cID};
//        PhotonNetwork.RaiseEvent(SPAWN_CODE, spawnData, RaiseEventOptions.Default, SendOptions.SendReliable);
//    }
//    public void SpawnBullet(float xpos,float ypos , float zpos, float xrot, float yrot, float zrot, float xforce,float yforce,float zforce)
//    {
       
//        bulletData = new object[] {xpos,ypos,zpos,xrot,yrot,zrot,xforce,yforce,zforce};
//        PhotonNetwork.RaiseEvent(BULLET_SPAWN_CODE, bulletData, RaiseEventOptions.Default, SendOptions.SendReliable);
//    }

//    public void SendHitEvent(float damage, float controllerNr)
//    {
//        hitData = new object[] {damage, controllerNr};
//        RaiseEventOptions options = new RaiseEventOptions { TargetActors = new int[] { (int)controllerNr } };
//        PhotonNetwork.RaiseEvent(HIT_CODE, hitData,options, SendOptions.SendReliable);
//    }
//    public void SendPlayerHitEvent(float damage, float controllerNr)
//    {
//        playerHitData = new object[] { damage, controllerNr };
//        RaiseEventOptions options = new RaiseEventOptions { TargetActors = new int[] { (int)controllerNr } };
//        PhotonNetwork.RaiseEvent(PLAYER_HIT_CODE, playerHitData, options, SendOptions.SendReliable);
//    }

//    public void SendPosAndRotData(float xpos, float ypos, float zpos, float xrot, float yrot, float zrot, float ID)
//    {
//        posAndRotData = new object[] {xpos, ypos,zpos,xrot,yrot,zrot,ID};
//        PhotonNetwork.RaiseEvent(POSANDROT_CODE, posAndRotData, RaiseEventOptions.Default, SendOptions.SendUnreliable);
        
       
//    }
//    public void DestroyPlaneByID(float ID)
//    {
//        destroyData = new object[] {ID};
//        PhotonNetwork.RaiseEvent(DESTROY_CODE, destroyData, RaiseEventOptions.Default, SendOptions.SendUnreliable);


//    }

//    public void RunTestCode(float coolValue)
//    {
//        testData = new object[] { };
//        RaiseEventOptions options = new RaiseEventOptions { TargetActors = new int[] { (int)coolValue } };
//        PhotonNetwork.RaiseEvent(TEST_CODE, testData, options, SendOptions.SendUnreliable);

//    }

//    void OnEnable()
//    {
//        PhotonNetwork.NetworkingClient.EventReceived += NetworkingClient_EventReceived;
//    }
//    void OnDisable()
//    {
//        PhotonNetwork.NetworkingClient.EventReceived -= NetworkingClient_EventReceived;
//    }
//    void NetworkingClient_EventReceived(EventData eventData)
//    {
        
//        if (eventData.Code == SPAWN_CODE)
//        {
//            object[] data = (object[])eventData.CustomData;

//            float id = (float)data[0];
//            float controllerID = (float)data[1];
//            GameObject planeNetwork = Instantiate(plane);
//            planeNetwork.AddComponent<NetworkedPlane>();
//            planeNetwork.GetComponent<NetworkedPlane>().GiveMeMyID(id);
//            planeNetwork.GetComponent<NetworkedPlane>().controllerID = controllerID;
//        }
//        if(eventData.Code == POSANDROT_CODE)
//        {
            
//            object[] data = (object[])eventData.CustomData;
            
//            float xpos = (float)data[0];
//            float ypos = (float)data[1];
//            float zpos = (float)data[2];

           
//            float xrot = (float)data[3];
//            float yrot = (float)data[4];
//            float zrot = (float)data[5];

           
//            float id = (float)data[6];
            
           
//            foreach (NetworkedPlane plane in FindObjectsOfType<NetworkedPlane>())
//            {
//                if(plane.ID == id)
//                {
                    
//                    plane.transform.position = new Vector3(xpos, ypos, zpos);
//                    plane.transform.eulerAngles = new Vector3(xrot, yrot, zrot);
//                }
//            }
//        }

//        if (eventData.Code == DESTROY_CODE)
//        {
//            object[] data = (object[])eventData.CustomData;

//            float id = (float)data[0];
//            foreach (NetworkedPlane plane in FindObjectsOfType<NetworkedPlane>())
//            {
//                if (plane.ID == id)
//                {
//                    Debug.Log("Destroying Plane With ID :" + id);
//                    plane.DestroyMe();
//                }
//            }
//        }
//        if (eventData.Code == HIT_CODE)
//        {
//            Debug.Log("Just Got Hit!");
//            object[] data = (object[])eventData.CustomData;

//            float shooterView = (float)data[1];
            
//            float damage = (float)data[0];

//            GameObject.FindObjectOfType<Missile>().TakeDamage(damage);

//        }
//        if (eventData.Code == PLAYER_HIT_CODE)
//        {
//            Debug.Log("YOU Just Got Hit!");
//            object[] data = (object[])eventData.CustomData;

            
//            float damage = (float)data[0];

//            GameObject.FindObjectOfType<PlayerClass>().TakeDamage(damage);

//        }

//        if (eventData.Code == HITPARTICLE_CODE)
//        {
           
//            object[] data = (object[])eventData.CustomData;

//            float xpos = (float)data[0];
//            float ypos = (float)data[1];
//            float zpos = (float)data[2];



//            GameObject particle = Instantiate(hitParticle);
//            particle.AddComponent<DestroyerClass>();
//            particle.transform.position = new Vector3(xpos, ypos, zpos);

//        }
//        if (eventData.Code == DEATHPARTICLE_CODE)
//        {

//            object[] data = (object[])eventData.CustomData;

//            float xpos = (float)data[0];
//            float ypos = (float)data[1];
//            float zpos = (float)data[2];



//            GameObject particle = Instantiate(deathParticle);
//            particle.AddComponent<DestroyerClass>();
//            particle.transform.position = new Vector3(xpos, ypos, zpos);

//        }
//        if (eventData.Code == BULLET_SPAWN_CODE)
//        {

//            object[] data = (object[])eventData.CustomData;

//            float xpos = (float)data[0];
//            float ypos = (float)data[1];
//            float zpos = (float)data[2];

//            float xrot = (float)data[3];
//            float yrot = (float)data[4];
//            float zrot = (float)data[5];


//            float xforce = (float)data[6];
//            float yforce = (float)data[7];
//            float zforce = (float)data[8];



  



//            GameObject bullet = Instantiate(bulletToSpawn);
//            bullet.AddComponent<DestroyerClass>();
//            bullet.transform.position = new Vector3(xpos, ypos, zpos);
//            bullet.transform.eulerAngles = new Vector3(xrot, yrot, zrot);
//            bullet.AddComponent<Rigidbody>();
//            bullet.GetComponent<Rigidbody>()
//                .AddForce(bullet.transform.forward * 20, ForceMode.Impulse);
//            bullet.GetComponent<Rigidbody>().useGravity = false;
//            bullet.AddComponent<BulletClass>().damage = 0;

//            bullet.GetComponent<BoxCollider>().enabled = false;
//            bullet.layer = 0;

//        }

//        if (eventData.Code == TEST_CODE)
//        {
//            Debug.Log("Test Code Came Trough!");
//        }

//    }
//}