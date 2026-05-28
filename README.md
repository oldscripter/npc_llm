# 🧙 NPC LLM – Gandalf Dialogue Prototype

**First steps & research of LLM integration into an NPC dialogue system in Unity.**

This project is a minimal but expressive proof-of-concept: an NPC (Gandalf) who does nothing but **talk** – powered by a local Large Language Model. No combat, no pathfinding, just conversation.

The goal is to demonstrate a foundation for integrating LLMs into game characters, with future plans to add **RAG (Retrieval-Augmented Generation)** for lore-accurate responses.

## 🎯 Purpose

- Show a working Unity–LLM pipeline for NPC dialogue.
- Provide a starting point for game developers interested in generative AI characters.
- Serve as a base for future RAG integration (Tolkien lore, quest knowledge, memory).

## 🧠 How It Works

1. Player interacts with Gandalf (e.g., via UI input or console).
2. The question is sent to a configured LLM (local or API).
3. Gandalf responds in-character, using a system prompt that defines his personality.
4. The reply appears as speech in the Unity scene.

*No external data store or vector database yet – pure LLM improvisation.*

## 🕹️ Current State

| Feature         | Status               |
|----------------|----------------------|
| Movement / Abilities | ❌ Not implemented |
| Dialogue Input       | ✅ Basic integration |
| LLM Response         | ✅ Working           |
| RAG / Lore Memory    | ⏳ Planned           |

## 🛠️ Tech Stack

- **Unity** (ShaderLab, HLSL, C#)
- **LLM** (local – ready for Ollama, LM Studio, or OpenAI‑compatible API)
- **Serialization / HTTP client** in Unity

## 📂 Project Structure

```
Assets/               # Unity assets, scripts, shaders
Packages/             # Unity package dependencies
ProjectSettings/      # Unity project settings
NPC LLM.slnx          # Solution file
.gitignore
```

## 🚀 Getting Started

1. Clone the repository.
2. Open the project in Unity (2022.3 LTS or newer recommended).
3. Download LLM (click 'Download model' in LLM_Core object) - I've used `Mistral 7B Instruct v0.2`
4. Set up your LLM endpoint in the configuration (see `LLMConfig` script or inspector).
5. Run the scene and talk to Gandalf.

> *Note: pay attention to p.3 - you'll need a local LLM server (e.g., Ollama with `mistral` or `llama3`) or an API key for a cloud provider.*

## 🔮 Roadmap

- [x] Basic LLM dialogue
- [ ] RAG pipeline with lore chunking & embedding
- [ ] Persistent character memory
- [ ] Conditional dialogue (based on game state)
- [ ] Speech bubble / voice synthesis (optional)

## 🤝 Contributing

Ideas, issues, and PRs are welcome – especially from people working on LLMs in game dev.

---

*“Even the very wise cannot see all ends.” – Gandalf*  
But with RAG, they might get closer.
