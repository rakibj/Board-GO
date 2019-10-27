using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _BoardGo.Scripts.Enemy;
using _BoardGo.Scripts.Player;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace _BoardGo.Scripts.Generic
{
    public enum Turn
    {
        Player,
        Enemy
    }
    public class GameManager : MonoBehaviour
    {
        private Board.Board m_board;
        private PlayerManager m_player;
        private List<EnemyManager> m_enemies;
        public List<EnemyManager> Enemies => m_enemies;
        private Turn m_currentTurn = Turn.Player;
        public Turn CurrentTurn => m_currentTurn;
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
            m_enemies = FindObjectsOfType<EnemyManager>().ToList();
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

        public void UpdateTurn()
        {
            if (m_currentTurn == Turn.Player)
            {
                if (m_player.IsTurnComplete)
                    PlayEnemyTurn();
            }
            else if (m_currentTurn == Turn.Enemy)
            {
                if(IsEnemyTurnComplete())
                    PlayPlayerTurn();
            }
        }

        private void PlayPlayerTurn()
        {
            m_currentTurn = Turn.Player;
            m_player.IsTurnComplete = false;
        }

        private void PlayEnemyTurn()
        {
            m_currentTurn = Turn.Enemy;
            foreach (var enemy in m_enemies)
            {
                enemy.IsTurnComplete = false;
                enemy.PlayTurn();
            }
        }

        private bool IsEnemyTurnComplete()
        {
            foreach (var enemy in m_enemies)
            {
                if (!enemy.IsTurnComplete)
                    return false;
            }

            return true;
        }
    }
}
