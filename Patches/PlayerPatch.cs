using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BepInEx.Logging;
using GameNetcodeStuff;
using HarmonyLib;
using static Booling2.Booling2;
using Unity.Netcode;
using UnityEngine;

//Test Patch
namespace Booling2.Patches
{
    [HarmonyPatch(typeof(PlayerControllerB))]
    internal class PlayerPatch
    {
        [HarmonyPatch("Update")]
        [HarmonyPostfix]
        private static void InfiniteSprint(ref float ___sprintMeter) {
            //___sprintMeter = 1f;
        }

        [HarmonyPatch("ConnectClientToPlayerObject")]
        [HarmonyPostfix]
        private static void AddMovementScript(PlayerControllerB __instance)
        {

                if ((UnityEngine.Object)(object)__instance != (UnityEngine.Object)null && (UnityEngine.Object)(object)((Component)__instance).gameObject.GetComponentInChildren<MovementScript>() == (UnityEngine.Object)null && ((NetworkBehaviour)__instance).IsOwner && __instance.isPlayerControlled)
                {
                    ((Component)__instance).gameObject.AddComponent<MovementScript>().myPlayer = __instance;
                    Log.LogMessage((object)"Gave player the movement script");

                }
            
        }

        internal class MovementScript : MonoBehaviour
        {
            public PlayerControllerB myPlayer;

            private bool inAir;

            public static Vector3 wantedVelToAdd;

            public float jumpTime;

            private Vector3 previousForward;

            public void Update()
            {
                if (base.transform.position.y > -80f) { 
                    if (myPlayer.playerBodyAnimator.GetBool("Jumping") && jumpTime < 0.1f)
                    {
                        myPlayer.fallValue = myPlayer.jumpForce;
                        jumpTime += Time.deltaTime * 10f;
                    }
                    myPlayer.sprintMeter = 100f;
                    if (!myPlayer.thisController.isGrounded && !myPlayer.isClimbingLadder)
                    {
                        if (!inAir)
                        {
                            inAir = true;
                            Vector3 velocity = myPlayer.thisController.velocity;
                            velocity.y = 0f;
                            wantedVelToAdd += 0.006f * velocity;
                        }
                        wantedVelToAdd.y = 0f;
                        myPlayer.thisController.Move(((Component)myPlayer).gameObject.transform.forward * ((Vector3)(wantedVelToAdd)).magnitude);
                        Vector3 forward = ((Component)myPlayer).gameObject.transform.forward;
                        Vector3 val = forward - previousForward;
                        float num = 0.01f;
                        if (((Vector3)(val)).magnitude > num)
                        {
                            wantedVelToAdd += new Vector3(0.0005f, 0.0005f, 0.0005f);
                        }
                        previousForward = forward;
                    }
                    else
                    {
                        wantedVelToAdd = Vector3.Lerp(wantedVelToAdd, Vector3.zero, Time.deltaTime * 0.001f);
                        inAir = false;
                        jumpTime = 0f;
                    }
                    _ = myPlayer.thisController.isGrounded;
                }
            }
        }
    }
}
