
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

        if(equipmentSlot.type is ItemSlotType.Inventory)
        {

            return true;
            
        }else if(item is Equipment equipItem)
        {
            
            if(equipItem.type is EquipmentType.Helmet && equipmentSlot.type is ItemSlotType.Helmet)
            {

                return true;

            }else if(equipItem.type is EquipmentType.Amulet && equipmentSlot.type is ItemSlotType.Amulet)
            {

                return true;

            }else if(equipItem.type is EquipmentType.Chest && equipmentSlot.type is ItemSlotType.Chest)
            {

                return true;

            }else if(equipItem.type is EquipmentType.Glove && equipmentSlot.type is ItemSlotType.Glove)
            {

                return true;

            }else if(equipItem.type is EquipmentType.Boot && equipmentSlot.type is ItemSlotType.Boot)
            {

                return true;

            }else if(equipItem.type is EquipmentType.MainHand && equipmentSlot.type is ItemSlotType.MainHand)
            {

                return true;

            }else if(equipItem.type is EquipmentType.OffHand && equipmentSlot.type is ItemSlotType.OffHand)
            {

                return true;

            }else if(equipItem.type is EquipmentType.Ring && equipmentSlot.type is ItemSlotType.RingPrimary || 
                     equipItem.type is EquipmentType.Ring && equipmentSlot.type is ItemSlotType.RingSecondary)
            {

                return true;

            }else if(equipItem.type is EquipmentType.SwitchHand && equipmentSlot.type is ItemSlotType.MainHand || 
                     equipItem.type is EquipmentType.SwitchHand && equipmentSlot.type is ItemSlotType.OffHand)
            {

                return true;
            }
        }

        return false;
    }

    public static Vector3 CoordToPos(Vector2Int coord, float xMod = 0, float yMod = 0, float zMod = 0)
    {

        return new Vector3((float)coord.x + xMod, yMod, (float)coord.y + zMod);
    }

    public static Vector2Int PosToCoord(Vector3 pos)
    {

        return new Vector2Int((int)pos.x, (int)pos.z);
    }
}
