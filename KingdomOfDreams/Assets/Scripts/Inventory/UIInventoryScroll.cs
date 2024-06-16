using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIInventoryScroll : MonoBehaviour
{
    public Transform content;
    public GameObject itemCellPrefab;

    public void Start()
    {
        
    }
    public void Init()
    {
        for (int i = 0; i < InfoManager.instance.IngredientInfos.Count; i++)
        {
            var go = Instantiate(this.itemCellPrefab, this.content);
            var itemCell = go.GetComponent<UIInventoryCell>();

            //id, 아이콘, 수량
            //itemCell.Init();
            var info = InfoManager.instance.IngredientInfos[i];
            var data = DataManager.instance.GetIngredientData(info.id);
            var atlas = AtlasManager.instance.GetAtlasByName("inventoryItem");
            var sprite = atlas.GetSprite(data.sprite_name);
            var amount = info.amount;
            itemCell.Init(info.id, data.name, sprite, amount);

        }

        EventDispatcher.instance.AddEventHandler((int)LHMEventType.eEventType.REFRESH_UI_INVENTORY, new EventHandler((type) =>
        {
            this.Refresh();
        }));
    }

    public void Refresh()
    {
        if (this.content != null)
        {
            foreach (Transform child in this.content)
            {
                Destroy(child.gameObject);
            }

            for (int i = 0; i < InfoManager.instance.IngredientInfos.Count; i++)
            {
                var go = Instantiate(this.itemCellPrefab, this.content);
                var itemCell = go.GetComponent<UIInventoryCell>();

                var info = InfoManager.instance.IngredientInfos[i];
                var data = DataManager.instance.GetIngredientData(info.id);
                var atlas = AtlasManager.instance.GetAtlasByName("inventoryItem");
                var sprite = atlas.GetSprite(data.sprite_name);
                var amount = info.amount;
                itemCell.Init(info.id, data.name, sprite, amount);

            }
        }
    }
}
