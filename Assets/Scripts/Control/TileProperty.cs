using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileProperty
{
    public Vector2Int position { get; protected set; }
    public float pathCost { get; protected set; }
    public float fertility { get; protected set; }

    public bool blockPath { get; protected set; }
    public bool blockPlant { get; protected set; }
    public bool blockStackable { get; protected set; }
    public bool blockBuilding { get; protected set; }
    public bool supportRoof { get; protected set; }
    public bool reserved { get; set; }

    public float gCost { get; set; }
    public float hCost { get; set; }
    public float fCost { get { return this.gCost + this.hCost; } }
    public TileProperty parent { get; set; }

    public List<BaseCharacter> characters { get; set; }

    public TileProperty(Vector2Int position)
    {
        this.position = position;
        this.characters = new List<BaseCharacter>();
        this.Reset();
    }

    public void Reset()
    {
        this.reserved = false;
        this.fertility = 1f;
        this.pathCost = 1f;
        this.blockPath = false;
        this.blockPlant = false;
        this.blockStackable = false;
        this.blockBuilding = false;
        this.supportRoof = false;
        this.gCost = 0;
        this.hCost = 0;
        this.parent = null;
    }

    public void Update()
    {
        this.Reset();

        foreach (Tilable tilable in Loki.map.GetAllTilablesAt(this.position))
        {
            if (this.fertility != 0f)
            {
                this.fertility *= tilable.def.fertility;
            }
            if (!this.blockPath && this.pathCost != 0)
            {
                this.pathCost *= tilable.def.pathCost;
            }
            if (this.blockPath == false)
            {
                this.blockPath = tilable.def.blockPath;
            }
            if (this.blockStackable == false)
            {
                this.blockStackable = tilable.def.blockStackable;
            }
            if (this.blockPlant == false)
            {
                this.blockPlant = tilable.def.blockPlant;
            }
            if (this.blockBuilding == false)
            {
                this.blockBuilding = tilable.def.blockBuilding;
            }
            if (this.supportRoof == false)
            {
                this.supportRoof = tilable.def.supportRoof;
            }
        }


        if (this.blockPath)
        {
            this.pathCost = 0f;
        }
        else if (this.pathCost <= 0)
        {
            this.blockPath = true;
        }
    }

    // A tilable is just an entity contain in a tile/layer.
    public class Tilable 
    {
        /* Scale */
        public Vector3 scale = Vector3.one;

        /* Tilable tick counts */
        protected int ticks = 0;

        /* Matrix */
        private Dictionary<int, Matrix4x4> _matrices;

        /* Do we need to reset matrices */
        public bool resetMatrices = false;

        /* If this is True the tilable is not drawn */
        public bool hidden = false;

        /* Parent bucket */
        public LayerGridBucket bucket { get; protected set; }

        /// Register the bucket of the tilable
        public void SetBucket(LayerGridBucket bucket)
        {
            this.bucket = bucket;
        }

        /// Destroy this tilable
        public virtual void Destroy()
        {
            if (this.bucket != null)
            {
                this.bucket.DelTilable(this);
            }
        }

        /// Generic method to update graphics
        public virtual void UpdateGraphics() { }

        /// Get the matrice of our tilable
        public Matrix4x4 GetMatrice(int graphicUID)
        {
            if (this._matrices == null || this.resetMatrices)
            {
                this._matrices = new Dictionary<int, Matrix4x4>();
                this.resetMatrices = true;
            }
            if (!this._matrices.ContainsKey(graphicUID))
            {
                Matrix4x4 mat = Matrix4x4.identity;
                mat.SetTRS(
                    new Vector3(
                        this.position.x
                        - this.def.graphics.pivot.x * this.scale.x
                        + (1f - this.scale.x) / 2f
                        , this.position.y
                        - this.def.graphics.pivot.y * this.scale.y
                        + (1f - this.scale.y) / 2f
                        , LayerUtils.Height(this.def.layer) + GraphicInstance.instances[graphicUID].priority
                    ),
                    Quaternion.identity,
                    this.scale
                );
                this._matrices.Add(graphicUID, mat);
            }
            return this._matrices[graphicUID];
        }
    }
}
