using Assets.Scripts.Figures;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Assets.Scripts.Machines
{
    public class SizeChangerMachine : BaseMachine
    {
        public TileBase SizeUpTile;

        public TileBase SizeDownTile;

        public override MachineType MachineType => MachineType.SizeChangerMachine;

        private bool _isUpChanger = true;

        public bool IsUpChanger
        {
            get => _isUpChanger;
            set
            {
                _isUpChanger = value;
                InvokeTileChanged();
            }
        }

        public override TileBase Tile => IsUpChanger ? SizeUpTile : SizeDownTile;

        public override void Next()
        {
            IsUpChanger = !IsUpChanger;
        }



        public override void UpdateFigure(BaseFigure figure)
        {
            float scale = figure.transform.localScale.x;
            if (IsUpChanger)
            {
                scale += Speed * Time.deltaTime;

                if (scale >= figure.Target)
                {
                    scale = figure.Target;
                    Belt exit = GetExitBelt();
                    figure.OnMachineExit(this);
                    figure.SetActiveMachine(exit);
                }
            }
            else
            {
                scale -= Speed * Time.deltaTime;

                if (scale <= figure.Target)
                {
                    scale = figure.Target;
                    Belt exit = GetExitBelt();
                    figure.OnMachineExit(this);
                    figure.SetActiveMachine(exit);
                }
            }

            Vector2 position = figure.Position;
            figure.transform.localScale = new Vector3(scale, scale, figure.transform.localScale.z);
            figure.Position = position;
        }

        public override void AcceptFigure(BaseFigure figure)
        {
            base.AcceptFigure(figure);

            if (IsUpChanger)
                figure.Target = figure.transform.localScale.x * 2f;
            else
                figure.Target = figure.transform.localScale.x / 2f;
        }
    }
}
