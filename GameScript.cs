using Godot;
using System;

public class GameScript : Spatial
{
    public override void _Ready()
    {
        GetChild<Spatial>(0).Connect("game_over", this, "GameOver");
        GetChild<Spatial>(1).Connect("game_over", this, "GameOver");
    }


    private void GameOver(bool victory)
    {
        GetChild<Spatial>(1).CallDeferred("Stop");
        if (victory)
        {
            GetChild<Label>(3).Visible = true;
        }
        else
        {
            GetChild<Label>(4).Visible = true;
        }
    }

    private void Restart()
    {
        GetTree().ReloadCurrentScene();
    }


}
