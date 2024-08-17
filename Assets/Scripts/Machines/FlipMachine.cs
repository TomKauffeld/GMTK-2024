using UnityEngine.Tilemaps;

namespace Assets.Scripts.Machines
{
    public class FlipMachine : BaseMachine
    {
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
    }
}
