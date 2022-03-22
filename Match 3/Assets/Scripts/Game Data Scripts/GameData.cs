using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

[Serializable]
public class SaveData {
    public bool[] isActive;
    public int[] highScores;    
    public int[] stars;

}

public class GameData : MonoBehaviour {
    public static GameData gameData;
    public SaveData saveData;


    void Awake() {
        if (gameData == null) {
            // 某物体创建之后不再随场景的切换而销毁
            Debug.Log("Dont destroy gameobject of game data");
            DontDestroyOnLoad(this.gameObject);
            gameData = this;
        } else {
            Debug.Log("Destroy gameobject of game data");
            Destroy(this.gameObject);
        }
        Load();
    }

    void Start() {
        
    }
    /// <summary>
	/// 保存数据
	/// </summary>
    void Save() {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream file = File.Open(Application.persistentDataPath + "/player.dat", FileMode.Create);
        SaveData data = new SaveData();
        data = saveData;
        formatter.Serialize(file, data);
        file.Close();
        Debug.Log("Saved");
    }
    /// <summary>
	/// 加载数据
	/// </summary>
    public void Load() {
        if (File.Exists(Application.persistentDataPath + "/player.dat")) {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/player.dat", FileMode.Open);
            saveData = formatter.Deserialize(file) as SaveData;
            file.Close();
            Debug.Log("Loaded");
        }
    }

    private void OnDisable() {
        Debug.Log("OnDisable");
        Save();
    }
}
    
