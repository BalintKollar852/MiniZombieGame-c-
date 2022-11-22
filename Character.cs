using Godot;
using System;

public class Character : Node2D
{
    int sebesseg;
    Vector2 Move = new Vector2(0, 0);
    Vector2 iranycucc;
    private AnimatedSprite animatedSprite;
    private bool mousedown;
    private bool moveornot;
    public override void _Ready()
    {
        animatedSprite = GetNode("KinematicBody2D/AnimatedSprite") as AnimatedSprite;
    }
    public override void _Process(float delta)
    {
        var alak = GetNode("KinematicBody2D") as KinematicBody2D;
        var knifearea = GetNode("KinematicBody2D/KnifeArea") as Area2D;
        alak.LookAt(GetGlobalMousePosition());
        knifearea.LookAt(GetGlobalMousePosition());

        Move.x = 0;
        Move.y = 0;
        if (Input.IsActionPressed("ui_left"))
        {
            Move.x -= 100;
        }
        if (Input.IsActionPressed("ui_right"))
        {
            Move.x += 100;
        }
        if (Input.IsActionPressed("ui_up"))
        {
            Move.y -= 100;
        }
        if (Input.IsActionPressed("ui_down"))
        {
            Move.y += 100;
        }
        if (Input.IsActionPressed("ui_down") || Input.IsActionPressed("ui_left")|| Input.IsActionPressed("ui_up") || Input.IsActionPressed("ui_right"))
        {
            moveornot = true;
        }
        else{
            moveornot = false;
        }
        if(moveornot){
            animatedSprite.Animation = "move";
        }
        if(moveornot == false){
            animatedSprite.Animation = "idle";
        }
        Move += Move.Normalized() * delta;
        alak.MoveAndSlide(Move);
    }
}
