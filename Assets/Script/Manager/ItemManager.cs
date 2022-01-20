using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
struct ItemSlot
{
    public Image Icon;
    public TextMeshProUGUI TMP;
}

[System.Serializable]
struct InfoView
{
    public RectTransform Parent;
    public Image Icon;
    public TextMeshProUGUI Name;
    public TextMeshProUGUI Desc;
    public Button Release;
    public Button Sell;
    public Button Close;
}

public class ItemManager : MonoBehaviour
{
    public static ItemManager Instance { get; private set; } = null;

    public RectTransform ItemContent;

    [SerializeField] ItemView ItemViewPrefab;
    [SerializeField] Image SelectUnitImg;
    [SerializeField] InfoView InfoView;
    [SerializeField] ItemSlot[] ItemSlots = new ItemSlot[2];
    [SerializeField] Button CloseWindow;
    [SerializeField] Button[] OpenWindow = new Button[2];

    static Item Select;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        SelectUnitImg.sprite = GameManager.SelectUnit.Info.Icon;
        SelectUnitImg.SetNativeSize();
        if (ButtonActions.Instance.CheckReEntering("D-03_UnitItemInfo"))
        {
            InfoView.Close.onClick.AddListener(() => InfoView.Parent.gameObject.SetActive(false));

            CloseWindow.onClick.AddListener(() =>
            {
                InfoView.Parent.gameObject.SetActive(false);
            });

            InfoView.Release.onClick.AddListener(() =>
            {
                var find = FindInGameManager(Select);
                if (GameManager.SelectUnit.Items.Contains(find)) Release(Select);
                else Equip(find);
                if (find.Owner) InfoView.Release.image.sprite = UIManager.Instance.EnquipSwitchSprite.ASprite;
                else InfoView.Release.image.sprite = UIManager.Instance.EnquipSwitchSprite.BSprite;
            });

            foreach (var item in GameManager.Instance.Items)
            {
                ItemView obj = Instantiate(ItemViewPrefab);
                obj.Item = item;
                obj.GetComponent<RectTransform>().SetParent(ItemContent, false);
                obj.Btn.onClick.AddListener(() =>
                {
                    Select = obj.Item;
                    if (obj.Item.Owner && GameManager.SelectUnit.Info.Name == obj.Item.Owner.Info.Name) InfoView.Release.image.sprite = UIManager.Instance.EnquipSwitchSprite.ASprite;
                    else InfoView.Release.image.sprite = UIManager.Instance.EnquipSwitchSprite.BSprite;
                    ActiveInfoView();
                });
            }
        }

        Refresh();
    }

    public void Refresh()
    {
        var selected = GameManager.SelectUnit;

        if (selected != null)
        {
            for (int i = 0; i < 2; i++)
            {
                var itemslot = ItemSlots[i];

                if (selected.Items.Count > i && selected.Items[i] != null)
                {
                    var item = selected.Items[i];

                    if (item.Icon)
                    {
                        itemslot.Icon.color = Color.white;
                        itemslot.Icon.sprite = item.Icon;
                    }
                    itemslot.TMP.text = $"{item.Desc}";
                }
                else
                {
                    itemslot.Icon.color = Color.clear;
                    itemslot.Icon.sprite = null;
                    itemslot.TMP.text = $"";
                }
            }
        }
    }

    public void ActiveInfoView()
    {
        if (Select.Icon) InfoView.Icon.sprite = Select.Icon;
        InfoView.Name.text = $"{Select.Name}";
        InfoView.Desc.text = $"{Select.Desc}";

        InfoView.Parent.gameObject.SetActive(true);
    }

    public void Equip(Item item)
    {
        print("E");
        if (GameManager.SelectUnit.Items.Count < 3 && !GameManager.SelectUnit.Items.Contains(item))
        {
            print("E IN");
            item.Owner = GameManager.SelectUnit;
            GameManager.SelectUnit.Items.Add(item);
            // 소환할때 Owner 실제 obj의 Unit Instnce로 갱신 필요
            item.Equip();
            Refresh();
        }
    }

    public void Release(Item item)
    {
        print("R");
        item = GameManager.SelectUnit.Items.Find((o) => { return o.Name == item.Name; });
        item.Owner = null;
        Refresh();
    }

    public Item FindInGameManager(Item item) => GameManager.Instance.Items.Find((o) => { return o.Name == item.Name; });
}
