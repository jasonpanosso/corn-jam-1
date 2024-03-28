using UnityEngine;

[CreateAssetMenu(fileName = "WorldOrderData", menuName = "Custom/World Order Data")]
public class WorldOrderData : ScriptableObject
{
    public WorldType[] order;
}
