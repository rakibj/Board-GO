using _BoardGo.Scripts.Generic;
using UnityEngine;

namespace _BoardGo.Scripts.Player
{
    [RequireComponent(typeof(PlayerMover))]
    [RequireComponent(typeof(IInput))]
    public class PlayerManager : MonoBehaviour
    {
        [HideInInspector] public PlayerMover playerMover;
        [HideInInspector] public IInput playerInput;

        private void Awake()
        {
            playerMover = GetComponent<PlayerMover>();
            playerInput = GetComponent<IInput>();
            playerInput.InputEnabled = true;
        }

        private void Update()
        {
            if (playerMover.isMoving)
                return;
            
            //Input taking and moving
            playerInput.GetInput();
            if (playerInput.Vertical == 0)
            {
                if (playerInput.Horizontal < 0)
                {
                    playerMover.MoveLeft();
                }
                else if (playerInput.Horizontal > 0)
                {
                    playerMover.MoveRight();
                }
            }
            else if (playerInput.Horizontal == 0)
            {
                if (playerInput.Vertical < 0)
                {
                    playerMover.MoveBackward();
                }
                else if (playerInput.Vertical > 0)
                {
                    playerMover.MoveForward();
                }
            }
        }
    }
}
