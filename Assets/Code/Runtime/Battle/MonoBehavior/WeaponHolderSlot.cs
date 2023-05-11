using Code.Configuration;
using Code.Runtime.GameBase;
using UnityEngine;

namespace Code.Runtime.Battle.MonoBehavior
{
    public class WeaponHolderSlot : MonoBehaviour
    {
        public Transform parentOverride;
        public bool isLeftHandSlot;

        public GameObject currentWeaponModel;

        public void UnloadWeapon()
        {
            if (currentWeaponModel != null)
                currentWeaponModel.SetActive(false);
        }

        public void UnloadWeaponAndDestroy()
        {
            if (currentWeaponModel != null)
                Destroy(currentWeaponModel);
        }

        public void LoadWeaponModel(int weaponId)
        {
            UnloadWeaponAndDestroy();
            
            if (!WeaponConfig.D.ContainsKey(weaponId))
            {
                Debug.LogError("Weapon :" + weaponId + " does not exist.");
                UnloadWeapon();
                return;
            }

            var weaponConfig = WeaponConfig.D[weaponId];
            
            var model = GameManager.AssetManager.InitializeObject(weaponConfig.Prefab,
                parentOverride != null ? parentOverride : transform);
            if (model != null)
            {
                model.transform.localPosition = Vector3.zero;
                model.transform.localRotation = Quaternion.identity;
                model.transform.localScale = Vector3.one;
            }

            currentWeaponModel = model;
        }
    }
}