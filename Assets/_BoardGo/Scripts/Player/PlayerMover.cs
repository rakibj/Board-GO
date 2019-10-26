using System;
using System.Collections;
using _BoardGo.Scripts.Generic;
using UnityEngine;

namespace _BoardGo.Scripts.Player
{
    public class PlayerMover : Mover
    {
        private PlayerCompass m_playerCompass;

        protected override void Awake()
        {
            base.Awake();
            m_playerCompass = FindObjectOfType<PlayerCompass>();
        }

        protected override void Start()
        {
            base.Start();
            UpdateBoard();
        }

        protected override IEnumerator MoveRoutine(Vector3 destinationPos, float delayTime)
        {
            if(m_playerCompass != null) m_playerCompass.ShowArrows(false);
            //Run parent move routine
            yield return StartCoroutine(base.MoveRoutine(destinationPos, delayTime));
            UpdateBoard();
            if(m_playerCompass != null) m_playerCompass.ShowArrows(true);
            GameEvents.Instance.OnPlayerMoveFinish();
        }

        private void UpdateBoard()
        {
            if (m_board != null)
                m_board.UpdatePlayerNode();
        }
    }
}
