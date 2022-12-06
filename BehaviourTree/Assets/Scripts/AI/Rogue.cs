using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.AI;

public class Rogue : MonoBehaviour, IHasState
{
    private enum RogueState
    {
        Following = 0,
        Hiding = 1,
        Throwing = 2,
        FindingHidingPlace = 3
    }

    private Guard guard;
    private Player player;
    private BTBaseNode tree;
    private NavMeshAgent agent;
    private Animator animator;
    private RogueState rogueState;
    private Blackboard blackboard = new Blackboard();
    
    private float DistanceToPlayer => Vector3.Distance(transform.position, player.transform.position);

    private const string POSITION_KEY = "PosKey";
    private const string WALKING_ANIM = "Rifle Walk";
    private const string IDLE_ANIM = "Idle";
    private const float STOP_FOLLOWING_DISTANCE = 2;
    
    private void Awake()
    {
        guard = FindObjectOfType<Guard>();
        player = FindObjectOfType<Player>();
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        tree = new BT_Selector(new BTBaseNode[]
        {
            new BT_Sequence(new BTBaseNode[]
            {
               new BT_Condition(() => guard.IsAggressive),
               new BT_Selector(new BTBaseNode[]
               {
                   new BT_Sequence(new BTBaseNode[]
                   {
                       //TODO if we're not already hiding, find a place to hide.
                       new BT_Condition(() => rogueState != RogueState.Hiding),
                       new BT_SetState(this, (int)RogueState.FindingHidingPlace),
                       new BT_FindClosest<HidingPlace>(transform, blackboard, POSITION_KEY),
                       new BT_PlayAnimation(animator, WALKING_ANIM),
                       new BT_MoveTowards(agent, blackboard, POSITION_KEY),
                       new BT_SetState(this, (int)RogueState.Hiding)
                   }),
                   new BT_Sequence(new BTBaseNode[]
                   {
                       new BT_Wait(0.5f),
                       new BT_Throw(guard),
                       new BT_SetState(this, (int)RogueState.Throwing)
                   })
               })
            }),
            new BT_Sequence( () => DistanceToPlayer > STOP_FOLLOWING_DISTANCE, new BTBaseNode[]
            {
                new BT_SetState(this, (int)RogueState.Following),
                new BT_PlayAnimation(animator, WALKING_ANIM),
                new BT_MoveTowards(agent, player.transform)
            }),
            new BT_PlayAnimation(animator, IDLE_ANIM)
        });
    }

    private void FixedUpdate()
    {
        tree?.Run();
    }
    
    public void SetState(int _state)
    {
        rogueState = (RogueState)_state;
    }

    public void OnDrawGizmos()
    {
        Handles.Label(transform.position + new Vector3(0, 2.5f, 0), rogueState.ToString(),
            new GUIStyle() { alignment = TextAnchor.MiddleCenter });
    }
}
