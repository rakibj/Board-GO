﻿using System;
using System.Collections;
using _BoardGo.Scripts.Player;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace _BoardGo.Scripts.Generic
{
    public class GameManager : MonoBehaviour
    {
        private Board.Board m_board;
        private PlayerManager m_player;

        private bool m_hasLevelStartd = false;
        private bool m_isGamePlaying = false;
        private bool m_isGameOver = false;
        private bool m_hasLevelFinished = false;
        public bool HasLevelStartd => m_hasLevelStartd;
        public bool IsGamePlaying => m_isGamePlaying;
        public bool IsGameOver => m_isGameOver;
        public bool HasLevelFinished => m_hasLevelFinished;

        public UnityEvent startLevelEvent;
        public UnityEvent playLevelEvent;
        public UnityEvent endLevelEvent;
        
        private void Awake()
        {
            m_board = FindObjectOfType<Board.Board>();
            m_player = FindObjectOfType<PlayerManager>();
        }

        private void Start()
        {
            if (m_board != null && m_player != null)
            {
                StartCoroutine(RunGameLoop());
                //PlayLevel();
            }
            else
                Debug.LogWarning("GameManager.cs: Board or Player not found");
        }

        private IEnumerator RunGameLoop()
        {
            yield return StartCoroutine(StartLevelRoutine());
            yield return StartCoroutine(PlayLevelRoutine());
            yield return StartCoroutine(EndLevelRoutine());
        }

        private IEnumerator StartLevelRoutine()
        {
            m_player.playerInput.InputEnabled = false;
            while (!m_hasLevelStartd)
            {
                yield return null;
            }
            startLevelEvent?.Invoke();
        }

        private IEnumerator PlayLevelRoutine()
        {
            m_isGamePlaying = true;
            yield return new WaitForSeconds(0);
            m_player.playerInput.InputEnabled = true;
            playLevelEvent?.Invoke();
            while (!m_isGameOver)
            {
                yield return null;
                m_isGameOver = IsWinner();
            }

            Debug.Log("Win===============");
        }

        

        private IEnumerator EndLevelRoutine()
        {
            m_player.playerInput.InputEnabled = false;
            endLevelEvent?.Invoke();
            while (!m_hasLevelFinished)
            {
                yield return null;
            }

            RestartLevel();
        }

        private void RestartLevel()
        {
            Scene scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.name);
        }

        public void PlayLevel()
        {
            m_hasLevelStartd = true;
        }
        private bool IsWinner()
        {
            if (m_board.PlayerNode != null)
            {
                return m_board.PlayerNode == m_board.GoalNode;
            }

            return false;
        }
    }
}
