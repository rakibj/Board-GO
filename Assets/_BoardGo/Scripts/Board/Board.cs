using System;
using System.Collections.Generic;
using _BoardGo.Scripts.Player;
using UnityEngine;

namespace _BoardGo.Scripts.Board
{
    public class Board : MonoBehaviour
    {
        public static float spacing = 2f;

        public static readonly Vector2[] directions =
        {
            new Vector2(spacing, 0f),
            new Vector2(-spacing, 0f),
            new Vector2(0f, spacing),
            new Vector2(0f, -spacing),
        };
        
        private List<Node> m_allNodes = new List<Node>();
        public List<Node> AllNodes => m_allNodes;

        private Node m_playerNode;
        public Node PlayerNode => m_playerNode;
        private Node m_goalNode;
        public Node GoalNode => m_goalNode;
        public GameObject goalPrefab;
        public float drawGoalTime = 2f;
        public float drawGoalDelay = 2f;
        public iTween.EaseType drawGoalEaseType = iTween.EaseType.easeOutExpo;
        
        private PlayerMover m_player;
        private void Awake()
        {
            m_player = FindObjectOfType<PlayerMover>();
            GetNodeList();
            m_goalNode = FindGoalNode();
        }

        private void GetNodeList()
        {
            var nList = FindObjectsOfType<Node>();
            m_allNodes = new List<Node>(nList);
        }

        public Node FindNodeAt(Vector3 pos)
        {
            Vector2 nodeCoord = Utility.Vector2Round(new Vector2(pos.x, pos.z));
            return m_allNodes.Find(n => n.Coordinate == nodeCoord);
        }

        public Node FindPlayerNode()
        {
            if (m_player == null)
                return null;
            if (m_player.isMoving)
                return null;

            return FindNodeAt(m_player.transform.position);
        }

        public void UpdatePlayerNode()
        {
            m_playerNode = FindPlayerNode();
        }
        public Node FindGoalNode()
        {
            return m_allNodes.Find(n => n.isLevelGoal);
        }

        public void DrawGoal()
        {
            if (goalPrefab != null && m_goalNode != null)
            {
                GameObject goalInstance = Instantiate(goalPrefab, m_goalNode.transform.position, Quaternion.identity);
                iTween.ScaleFrom(goalInstance, iTween.Hash(
                    "scale", Vector3.zero,
                    "time", drawGoalTime,
                    "delay", drawGoalDelay,
                    "easetype", drawGoalEaseType
                    ));
            }
        }
        
        public void InitBoard()
        {
            if (m_playerNode != null)
            {
                m_playerNode.InitNode();
            }
        }
    }
}
