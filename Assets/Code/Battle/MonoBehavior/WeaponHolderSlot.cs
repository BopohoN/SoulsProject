using Code.Battle.Information;
using UnityEngine;

namespace Code.Battle.MonoBehavior
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
        
        public void LoadWeaponModel(WeaponItem item)
        {
            if (item == null)
            {
                UnloadWeapon();
                return;
            }

            var model = Instantiate(item.modelPrefab, parentOverride != null ? parentOverride : transform);
            if (model!= null)
            {
                model.transform.localPosition = Vector3.zero;
                model.transform.localRotation = Quaternion.identity;
                model.transform.localScale = Vector3.one;
            }

            currentWeaponModel = model;
        }
    }
}
