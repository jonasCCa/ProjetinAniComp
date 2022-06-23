using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interpretador : MonoBehaviour
{
    public static Exp interpretar(Exp expressao, CustomParticle p)  {
        return expressao.smallstep(p);
    }

    public static Exp gerar(string parseIn) {
        

        return null;
    }
}
