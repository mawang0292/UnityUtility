using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnMaster : BaseSingleton<SpawnMaster>, IBaseSingleton
{
    private ObjectPool objectPool;

    public void OnCreateInstance()
    {
        Initialize();
    }

    public void Initialize()
    {
        objectPool = new ObjectPool();

        var keyCollection = ResourceManager.Instance.GetPrefabKeyCollection();
        foreach (var key in keyCollection)
        {
            objectPool.Regist(key, 0);
        }
    }


  

    public void OnDestroyInstance()
    {
        Release();
    }

    public void Release()
    {
        if (objectPool != null)
        {
            objectPool = null;
        }
    }

    public static bool TrySpawnMonoBehaviour<T>(string key, Vector3 pos, Quaternion rot, out T obj) where T : MonoBehaviour
    {
        var gobj = Instance.objectPool.Instantiate(key, pos, rot);
        if (gobj == null)
        {
            obj = null;
            return false;
        }

        obj = gobj.GetComponent<T>();
        return (obj != null);
    }

    public static bool TrySpawnObject<T>(string key, Vector3 pos, Quaternion rot, out T obj) where T : Object
    {
        var gobj = Instance.objectPool.Instantiate(key, pos, rot);
        if (gobj == null)
        {
            obj = null;
            return false;
        }

        obj = gobj.GetComponent<T>();
        return (obj != null);
    }

    public static bool TrySpawnUI<T>(string key, Transform parentTf, out T obj) where T : UIBase
    {
        var gobj = Instance.objectPool.Instantiate(key, new Vector3(0, 0, -1000), Quaternion.identity);
        if (gobj == null)
        {
            obj = null;
            return false;
        }

        obj = gobj.GetComponent<T>();
        if (obj == null) return false;

        obj.transform.SetParent(parentTf);
        obj.rtf.localScale = Vector3.one;
        obj.rtf.rotation = Quaternion.identity;
        return true;
    }

    public static bool TrySpawnGridBlock<T>(string key, Vector3 pos, Quaternion rot, out T obj) where T : GridBlock
    {
        var gobj = Instance.objectPool.Instantiate(key, pos, rot);
        if (gobj == null)
        {
            obj = null;
            return false;
        }

        obj = gobj.GetComponent<T>();
        if (obj != null)
        {
            obj.Initialize();
            return true;
        }
        else
        {
            return false;
        }
    }

    public static bool TrySpawnFx<T>(string key, Vector3 pos, Quaternion rot, out T obj) where T : FxBase
    {
        var gobj = Instance.objectPool.Instantiate(key, pos, rot);
        if (gobj == null)
        {
            obj = null;
            return false;
        }

        obj = gobj.GetComponent<T>();
        return (obj != null);
    }


    #region < Spawn Unit >

    private static double UnitIDIdx = 0;
    public static double CreateUnitIDIdx()
    {
        if (UnitIDIdx >= double.MaxValue)
        {
            UnitIDIdx = 0;
        }
        return UnitIDIdx++;
    }

    public static bool TrySpawnMonster(string unitId, int level, Vector3 pos, Quaternion rot, out Monster obj, Transform hpUILayerRf = null, float abilityRatio = 1)
    {
        v2.UnitData data;
        UnitHpUI hpUI = null;
        if (TrySpawnMonoBehaviour(unitId, pos, rot, out obj) && v2.DataManager.unitDataStorage.TryGetUnitData(unitId, out data))
        {
            if (hpUILayerRf != null && TrySpawnUI("UnitHpUI", hpUILayerRf, out hpUI))
            {
                obj.gameObject.name = string.Format("{0}_{1}", unitId, CreateUnitIDIdx());
                obj.Initialize();
                obj.SetData(data, Unit.EOwnerType.Enemy, hpUI, level);
                obj.SetMonsterData();
                obj.Ready();
            }
            else
            {
                obj.gameObject.name = string.Format("{0}_{1}", unitId, CreateUnitIDIdx());
                obj.Initialize();
                obj.SetData(data, Unit.EOwnerType.Enemy, null, level);
                obj.SetMonsterData();
                obj.Ready();
            }

            obj.equipment.EquipItem(EquipSlotType.MainWeapon, new IngameItemInfo(Global.CreateGUID(), GameSettingManager.defaultCharacterWeaponId, 1,0,0));
            obj.equipment.EquipItem(EquipSlotType.SubWeapon, new IngameItemInfo(Global.CreateGUID(), GameSettingManager.defaultCharacterWeaponId, 1,0,0));

            return true;
        }
        return false;
    }

    public static bool TrySpawnTower(string unitId, int level, Unit.EOwnerType ownerType, Vector3 pos, Quaternion rot, out Tower obj, Transform hpUILayerRf = null )
    {
        v2.UnitData data;
        UnitHpUI hpUI = null;
        if (TrySpawnMonoBehaviour(unitId, pos, rot, out obj) && v2.DataManager.unitDataStorage.TryGetUnitData(unitId, out data))
        {
            if (hpUILayerRf != null && TrySpawnUI("UnitHpUI", hpUILayerRf, out hpUI))
            {
                obj.gameObject.name = string.Format("{0}_{1}", unitId, CreateUnitIDIdx());
                obj.Initialize();
                obj.SetData(data, ownerType, hpUI, level);
                obj.Ready();
            }
            else
            {
                obj.gameObject.name = string.Format("{0}_{1}", unitId, CreateUnitIDIdx());
                obj.Initialize();
                obj.SetData(data, ownerType, null, level);
                obj.Ready();
            }


            // 기본 장비 장착   
            obj.equipment.EquipItem(EquipSlotType.MainWeapon, new IngameItemInfo(Global.CreateGUID(), GameSettingManager.defaultCharacterWeaponId, 1,0,0));
            obj.equipment.EquipItem(EquipSlotType.SubWeapon, new IngameItemInfo(Global.CreateGUID(), GameSettingManager.defaultCharacterWeaponId, 1,0,0));


            return true;
        }
        return false;
    }

    public static bool TrySpawnSubTower(string unitId, int level, float duration, Vector3 pos, Quaternion rot, out SubTower obj, Unit owner, Transform hpUILayerRf = null)
    {
        v2.UnitData data;
        UnitHpUI hpUI = null;
        if (TrySpawnMonoBehaviour(unitId, pos, rot, out obj) && v2.DataManager.unitDataStorage.TryGetUnitData(unitId, out data))
        {
            if (hpUILayerRf != null && TrySpawnUI("UnitHpUI", hpUILayerRf, out hpUI))
            {
                obj.gameObject.name = string.Format("{0}_{1}", unitId, CreateUnitIDIdx());
                obj.Initialize();
                obj.SetData(data, owner.ownerType, hpUI, level);
                obj.SetSubTowerData(owner, duration);
                obj.Ready();
            }
            else
            {
                obj.gameObject.name = string.Format("{0}_{1}", unitId, CreateUnitIDIdx());
                obj.Initialize();
                obj.SetData(data, owner.ownerType, null, level);
                obj.SetSubTowerData(owner, duration);
                obj.Ready();
            }

            // 기본 장비 장착
            obj.equipment.EquipItem(EquipSlotType.MainWeapon, new IngameItemInfo(Global.CreateGUID(), GameSettingManager.defaultCharacterWeaponId, 1,0,0));
            obj.equipment.EquipItem(EquipSlotType.SubWeapon, new IngameItemInfo(Global.CreateGUID(), GameSettingManager.defaultCharacterWeaponId, 1,0,0));

            return true;
        }
        return false;
    }

    #endregion

    public static void Destroy(GameObject obj, string key)
    {
        Instance.objectPool.Destroy(obj, key);
    }
}
