using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace UnityStandardAssets._2D
{
    [RequireComponent(typeof (PlatformerCharacter2D))]
    public class Platformer2DUserControl : MonoBehaviour
    {
        private PlatformerCharacter2D m_Character;
        private bool m_Jump;


        private void Awake()
        {
            m_Character = GetComponent<PlatformerCharacter2D>();
        }


        private void Update()
        {
            if (!m_Jump)
            {
                // Read the jump input in Update so button presses aren't missed.
                //m_Jump = CrossPlatformInputManager.GetButtonDown("Jump");
				int nbTouches = Input.touchCount;

				if(nbTouches > 0)
				{
					print(nbTouches + " touch(es) detected");

					for (int i = 0; i < nbTouches; i++)
					{
						Touch touch = Input.GetTouch(i);
						m_Jump = true;
						//print("Touch index " + touch.fingerId + " detected at position " + touch.position);
					}
				}
//				m_Jump = Input.GetTouch;
            }
        }


        private void FixedUpdate()
        {
            // Read the inputs.
            bool crouch = Input.GetKey(KeyCode.LeftControl);
            // float h = CrossPlatformInputManager.GetAxis("Horizontal");
            // Pass all parameters to the character control script.
            m_Character.Move(1, false, m_Jump);
            m_Jump = false;
        }
    }
}
