using _BoardGo.Scripts.Generic;
using UnityEngine;

namespace _BoardGo.Scripts.Player
{
    
    public class PlayerInputKeyboard : MonoBehaviour, IInput
    {
        private float m_h;
        private float m_v;
        public float Horizontal => m_h;
        public float Vertical => m_v;
        private bool m_inputEnabled = false;
        public bool InputEnabled
        {
            get { return m_inputEnabled;}
            set { m_inputEnabled = value; }
        }
        public void GetInput()
        {
            if (!m_inputEnabled)
            {
                m_h = 0;
                m_v = 0;
                return;
            }

            m_h = Input.GetAxisRaw("Horizontal");
            m_v = Input.GetAxisRaw("Vertical");
        }
    }
}
