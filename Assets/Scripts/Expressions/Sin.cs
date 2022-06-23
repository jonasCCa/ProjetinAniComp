using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sin : Exp
{
    private Exp exp;

    public Sin(Exp exp) {
        this.exp = exp;
    }

    override public string ToString() {
        return " Sin( " + exp + " )";
    }

    override public Exp smallstep(CustomParticle p) {
        if (!(exp is Num)) {
            return new Sin(exp.smallstep(p));
        } else {
            return new Num(Mathf.Sin(((Num)exp).getValor()));
        }
    }
}
