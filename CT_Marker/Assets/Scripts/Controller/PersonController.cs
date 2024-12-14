using UnityEngine;

namespace Controller
{ 
    public class PersonController : MonoBehaviour, IPoolable
    {
        private float moveSpeed;
        private Vector3 targetPosition;
        private bool isMoving;

        public void Initialize(float speed)
        {
            moveSpeed = speed;
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
    
        public void OnSpawn()
        {
        }
    
        public void OnDespawn()
        {
        }
    }
}