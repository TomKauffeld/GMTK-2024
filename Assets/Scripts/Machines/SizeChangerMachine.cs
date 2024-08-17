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
    }
}
