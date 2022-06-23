using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Num : Exp {
    private float valor;

    public Num(float valor) {
        this.valor = valor;
    }

    public float getValor() {
        return valor;
    }

    override public Exp smallstep(CustomParticle p) {
        return this;
    }

    override public string ToString() {
        return valor.ToString();
    }
}
