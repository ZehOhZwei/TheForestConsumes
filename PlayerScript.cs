using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

public class PlayerScript : Spatial
{
    float rotationAmount = 0.0025f;
    int recordInterval = 60;
    int moveCounter = 0;
    Vector3[] vertexes = {
        new Vector3(0, 0.1f, 1),
        new Vector3(0, -0.1f, 1),
        new Vector3(-0.1f, 0, 1),
        new Vector3(0.1f, 0, 1),
    };

    int[] indexes =
    {
        0, 1, 2,
        0, 1, 3,
    };

    Vector3[] tempVertexes = {

    };

    int[] tempIndexes =
    {

    };

    public override void _Ready()
    {
        //DrawForest();

        var shape = new ConvexPolygonShape();
        shape.Points = vertexes;

        GetParent().GetChild<Area>(2).GetChild<CollisionShape>(0).Shape = shape;
    }

    private void OnForestEntered(Area area)
    {
        foreach (var vertex in tempVertexes)
        {
            vertexes = vertexes.Append(vertex).ToArray();
        }

        foreach (var index in tempIndexes)
        {
            indexes = indexes.Append(index).ToArray();
        }

        var shape = new ConvexPolygonShape();
        shape.Points = vertexes;

        GetParent().GetChild<Area>(2).GetChild<CollisionShape>(0).Shape = shape;
    }

    public override void _PhysicsProcess(float delta)
    {


        var isInForest = false;

        if (moveCounter >= recordInterval && !isInForest)
        {
            moveCounter = 0;
            var playerPosition = GetChild<CSGCylinder>(0).GlobalPosition;

            tempVertexes = tempVertexes.Append(playerPosition).ToArray();
            var newIndexes = GetClosestTwoVertexes(playerPosition);
            tempIndexes = indexes.Append(vertexes.Length).ToArray();
            tempIndexes = indexes.Append(newIndexes[0]).ToArray();
            tempIndexes = indexes.Append(newIndexes[1]).ToArray();
        }

        var RotationX = 0f;
        var RotationY = 0f;

        if (Input.IsActionPressed("moveRight"))
        {
            RotationX += rotationAmount;
        }
        if (Input.IsActionPressed("moveLeft"))
        {
            RotationX += -rotationAmount;
        }

        RotationY += -rotationAmount;




        RotateObjectLocal(Vector3.Forward, RotationX * 10);
        RotateObjectLocal(Vector3.Right, RotationY);

        moveCounter++;

    }

    private void DrawForest()
    {
        GetParent().CallDeferred("remove_child", GetParent().GetChild<MeshInstance>(3));

        var arrMesh = new ArrayMesh();
        Godot.Collections.Array arrays = new Godot.Collections.Array();
        arrays.Resize((int)Mesh.ArrayType.Max);
        arrays[(int)Mesh.ArrayType.Vertex] = vertexes;
        arrays[(int)Mesh.ArrayType.Index] = indexes;

        arrMesh.AddSurfaceFromArrays(Mesh.PrimitiveType.TriangleStrip, arrays);
        var mesh = new MeshInstance();
        mesh.Mesh = arrMesh;

        var material = new SpatialMaterial();
        material.AlbedoColor = new Color(0, 1, 0);
        mesh.MaterialOverride = material;

        GetParent().CallDeferred("add_child", mesh);
    }

    private int[] GetClosestTwoVertexes(Vector3 position)
    {
        var result = new int[2];

        var closest = new Vector3(0, 0, 0);
        var secondClosest = new Vector3(0, 0, 0);

        for (int i = 0; i < vertexes.Length; i++)
        {
            if (position.DistanceTo(vertexes[i]) < position.DistanceTo(closest))
            {
                closest = vertexes[i];
                result[0] = i;
            }
            else if (position.DistanceTo(vertexes[i]) < position.DistanceTo(secondClosest))
            {
                secondClosest = vertexes[i];
                result[1] = i;
            }
        }
        return result;
    }
}
