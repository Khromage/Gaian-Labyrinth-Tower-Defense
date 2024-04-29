using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GLTD/StatusList", fileName = "StatusList")]
public class StatusList : ScriptableObject
{
    public string[] statusIDs;

    public int burnID { get => System.Array.IndexOf(statusIDs, "burn") ; set => burnID = value; }
    public int chillID { get => System.Array.IndexOf(statusIDs, "chill"); set => chillID = value; }
    public int shockID { get => System.Array.IndexOf(statusIDs, "burn"); set => shockID = value; }
    public int wetID { get => System.Array.IndexOf(statusIDs, "burn"); set => wetID = value; }
    public int vulnerabilityID { get => System.Array.IndexOf(statusIDs, "burn"); set => vulnerabilityID = value; }
    public int frostZoneID { get => System.Array.IndexOf(statusIDs, "burn"); set => frostZoneID = value; }
    public int mudZoneID { get => System.Array.IndexOf(statusIDs, "burn"); set => mudZoneID = value; }
    public int whirlpoolZoneID { get => System.Array.IndexOf(statusIDs, "burn"); set => whirlpoolZoneID = value; }
}
