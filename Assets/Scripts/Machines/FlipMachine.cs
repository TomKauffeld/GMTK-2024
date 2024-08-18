using Assets.Scripts.Figures;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Assets.Scripts.Machines
{
    public class FlipMachine : BaseMachine
    {
        public float RotationMultiplier = 180f;

        public TileBase FlipHorizontalTile;
        public TileBase FlipVerticalTile;

        public override MachineType MachineType => MachineType.FlipMachine;

        public override TileBase Tile => FlipVertical ? FlipVerticalTile : FlipHorizontalTile;

        private bool _flipVertical = true;

        public bool FlipVertical
        {
            get => _flipVertical;
            set
            {
                _flipVertical = value;
                InvokeTileChanged();
            }
        }

        public override void Next()
        {
            FlipVertical = !FlipVertical;
        }



        public override void UpdateFigure(BaseFigure figure)
        {
            Vector2 position = figure.Position;

            Vector3 rotation = figure.Rotation.eulerAngles;

            float delta = Time.deltaTime * Speed * RotationMultiplier;


            if (FlipVertical)
                rotation.y += (figure.Target > rotation.y ? 1 : -1) * delta;
            else
                rotation.x += (figure.Target > rotation.x ? 1 : -1) * delta;

            float angle = Mathf.Abs((FlipVertical ? rotation.y : rotation.x) - figure.Target);

            figure.Rotation = Quaternion.Euler(rotation);

            if (angle >= -delta && angle <= delta)
            {
                figure.Rotation = FlipVertical
                    ? Quaternion.Euler(rotation.x, figure.Target, rotation.z)
                    : Quaternion.Euler(figure.Target, rotation.y, rotation.z);

                Belt belt = GetExitBelt();
                figure.OnMachineExit(this);
                figure.SetActiveMachine(belt);
            }

            figure.Position = position;
        }

        public override void AcceptFigure(BaseFigure figure)
        {
            base.AcceptFigure(figure);

            if (FlipVertical)
                figure.Target = figure.Rotation.eulerAngles.y is >= -float.Epsilon and <= float.Epsilon ? 180f : 0f;
            else
                figure.Target = figure.Rotation.eulerAngles.x is >= -float.Epsilon and <= float.Epsilon ? 180f : 0f;
        }
    }
}
