using Assets.Scripts.Machines;
using JetBrains.Annotations;
using System;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Assets.Scripts.World
{
    [RequireComponent(typeof(Tilemap))]
    public class WorldRenderer : MonoBehaviour
    {
        public TileBase EmptyTile;

        public Camera RenderingCamera;
        private Vector2Int _renderSize = Vector2Int.zero;

        public bool IsReady { get; private set; } = false;

        public Vector2Int RenderSize
        {
            get => _renderSize;
            set
            {
                _renderSize = value;
                UpdateRenderSize();
            }
        }

        private void UpdateRenderSize()
        {
            if (_renderSize.x <= 0 || _renderSize.y <= 0)
                return;


            //TODO: Handle if screen is higher than large (mobile)
            RenderingCamera ??= Camera.main;

            RenderingCamera.transform.position = new Vector3(
                _renderSize.x / 2f,
                _renderSize.y / 2f,
                RenderingCamera.transform.position.z
            );

            RenderingCamera.orthographicSize = Math.Max(_renderSize.x, _renderSize.y) / 2f + 4f;
        }

        public event Action OnReady;


        private Tilemap _tilemap;

        void Start()
        {
            _tilemap = GetComponent<Tilemap>();
            IsReady = true;
            OnReady?.Invoke();
        }





        internal void SetMachine([NotNull] BaseMachine machine)
        {
            SetTile(machine.Position, machine.Tile);
        }

        internal void SetEmpty(Vector2Int position)
        {
            SetTile(position, EmptyTile);
        }

        private void SetTile(Vector2Int position, [CanBeNull] TileBase tile)
        {
            _tilemap.SetTile(new Vector3Int(position.x, position.y), tile);
        }


        internal void ClearMachine(Vector2Int position)
        {
            SetTile(position, null);
        }

        public Vector2Int WorldToCell(Vector3 position)
        {
            Vector3Int tmp = _tilemap.WorldToCell(position);
            return new Vector2Int(tmp.x, tmp.y);
        }
    }
}
