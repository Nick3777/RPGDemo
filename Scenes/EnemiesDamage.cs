using Godot;
using System;
using System.Collections.Generic;

public partial class EnemiesDamage : Node
{
	public static EnemiesDamage Instance { get; private set; }

	public override void _Ready()
	{
		Instance = this;
	}
	
	public static readonly Dictionary<String, int> enemyDamage = new Dictionary<String, int>
	{
		{ "Skeleton", 1 },
		{ "Knight", 7 },
		{ "Troll", 3 }
	};
	
	public static int GetDamage(String enemyType)
	{
		if (enemyDamage.ContainsKey(enemyType))
			return enemyDamage[enemyType];
		else
		{
			GD.PrintErr($"Tipo di nemico {enemyType} non trovato.");
			return 0; 
		}
	}
}
