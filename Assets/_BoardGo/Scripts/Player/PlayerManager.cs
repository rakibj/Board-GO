using System.Text.RegularExpressions;
using _BoardGo.Scripts.Generic;
using UnityEngine;

namespace _BoardGo.Scripts.Player
{
    [RequireComponent(typeof(PlayerMover))]
    [RequireComponent(typeof(IInput))]
    public class PlayerManager : TurnManager
    {
        [HideInInspector] public PlayerMover playerMover;
        [HideInInspector] public IInput playerInput;
        private Board.Board m_board;
        protected override void Awake()
        {
            base.Awake();
            m_board = FindObjectOfType<Board.Board>();
            playerMover = GetComponent<PlayerMover>();
            playerInput = GetComponent<IInput>();
            playerInput.InputEnabled = true;
        }

        private void Update()
        {
            if (playerMover.isMoving || m_gameManager.CurrentTurn != Turn.Player)
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

        private void CaptureEnemies()
        {
            if (m_board != null)
            {
                var enemies = m_board.FindEnemiesAt(m_board.PlayerNode);
                if (enemies.Count != 0)
                {
                    foreach (var enemy in enemies)
                    {
                        if (enemy != null)
                        {
                            enemy.Die();
                            Destroy(enemy.gameObject);
                        }
                    }
                }
            }
        }

        public override void FinishTurn()
        {
            CaptureEnemies();
            base.FinishTurn();
        }
    }
}
