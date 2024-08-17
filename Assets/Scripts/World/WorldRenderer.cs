using Assets.Scripts.Machines;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Assets.Scripts.World
{
    [RequireComponent(typeof(Tilemap))]
    public class WorldRenderer : MonoBehaviour
    {
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

        public const float TimeBetweenFrames = 0.5f;
        private float _timer = TimeBetweenFrames;
        public ushort Index = 0;

        private void Update()
        {
            _timer -= Time.deltaTime;
            if (_timer < 0f)
            {
                _timer += TimeBetweenFrames;
                ++Index;
                UpdateTiles();
            }
        }

        private void UpdateTiles()
        {
            foreach (Vector2Int position in _tiles.Keys)
            {
                SetTile(position);
            }
        }

        private readonly Dictionary<Vector2Int, TileBase[]> _tiles = new();


        internal void SetMachine([NotNull] BaseMachine machine)
        {
            SetTiles(machine.Position, machine.Tile);
        }

        internal void SetEmpty(Vector2Int position)
        {
            SetTiles(position, null);
        }

        private void SetTiles(Vector2Int position, TileBase[] tile)
        {
            if (tile == null || tile.Length == 0)
            {
                _tiles.Remove(position);
            }
            else
            {
                _tiles[position] = tile;
            }
            SetTile(position);
        }

        private void SetTile(Vector2Int position)
        {
            if (_tiles.TryGetValue(position, out TileBase[] tiles) && tiles is { Length: > 0 })
                _tilemap.SetTile(new Vector3Int(position.x, position.y), tiles[Index % tiles.Length]);
            else
                _tilemap.SetTile(new Vector3Int(position.x, position.y), null);
        }

        public Vector2Int WorldToCell(Vector3 position)
        {
            Vector3Int tmp = _tilemap.WorldToCell(position);
            return new Vector2Int(tmp.x, tmp.y);
        }
    }
}
