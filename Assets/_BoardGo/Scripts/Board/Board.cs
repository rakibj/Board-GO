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

        private PlayerMover m_player;
        private void Awake()
        {
            m_player = FindObjectOfType<PlayerMover>();
            GetNodeList();
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
        
    }
}
