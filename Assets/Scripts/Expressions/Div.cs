using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Div : Exp
{
    private Exp exp1;
    private Exp exp2;

    public Div(Exp exp1, Exp exp2) {
        this.exp1 = exp1;
        this.exp2 = exp2;
    }

    override public string ToString() {
        return exp1 + " / " + "( " + exp2 + " )";
    }

    override public Exp smallstep(CustomParticle p) {
        if (!(exp1 is Num)) {
            return new Div(exp1.smallstep(p), exp2);
        } else if (!(exp2 is Num)) {
            return new Div(exp1, exp2.smallstep(p));
        } else {
            return new Num(((Num)exp1).getValor() / ((Num)exp2).getValor());
        }

    }
}
