using UnityEngine;

public class ZoneTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        NPCStateController npc = other.GetComponent<NPCStateController>();
        if (npc != null)
        {
            // Set the NPC to InZone state when it enters the zone
            npc.SetInZone(true);
        }
    }

    //private void OnTriggerExit(Collider other)
    //{
    //    Debug.Log("Exit Zone");
    //    NPCStateController npc = other.GetComponent<NPCStateController>();
    //    if (npc != null)
    //    {
    //        // Reset the NPC state when it leaves the zone
    //        npc.SetInZone(false);
    //    }
    //}
}
