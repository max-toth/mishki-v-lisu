using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace pcpp
{

    public class PlayerController : NetworkBehaviour
    {

        private float horizontal;
        private float vertical;

        public NetworkVariable<Vector3> Position = new NetworkVariable<Vector3>();

        private float speed = 8f;

        // [SerializeField] private Rigidbody2D rb;

        // Update is called once per frame
        void Update()
        {
            horizontal = Input.GetAxisRaw("Horizontal");
            vertical = Input.GetAxisRaw("Vertical");
        }

        private void FixedUpdate()
        {
            // rb.linearVelocity = new Vector2(horizontal, vertical).normalized;
        }

        public override void OnNetworkSpawn()
        {
            if (IsOwner)
            {
                Move();
            }
        }

        public void Move()
        {
            SubmitPositionRequestRpc();
        }    

        
        [Rpc(SendTo.Server)]
        void SubmitPositionRequestRpc(RpcParams rpcParams = default)
        {
            transform.position = new Vector3(horizontal, vertical, 0f);
            Position.Value = transform.position;
        }
    }
}
