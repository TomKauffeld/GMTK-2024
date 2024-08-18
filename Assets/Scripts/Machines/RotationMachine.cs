using Assets.Scripts.Figures;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Assets.Scripts.Machines
{
    public class RotationMachine : BaseMachine
    {
        public float RotationMultiplier = 45f;

        public TileBase TurnLeftTile;
        public TileBase TurnRightTile;

        public override MachineType MachineType => MachineType.RotationMachine;

        private bool _rotateRight = true;

        public bool RotateRight
        {
            get => _rotateRight;
            set
            {
                _rotateRight = value;
                InvokeTileChanged();
            }
        }

        public override TileBase Tile => RotateRight ? TurnRightTile : TurnLeftTile;

        public override void Next()
        {
            RotateRight = !RotateRight;
        }


        public override void UpdateFigure(BaseFigure figure)
        {
            Vector2 position = figure.Position;

            Quaternion target = figure.transform.rotation;
            target = Quaternion.Euler(target.eulerAngles.x, target.eulerAngles.y, figure.Target);

            figure.transform.rotation = Quaternion.RotateTowards(figure.transform.rotation, target, Speed * RotationMultiplier * Time.deltaTime);

            float angle = Quaternion.Angle(figure.transform.rotation, target);
            if (angle is >= -float.Epsilon and <= float.Epsilon)
            {
                figure.transform.rotation = target;
                Belt belt = GetExitBelt();
                figure.OnMachineExit(this);
                figure.SetActiveMachine(belt);
            }

            figure.Position = position;
        }

        public override void AcceptFigure(BaseFigure figure)
        {
            base.AcceptFigure(figure);

            if (RotateRight)
                figure.Target = figure.transform.rotation.eulerAngles.z - 45f;
            else
                figure.Target = figure.transform.rotation.eulerAngles.z + 45f;
        }
    }
}
