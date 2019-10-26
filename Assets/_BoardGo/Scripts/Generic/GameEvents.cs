using System;
using UnityEngine;

namespace _BoardGo.Scripts.Generic
{
    public class GameEvents : MonoBehaviour
    {
        public static GameEvents Instance;
        private void Awake()
        {
            Instance = this;
        }

        public event Action onPlayerMoveFinish;
        public void OnPlayerMoveFinish()
        {
            onPlayerMoveFinish?.Invoke();
        }
    }
}
