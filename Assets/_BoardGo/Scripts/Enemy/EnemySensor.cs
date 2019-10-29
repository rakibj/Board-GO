using System;
using _BoardGo.Scripts.Board;
using _BoardGo.Scripts.Generic;
using UnityEngine;

namespace _BoardGo.Scripts.Enemy
{
    public class EnemySensor : MonoBehaviour
    {
        private Vector3 m_directionToSearch;
        private Node m_nodeToSearch;
        private Board.Board m_board;
        private bool m_foundPlayer = false;
        private Node m_currentNode;
        public bool FoundPlayer => m_foundPlayer;
        private void Awake()
        {
            m_board = FindObjectOfType<Board.Board>();
            m_directionToSearch = new Vector3(0, 0, Board.Board.spacing);
        }

        public void UpdateSensor()
        {
            
            var worldSpacePositionToSearch = transform.TransformVector(m_directionToSearch) + transform.position;
            if (m_board != null)
            {
                m_currentNode = m_board.FindNodeAt(transform.position);
                m_nodeToSearch = m_board.FindNodeAt(worldSpacePositionToSearch);
                if (m_nodeToSearch == m_board.PlayerNode && m_currentNode.LinkedNodes.Contains(m_nodeToSearch) && !m_nodeToSearch.isLevelGoal)
                {
                    m_foundPlayer = true;
                    Debug.Log("Found Player");
                }
            }
        }
    }
}
