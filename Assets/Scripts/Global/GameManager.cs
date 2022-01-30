using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    private struct Constants
    {
        public static string filePathName = "/player.json";
    }
    
    public static GameManager Instance;
    public Player player = new Player();
    private String currentPlayerName;
    
    public InputField playerField;
    public Text titleText;
    
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        
        LoadPlayer();
    }
    
    [Serializable]
    public class Player
    {
        public String name;
        public int score;
    }

    public void SaveCurrentPlayerName()
    {
        currentPlayerName = playerField.text;
    }
    
    public void SavePlayer()
    {
        // we should to update best player name
        player.name = currentPlayerName;

        var json = JsonUtility.ToJson(player);
        File.WriteAllText(Application.persistentDataPath + Constants.filePathName, json);
    }

    public void LoadPlayer()
    {
        var path = Application.persistentDataPath + Constants.filePathName;
        if (File.Exists(path))
        {
            var json = File.ReadAllText(path);
            var savedPlayer = JsonUtility.FromJson<Player>(json);

            player.name = savedPlayer.name;
            player.score = savedPlayer.score;

            // prepare field name
            playerField.text = player.name;
        }
        else
        {
            SetupDefaultPlayer();
        }

        UpdateTitleText();
    }

    private void UpdateTitleText()
    {
        titleText.text = $"Best Score: {player.name} -> {player.score}";
    }

    private void SetupDefaultPlayer()
    {
        player.name = playerField.text ?? "Unknown";
        player.score = 0;
        
        SavePlayer();
    }

    private void ClearData()
    {
        string[] filePaths = Directory.GetFiles(Application.persistentDataPath); 
        foreach (string filePath in filePaths) File.Delete(filePath);
    }
}
