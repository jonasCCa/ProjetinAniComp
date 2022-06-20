using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomParticleSystem : MonoBehaviour
{
    [Header("Eu entendi a referência")]
    public GameObject particlePrefab;

    [Header("Particles")]
    public bool isSpawning;
    public float spawnDelay;
    public int maxSpawnPerFrame = 1;
    float lastSpawned;
    public int particleQuantity;
    public float minTTL, maxTTL;
    public List<CustomParticle> particleArray;

    [Header("Physics")]
    // Speed
    public Vector3 minSpeed;
    public Vector3 maxSpeed;
    // Acceleration
    public Vector3 minAccel, maxAccel;
    // Gravity
    public bool useGravity;

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
                    CustomParticle p = Instantiate(particlePrefab, transform.position, Quaternion.identity, transform).GetComponent<CustomParticle>();
                    particleArray.Add(p);
                    //setar configs da partícula                    
                    p.timeToLive = Random.Range(minTTL, maxTTL);

                    p.speed.x = Random.Range(minSpeed.x, maxSpeed.x);
                    p.speed.y = Random.Range(minSpeed.y, maxSpeed.y);
                    p.speed.z = Random.Range(minSpeed.z, maxSpeed.z);

                    p.accel.x = Random.Range(minAccel.x, maxAccel.x);
                    p.accel.y = Random.Range(minAccel.y, maxAccel.y);
                    p.accel.z = Random.Range(minAccel.z, maxAccel.z);

                    p.spawnTime = Time.time;

                    // Setar Rigidbody
                    p.rb = p.GetComponent<Rigidbody>();
                    // Gravidade da engine
                    p.rb.useGravity = useGravity;
                    // Velocidade inicial
                    p.rb.AddForce(p.speed);
                }
            } else {
                // spawn com delay
                if(spawnDelay < Time.time - lastSpawned) {
                    int i =0;
                    while(particleArray.Count < particleQuantity && i < maxSpawnPerFrame) {
                        CustomParticle p = Instantiate(particlePrefab, transform.position, Quaternion.identity, transform).GetComponent<CustomParticle>();
                        particleArray.Add(p);
                        //setar configs da partícula
                        p.timeToLive = Random.Range(minTTL, maxTTL);

                        p.speed.x = Random.Range(minSpeed.x, maxSpeed.x);
                        p.speed.y = Random.Range(minSpeed.y, maxSpeed.y);
                        p.speed.z = Random.Range(minSpeed.z, maxSpeed.z);

                        p.accel.x = Random.Range(minAccel.x, maxAccel.x);
                        p.accel.y = Random.Range(minAccel.y, maxAccel.y);
                        p.accel.z = Random.Range(minAccel.z, maxAccel.z);

                        p.spawnTime = Time.time;

                        // Setar Rigidbody
                        p.rb = p.GetComponent<Rigidbody>();
                        // Gravidade da engine
                        p.rb.useGravity = useGravity;
                        // Velocidade inicial
                        p.rb.AddForce(p.speed);

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

            if (p.isDead()) {
                Destroy(particleArray[i].gameObject);

                particleArray.RemoveAt(i);
                i--;
            }
        }
    }
}
