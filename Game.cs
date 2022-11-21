using Godot;
using System;
public class Game : Node2D
{
    // Amerre néz a karakter egy kis flasflight (picit sötét lenne a map)
    // A késnél egy area 2d collisonje mindig a az egér fele nézne és ha ebbe az area 2d-be beleér egy enemy és megnyomod a bal klikket akkor sebez rá
    [Export]
	public PackedScene psBullet;
    [Export]
	public PackedScene psEnemy;
    [Export]
	public PackedScene psHeal;
    [Export]
	public PackedScene psAmmo;
    private bool mousedown;
    private float elapsedtime;
    private float reloadtime;
    private float enemyspawntime;
    private int bulletnumber = 30;
    private int hp = 100;
    private int gold;
    private int maxammo = 150;
    private bool weaponuse = true;
    public override void _Ready()
    {
        Random random = new Random();
        Node2D heal = (Node2D)psHeal.Instance();
        heal.Position = new Vector2(random.Next(200, 400), random.Next(0, 200));
        heal.Connect("CharacterHeal",this,"on_characterheal");
        AddChild(heal);

        Node2D ammo = (Node2D)psAmmo.Instance();
        ammo.Position = new Vector2(random.Next(400, 600), random.Next(0, 200));
        ammo.Connect("AmmoBoxPickUp",this,"on_ammoboxpickup");
        AddChild(ammo);
    }
    public void on_enemyattack(){
        hp -= 5;
        // Legyen a pályán egy item amit fel lehessen venni és healeljen
    }
    public void on_goldpickup(){
        gold++;
    }
    public void on_characterheal(){
        if((hp + 25) <= 100){
            hp += 25;
        }
        else{
            hp = 100;
        }
    }
    public void on_ammoboxpickup(){
        if((maxammo + 60) <= 150){
            maxammo += 150;
        }
        else{
            maxammo = 150;
        }
    }
    public override void _Input(InputEvent esemeny)
	{
		if(esemeny is InputEventMouseButton eger){
            if(eger.Pressed && eger.ButtonIndex == 1){
			    mousedown = true;
        	}
            else if(eger.ButtonIndex == 1 && eger.Pressed == false){
			    mousedown = false;
        	}
        }
        if (Input.IsActionJustPressed("1"))
        {
            weaponuse = true;
        }
        if (Input.IsActionJustPressed("2"))
        {
            weaponuse = false;
        }
    }
    public override void _Process(float delta)
    {
        var knife = GetNode("Character/HUD/Knife/Sprite") as Sprite;
        var rifle = GetNode("Character/HUD/Rifle/Sprite") as Sprite;
        var rifle_visible = GetNode("Character/HUD/Rifle/Name") as Label;
        var knife_visible = GetNode("Character/HUD/Knife/Name") as Label;
        var weapon_background = GetNode("Character/HUD/Weapon_BackGround") as Sprite;
        var hpnumber = GetNode("Character/HUD/HPBar/Count/Background/Number") as Label;
        var hptexture = GetNode("Character/HUD/HPBar/TextureProgress") as TextureProgress;
        var goldtexture = GetNode("Character/HUD/Gold") as Label;
        var karakter = GetNode("Character") as Node2D;
        var karakterbody = GetNode("Character/KinematicBody2D") as KinematicBody2D;
        var rifle_bulletnumber = GetNode("Character/HUD/Rifle/BulletNumber") as Label;
        if(weaponuse){
            knife.Modulate = Color.Color8((byte) 128, (byte)127, (byte)108);
            knife_visible.Visible = false;
            rifle_visible.Visible = true;
            weapon_background.Position = new Vector2(960, 475);
            rifle.Modulate = Color.Color8((byte) 255, (byte)255, (byte)255);
        }
        else{
            rifle.Modulate = Color.Color8((byte) 128, (byte)127, (byte)108);
            rifle_visible.Visible = false;
            knife_visible.Visible = true;
            weapon_background.Position = new Vector2(960, 542);
            knife.Modulate = Color.Color8((byte) 255, (byte)255, (byte)255);
        }
        hpnumber.Text = Convert.ToString(hp);
        hptexture.Value = hp;
        goldtexture.Text = Convert.ToString(gold);
        enemyspawntime += delta;
        if(enemyspawntime >= 3){
            Random random = new Random();
            Node2D enemy = (Node2D)psEnemy.Instance();
            enemy.Position = new Vector2(random.Next(0, 200), random.Next(0, 200));
            enemy.Connect("EnemyAttack",this,"on_enemyattack");
            AddChild(enemy);
            enemyspawntime = 0;
        }
        elapsedtime += delta;
        if(weaponuse){
            if(mousedown == true){
                if(elapsedtime >= 0.3f && bulletnumber > 0){
                    Node2D bullet = (Node2D)psBullet.Instance();
		    	    bullet.Position = karakter.Position + karakterbody.Position;
                    AddChild(bullet); 
                    bulletnumber--;
                    elapsedtime = 0;
                }
            }
            if (Input.IsActionPressed("r"))
            {
                reloadtime += delta;
                if(reloadtime >= 4){
                    if(maxammo >= 0 && maxammo <= 150){
                        if(30 - bulletnumber <= maxammo){
                            maxammo -= 30 - bulletnumber;
                            bulletnumber = 30;
                            reloadtime = 0;
                        }
                        if(30 - bulletnumber > maxammo){
                            maxammo = 0;
                            bulletnumber = bulletnumber + maxammo;
                            reloadtime = 0;
                        }
                    }
                }
            }
            else{
            reloadtime = 0;
            }
        }
        rifle_bulletnumber.Text = Convert.ToString(bulletnumber) + " / " + Convert.ToString(maxammo);
    }

}

