﻿using UnityEngine;
using System.Collections;

/**
 * This class is used to reference path names for resources in the resources folder (gameworld objects, art, etc.)
 * Basically for anything that needs to be dynamically loaded
 */
public class ResourcePaths {
    ///////////
    //PREFABS//
    ///////////
    private static readonly string PrefabsRoot = "Prefabs/";
    private static readonly string EntityPrefabsRoot = PrefabsRoot + "Entities/";
    private static readonly string EnvironmentPrefabsRoot = EntityPrefabsRoot + "Environment/";
    private static readonly string PickupsRoot = EnvironmentPrefabsRoot + "Pickups/";
    private static readonly string EnemyPrefabsRoot = EntityPrefabsRoot + "Enemies/";
    private static readonly string BossPrefabsRoot = EnemyPrefabsRoot + "Bosses/";
    private static readonly string SakuraTreePrefabsRoot = BossPrefabsRoot + "SakuraTree/";
    private static readonly string PlayerPrefabsRoot = EntityPrefabsRoot + "Player/";
    private static readonly string FXPrefabsRoot = PrefabsRoot + "FX/";

    //ENEMIES
    private static readonly string SnakePrefabsRoot = EnemyPrefabsRoot + "SnakeComponents/"; //SNAKE? SNAAAAAAAKE
    public static readonly string SnakeHeadGraphicPrefab = SnakePrefabsRoot + "HeadGraphic";
    public static readonly string SnakeBodySectionPrefab = SnakePrefabsRoot + "BodySection";
    public static readonly string SnakeHeadOnlyPrefab = SnakePrefabsRoot + "SnakeHeadOnly";
    public static readonly string SnakePacketPrefab = SnakePrefabsRoot + "SnakePacket";

    //BOSSES
    public static readonly string SakuraLeafPrefab = SakuraTreePrefabsRoot + "SakuraLeaf";

    //POWERUPS
    public static readonly string FirewallPrefab = PlayerPrefabsRoot + "Firewall";
    public static readonly string AfterimagePrefab = PlayerPrefabsRoot + "Afterimage";
    public static readonly string ReclaimedPacketPrefab = PickupsRoot + "ReclaimedPacket";
    
    //FX
    public static readonly string HitBurstPrefab = FXPrefabsRoot + "HitBurst";
    public static readonly string SmallHitPrefab = FXPrefabsRoot + "SmallHit";
    public static readonly string HackFXPrefab = FXPrefabsRoot + "HackFX";
    public static readonly string TeleportFXPrefab = FXPrefabsRoot + "TeleportFX";
    public static readonly string TrapDoorFXPrefab = FXPrefabsRoot + "TrapDoorFX";


    /////////////
    //Materials//
    /////////////
    private static readonly string MaterialsRoot = "Materials/";
    private static readonly string EntityMaterialsRoot = MaterialsRoot + "Entities/";

    public static readonly string AfterimageMaterial = EntityMaterialsRoot + "Afterimage";	
}
