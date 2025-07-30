using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    // 库存字典
    public Dictionary<string, int> inventory = new();

    // 显示库存的 Text（场景里物体需打 Tag = InventoryText）
    public TextMeshProUGUI uiText;

    // ====== 可在 Inspector 调整的“开局初始数量” ======
    [Header("Initial Amount Per Item")]
    [SerializeField] int initMango = 1;
    [SerializeField] int initCoconut = 1;
    [SerializeField] int initGrape = 1;
    [SerializeField] int initIce = 1;

    // 避免重复初始化
    bool dataInitialized = false;

    // ---------- 生命周期 ----------
    void Awake()
    {
        // 单例防止重复
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        // 仅第一次 Awake 初始化数据
        if (!dataInitialized)
        {
            ResetInventory();
            dataInitialized = true;
        }

        SceneManager.sceneLoaded += OnSceneLoaded;

        RebindUIText();
        UpdateUI();
    }

    void OnDestroy() => SceneManager.sceneLoaded -= OnSceneLoaded;

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // 若进入的是游戏关卡场景，则重置库存
        if (scene.name.StartsWith("Test"))      // 如需改名，修改此条件
            ResetInventory();

        RebindUIText();
        UpdateUI();
    }

    // ---------- 公共 API ----------
    public int AddItem(string name)
    {
        Modify(name, +1);
        return GetCount(name);
    }

    public bool UseItem(string name) => Modify(name, -1);

    public int GetCount(string n)
    {
        inventory.TryGetValue(Norm(n), out var v);
        return v;
    }

    public bool HasItem(string n) => GetCount(n) > 0;

    // ---------- 私有实现 ----------
    string Norm(string s) => string.IsNullOrWhiteSpace(s) ? "" : s.Trim().ToLowerInvariant();

    bool Modify(string name, int delta)
    {
        string k = Norm(name);
        if (k == "") return false;

        inventory.TryAdd(k, 0);
        if (inventory[k] + delta < 0) return false;  // 不够用

        inventory[k] += delta;
        UpdateUI();
        return true;
    }

    void ResetInventory()
    {
        inventory.Clear();
        inventory["mango"] = initMango;
        inventory["coconut"] = initCoconut;
        inventory["grape"] = initGrape;
        inventory["ice"] = initIce;
    }

    void RebindUIText()
    {
        if (uiText != null) return;  // 已绑定无需再找
        var go = GameObject.FindGameObjectWithTag("InventoryText");
        if (go) uiText = go.GetComponent<TextMeshProUGUI>();
    }

    public void UpdateUI()
    {
        if (uiText == null) return;
        uiText.text =
            $"Mango:   {GetCount("mango")}\n" +
            $"Coconut: {GetCount("coconut")}\n" +
            $"Grape:   {GetCount("grape")}\n" +
            $"Ice:     {GetCount("ice")}";
    }
}

