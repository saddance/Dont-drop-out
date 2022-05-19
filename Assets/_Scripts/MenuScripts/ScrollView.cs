using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScrollView : MonoBehaviour
{
    public RectTransform prefab;
    public RectTransform content;

    public void LoadSaves()
    {
        foreach (Transform child in content)
        {
            Destroy(child.gameObject);
        }
        foreach (var name in SavingManager.GetSaveNames())
        {
            var instance = Instantiate(prefab.gameObject, content).transform;
            instance.SetParent(content, false);
            InitializeItemView(instance, name);
        }
    }

    public void InitializeItemView(Transform viewGameObject, string name)
    {
        var clickButton = viewGameObject.Find("Button").GetComponent<Button>();
        clickButton.GetComponentInChildren<TMP_Text>().text = name;
        clickButton.onClick.AddListener(() =>
        {
            GameManager.LoadSavedGame(name);
        });
    }
}
