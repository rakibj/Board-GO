using System;
using System.Collections;
using _BoardGo.Scripts.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace _BoardGo.Scripts.Enemy
{
    [RequireComponent(typeof(EnemyMover))]
    [RequireComponent(typeof(EnemySensor))]
    public class EnemyManager : TurnManager
    {
        private EnemyMover m_enemyMover;
        private EnemySensor m_enemySensor;
        private Board.Board m_board;
        private bool m_isDead = false;
        public bool IsDead => m_isDead;
        public UnityEvent deathEvent;
        protected override void Awake()
        {
            base.Awake();
            m_enemyMover = GetComponent<EnemyMover>();
            m_enemySensor = GetComponent<EnemySensor>();
            m_board = FindObjectOfType<Board.Board>();
        }

        public void PlayTurn()
        {
            if (m_isDead)
            {
                FinishTurn();
                return;
            }

            StartCoroutine(PlayTurnRoutine());
        }

        private IEnumerator PlayTurnRoutine()
        {
            //detect player
            m_enemySensor.UpdateSensor();
            yield return new WaitForSeconds(0);
            if (m_enemySensor.FoundPlayer)
            {
                //notify gamemanager
                m_gameManager.LoseLevel();
                
                //move to player
                var playerPosition = new Vector3(m_board.PlayerNode.Coordinate.x, 0, m_board.PlayerNode.Coordinate.y);
                m_enemyMover.Move(playerPosition);
                while (m_enemyMover.isMoving)
                    yield return null;
                
                //Attack now
                Debug.Log("ATTACK ->->->");
            }
            else
            {
                //movement
                m_enemyMover.MoveOneTurn();
            }
        }

        public void Die()
        {
            if (m_isDead)
                return;

            m_isDead = true;
            m_gameManager.Enemies.Remove(this);
            if(deathEvent != null) deathEvent.Invoke();
        }
    }
}
