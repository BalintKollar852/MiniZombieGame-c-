using Godot;
using System;

public class Gold : Node2D
{
    [Signal] delegate void GoldPickUp();
    private bool pickup;
    public override void _Ready()
    {
        //Amikor a karakter belemegy az Area2D be akkor egy labelt dobjon fel és e betűvel lehessen felvenni
        
    }
    public override void _Process(float delta)
	{
        if(pickup){
            EmitSignal("GoldPickUp");
            QueueFree();
        } 
    }
    public void _on_Area2D_body_entered(KinematicBody2D karakter){
        if(karakter.Owner.Name == "Character"){ 
            pickup = true;
        }
    }
    public void _on_Area2D_body_exited(KinematicBody2D karakter){
        if(karakter.Owner.Name == "Character"){ 
            pickup = false;
        }
    }
}
