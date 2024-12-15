using State;
using UnityEngine;
using UnityEngine.AI;

namespace Controller
{
    public class PersonController : MonoBehaviour, IPoolable
    {
        private BasePersonState currentState;
        private NavMeshAgent agent;
        private float moveSpeed;

        private void Awake()
        {
            agent = GetComponent<NavMeshAgent>();
        }

        public void Initialize(float speed)
        {
            moveSpeed = speed;
            agent.speed = moveSpeed;
            SetState(new IdleState(this));
        }

        public void StartMoving(Vector3 destination)
        {
            agent.SetDestination(destination);
            agent.isStopped = false;
        }

        public void StopMoving()
        {
            agent.isStopped = true;
        }

        private void Update()
        {
            currentState?.UpdateState();
        }

        public bool HasReachedDestination()
        {
            return !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance;
        }

        public void SetState(BasePersonState newState)
        {
            currentState?.ExitState();
            currentState = newState;
            currentState.EnterState();
        }

        public void OnSpawn()
        {
            SetState(new IdleState(this));
        }

        public void OnDespawn()
        {
            currentState?.ExitState();
            currentState = null;
            agent.isStopped = true;
        }
    }
}