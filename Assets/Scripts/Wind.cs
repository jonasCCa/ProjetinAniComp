using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class Wind : MonoBehaviour
{
    public CustomParticleSystem partSystem;
    public float windForce = 10f;

    [Button("LigarVento", BindingFlags.NonPublic | BindingFlags.Instance)] public bool foo;
    void LigarVento() {
        partSystem.externalForce.x = windForce;
    }

    [Button("DesligarVento", BindingFlags.NonPublic | BindingFlags.Instance)] public bool bar;
    void DesligarVento() {
        partSystem.externalForce.x = 0;
    }
}
