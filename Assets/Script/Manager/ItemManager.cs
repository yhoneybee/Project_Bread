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

    [SerializeField] RectTransform ItemContent;
    [SerializeField] InfoView InfoView;
    [SerializeField] ItemSlot[] ItemSlots = new ItemSlot[2];

    static Item Select;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        InfoView.Close.onClick.AddListener(() => InfoView.Parent.gameObject.SetActive(false));

        foreach (var item in GameManager.Instance.Items)
        {
            var obj = Instantiate(item);
            obj.GetComponent<RectTransform>().SetParent(ItemContent, false);
            obj.GetComponent<Button>().onClick.AddListener(() =>
            {
                Select = obj;
                ActiveInfoView();
            });
        }

        Refresh();
    }

    public void Refresh()
    {
        var selected = GameManager.SelectUnit;

        if (selected != null)
        {
            for (int i = 0; i < selected.Items.Count; i++)
            {
                var itemslot = ItemSlots[i];
                var item = selected.Items[i];

                itemslot.Icon.sprite = item.Icon;
                itemslot.TMP.text = $"{item.Desc}";
            }
        }
    }

    public void ActiveInfoView()
    {


        InfoView.Parent.gameObject.SetActive(true);
    }

    public void Equip(Item item)
    {
        if (GameManager.SelectUnit.Items.Count < 3 && !GameManager.SelectUnit.Items.Contains(item))
        {
            GameManager.SelectUnit.Items.Add(item);
            item.Owner = GameManager.SelectUnit;
            // 소환할때 Owner 실제 obj의 Unit Instnce로 갱신 필요
            Refresh();
        }
    }

    public void Release(Item item)
    {
        GameManager.SelectUnit.Items.Remove(item);
        item.Owner = null;
        Refresh();
    }
}
