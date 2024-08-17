using UnityEngine.Tilemaps;

namespace Assets.Scripts.Machines
{
    public class Target : BaseMachine
    {
        public TileBase[] Tiles;

        public override MachineEnum MachineType => MachineEnum.Target;


        public override bool Placeable => false;

        public override TileBase[] Tile => Tiles;

        public override void Next()
        {
        }
    }
}
