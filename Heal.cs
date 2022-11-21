using Godot;
using System;

public class Heal : Node2D
{
    [Signal] delegate void CharacterHeal();
    public override void _Ready()
    {
        
    }
    public void _on_Area2D_body_entered(KinematicBody2D karakter){
        if(karakter.Owner.Name == "Character"){
            EmitSignal("CharacterHeal");
            QueueFree();
        }
    }
}
