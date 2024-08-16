using UnityEngine;

namespace Assets.Scripts
{
    public class PlayerController : MonoBehaviour
    {
        public float Speed = 10f;
        public float TurnSpeed = 100f;
        Vector2 Movement;
        Vector2 Turn;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        // Update is called once per frame
        void Update()
        {

            if (Input.GetKeyDown(KeyCode.Space))
            {
                Cursor.visible = !Cursor.visible;
                Cursor.lockState = Cursor.lockState == CursorLockMode.Locked ? CursorLockMode.None : CursorLockMode.Locked;
            }

            Movement = new Vector2(
                Input.GetAxis(Inputs.AXIS_HORIZONTAL),
                Input.GetAxis(Inputs.AXIS_VERTICAL)
            ) * Speed;

            if (Cursor.lockState == CursorLockMode.Locked)
                Turn = new Vector2(
                    Input.GetAxis(Inputs.AXIS_MOUSE_X),
                    Input.GetAxis(Inputs.AXIS_MOUSE_Y)
                ) * TurnSpeed;
            else
                Turn = Vector2.zero;

            transform.Translate(Movement.x * Time.deltaTime, 0, Movement.y * Time.deltaTime);

            transform.Rotate(0, Turn.x * Time.deltaTime, 0);
        }
    }
}
