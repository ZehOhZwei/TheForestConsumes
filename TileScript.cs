using Godot;
using System.Linq;
using System.Collections.Generic;
using System;

public class TileScript : Spatial
{
    string Globid { get;} = Guid.NewGuid().ToString();
    MeshInstance MeshInstance { get; set; }
    Area Area { get; set; }
    int VerticesCount { get; set; }
    Dictionary<string, int>  AdjacentTileStatuses { get; set; } = new Dictionary<string, int>();
    int TileStatus { get; set; }
    int Progress { get; set; } = 50;

    public override void _Ready()
    {
        MeshInstance = GetChild<MeshInstance>(0);
        Area = GetChild<Area>(1);

        Area.GetChild<CollisionShape>(0).Shape = MeshInstance.Mesh.CreateConvexShape();
        Area.GetChild<CollisionShape>(0).Shape.Margin = 0.001f;

        var vertices = (Vector3[])MeshInstance.Mesh.SurfaceGetArrays(0)[0];
        VerticesCount = vertices.Length;
        var total = new Vector3();
        foreach (var vertex in vertices)
        {
            total += vertex;
        }
        var position = total / vertices.Length;
        Translate(position);
        MeshInstance.Translate(-position);
        Area.Translate(-position);

        var r = new Random();
        TileStatus = r.Next(-3, 3);
        SetColor();
    }

    private void AdjacentStatusChanged(string Globid, int status)
    {
        if (AdjacentTileStatuses.Keys.Contains(Globid))
        {
            AdjacentTileStatuses[Globid] = status;
        }
        else if(AdjacentTileStatuses.Count < VerticesCount)
        {
            AdjacentTileStatuses.Add(Globid, status);
        }
    }

    private void UpdateStatus()
    {
        Progress += AdjacentTileStatuses.Values.Sum();
        if (Progress > 100)
        {
            Forestify();
            Progress = 50;
        }
        else if (Progress < 0)
        {
            Urbanize();
            Progress = 50;

        }
    }

    private void PlayerEntered(Node body)
    {
        if (body.IsInGroup("player"))
        {            
            Forestify();
            Forestify();
        }
    }

    private void Forestify()
    {
        if (TileStatus < 3)
        {
            TileStatus++;
            SetColor();
            EmitSignal(nameof(status_changed), Globid, TileStatus);
        }
    }

    private void Urbanize()
    {
        if (TileStatus > -3)
        {            
            TileStatus--;
            SetColor();
            EmitSignal(nameof(status_changed), Globid, TileStatus);
        }
    }

    private void SetColor()
    {
        var material = new SpatialMaterial();

        switch (TileStatus)
        {
            case -3:
                material.AlbedoColor = new Color("#888888");
                break;
            case -2:
                material.AlbedoColor = new Color("#9F9771");
                break;
            case -1:
                material.AlbedoColor = new Color("#BCAA54");
                break;
            case 0:
                material.AlbedoColor = new Color("#DEBF33");
                break;
            case 1:
                material.AlbedoColor = new Color("#A3CA33");
                break;
            case 2:
                material.AlbedoColor = new Color("#69D533");
                break;
            case 3:
                material.AlbedoColor = new Color("#00FF00");
                break;

        }
        MeshInstance.SetSurfaceMaterial(0, material);

    }

    private void RegisterAdjacentTiles()
    {
        var result = GetParent().GetChildren().Cast<Spatial>().OrderBy(x => x.Position.DistanceTo(Position)).Take(VerticesCount);
        foreach (var tile in result)
        {
            tile.Connect("status_changed", this, "AdjacentStatusChanged");
        }

        EmitSignal(nameof(status_changed), Globid, TileStatus);

    }

    [Signal]
    public delegate void status_changed(string Globid, int status);
}
