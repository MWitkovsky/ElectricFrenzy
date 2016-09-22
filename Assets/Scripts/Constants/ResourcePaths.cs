using UnityEngine;
using System.Collections;

/**
 * This class is used to reference path names for resources in the resources folder (gameworld objects, art, etc.)
 * Basically for anything that needs to be dynamically loaded
 */
public class ResourcePaths {
    ///////////
    //PREFABS//
    ///////////
    private static string PrefabsRoot = "Prefabs/";
    private static string EntityPrefabsRoot = PrefabsRoot + "Entities/";
    private static string EnemyPrefabsRoot = EntityPrefabsRoot + "Enemies/";
    private static string PlayerPrefabsRoot = EntityPrefabsRoot + "Player/";

    //ENEMIES
    private static string SnakePrefabsRoot = EnemyPrefabsRoot + "Snake/"; //SNAKE? SNAAAAAAAKE
    public static string SnakeHeadGraphicPrefab = SnakePrefabsRoot + "HeadGraphic";
    public static string SnakeBodySectionPrefab = SnakePrefabsRoot + "BodySection";

    //POWERUPS
    public static string FirewallPrefab = PlayerPrefabsRoot + "Firewall";
    public static string AfterimagePrefab = PlayerPrefabsRoot + "Afterimage";

    /////////////
    //Materials//
    /////////////
    private static string MaterialsRoot = "Materials/";
    private static string EntityMaterialsRoot = MaterialsRoot + "Entities/";

    public static string AfterimageMaterial = EntityMaterialsRoot + "Afterimage";	
}
