using System;
using UnityEngine;


public class Projectile : MonoBehaviour
{
    [SerializeField] float projectileSpeed = 1f;
    [SerializeField] bool isHoming = false;
    [SerializeField] float rotationSpeed = 2f;
    [SerializeField] float homingDelay = 0.3f;
    [SerializeField] GameObject impactVfx;
    [SerializeField] GameObject[] destroyOnHit = null;
    [SerializeField] SimpleAudioEventSO impactSFX;
    [SerializeField] float radius;
    public bool projectilePerTarget = false;

    public event EventHandler OnImpact;
    private PowerSO attackPower;
    private IDamageable targetHealth = null;
    private float range = 0;
    private Vector3 target;

    private Vector3 startPosition;
    private float flyTime;

    private CharacterCombat attacker;

    //------------------------------------------
    //---------LifeCycle Methods----------------
    //------------------------------------------

    private void Start()
    {
        startPosition = transform.position;
        if (target == null) return;
target = GetAimLocation();
        if (!isHoming)
        {         
            transform.LookAt(target);
        }
    }

    private void Update()
    {
        flyTime += Time.deltaTime;

        if (isHoming && targetHealth.canBeTarget&&flyTime>homingDelay)
        {
            Vector3 targetDirection=target-transform.position;
            Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, rotationSpeed * Time.deltaTime,0f);
            transform.rotation = Quaternion.LookRotation(newDirection);
        }  

        if (Vector3.Distance(transform.position, startPosition) > 2 * range)
            Destroy(gameObject);

        if (target == null) return;

        //Here could be a test if the projectile will hit next turn 

        transform.Translate(Vector3.forward * projectileSpeed * Time.deltaTime);
    }

    //------------------------------------------
    //---------Public Methods----------------
    //------------------------------------------



    public void Setup(CharacterCombat attacker, IDamageable target, float range, PowerSO power)
    {
        attackPower = power;
        this.attacker = attacker;
        this.targetHealth = target;
        this.range = range;
      

    }

    //------------------------------------------
    //---------Private Methods----------------
    //------------------------------------------


    private Vector3 GetAimLocation()
    {
        float targetHeight;
        Transform targetTransform=targetHealth.GetTransform();

        if (targetHealth is CharacterHealth)
        {           
            CapsuleCollider targetCapsule = targetTransform.GetComponentInChildren<CapsuleCollider>();
            if (targetCapsule != null)
                targetHeight = targetCapsule.height / 2;
            else
                targetHeight = 2f;
        }
        else
        {           
            BoxCollider targetBox = targetTransform.GetComponentInChildren<BoxCollider>();
            if (targetBox != null)
                targetHeight = targetBox.size.y / 2;
            else
                targetHeight = 1f;
        }

        return targetTransform.position + Vector3.up * targetHeight / 0.7f;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other == attacker.GetComponentInChildren<CapsuleCollider>())
            return;

        Debug.Log("Projectile "+name+" hitting " + other.name);
        Impact(other);
    }

    private void Impact(Collider other)
    {
        
        if (attacker.ManageHit(targetHealth))
        {
            projectileSpeed = 0;
            Vector3 pointOfContact = other.ClosestPoint(transform.position);
            Vector3 collisionNormal = (pointOfContact - transform.position).normalized;
            Quaternion rot = Quaternion.FromToRotation(Vector3.up, collisionNormal);
            if (impactVfx)
            {
                //Debug.Log(name + " playing impact VFX");
                GameObject vfx = Instantiate(impactVfx, pointOfContact, rot);

                if (vfx != null)
                    Destroy(vfx, 2);


                if (destroyOnHit.Length == 0)
                    Destroy(gameObject);
                else
                {
                    float lifeAfterImpact = .3f;
                    foreach (GameObject toDestroy in destroyOnHit)
                        Destroy(toDestroy);
                    Destroy(gameObject, lifeAfterImpact);
                }
            }
            if (impactSFX)
            {
                targetHealth.GetTransform().GetComponent<Audio>().PlayHitSound(impactSFX);
            }

            if(radius>0)
            {
                IDamageable[] targetArray;
                if (attacker.IsHero)
                    targetArray = SpawnPointController.Instance.GetAllInRadius(targetHealth.GetTransform(), radius, Faction.Enemy).ToArray();
                else
                    targetArray = SpawnPointController.Instance.GetAllInRadius(targetHealth.GetTransform(), radius, Faction.Hero).ToArray();

                Debug.Log(targetArray.Length + "Explosion Targets");

                foreach(IDamageable id in targetArray)
                {
                    if (id == targetHealth) continue;
                    Debug.Log("Explosion also hitting " + id.GetTransform().name);
                    attacker.ManageHit(id);
                }

            }
            return;
        }
        
        Debug.Log("Projectile Miss");
        
    }

}