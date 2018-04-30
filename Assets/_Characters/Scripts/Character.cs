using System;
using UnityEngine;
using UnityEngine.AI;

using RPG.CameraUI;

namespace RPG.Characters
{
    [SelectionBase]
    public class Character : MonoBehaviour
    {
        [Header("Animator")] 
        [SerializeField] RuntimeAnimatorController animatorController;
        [SerializeField] AnimatorOverrideController animatorOverrideController;
        [SerializeField] Avatar characterAvatar;

        [Header("Audio Source")]
        [Range(0,1f)] [SerializeField] float audioSourceSpatialBlend = 0.5f;

        [Header("Capsule Collider")]
        [SerializeField] Vector3 colliderCenter = new Vector3(0,1.03f,0);
        [SerializeField] float colliderRadius = 0.2f;
        [SerializeField] float colliderHeight = 2.03f;

        [Header("Movement")] 
        [SerializeField] float moveSpeedMultiplier = 1.5f;
        [SerializeField] float animationSpeedMultiplier = 1.5f;
        [SerializeField] float movingTurnSpeed = 360;
        [SerializeField] float stationaryTurnSpeed = 180;
        [SerializeField] float moveThreshold = 1f;

        [Header("Nav Mesh")] 
        [SerializeField] float navMeshAgentStoppingDistance = 1.3f;
        [SerializeField] float navMeshAgentSteeringSpeed = 1.0f;


        Rigidbody rigidBody;
        Animator animator;
        NavMeshAgent navMeshAgent;
        float origGroundCheckDistance;
        float turnAmount;
        float forwardAmount;
        bool isAlive = true;

        public AnimatorOverrideController GetAnimatorOverrideController()
        {
                return animatorOverrideController;
        }

        void Awake()
        {
            AddRequiredComponents();
        }

        private void AddRequiredComponents()
        {
            // Animator
            animator = gameObject.AddComponent<Animator>();
            animator.runtimeAnimatorController = animatorController;
            animator.avatar = characterAvatar;
            animator.applyRootMotion = true;

            // Audio Source
            var audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.spatialBlend = audioSourceSpatialBlend;
            
            // Capsule Collider
            var capsuleCollider = gameObject.AddComponent<CapsuleCollider>();
            capsuleCollider.center = colliderCenter;
            capsuleCollider.radius = colliderRadius;
            capsuleCollider.height = colliderHeight;

            // Nav Mesh agent
            navMeshAgent = gameObject.AddComponent<NavMeshAgent>();
            navMeshAgent.updateRotation = false;
            navMeshAgent.updatePosition = true;
            navMeshAgent.autoBraking = false;
            navMeshAgent.speed = navMeshAgentSteeringSpeed;
            navMeshAgent.stoppingDistance = navMeshAgentStoppingDistance;

            // Rigid Body
            rigidBody = gameObject.AddComponent<Rigidbody>();
            rigidBody.constraints = RigidbodyConstraints.FreezeRotation;
        }

        void Update()
        {
            if (navMeshAgent.remainingDistance > navMeshAgent.stoppingDistance && isAlive)
            {
                Move(navMeshAgent.desiredVelocity);
            }
            else
            {
                Move(Vector3.zero);
            }
        }

        public void Kill()
        {
            isAlive = false;
        }

        public void SetDestination(Vector3 worldPosition)
        {
            navMeshAgent.destination = worldPosition;
        }

        void OnAnimatorMove()
        {
            if (Time.deltaTime > 0)
            {
                Vector3 velocity = (animator.deltaPosition * moveSpeedMultiplier) / Time.deltaTime;

                // we preserve the existing y part of the current velocity.
                velocity.y = rigidBody.velocity.y;
                rigidBody.velocity = velocity;
            }
        }

        void Move(Vector3 move)
        {
            SetForwardAndTurn(move);
            ApplyExtraTurnRotation();
            UpdateAnimator();
        }


        private void SetForwardAndTurn(Vector3 move)
        {
            // convert the world relative moveInput vector into a local-relative
            // turn amount and forward amount required to head in the desired
            // direction.
            if (move.magnitude > moveThreshold)
            {
                move.Normalize();
            }
            var localMove = transform.InverseTransformDirection(move);
            turnAmount = Mathf.Atan2(localMove.x, localMove.z);
            forwardAmount = localMove.z;
        }

        void UpdateAnimator()
        {
            animator.SetFloat("Forward", forwardAmount, 0.1f, Time.deltaTime);
            animator.SetFloat("Turn", turnAmount, 0.1f, Time.deltaTime);
            animator.speed = animationSpeedMultiplier;
        }


        void ApplyExtraTurnRotation()
        {
            // help the character turn faster (this is in addition to root rotation in the animation)
            float turnSpeed = Mathf.Lerp(stationaryTurnSpeed, movingTurnSpeed, forwardAmount);
            transform.Rotate(0, turnAmount * turnSpeed * Time.deltaTime, 0);
        }


    }
}