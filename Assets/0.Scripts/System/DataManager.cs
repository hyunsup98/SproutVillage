using System.Collections.Generic;
using UnityEngine;
using System.IO;

[System.Serializable]
public struct DicItem
{
    public int key;     //슬롯 인덱스
    public int id;      //아이템 아이디
    public int count;   //아이템 개수
}

[System.Serializable]
public class SaveData
{
    public List<DicItem> inventorySlots = new List<DicItem>();

    public int gold;
}

public class DataManager : Singleton<DataManager>
{
    [SerializeField] private List<Item> items = new List<Item>();

    private string originPath;
    private string gameDataPath;

    private void Start()
    {
        //경로 지정
        originPath = Path.Combine(Application.dataPath + "/Save/");
        gameDataPath = Path.Combine(originPath, "gameData.json");
    }

    public void DataSave()
    {
        SaveData saveData = new SaveData();

        //인벤토리 슬롯을 차례대로 검사
        for (int i = 0; i < UIManager.Instance.InvenScripts.slots.Count; i++)
        {
            //해당 슬롯에 아이템이 있다면 SaveData에 저장
            if (UIManager.Instance.InvenScripts.slots[i].SlotItem != null)
            {
                DicItem dic = new DicItem();
                dic.key = i;
                dic.id = UIManager.Instance.InvenScripts.slots[i].SlotItem.itemData.ItemId;
                dic.count = UIManager.Instance.InvenScripts.slots[i].SlotItem.Count;
                saveData.inventorySlots.Add(dic);
            }
        }

        saveData.gold = GameManager.Instance.Player.Gold;

        UTFFromString(saveData, gameDataPath);
    }

    public void DataLoad()
    {
        SaveData saveData = new SaveData();

        if(File.Exists(gameDataPath))
        {
            StringFromUTF(ref saveData, gameDataPath);

            if(saveData != null)
            {
                if(saveData.inventorySlots.Count > 0)
                {
                    foreach(var dic in saveData.inventorySlots)
                    {
                        for(int i = 0; i < items.Count; i++)
                        {
                            if(dic.id == items[i].itemData.ItemId)
                            {
                                Item item = ItemPool.Instance.GetObjects(items[i], ItemPool.Instance.transform);
                                item.Count = dic.count;
                                UIManager.Instance.InvenScripts.slots[dic.key].UpdateSlotData(item);
                            }
                        }
                    }
                }

                GameManager.Instance.Player.Gold = saveData.gold;
            }
        }
    }

    private void UTFFromString<T> (T Data, string path)
    {
        string[] pathSplit = path.Split('/');
        string curPath = string.Empty;

        //path 경로에 폴더/파일이 있는지 체크 후 없다면 직접 폴더/파일을 생성
        for (int i = 0; i < pathSplit.Length; i++)
        {
            curPath += pathSplit[i];

            //폴더가 없다면 생성, 마지막 주소를 제외하는 이유는 마지막 주소는 파일명이 들어있기 때문(폴더가 아니라서)
            if (i < pathSplit.Length - 1)
            {
                if(!Directory.Exists(curPath))
                {
                    Directory.CreateDirectory(curPath);
                }
                curPath += "/";
            }
            else
            {
                if(!File.Exists(curPath))
                {
                    FileStream file = File.Create(curPath);

                    //파일 생성 직후 파일이 열려있는 상태이기 때문에 수정하기 위해서 먼저 닫아줘야함
                    file.Close();
                }
            }
        }

        string json = JsonUtility.ToJson(Data, true);
        byte[] bytes = System.Text.Encoding.UTF8.GetBytes(json);
        string encodeJson = System.Convert.ToBase64String(bytes);
        File.WriteAllText(path, encodeJson);
    }

    private void StringFromUTF<T>(ref T Data, string path)
    {
        string json = File.ReadAllText(path);
        byte[] bytes = System.Convert.FromBase64String(json);
        string decodeJson = System.Text.Encoding.UTF8.GetString(bytes);
        Data = JsonUtility.FromJson<T>(decodeJson);
    }
}
