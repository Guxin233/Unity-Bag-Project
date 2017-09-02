using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class VendorSlot : Slot {

    public override void OnPointerDown(PointerEventData eventData)
    {
        return; // 覆盖掉父类的方法。小贩只能右键购买物品
    }
}
