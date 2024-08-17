using UnityEngine.Tilemaps;

namespace Assets.Scripts.Machines
{
    public class Spawner : BaseMachine
    {
        public TileBase SpawnerTile;

        public override MachineType MachineType => MachineType.Spawner;

        public override bool Placeable => false;

        public override TileBase Tile => SpawnerTile;

        public override void Next()
        {
        }
    }
}
