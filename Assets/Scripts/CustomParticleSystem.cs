using System.Reflection;
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
    [Tooltip("Usage: \nElement 0 for x = ...\nElement 1 for y = ...\nElement 2 for z = ...")]
    public string[] customFormulas;
    bool usingFormula;
    [Button("SetFormulas", BindingFlags.NonPublic | BindingFlags.Instance)] public bool foo;
    void SetFormulas() {
        if(ValidateFormulas())
            usingFormula = true;
    }
    [Button("StopFormulas", BindingFlags.NonPublic | BindingFlags.Instance)] public bool bar;
    void StopFormulas() {
        usingFormula = false;
    }
    
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
    Expression expX, expY, expZ;

    // Start is called before the first frame update
    void Start()
    {

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
            
            if(!usingFormula) {
                // Aceleração
                p.rb.AddForce(p.accel);

                // Força externa
                p.rb.AddForce(externalForce);
            } else {
                argX.setArgumentValue(p.transform.position.x - p.spawnPos.x);
                argY.setArgumentValue(p.transform.position.y - p.spawnPos.y);
                argZ.setArgumentValue(p.transform.position.z - p.spawnPos.z);

                float movX = (float)expX.calculate();
                float movY = (float)expY.calculate();
                float movZ = (float)expZ.calculate();

                p.rb.MovePosition(new Vector3(p.spawnPos.x+movX,
                                              p.spawnPos.y+movY,
                                              p.spawnPos.z+movZ));
            }

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
        // Mudar poisção inicial da partícula para emissões diferentes
        CustomParticle p = Instantiate(particlePrefab, transform.position, Quaternion.identity, transform).GetComponent<CustomParticle>();
        particleArray.Add(p);

        // Momento do nascimento da partícula
        p.spawnTime = Time.time;
        // Posição inicial da partícula
        p.spawnPos = p.transform.position;

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
        if(!usingFormula)
            p.rb.AddForce(p.speed);
    }

    public void setExternalForce(Vector3 input) {
        externalForce = input;
    }

    [ContextMenu("Set Formulas")]
    public bool ValidateFormulas() {
        if(customFormulas.Length==3) {
            if(customFormulas[0].Length>0 && customFormulas[1].Length>0 && customFormulas[2].Length>0) {
                expX = new Expression(customFormulas[0], argX, argY, argZ);
                expY = new Expression(customFormulas[1], argX, argY, argZ);
                expZ = new Expression(customFormulas[2], argX, argY, argZ);

                if(!expX.checkSyntax() || !expY.checkSyntax() || !expZ.checkSyntax()){
                    Debug.LogWarning("Invalid formula(s)\nDetails:\n" + expX.getErrorMessage() + "\n"
                                                                      + expY.getErrorMessage() + "\n"
                                                                      + expZ.getErrorMessage());
                    return false;
                }

                return true;
            }
            Debug.LogWarning("Invalid empty formula");
            return false;
        }

        Debug.LogWarning("Too few formulas; 3 are needed, one for each position axis");
        return false;
    }
}
