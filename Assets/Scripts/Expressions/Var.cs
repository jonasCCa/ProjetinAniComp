using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Var : Exp
{
    private VarType identificador;

    public Var(VarType identificador) {
        this.identificador = identificador;
    }

    override public Exp smallstep(CustomParticle p) {
        //return new Num(p.ler(identificador));

        // Switch case para pegar qualquer variável da partícula p
        switch(identificador) {
            case VarType.X_POS:
                return new Num(p.transform.position.x);

            case VarType.Y_POS:
                return new Num(p.transform.position.y);
                
            case VarType.Z_POS:
                return new Num(p.transform.position.z);

            case VarType.SPEED:
                return new Num(p.speed.x + p.speed.y + p.speed.z);
                
            case VarType.ACCEL:
                return new Num(p.accel.x + p.accel.y + p.accel.z);
                
            case VarType.MASS:
                return new Num(p.mass);
        }

        return null;
    }

    override public string ToString() {
        return identificador.ToString();
    }

    public VarType getValor(){
        return identificador;
    }
}
