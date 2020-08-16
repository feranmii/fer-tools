using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using Random = UnityEngine.Random;

public class AIShooting : MonoBehaviour
{
    [Foldout("References")] public AIAgent agent;
    [Foldout("References")] public AgentStats stats;
    [Foldout("Bullet")] public GameObject bulletObject;
    [Foldout("Bullet")] public Transform bulletSpawnPoint;

    [Foldout("Bullet"), Min(1)] public int pelletsPerShot = 1;
    [Foldout("Bullet")] public bool isRocketLauncher;

    //Secondary Fire
    [Foldout("Secondary Fire")] public GameObject secondaryFireObject;

    [Range(0.0f, 1.0f)] public float oddsToSecondaryFire = 0.1f;
    [Foldout("Secondary Fire")] public float minDistForSecondaryFire = 10;
    [Foldout("Secondary Fire")] public float maxDistForSecondaryFire = 50;

    Vector3 lastPosTargetSeen = Vector3.zero;
    [Foldout("Secondary Fire")] public bool needsLOSForSecondaryFire = false;

    [Foldout("Secondary Fire")] public float minTimeBetweenSecondaryFire = 4;
    [Foldout("Secondary Fire")] public Transform granadeSpawnPoint;

    [Foldout("Line of Sight")] public float timeBetweenLOSChecks = 2;


    [Foldout("Fire Rate")] public int minBurstsPerVolley;
    [Foldout("Fire Rate")] public int maxBurstsPerVolley;


    [Foldout("Accuracy")] [Range(0.0f, 90.0f)]
    public float maxFiringAngle = 10;

    [Foldout("Accuracy")] [Range(0.0f, 90.0f)]
    public float maxSecondaryFireAngle = 40;


    //Secondary Fire
    bool canFireGrenadeAgain = false;
    bool canThrowGrenade = true;


    //Shooting
    float timeBetweenBursts;
    float timeBetweenBurstBullets;
    private bool canCurrentlyFire = true;
    private int currentRoundsPerVolley;

    private float rayDist;

    public bool checkForFriendlyFire;
    public string friendlyTag;

    //Accuracy
    Quaternion fireRotation;

    //Transforms
    Transform targetTransform;
    Transform LOSTargetTransform;

    //Line of Sight stuff
    LayerMask LOSLayermask;


    //Reloading
    bool isReloading = false;
    int currentBulletsUntilReload = 0;

    //Grenade
    bool locatedNewGrenadeTargetYet;


    //Private status stuff
    bool isAware = false;
    bool isFiring = false;
    bool isWaiting = false;


    private int[] enemyTeams;

    //TODO: Cover and Sound


    public float minimumDistToFireGun = 0;
    public float maximumDistToFireGun = 9999;


    float timer = 30;

    private void Awake()
    {
        agent = GetComponent<AIAgent>();

        LOSLayermask = AIController.Instance.layerMask;

        if (!granadeSpawnPoint)
        {
            granadeSpawnPoint = bulletSpawnPoint;
        }

        isFiring = false;
        isWaiting = false;
        currentBulletsUntilReload = stats.bulletsUntilReload;
        timeBetweenBurstBullets = 1 / stats.burstFireRate;
        timeBetweenBursts = 1 / stats.rateOfFire;
        minBurstsPerVolley = (int) (stats.minRoundsPerVolley / stats.shotsPerBurst);
        maxBurstsPerVolley = (int) (stats.maxRoundsPerVolley / stats.shotsPerBurst);
        maxFiringAngle = stats.maxFiringAngle / 2;
        maxSecondaryFireAngle = stats.maxSecondaryFireAngle / 2;
        minimumDistToFireGun = stats.minimumDistToFireGun * stats.minimumDistToFireGun;
        maximumDistToFireGun = stats.maximumDistToFireGun * stats.maximumDistToFireGun;
    }

    private void Start()
    {
        enemyTeams = agent.GetEnemyTeamIDs();
    }


    private void LateUpdate()
    {
        if (isAware)
        {
            if (!isFiring && !isWaiting && bulletObject)
            {
                StartCoroutine(BulletFiringCycle());
            }
            else if (!bulletObject)
            {
                Debug.LogWarning("Can't fire because there is no bullet object selected!");
            }
        }

        timer--;
    }

