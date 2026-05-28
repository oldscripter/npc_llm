using UnityEngine;
using LLMUnity;
using TMPro; // используем TextMeshPro для красивого текста

public class SimpleNPC : MonoBehaviour
{
    [Header("AI")]
    public LLMCharacter llmCharacter; // перетащи сюда SimpleNPC
    
    [Header("UI")]
    public TMP_InputField inputField;
    public TextMeshProUGUI aiResponseText;
    
    private void Start()
    {
        if (inputField != null)
        {
            inputField.onSubmit.AddListener(OnUserMessage);
            inputField.Select();
        }
        
        if (aiResponseText != null)
            aiResponseText.text = "Hmm?";
    }
    
    private void OnUserMessage(string message)
    {
        if (string.IsNullOrWhiteSpace(message)) return;
        
        // Показываем вопрос в UI (опционально)
        Debug.Log($"Hero: {message}");
        
        // Блокируем ввод на время ответа
        inputField.interactable = false;
        aiResponseText.text = "...";
        
        // Отправляем запрос модели
        _ = llmCharacter.Chat(message, OnReplyReceived, OnReplyComplete);
    }
    
    private void OnReplyReceived(string reply)
    {
        // Обновляем текст по мере генерации (стриминг)
        aiResponseText.text = reply;
        Debug.Log($"Gandalf: {reply}");
    }
    
    private void OnReplyComplete()
    {
        // Разблокируем ввод и очищаем поле
        inputField.interactable = true;
        inputField.text = "";
        inputField.Select();
    }
}