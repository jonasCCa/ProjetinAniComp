using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomParticle : MonoBehaviour
{
    [Space(5)]
    [Header("Lifetime")]
    public float timeToLive; // In seconds
    public float spawnTime;

    [Space(5)]
    [Header("Physics")]
    [Space(5)]
    // Mass
    public float mass;
    // Rigidbody
    public Rigidbody rb;
    // Speed
    public Vector3 speed;
    // Acceleration
    public Vector3 accel;

    [Space(5)]
    [Header("Visual")]
    [Space(5)]
    // Size
    public float sizeModifier;
    // Color
    public float rModifier, gModifier, bModifier;
    // Shape
    public bool is3D;
    public Sprite sprite;
    public GameObject mesh;
    // Transparency
    public float alphaMultiplier;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool isDead() {
        if(timeToLive < Time.time - spawnTime)
            return true;
        
        return false;
    }
}
