using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GLTD/StatusList", fileName = "StatusList")]
public class StatusList : ScriptableObject
{
    public string[] statusIDs;

    public int burnID { get => System.Array.IndexOf(statusIDs, "burn") ; set => burnID = value; }
    public int chillID { get => System.Array.IndexOf(statusIDs, "chill"); set => chillID = value; }
    public int shockID { get => System.Array.IndexOf(statusIDs, "shock"); set => shockID = value; }
    public int wetID { get => System.Array.IndexOf(statusIDs, "wet"); set => wetID = value; }
    public int vulnerabilityID { get => System.Array.IndexOf(statusIDs, "vulnerability"); set => vulnerabilityID = value; }
    public int fortifiedID { get => System.Array.IndexOf(statusIDs, "fortified"); set => fortifiedID = value; }
    public int soapedID { get => System.Array.IndexOf(statusIDs, "soaped"); set => soapedID = value; }
    public int frostZoneID { get => System.Array.IndexOf(statusIDs, "frostZone"); set => frostZoneID = value; }
    public int mudZoneID { get => System.Array.IndexOf(statusIDs, "mudZone"); set => mudZoneID = value; }
    public int whirlpoolZoneID { get => System.Array.IndexOf(statusIDs, "whirlpoolZone"); set => whirlpoolZoneID = value; }
}
