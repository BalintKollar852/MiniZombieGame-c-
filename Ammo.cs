using Godot;
using System;

public class Ammo : Node2D
{
    // Ez is úgyan úgy működne mint a heal (tf2-höz hasonlóan)
    [Signal] delegate void AmmoBoxPickUp();
    public override void _Ready()
    {
        
    }
    public void _on_Area2D_body_entered(KinematicBody2D karakter){
        if(karakter.Owner.Name == "Character"){
            EmitSignal("AmmoBoxPickUp");
            QueueFree();
        }
    }
}
