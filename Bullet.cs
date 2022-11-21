using Godot;
using System;

public class Bullet : Node2D
{
    public int speed;
    public Vector2 irany;
    private float livetime;
    public override void _Ready()
    {
        var target = Vector2.Zero;
        var bullet = GetNode("Area2D") as Area2D;
        speed = 10;
        target = GetGlobalMousePosition();
        bullet.Rotation = GetAngleTo(target);
        irany = GlobalPosition.DirectionTo(target) * speed;
    }
    public override void _Process(float delta)
    {
        livetime += delta;
        var bullet = GetNode("Area2D") as Area2D;
        bullet.Position += irany;
        if(livetime >= 3){
            QueueFree();
        }
    }
    public void _on_Area2D_body_entered(KinematicBody2D enemy){
        if(enemy.Owner.Name != "Character" && enemy.Name != "EnemyBody"){
            QueueFree();
        }
    }
}
