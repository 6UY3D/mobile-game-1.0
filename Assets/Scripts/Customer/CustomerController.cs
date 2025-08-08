// Filename: CustomerController.cs
using UnityEngine;
using IdleShopkeeping.Managers;

namespace IdleShopkeeping.Customer
{
    /// <summary>
    /// Controls the behavior of an individual customer instance in the scene.
    /// </summary>
    public class CustomerController : MonoBehaviour
    {
        public CustomerData Data { get; private set; }
        private enum State { Entering, Browse, Leaving }
        private State _currentState;
        
        // In a real project, this would use a proper navigation system like NavMeshAgent.
        private Vector3 _targetPosition;
        private float _speed = 2.0f;

        public void Initialize(CustomerData data, Vector3 spawnPoint, Vector3 browsePosition)
        {
            Data = data;
            transform.position = spawnPoint;
            _targetPosition = browsePosition;
            _currentState = State.Entering;
            gameObject.name = $"Customer_{Data.DisplayName}";
        }

        private void Update()
        {
            // Simple state machine for movement
            if (Vector3.Distance(transform.position, _targetPosition) > 0.1f)
            {
                transform.position = Vector3.MoveTowards(transform.position, _targetPosition, _speed * Time.deltaTime);
            }
            else
            {
                if (_currentState == State.Entering)
                {
                    _currentState = State.Browse;
                    // Start a timer to leave after Browse
                    Invoke(nameof(StartLeaving), Random.Range(10f, 20f));
                }
                else if (_currentState == State.Leaving)
                {
                    // Customer has reached the exit, destroy them
                    Destroy(gameObject);
                }
            }
        }

        private void StartLeaving()
        {
            // Set target to an "exit" position (e.g., the spawn point)
            _targetPosition = CustomerSpawner.Instance.GetSpawnPoint(); 
            _currentState = State.Leaving;
        }

        /// <summary>
        /// Called when the player taps on the customer.
        /// </summary>
        private void OnMouseDown()
        {
            if (_currentState == State.Browse && Data.GreetingDialogue != null)
            {
                DialogueManager.Instance.StartDialogue(Data.GreetingDialogue);
            }
        }
    }
}
