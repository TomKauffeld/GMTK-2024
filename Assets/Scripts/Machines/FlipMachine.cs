using UnityEngine.Tilemaps;

namespace Assets.Scripts.Machines
{
    public class FlipMachine : BaseMachine
    {
        public TileBase[] FlipHorizontalTiles;
        public TileBase[] FlipVerticalTiles;

        public override MachineEnum MachineType => MachineEnum.FlipMachine;

        public override TileBase[] Tile => FlipVertical ? FlipVerticalTiles : FlipHorizontalTiles;

        private bool _flipVertical = false;

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
