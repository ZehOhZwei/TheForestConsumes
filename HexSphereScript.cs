using Godot;
using System.Linq;
using System.Collections.Generic;
using System;


public class HexSphereScript : Spatial
{
    Dictionary<string, int> TileStatuses { get; set; } = new Dictionary<string, int>();

    public override void _Ready()
    {
        var packedScene = GD.Load<PackedScene>("res://Tile.tscn");
        var childFaces = GD.Load<PackedScene>("res://HexSphere5.tscn").Instance().GetChildren();
        var cities = 10;



        foreach (MeshInstance meshInstance in childFaces)
        {
            var tile = packedScene.Instance();

            tile.GetChild<MeshInstance>(0).Mesh = meshInstance.Mesh;

            AddChild(tile);

        }

        var tiles = GetChildren().Cast<Spatial>();

        foreach (Spatial child in tiles)
        {
            child.Call("RegisterAdjacentTiles");
        }

        tiles = tiles.OrderBy(x => x.GlobalPosition.DistanceTo(new Vector3(0, 0, 2)));
        tiles.First().Call("InitialForest", true);

        for (int i = 1; i <= 6; i++)
        {
            tiles.ElementAt(i).Call("InitialForest", false);
        }

        var r = new Random();
        for (int i = 0; i < cities; i++)
        {
            Spatial randTile = (Spatial)tiles.ElementAt(r.Next(tiles.Count()));
            EmitSignal(nameof(init_city), randTile.Get("Guid"));
        }

    }

    private void ChildStatusChanged(string guid, int status)
    {
        if (TileStatuses.Keys.Contains(guid))
        {
            TileStatuses[guid] = status;
        }
        else
        {
            TileStatuses.Add(guid, status);
        }
    }

    private void CheckChildStatuses()
    {
        if (!TileStatuses.Values.Where(x => x < 0).Any())
        {
            EmitSignal(nameof(game_over), true);
        }
        else if (!TileStatuses.Values.Where(x => x > 0).Any())
        {
            EmitSignal(nameof(game_over), false);
        }
    }


    [Signal]
    public delegate void init_city(string Guid);

        [Signal]
    public delegate void game_over(bool victory);
}
