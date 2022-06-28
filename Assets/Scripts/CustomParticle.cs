using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomParticle : MonoBehaviour
{
    [Space(5)]
    [Header("Lifetime")]
    public float timeToLive; // In seconds
    public float spawnTime;
    public Vector3 spawnPos;

    [Space(5)]
    [Header("Physics")]
    [Space(5)]
    // Rigidbody
    public Rigidbody rb;
    // Speed
    public Vector3 speed;
    // Acceleration
    public Vector3 accel;

    [Space(5)]
    [Header("Skew (With Formulas")]
    [Space(5)]
    public Vector3 randSkew;

    [Space(5)]
    [Header("Visual")]
    [Space(5)]
    // Size
    public float sizeModifier;
    // Color
    public float rModifier, gModifier, bModifier;

    public bool isDead() {
        if(timeToLive < Time.time - spawnTime)
            return true;
        
        return false;
    }
}
