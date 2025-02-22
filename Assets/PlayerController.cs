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
        private float movementSpeedMultiplier = 1;
        private Vector2 currentMoveDirection;
        public NetworkVariable<int> playerScore = new NetworkVariable<int>();

        public override void OnNetworkSpawn()
        {
            if (!IsOwner)
            {
                enabled = false;
                return;
            }
        }

        void Start()
        {
            animator = GetComponent<Animator>();
            rb = GetComponent<Rigidbody2D>();
        }

        void Update()
        {
            Move();
        }

        private void Move()
        {
            Vector2 movementDirection = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            MoveServerRPC(movementDirection, movementSpeedBase, movementSpeedMultiplier);
        }

        [ServerRpc]
        private void MoveServerRPC(Vector2 movementDirection, float movementSpeedBase, float movementSpeedMultiplier)
        {
            Vector2 moveVector = movementDirection.normalized * movementSpeedBase * movementSpeedMultiplier;

            animator = GetComponent<Animator>();
            rb = GetComponent<Rigidbody2D>();

            animator.SetFloat("Speed", moveVector.magnitude);
            rb.linearVelocity = moveVector;

            if (moveVector != Vector2.zero)
            {
                currentMoveDirection = new Vector2(moveVector.normalized.x, moveVector.normalized.y);
                animator.SetFloat("Horizontal", moveVector.normalized.x);
                animator.SetFloat("Vertical", moveVector.normalized.y);
                Debug.Log("" + moveVector.normalized.x + ":" + moveVector.normalized.y);
            }
        }
    }
}