using StringEnum;

public enum LibraryCatalogNames : uint
{
    //Other
    [StringValue("")] EMPTY = 0,
    [StringValue("Assets/Media/Prefabs/Pool.prefab")] POOL_PREFAB = 1,
    
    //Units
    [StringValue("Assets/Media/Prefabs/Units/BlueTeam/Blue.prefab")] BLUE_TEAM_UNIT = 1001,
    [StringValue("Assets/Media/Prefabs/Units/RedTeam/Red.prefab")] RED_TEAM_UNIT = 1002,
    [StringValue("Assets/Media/Prefabs/Units/Assassin/AssassinUnit.prefab")] ASSASSIN_UNIT = 1003,
    [StringValue("Assets/Media/Prefabs/Units/Soldier/SoldierUnit.prefab")] SOLDIER_UNIT = 1004,
    [StringValue("Assets/Media/Prefabs/Units/Archer/ArcherUnit.prefab")] ARCHER_UNIT = 1005
}