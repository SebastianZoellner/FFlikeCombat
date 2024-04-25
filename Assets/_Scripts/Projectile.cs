using System;
using UnityEngine;


public class Projectile : MonoBehaviour
{
    [SerializeField] float projectileSpeed = 1f;
    [SerializeField] bool isHoming = false;
    [SerializeField] GameObject impactVfx;
    [SerializeField] GameObject[] destroyOnHit = null;
    [SerializeField] SimpleAudioEventSO impactSFX;
    [SerializeField] float radius;

    public event EventHandler OnImpact;
    private PowerSO attackPower;
    private CharacterHealth targetHealth = null;
    private float range = 0;
    private Vector3 target;

    private Vector3 startPosition;

    private CharacterCombat attacker;

    //------------------------------------------
    //---------LifeCycle Methods----------------
    //------------------------------------------

    private void Start()
    {
        startPosition = transform.position;
        if (target == null) return;
        target = GetAimLocation();
        transform.LookAt(target);
    }

    private void Update()
    {
        
            if (isHoming && targetHealth.canBeTarget)
                transform.LookAt(GetAimLocation());
            //Here we could have a slower turn;
        

        if (Vector3.Distance(transform.position, startPosition) > 2 * range)
            Destroy(gameObject);

        if (target == null) return;

        //Here could be a test if the projectile will hit next turn 

        transform.Translate(Vector3.forward * projectileSpeed * Time.deltaTime);
    }

    public void Setup(CharacterCombat attacker, CharacterHealth target, float range, PowerSO power)
    {
        attackPower = power;
        this.attacker = attacker;
        this.targetHealth = target;
        this.range = range;
      

    }

    private Vector3 GetAimLocation()//this assumes all possible targets have a capsule collider
    {
        float targetHeight;
        CapsuleCollider targetCapsule = targetHealth.GetComponentInChildren<CapsuleCollider>();
        if (targetCapsule != null)
            targetHeight = targetCapsule.height / 2;
        else
            targetHeight = 2f;

        return targetHealth.transform.position + Vector3.up * targetHeight / 0.7f;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other == attacker.GetComponentInChildren<CapsuleCollider>())
            return;

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
                targetHealth.GetComponent<CharacterAudio>().PlayHitSound(impactSFX);
            }

            if(radius>0)
            {
                CharacterHealth[] targetArray;
                if (attacker.IsHero)
                    targetArray = SpawnPointController.Instance.GetAllInRadius(targetHealth, radius, Fraction.Enemy).ToArray();
                else
                    targetArray = SpawnPointController.Instance.GetAllInRadius(targetHealth, radius, Fraction.Hero).ToArray();

                Debug.Log(targetArray.Length + "Explosion Targets");

                foreach(CharacterHealth ch in targetArray)
                {
                    if (ch == targetHealth) continue;
                    Debug.Log("Explosion also hitting " + ch.name);
                    attacker.ManageHit(ch);
                }

            }
            return;
        }
        
        Debug.Log("Miss");
        
    }

}