using System;
using System.Collections;
using _BoardGo.Scripts.Generic;
using UnityEngine;

namespace _BoardGo.Scripts.Enemy
{
    [RequireComponent(typeof(EnemyMover))]
    [RequireComponent(typeof(EnemySensor))]
    public class EnemyManager : TurnManager
    {
        private EnemyMover m_enemyMover;
        private EnemySensor m_enemySensor;
        private Board.Board m_board;

        protected override void Awake()
        {
            base.Awake();
            m_enemyMover = GetComponent<EnemyMover>();
            m_enemySensor = GetComponent<EnemySensor>();
            m_board = FindObjectOfType<Board.Board>();
        }

        public void PlayTurn()
        {
            StartCoroutine(PlayTurnRoutine());
        }

        private IEnumerator PlayTurnRoutine()
        {
            //detect player
            m_enemySensor.UpdateSensor();
            //attack
            
            yield return new WaitForSeconds(0);
            //movement
            m_enemyMover.MoveOneTurn();
        }
    }
}
