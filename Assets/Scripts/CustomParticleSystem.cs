using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        for(int i=0; i<particleArray.Count; i++) {
            CustomParticle p = particleArray[i];
            
            // Executa particula

            // Aceleração
            p.rb.AddForce(p.accel);

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
        p.rb.AddForce(p.speed);
    }
}
