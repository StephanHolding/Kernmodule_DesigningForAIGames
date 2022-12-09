using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class Guard : PickupController, IHasState
{
    private enum GuardState
    {
        Patrolling = 0,
        FindingWeapon = 1,
        Chasing = 2,
        Attacking = 3,
        Confused = 4
    }


    [Header("AI Behaviour")] 
    public Transform[] patrollingWaypoints;
    
    private const float ATTACK_RANGE = 0.25f;
    private const float SMOKEBOMB_TIME = 10f;
    
    private float DistanceToPlayer => Vector3.Distance(gameObject.transform.position, thePlayer.transform.position);
    private bool PlayerDetected => coneOfView.CheckConeOfView();
    public bool IsAggressive => guardState is GuardState.Attacking or GuardState.Chasing;

    private GuardState guardState;
    private BTBaseNode tree;
    private NavMeshAgent agent;
    private Animator animator;
    private Player thePlayer;
    private ParticleSystem smokeCloud;
    private ConeOfView coneOfView;
    private TextMeshPro textMeshPro;

    private readonly Blackboard guardBlackboard = new Blackboard();
    private const string BLACKBOARD_MOVEMENT_TARGET = "MovementTarget";

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
        smokeCloud = GetComponentInChildren<ParticleSystem>();
        thePlayer = FindObjectOfType<Player>();
        coneOfView = GetComponent<ConeOfView>();
        textMeshPro = GetComponentInChildren<TextMeshPro>();
    }

    private void Start()
    {
        CreateBehaviourTree();
    }

    private void FixedUpdate()
    {
        tree.Run();
        LogStateToUI();
    }
    
    private void CreateBehaviourTree()
    {
        // classes like BT_NeedsWeapon and BT_IsOutOfRange are basically a bool wrapped in a class. Is this desirable?
        // How do I interrupt the tree? (For when the player is detected, or when the AI agent dies.)

        tree = new BT_Selector(new BTBaseNode[]
        {
            new BT_Sequence(new BTBaseNode[]
            {
               new BT_Condition(() => guardState == GuardState.Confused),
               new BT_PlayAnimation(animator, "Idle")
            }),
            new BT_Sequence(() => thePlayer.IsDead || !PlayerDetected,
                new BTBaseNode[] //Patrolling Behaviour
                {
                    new BT_SetState(this, (int)GuardState.Patrolling),
                    new BT_GetNextPatrolWaypoint(patrollingWaypoints, guardBlackboard, BLACKBOARD_MOVEMENT_TARGET),
                    new BT_PlayAnimation(animator, "Rifle Walk"),
                    new BT_MoveTowards(agent, guardBlackboard, BLACKBOARD_MOVEMENT_TARGET),
                    new BT_PlayAnimation(animator, "Idle"),
                    new BT_Wait(3.0f)
                }),
            new BT_Sequence(new BTBaseNode[]
            {
                new BT_Condition(() => thePlayer.IsDead == false),
                new BT_Selector(new BTBaseNode[]
                {
                    new BT_Sequence(new BTBaseNode[] //Sequence find weapon
                    {
                        new BT_Condition(() => holdingObject == null),
                        new BT_Log("Finding a weapon..."),
                        new BT_SetState(this, (int)GuardState.FindingWeapon),
                        new BT_FindClosest<KickingShoes>(transform, guardBlackboard, BLACKBOARD_MOVEMENT_TARGET),
                        new BT_PlayAnimation(animator, "Rifle Walk"),
                        new BT_MoveTowards(agent, guardBlackboard, BLACKBOARD_MOVEMENT_TARGET),
                        new BT_PickupWeapon(() => pickupableInRange, this),
                        new BT_PlayAnimation(animator, "Idle"),
                        new BT_Wait(3.0f)
                    }),
                    new BT_Selector(new BTBaseNode[]
                    {
                        new BT_Sequence(() => DistanceToPlayer > ATTACK_RANGE && guardState != GuardState.Confused, new BTBaseNode[] // chase the player
                        {
                            new BT_Log("Chasing player"),
                            new BT_SetState(this, (int)GuardState.Chasing),
                            new BT_PlayAnimation(animator, "Rifle Walk"),
                            new BT_MoveTowards(agent, thePlayer.transform)
                        }),
                        new BT_Sequence( () => guardState != GuardState.Confused,new BTBaseNode[] // attack the player
                        {
                            new BT_Log("Attacking player"),
                            new BT_SetState(this, (int)GuardState.Attacking),
                            new BT_Wait(0.5f),
                            new BT_PlayAnimation(animator, "Kick"),
                            new BT_Attack(thePlayer, gameObject),
                            new BT_Wait(2)
                        })
                    })
                })
            })
        });
    }

    public void GetSmokeBombed()
    {
        StartCoroutine(SmokeBombed());
    }
    
    public void SetState(int _state)
    {
        guardState = (GuardState)_state;
    }

    public void LogStateToUI()
    {
        textMeshPro.text = guardState.ToString();
    }

    private IEnumerator SmokeBombed()
    {
        smokeCloud.Play();
        SetState((int)GuardState.Confused);
        yield return new WaitForSeconds(SMOKEBOMB_TIME);
        SetState((int)GuardState.Chasing);
        smokeCloud.Stop();
    }
}