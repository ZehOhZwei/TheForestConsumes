using Godot;
using System.Linq;
using System.Collections.Generic;
using System;

public class TileScript : Spatial
{
    string globid { get;} = Guid.NewGuid().ToString();
    MeshInstance meshInstance;
    Area area;
    int verticesCount;
    Dictionary<string, int>  adjacentTileStatuses = new Dictionary<string, int>();
    int tileStatus;
    int progress = 50;

    public override void _Ready()
    {
        meshInstance = GetChild<MeshInstance>(0);
        area = GetChild<Area>(1);

        area.GetChild<CollisionShape>(0).Shape = meshInstance.Mesh.CreateConvexShape();
        area.GetChild<CollisionShape>(0).Shape.Margin = 0.001f;

        var vertices = (Vector3[])meshInstance.Mesh.SurfaceGetArrays(0)[0];
        verticesCount = vertices.Length;
        var total = new Vector3();
        foreach (var vertex in vertices)
        {
            total += vertex;
        }
        var position = total / vertices.Length;
        Translate(position);
        meshInstance.Translate(-position);
        area.Translate(-position);

        var r = new Random();
        tileStatus = r.Next(-3, 3);
        SetColor();
    }

    private void AdjacentStatusChanged(string globid, int status)
    {
        if (adjacentTileStatuses.Keys.Contains(globid))
        {
            adjacentTileStatuses[globid] = status;
        }
        else if(adjacentTileStatuses.Count < verticesCount)
        {
            adjacentTileStatuses.Add(globid, status);
        }
    }

    private void UpdateStatus()
    {
        progress += adjacentTileStatuses.Values.Sum();
        if (progress > 100)
        {
            Forestify();
            progress = 50;
        }
        else if (progress < 0)
        {
            Urbanize();
            progress = 50;

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
        if (tileStatus < 3)
        {
            tileStatus++;
            SetColor();
            EmitSignal(nameof(status_changed), globid, tileStatus);
        }
    }

    private void Urbanize()
    {
        if (tileStatus > -3)
        {            
            tileStatus--;
            SetColor();
            EmitSignal(nameof(status_changed), globid, tileStatus);
        }
    }

    private void SetColor()
    {
        var material = new SpatialMaterial();

        switch (tileStatus)
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
        meshInstance.SetSurfaceMaterial(0, material);

    }

    private void RegisterAdjacentTiles()
    {
        var result = GetParent().GetChildren().Cast<Spatial>().OrderBy(x => x.Position.DistanceTo(Position)).Take(verticesCount);
        foreach (var tile in result)
        {
            tile.Connect("status_changed", this, "AdjacentStatusChanged");
        }

        EmitSignal(nameof(status_changed), globid, tileStatus);

    }

    [Signal]
    public delegate void status_changed(string globid, int status);
}
