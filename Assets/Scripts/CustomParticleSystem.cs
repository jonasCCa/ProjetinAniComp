using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using org.mariuszgromada.math.mxparser;

public class CustomParticleSystem : MonoBehaviour
{
    [Header("Visuals")]
    public GameObject particlePrefab;
    public Color colorMin, colorMax;
    public bool changesColor;
    public Color finalColorMin, finalColorMax;
    public float sizeMin, sizeMax = 1f;
    public bool changesSize;
    public float finalSizeMin, finalSizeMax = 1f;

    [Header("Physics")]
    public bool useGravity;
    public Vector3 minSpeed, maxSpeed;
    public Vector3 minAccel, maxAccel;
    public Vector3 externalForce;

    //[Header("Custom Formula")]
    [Tooltip("Usage: \nElement 0 for x += ...\nElement 1 for y += ...\nElement 2 for z += ...")]
    public string[] customFormula;
    //Exp[] formula;
    
    [Header("Particles")]
    public bool isSpawning;
    public float spawnDelay;
    public int maxSpawnPerFrame = 1;
    float lastSpawned;
    public int particleQuantity;
    public float minTTL, maxTTL;
    public List<CustomParticle> particleArray;

    // Auxiliares
    float pScale;
    Renderer mr;
    Argument argX = new Argument("x",0);
    Argument argY = new Argument("y",0);
    Argument argZ = new Argument("z",0);
    Expression e;

    // Start is called before the first frame update
    void Start()
    {
        //formula = new Exp[3];

        //formula[0] = new Mult(new Sin(new Var(VarType.X_POS)), new Num(0.1f));
    }

    // Update is called once per frame
    void Update()
    {
        if(isSpawning) {
            // spawn sem delay
            if(spawnDelay == 0) {
                while(particleArray.Count < particleQuantity) {
                    InstantiateParticle();
                }
            } else {
                // spawn com delay
                if(spawnDelay < Time.time - lastSpawned) {
                    int i =0;
                    while(particleArray.Count < particleQuantity && i < maxSpawnPerFrame) {
                        InstantiateParticle();

                        i++;
                    }

                    lastSpawned = Time.time;
                }
            }
        }

        
    }

    void FixedUpdate() {
        // Executa partículas
        for(int i=0; i<particleArray.Count; i++) {
            CustomParticle p = particleArray[i];
            
            // Aceleração
            p.rb.AddForce(p.accel);

            // Força externa
            p.rb.AddForce(externalForce);

            // argX.setArgumentValue(p.transform.position.x);
            // argY.setArgumentValue(p.transform.position.y);
            // argZ.setArgumentValue(p.transform.position.z);

            // float mov = (float)e.calculate();

            // p.rb.MovePosition(new Vector3(p.transform.position.x+5*Time.deltaTime,
            //                               p.transform.position.y+mov,
            //                               p.transform.position.z));

            // Tamanho
            if(changesSize) {
                Vector3 pLoc = p.transform.localScale;
                p.transform.localScale = new Vector3(pLoc.x+p.sizeModifier*Time.deltaTime,
                                                     pLoc.y+p.sizeModifier*Time.deltaTime,
                                                     pLoc.z+p.sizeModifier*Time.deltaTime);
            }
            // Cor
            if(changesColor) {
                mr = p.GetComponent<Renderer>();
                mr.material.color = new Color(mr.material.color.r+p.rModifier*Time.deltaTime,
                                              mr.material.color.g+p.gModifier*Time.deltaTime,
                                              mr.material.color.b+p.bModifier*Time.deltaTime);
            }

            if (p.isDead()) {
                Destroy(particleArray[i].gameObject);

                particleArray.RemoveAt(i);
                i--;
            }
        }
    }

    void InstantiateParticle() {
        CustomParticle p = Instantiate(particlePrefab, transform.position, Quaternion.identity, transform).GetComponent<CustomParticle>();
        particleArray.Add(p);

        // Momento do nascimento da partícula
        p.spawnTime = Time.time;

        // Setar configs da partícula
        p.timeToLive = Random.Range(minTTL, maxTTL);

        //// Visuals ////
        mr = p.GetComponent<Renderer>();
        // Cor inicial
        mr.material.color = new Color(Random.Range(colorMin.r, colorMax.r),
                                      Random.Range(colorMin.g, colorMax.g),
                                      Random.Range(colorMin.b, colorMax.b));
        if(changesColor) {
            p.rModifier = (Random.Range(finalColorMin.r, finalColorMax.r) - mr.material.color.r)/p.timeToLive;
            p.gModifier = (Random.Range(finalColorMin.g, finalColorMax.g) - mr.material.color.g)/p.timeToLive;
            p.bModifier = (Random.Range(finalColorMin.b, finalColorMax.b) - mr.material.color.b)/p.timeToLive;
        }

        // Tamanho inicial
        pScale = Random.Range(sizeMin, sizeMax);
        p.transform.localScale = new Vector3(pScale, pScale, pScale);
        if(changesSize)
            p.sizeModifier = (Random.Range(finalSizeMin, finalSizeMax) - pScale)/p.timeToLive;

        //// Physics ////

        // Eixos da velocidade
        p.speed.x = Random.Range(minSpeed.x, maxSpeed.x);
        p.speed.y = Random.Range(minSpeed.y, maxSpeed.y);
        p.speed.z = Random.Range(minSpeed.z, maxSpeed.z);
        // Eixos da aceleração
        p.accel.x = Random.Range(minAccel.x, maxAccel.x);
        p.accel.y = Random.Range(minAccel.y, maxAccel.y);
        p.accel.z = Random.Range(minAccel.z, maxAccel.z);

        // Setar Rigidbody
        p.rb = p.GetComponent<Rigidbody>();
        // Gravidade da engine
        p.rb.useGravity = useGravity;
        // Velocidade inicial
        // if usar interpretador
        p.rb.AddForce(p.speed);
    }

    public void setExternalForce(Vector3 input) {
        externalForce = input;
    }

    [ContextMenu("Set Formulas")]
    public void setFormula() {
        e = new Expression(customFormula[1], argX, argY, argZ);
    }
}