    IEnumerator BulletFiringCycle()
    {

        isFiring = true;

        if (agent.IsEnaging())
        {
            if (LOSTargetTransform)
            {
                if (!Physics.Linecast(bulletSpawnPoint.position, LOSTargetTransform.position, LOSLayermask) ||
                    !locatedNewGrenadeTargetYet)
                {
                    lastPosTargetSeen = targetTransform.position;
                    locatedNewGrenadeTargetYet = true;
                    canFireGrenadeAgain = true;
                    // FireOneGrenade();
                    canFireGrenadeAgain = true;
                }
                else if (!needsLOSForSecondaryFire)
                {
                    lastPosTargetSeen = targetTransform.position;
                    //if (canFireGrenadeAgain)
                    //   FireOneGrenade();
                    canFireGrenadeAgain = true;
                }
            }

            if (true)
            {
                yield return StartCoroutine(Fire());
            }
        }

        //Transition
        isWaiting = true;
        isFiring = false;

        //If we aren't reloading wait for a while before firing another burst
        if (currentBulletsUntilReload > 0 && stats.reloadTime > 0)
        {
            yield return new WaitForSeconds(stats.minPauseTime + Random.value * stats.randomPauseTimeAdd);
        }
        else
        {
            isReloading = true;
            //If we're out of ammo, reload.
            // if (reloadSound)
            // {
            //     audioSource.volume = reloadSoundVolume;
            //     audioSource.PlayOneShot(reloadSound);
            // }
            //
            // if (animationScript)
            // {
            //     animationScript.PlayReloadAnimation();
            // }
            //
            // if (soundScript)
            // {
            //     soundScript.PlayReloadAudio();
            // }

            yield return new WaitForSeconds(stats.reloadTime);
            currentBulletsUntilReload = stats.bulletsUntilReload;
            isReloading = false;
            yield return new WaitForSeconds(stats.minPauseTime * Random.value);
        }

        isWaiting = false;
    }

    IEnumerator Fire()
    {

        //Check Distances
        float distSqr = Vector3.SqrMagnitude(bulletSpawnPoint.position - LOSTargetTransform.position);
        if (minimumDistToFireGun <= distSqr && maximumDistToFireGun >= distSqr)
            //Make sure we don't fire more bullets than the number remaining in the agent's magazine.
            currentRoundsPerVolley = Mathf.Min(Random.Range(minBurstsPerVolley, maxBurstsPerVolley),
                currentBulletsUntilReload);

        while (currentRoundsPerVolley > 0 && enabled)
        {
            if (LOSTargetTransform && canCurrentlyFire)
            {
                rayDist = Mathf.Max(0.00001f,
                    Vector3.Distance(bulletSpawnPoint.position, LOSTargetTransform.position));

                var rayHit = Physics.Raycast(bulletSpawnPoint.position,
                    LOSTargetTransform.position - bulletSpawnPoint.position, rayDist, LOSLayermask);
                if (rayDist == 0 || rayHit)
                {
                    var canFire = true;

                    if (checkForFriendlyFire)
                    {
                        var distance = Vector3.Distance(bulletSpawnPoint.position, LOSTargetTransform.position);
                        if (Physics.Raycast(bulletSpawnPoint.position,
                            targetTransform.position - bulletSpawnPoint.position, out var hit,
                            distance))
                        {
                            if (string.Equals(hit.transform.tag, friendlyTag))
                            {
                                canFire = false;
                            }
                        }
                    }

                    if (canFire)
                    {
                        for (int i = 0; i < stats.shotsPerBurst; i++)
                        {
                            if (i < stats.shotsPerBurst - 1)
                            {
                                yield return new WaitForSeconds(timeBetweenBurstBullets);
                            }

                            currentBulletsUntilReload--;
                            FireOneShot();
                        }
                    }
                }

                currentRoundsPerVolley--;
            }
            else
            {
                yield break;
            }

            if (currentRoundsPerVolley > 0)
            {
                yield return new WaitForSeconds(timeBetweenBursts);
            }
        }
    }

    void FireOneShot()
    {
        //TODO: Pass this to the gun script

        if (targetTransform)
        {
            var aimAtTarget =
                Vector3.Angle(bulletSpawnPoint.forward, targetTransform.position - bulletSpawnPoint.position) <
                maxFiringAngle;

            for (var i = 0; i < pelletsPerShot; i++)
            {
                if (aimAtTarget)
                {
                    fireRotation.SetLookRotation(targetTransform.position - bulletSpawnPoint.position);
                }
                else
                {
                    fireRotation = Quaternion.LookRotation(bulletSpawnPoint.forward);
                }

                //Simulate inaccuracy
                fireRotation *= Quaternion.Euler(Random.Range(-stats.inaccuracy, stats.inaccuracy),
                    Random.Range(-stats.inaccuracy, stats.inaccuracy), 0);

                var bullet =
                    (GameObject) (Instantiate(bulletObject, bulletSpawnPoint.position, fireRotation));


                // if (isRocketLauncher && bullet.GetComponent<AIBullet>())
                // {
                //     bullet.GetComponent<AIBullet>().SetAsHoming(targetTransform);
                // }
            }
        }
    }

    public void EndEngage()
    {
        targetTransform = null;
        isAware = false;
    }

    public void AssignTarget(Transform newTarget, Transform newLOSTarget)
    {
        targetTransform = newTarget;
        LOSTargetTransform = newLOSTarget;
        isAware = true;
    }

    public void SetCanCurrentlyFire(bool b)
    {
        canCurrentlyFire = b;
    }

    public bool IsFiring()
    {
        return isFiring;
    }
}