using System;
using UnityEngine;

namespace _BoardGo.Scripts.Generic
{
    public class TurnManager : MonoBehaviour
    {
        protected GameManager m_gameManager;
        private bool m_isTurnComplete = false;
        public bool IsTurnComplete
        {
            get => m_isTurnComplete;
            set => m_isTurnComplete = value;
        }


        protected virtual void Awake()
        {
            m_gameManager = FindObjectOfType<GameManager>();
        }

        public void FinishTurn()
        {
            IsTurnComplete = true;
            if (m_gameManager != null) m_gameManager.UpdateTurn();
        }
    }
}
