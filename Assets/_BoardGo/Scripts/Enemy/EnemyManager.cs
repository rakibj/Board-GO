using System;
using UnityEngine;

namespace _BoardGo.Scripts.Enemy
{
    [RequireComponent(typeof(EnemyMover))]
    [RequireComponent(typeof(EnemySensor))]
    public class EnemyManager : MonoBehaviour
    {
        private EnemyMover m_enemyMover;
        private EnemySensor m_enemySensor;
        private Board.Board m_board;

        private void Awake()
        {
            m_enemyMover = GetComponent<EnemyMover>();
            m_enemySensor = GetComponent<EnemySensor>();
            m_board = FindObjectOfType<Board.Board>();
        }
    }
}
