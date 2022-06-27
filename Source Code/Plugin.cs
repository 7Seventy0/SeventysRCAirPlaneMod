using BepInEx;
using System;
using System.Reflection;
using UnityEngine;
using UnityEngine.InputSystem;
using Utilla;
using System.Collections;
using SeventysGuidedMissileMod;
using UnityEngine.XR;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
using HarmonyLib;

namespace SeventysRCAirPlaneMod
{
    /// <summary>
    /// This is your mod's main class.
    /// </summary>

    /* This attribute tells Utilla to look for [ModdedGameJoin] and [ModdedGameLeave] */
    [ModdedGamemode]
    [BepInDependency("org.legoandmars.gorillatag.utilla", "1.5.0")]
    [BepInPlugin(SeventysGuidedMissileMod.PluginInfo.GUID, SeventysGuidedMissileMod.PluginInfo.Name, SeventysGuidedMissileMod.PluginInfo.Version)]
    public class Plugin : BaseUnityPlugin
    {
        bool inRoom;

        GameObject launcherInstance;
        GameObject missile;

        GameObject missileInstance;

        GameObject bullet;

        GameObject hitParticle;

        GameObject deathParticle;
        void OnEnable()
        {
            /* Set up your mod here */
            /* Code here runs at the start and whenever your mod is enabled*/

            HarmonyPatches.ApplyHarmonyPatches();
            Utilla.Events.GameInitialized += OnGameInitialized;
        }

        void OnDisable()
        {
            /* Undo mod setup here */
            /* This provides support for toggling mods with ComputerInterface, please implement it :) */
            /* Code here runs whenever your mod is disabled (including if it disabled on startup)*/

            HarmonyPatches.RemoveHarmonyPatches();
            Utilla.Events.GameInitialized -= OnGameInitialized;
        }

        void OnGameInitialized(object sender, EventArgs e)
        {
            StartCoroutine(SeventysStart());
        }

        IEnumerator SeventysStart()
        {
            yield return new WaitForSeconds(0.5f);
            Debug.Log("Yay");
            var fileStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("SeventysRCAirPlaneMod.Bundle.gorillalauncherbundle");
            var bundleLoadRequest = AssetBundle.LoadFromStreamAsync(fileStream);
            yield return bundleLoadRequest;
            Debug.Log("More Yay!!!");
            var myLoadedAssetBundle = bundleLoadRequest.assetBundle;
            if (myLoadedAssetBundle == null)
            {
                Debug.Log("Failed to load AssetBundle!");
                yield break;
            }

            var assetLoadRequest = myLoadedAssetBundle.LoadAssetAsync<GameObject>("GorillaLauncher");
            yield return assetLoadRequest;

            GameObject launcher = assetLoadRequest.asset as GameObject;
           launcherInstance = Instantiate(launcher);

            assetLoadRequest = myLoadedAssetBundle.LoadAssetAsync<GameObject>("gorillaMissile");
            yield return assetLoadRequest;

            missile = assetLoadRequest.asset as GameObject;

            assetLoadRequest = myLoadedAssetBundle.LoadAssetAsync<GameObject>("TurretBullet");
            yield return assetLoadRequest;

            bullet = assetLoadRequest.asset as GameObject;


            assetLoadRequest = myLoadedAssetBundle.LoadAssetAsync<GameObject>("HitParticle");
            yield return assetLoadRequest;

            hitParticle = assetLoadRequest.asset as GameObject;

            assetLoadRequest = myLoadedAssetBundle.LoadAssetAsync<GameObject>("DestroyParticle");
            yield return assetLoadRequest;

            deathParticle = assetLoadRequest.asset as GameObject;

            yield return new WaitForSeconds(0.2f);

            GameObject.Find("Player").AddComponent<PlayerClass>();
            // WOW

            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.transform.position = new Vector3(-49.14f, 17.060f, -119.2023f);
            cube.transform.eulerAngles = new Vector3(0, -23.08f, 0);
            cube.transform.localScale = new Vector3(2.834638f, 1.284105f, 0.14357f);
           cube.GetComponent<Renderer>().material.color = new Color(0, 255, 0);
            ApplyCosmetic();

        }

        void ApplyCosmetic()
        {
            GameObject body = GameObject.Find("OfflineVRRig/Actual Gorilla/rig/body");
            launcherInstance.transform.SetParent(body.transform, false);

            launcherInstance.transform.localEulerAngles = new Vector3(274.5074f, 0 ,0);
            launcherInstance.transform.localPosition = new Vector3(0f, 0.2f, -0.1f);
            launcherInstance.transform.localScale = new Vector3(0.02f, 0.02f, 0.02f);
        }

        IEnumerator Fire()
        {
            missileInstance = Instantiate(missile, GameObject.Find("MissileSpawnPoint B").transform.position, Quaternion.identity);
          
            missileInstance.AddComponent<Missile>().Bullet = bullet;
          
            yield return new WaitForSeconds(0.1f);
                       
        }
        private readonly XRNode lNode = XRNode.LeftHand;
        bool leftPrim;
        bool leftTrigger;
        private float nextAction;
        private float coolDown = 1;




        void Update()
        {


            if (Time.time > nextAction)
            {
                InputDevices.GetDeviceAtXRNode(lNode).TryGetFeatureValue(UnityEngine.XR.CommonUsages.primaryButton, out leftPrim);
                InputDevices.GetDeviceAtXRNode(lNode).TryGetFeatureValue(UnityEngine.XR.CommonUsages.triggerButton, out leftTrigger);

                if (leftTrigger && leftPrim && missileInstance == null)
                {
                    StartCoroutine(Fire());
                    nextAction = Time.time + coolDown;
                }
            }
            if (Keyboard.current.f5Key.wasPressedThisFrame)
            {
                StartCoroutine(Fire());
            }


        }




        /* This attribute tells Utilla to call this method when a modded room is joined */
        [ModdedGamemodeJoin]
        public void OnJoin(string gamemode)
        {
            /* Activate your mod here */
            /* This code will run regardless of if the mod is enabled*/

            inRoom = true;
        }

        /* This attribute tells Utilla to call this method when a modded room is left */
        [ModdedGamemodeLeave]
        public void OnLeave(string gamemode)
        {
            /* Deactivate your mod here */
            /* This code will run regardless of if the mod is enabled*/

            inRoom = false;
        }
    }
}

//sus
namespace SolidMonkeys.Patches 
{
    [HarmonyPatch(typeof(VRRig))]
    [HarmonyPatch("Start", MethodType.Normal)]
    internal class VRRigColliderPatch
    {
        public static bool ModEnabled { get; set; }

        private static void Postfix(VRRig __instance)
        {
            if (__instance.isOfflineVRRig)
                return;

            Photon.Pun.PhotonView photView = __instance.photonView;
            if (photView != null && photView.IsMine)
                return;

            var cc = __instance.gameObject.AddComponent<BoxCollider>();
            cc.enabled = true;

            cc.center = new Vector3 (0, 2, 0);
            cc.size = new Vector3 (0.7f, 0.7f, 0.7f);
            cc.gameObject.layer = 11;
            cc.isTrigger = true;

            // Debug.Log("Thank you Haunted!");
        }
    }
}
