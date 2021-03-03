using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PixelAdventure
{
    public class Protagonist : MonoBehaviour
    {
        [Header("Animation")]
        public string _walkingParameter = "IsWalking";
        public string _airBorneParameter = "IsAirborne";

        [HideInInspector] public int walkingHash;
        [HideInInspector] public int airBorneHash;

        void OnEnable()
        {
            GetParameterHash();
        }
        void GetParameterHash()
        {
            walkingHash = Animator.StringToHash(_walkingParameter);
            airBorneHash = Animator.StringToHash(_airBorneParameter);
        }
    }
}