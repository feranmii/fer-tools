using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class AIAnimation : MonoBehaviour
{
    #region Animation Hashes

    //Animation Hashes
    private int currentAngleHash;
    private int engagingHash;
    private int crouchingHash;
    private int reloadingHash;
    private int meleeHash;
    private int fireHash;
    private int forwardsMoveHash;
    private int sidewaysMoveHash;
    private int sprintingHash;
    private int dodgeRightHash;

    private int dodgeLeftHash;

    //private int leapHash;
    //private int vaultHash;
    private int staggerHash;
    private int grenadeHash;

    private int coverHash;
    private int centerHash;
    private int rightHash;
    private int leftHash;

    #endregion

    [Header("Offset")] public Vector3 bodyOffset;

    [Header("Cover")] public float minDistToCrouch = 1;

    [Header("Melee")] public float meleeAnimationLength = 3;

    [Header("Dynamic Objects")] public bool useCustomRotation = false;
    [Header("Dynamic Objects")] public float maxAngleDeviation = 10;

    //Speeds
    [Foldout("Speed")] public float maxMovementSpeed = -1f;
    [Foldout("Speed")] public float animatorSpeed = 1f;
    [Foldout("Speed")] public float meleeAnimationSpeed = 1f;


    [Foldout("References")] public Animator bodyTransform;

    [Foldout("References")] public Animator animator;

    //References
    private Transform mTransform;

    private AIRotateToAimGun rotateGun;
    private AIAgent agent;
    private AIShooting shooting;


    //Speed
    float currentVelocityRatio = 0;
    float animationDampTime = 0.1f;
    NavmeshInterface navi;


    //Dynamic Objects
    Quaternion currRotRequired;

    Vector3 directionToFace;

    //Rotation
    float myAngle;

    [Range(0.0f, 90.0f), Foldout("Rotation")]
    public float minAngleToRotateBase = 65;

    Quaternion newRotation;
    [Foldout("Rotation")] public float turnSpeed = 4.0f;

    private bool sprinting = false;

    private bool setHashes;

    // Start is called before the first frame update
    void Awake()
    {
        SetHashes();
    }

    // Update is called once per frame
    void Update()
    {
    }

    //Setters
    void SetHashes()
    {
        crouchingHash = Animator.StringToHash("Crouching");
        engagingHash = Animator.StringToHash("Engaging");
        reloadingHash = Animator.StringToHash("Reloading");
        meleeHash = Animator.StringToHash("Melee");
        fireHash = Animator.StringToHash("Fire");
        sidewaysMoveHash = Animator.StringToHash("Horizontal");
        forwardsMoveHash = Animator.StringToHash("Forwards");
        sprintingHash = Animator.StringToHash("Sprinting");
        dodgeRightHash = Animator.StringToHash("DodgeRight");
        dodgeLeftHash = Animator.StringToHash("DodgeLeft");
        //leapHash = Animator.StringToHash("Leap");
        //vaultHash = Animator.StringToHash("Vault");
        staggerHash = Animator.StringToHash("Stagger");
        grenadeHash = Animator.StringToHash("Grenade");

        coverHash = Animator.StringToHash("Cover");
        centerHash = Animator.StringToHash("CoverCenter");
        rightHash = Animator.StringToHash("CoverRight");
        leftHash = Animator.StringToHash("CoverLeft");
        setHashes = true;
    }
}