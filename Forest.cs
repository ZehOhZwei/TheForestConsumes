using Godot;
using System;

public class Forest : Spatial
{
    private void OnAreaEntered(Area area)
    {
        var myGroups = GetGroups();
        var theirGroups = area.GetParent().GetGroups();
        if (myGroups.Contains("forest") && theirGroups.Contains("forestHead"))
        {
            GetTree().CallGroup("growing", "Grow");
        }
    }

    private void Grow()
    {
        ColorIn();
        RemoveFromGroup("growing");
        AddToGroup("forest");
    }

    private void ColorIn()
    {
        var cylinder = GetChild<CSGCylinder>(0);
        var material = new SpatialMaterial();
        material.AlbedoColor = new Color(0, 1, 0);
        cylinder.Material = material;
    }

}
