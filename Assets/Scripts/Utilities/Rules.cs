
using UnityEngine;

public static class Rules
{
    
    public static float DetermineRotation(Vector3 start, Vector3 end)
    {
        
        if(start.x == end.x && start.z < end.z)
        {
            return 0f; //north
        }
        else if(start.x < end.x && start.z < end.z)
        {
            return 45f; //north east
        }
        else if(start.x < end.x && start.z == end.z)
        {
            return 90f; // east

        }else if(start.x < end.x && start.z > end.z)
        {
            return 135f; //south east

        }else if(start.x == end.x && start.z > end.z)
        {
            return 180f; // south

        }else if(start.x > end.x && start.z > end.z)
        {
            return 225f; //south west

        }else if(start.x > end.x && start.z == end.z)
        {
            return 270f; // west
            
        }else if(start.x > end.x && start.z < end.z)
        {
            return 315f; //north west
        }

        return 0f;
    }

    public static bool CheckValidEquipmentSlot(ItemSlot equipmentSlot, Item item)
    {

        if(equipmentSlot.type == "Inventory")
        {

            return true;
            
        }else if(item is Equipment equipItem)
        {
            
            if(equipItem.equipmentType == equipmentSlot.type)
            {

                return true;

            }else if(equipItem.equipmentType == "Ring" && equipmentSlot.type == "Ring 1" || equipItem.equipmentType == "Ring" && equipmentSlot.type == "Ring 2")
            {

                return true;

            }else if(equipItem.equipmentType == "Switch Hand" && equipmentSlot.type == "Main Hand" || equipItem.equipmentType == "Switch Hand" && equipmentSlot.type == "Off Hand")
            {

                return true;
            }
        }

        return false;
    }
}
