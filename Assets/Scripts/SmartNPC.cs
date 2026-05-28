using UnityEngine;
using LLMUnity;
using System.IO;
using System.Threading.Tasks;
using TMPro;
using Unity.VisualScripting;

public class SmartNPC : MonoBehaviour
{
    [Header("AI Components")]
    public LLMCharacter llmCharacter;  // твой NPC
    public RAG rag;                     // перетащи сюда RAG_System
    
    [Header("UI")]
    public TMP_InputField inputField;
    public TextMeshProUGUI aiResponseText;
    
    [Header("Knowledge Base")]
    public string loreFile = "lore.txt";  // файл в StreamingAssets
    
    private void Start()
    {
        if (inputField != null)
            inputField.onSubmit.AddListener(OnUserMessage);
        
        // Загружаем базу знаний при старте
        _ = InitializeRAG();
    }

    private async Task InitializeRAG()
    {
        
        aiResponseText.text = "Загружаю мудрость веков...";
        
        if (rag != null)
        {

            // Инициализируем RAG с настройками из Inspector
            // Вместо InitializeRAG с добавлением чанков
            if (File.Exists(Path.Combine(Application.streamingAssetsPath, "game_lore.zip")))
            {
                await rag.Load("game_lore.zip");
                aiResponseText.text = "Готов отвечать на твои вопросы!";
                Debug.Log("RAG загружен из кэша!");
                return;
            }

            // Load lore from StreamingAssets
            string lorePath = Path.Combine(Application.streamingAssetsPath, loreFile);
            
            if (File.Exists(lorePath))
            {
                string loreContent = File.ReadAllText(lorePath);
                
                // Разбиваем lore на логические части (по абзацам или предложениям)
                string[] loreChunks = loreContent.Split(new[] { "\n\n" }, System.StringSplitOptions.RemoveEmptyEntries);
                
                foreach (string chunk in loreChunks)
                {
                    if (!string.IsNullOrWhiteSpace(chunk))
                    {
                        await rag.Add(chunk, "lore");
                        Debug.Log($"RAG: добавлен чанк: {chunk.Substring(0, Mathf.Min(50, chunk.Length))}...");
                    }
                }
                
                Debug.Log($"RAG загружен: {loreChunks.Length} фрагментов");
                aiResponseText.text = "Готов отвечать на твои вопросы!";
            }
            else
            {
                Debug.LogWarning($"Lore file not found: {lorePath}");
                aiResponseText.text = "База знаний не найдена!";
            }
        }
        
        // Сохраняем embeddings для быстрой загрузки в следующий раз
        rag.Save("game_lore.zip");
        Debug.Log("RAG сохранен в StreamingAssets/game_lore.zip");
    }
    
    private async void OnUserMessage(string message)
    {
        if (string.IsNullOrWhiteSpace(message)) return;
        
        // Блокируем ввод
        inputField.interactable = false;
        aiResponseText.text = "Ищу ответ в древних свитках...";
        
        // Поиск по базе знаний
        string[] relevantLore = await SearchRelevantLore(message);
        
        // Строим промпт на основе найденного
        string prompt = BuildPromptWithContext(message, relevantLore);
        
        Debug.Log($"=== Сгенерированный промпт ===\n{prompt}\n==============================");
        
        // Отправляем запрос модели
        _ = llmCharacter.Chat(prompt, OnReplyReceived, OnReplyComplete);
    }
    
    private async Task<string[]> SearchRelevantLore(string question)
    {
        // Ищем 3 самых релевантных фрагмента
        (string[] results, float[] distances) = await rag.Search(question, 3, "lore");
        
        Debug.Log($"Поиск по '{question}': найдено {results.Length} фрагментов");
        for (int i = 0; i < results.Length; i++)
        {
            Debug.Log($"  [{i}] Score: {distances[i]:F2} | {results[i].Substring(0, Mathf.Min(60, results[i].Length))}");
        }
        
        return results;
    }
    
    private string BuildPromptWithContext(string question, string[] relevantLore)
    {
        string prompt = "Ты — мудрый волшебник Мерлин. Отвечай строго на основе provided lore.\n";
        prompt += "Если информация не найдена в lore, честно скажи, что не знаешь.\n\n";
        prompt += "=== Lore (только эта информация считается правдой) ===\n";
        
        foreach (string lore in relevantLore)
        {
            prompt += $"- {lore}\n";
        }
        
        prompt += "\n=== Диалог ===\n";
        prompt += $"Игрок спрашивает: {question}\n";
        prompt += "Твой ответ (от первого лица, как Мерлин):";
        
        return prompt;
    }
    
    private void OnReplyReceived(string reply)
    {
        aiResponseText.text = reply;
    }
    
    private void OnReplyComplete()
    {
        inputField.interactable = true;
        inputField.text = "";
        inputField.Select();
    }
}