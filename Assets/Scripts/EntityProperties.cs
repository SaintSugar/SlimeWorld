using System.Runtime.InteropServices.WindowsRuntime;
using System.Collections;
using System;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class EntityProperties : MonoBehaviour
{
    [Serializable]
    public struct AnimationStates {
        [ReadOnly]
        [SerializeField]
        public AnimationClip StateRunForward;
        [ReadOnly]
        [SerializeField]
        public AnimationClip StateRunSide;
        [ReadOnly]
        [SerializeField]
        public AnimationClip StateRunBack;
        
        [ReadOnly]
        [SerializeField]
        public AnimationClip StateIdle;
        [ReadOnly]
        [SerializeField]
        public AnimationClip StateIdleSide;
        [ReadOnly]
        [SerializeField]
        public AnimationClip StateIdleBack;
        [ReadOnly]
        [SerializeField]
        public AnimationClip StateJump;
    }
    [SerializeField]
    public AnimationStates animationStates;

    [Serializable]
    public struct Abilities {
        [SerializeField]
        [ReadOnly]
        public float maximumSpeed;
    }
    [SerializeField]
    public Abilities abilities;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
