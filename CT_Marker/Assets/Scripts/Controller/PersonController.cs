using UnityEngine;

namespace Controller
{ 
    public class PersonController : MonoBehaviour, IPoolable
    {
        private BasePersonState currentState;
        private float moveSpeed;
        private Vector3 targetPosition;
        private bool isMoving;

        public void Initialize(float speed)
        {
            moveSpeed = speed;
            SetState(new IdleState(this));
        }

        public void StartMoving(Vector3 destination)
        {
            targetPosition = destination;
            isMoving = true;
        }

        public void StopMoving()
        {
            isMoving = false;
        }
    
        private void Update()
        {
            currentState?.UpdateState();
    
            if (isMoving)
            {
                MoveTowardsTarget();
            }
        }
    
        private void MoveTowardsTarget()
        {
            Vector3 direction = (targetPosition - transform.position).normalized;
            transform.position += direction * moveSpeed * Time.deltaTime;

            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, 
                GameManager.Instance.Resources.rotationSpeed * Time.deltaTime);
        }

        public bool HasReachedDestination()
        {
            return Vector3.Distance(transform.position, targetPosition) < 
                   GameManager.Instance.Resources.stoppingDistance;
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
            isMoving = false;
        }
    }
}