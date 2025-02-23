using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace pcpp
{

    public class PlayerController : NetworkBehaviour
    {

        [SerializeField] float movementSpeedBase = 5;

        private Animator animator;
        private Rigidbody2D rb;
        private float movementSpeedMultiplier = 0.5f;
        private Vector2 currentMoveDirection;
        public NetworkVariable<int> playerScore = new NetworkVariable<int>();

        void Start()
        {
            animator = GetComponent<Animator>();
            rb = GetComponent<Rigidbody2D>();
        }

        void Update()
        {

            Vector2 input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

            if (IsServer && IsLocalPlayer) 
            {
                Move(input);
            } else if (IsClient && IsLocalPlayer) 
            {
                
                MoveServerRPC(input);
            }
        }

        private void Move(Vector2 _input)
        {
            
            Vector2 moveVector = _input.normalized * movementSpeedBase * movementSpeedMultiplier;

            animator = GetComponent<Animator>();
            rb = GetComponent<Rigidbody2D>();

            animator.SetFloat("Speed", moveVector.magnitude);
            rb.linearVelocity = moveVector;

            if (moveVector != Vector2.zero)
            {
                currentMoveDirection = new Vector2(moveVector.normalized.x, moveVector.normalized.y);
                animator.SetFloat("Horizontal", moveVector.normalized.x);
                animator.SetFloat("Vertical", moveVector.normalized.y);
            }
        }

        [Rpc(SendTo.Server)]
        private void MoveServerRPC(Vector2 _input)
        {
            Move(_input);
        }

    }
}