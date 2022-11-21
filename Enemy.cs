using Godot;
using System;

public class Enemy : Node2D
{
    // Majd a tilemapen csak megadott helyen spawnoljanak
    [Export] public PackedScene psGold;
    [Signal] delegate void EnemyAttack();
    public float speed;
    public Vector2 irany;
    public Vector2 target; 
    public Vector2 move; 
    private bool followorno = false; 
    private bool idleorno = true; 
    private bool attackorno = false; 
    private float attacktime; 
    private float fps; 
    private int hp = 100; 
    private int db; 
    public override void _Ready()
    {
        speed = 80f;
    }
    
    public override void _Process(float delta)
    {
        GD.Print(db);
        var hpbar = GetNode("EnemyBody/ProgressBar") as ProgressBar;
        hpbar.Value = hp;
        hpbar.RectRotation = 0;
        if(hp < 100){
            hpbar.Visible = true;
        }
        else{
            hpbar.Visible = false;
        }
        if(hp <= 0){
            QueueFree();
        }
        fps = 1 / delta;
        var player = GetNode<Character>("../Character");
        var playerbody = GetNode<KinematicBody2D>("../Character/KinematicBody2D");
        var enemy_animatedsprite = GetNode("EnemyBody/AnimatedSprite") as AnimatedSprite;
        var enemy = GetNode("EnemyBody") as KinematicBody2D;
        target = Position.DirectionTo((player.Position + playerbody.Position) - enemy.Position);
        if(attackorno == false && followorno){
            enemy_animatedsprite.Play("move");
            enemy.LookAt((player.Position + playerbody.Position));
            enemy.MoveAndSlide(target * speed);
        }
        if(attackorno){ 
            attacktime += delta;
            enemy_animatedsprite.Play("attack");
            enemy.LookAt((player.Position + playerbody.Position));
            enemy.MoveAndSlide(target * speed);
            if(attacktime >= 1){
                EmitSignal("EnemyAttack");
                attacktime = 0;
            }
        }
        if(idleorno){
            enemy_animatedsprite.Play("idle"); 
        }
    }
    public void _on_Enemy_tree_exiting(){
        var game = GetNode<Game>("/root/Game");
        int droporno;
        Random random = new Random();
        droporno = random.Next(0, 2);
        if(droporno == 1){
            var gamenode = GetTree().Root.GetNode("Game") as Node2D;
            var enemy = GetNode("EnemyBody") as KinematicBody2D;
            var enemynode = enemy.Owner as Node2D;
            Node2D gold = (Node2D)psGold.Instance();
            gold.Position = enemynode.Position + enemy.Position;
            gold.Connect("GoldPickUp",game,"on_goldpickup");
		    gamenode.AddChild(gold);
        }
    }
    public void _on_Follow_body_entered(KinematicBody2D karakter){
        var enemy_animatedsprite = GetNode("EnemyBody/AnimatedSprite") as AnimatedSprite;
        if(karakter.Owner.Name == "Character"){ 
            enemy_animatedsprite.Stop();
            followorno = true;
            idleorno = false;
        }
    }
    public void _on_Follow_body_exited(KinematicBody2D karakter){
        var enemy_animatedsprite = GetNode("EnemyBody/AnimatedSprite") as AnimatedSprite;
        if(karakter.Owner.Name == "Character"){ 
            enemy_animatedsprite.Stop();
            idleorno = true;
            followorno = false;
        }
    }
    public void _on_Attack_body_entered(KinematicBody2D karakter){
        var enemy_animatedsprite = GetNode("EnemyBody/AnimatedSprite") as AnimatedSprite;
        if(karakter.Owner.Name == "Character"){ 
            enemy_animatedsprite.Stop();
            attackorno = true;

        }
    }
    public void _on_Attack_body_exited(KinematicBody2D karakter){
        var enemy_animatedsprite = GetNode("EnemyBody/AnimatedSprite") as AnimatedSprite;
        if(karakter.Owner.Name == "Character"){ 
            enemy_animatedsprite.Stop();
            attackorno = false;
        }
    }
    public void _on_AreaShape_area_entered(Area2D areashape){
        // Ha több darab lövedék jön akkor valamiért szar
        // Ahány db lövedék van egyszerre benne anyiszor kene levonni
        if(areashape.Owner.Name == "Bullet"){
            hp -= 25;
            areashape.Owner.QueueFree();
        }
    }
    
}
