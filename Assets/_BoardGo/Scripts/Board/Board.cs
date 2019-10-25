using System;
using System.Collections.Generic;
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

        private void Awake()
        {
            GetNodeList();
        }

        private void GetNodeList()
        {
            var nList = FindObjectsOfType<Node>();
            m_allNodes = new List<Node>(nList);
        }
    }
}
