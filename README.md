
# LLM for Unity – Dynamické NPC dialógy s veľkými jazykovými modelmi

Tento repozitár obsahuje prototyp Unity projektu, v ktorom sú integrované veľké jazykové modely (LLM) pre dynamické generovanie dialógov, úloh a popisov predmetov v hre. Hlavným cieľom je demonštrovať, ako možno v prostredí Unity lokálne využívať LLM (napr. Mistral 7B) bez potreby API.

---

## Inštalácia a spustenie

## Implementácia LLM for Unity

### 1. Stiahni asset

> **LLM for Unity**:  
> https://assetstore.unity.com/packages/tools/ai-ml-integration/llm-for-unity-273604

- Zaregistruj sa a stiahni balíček `.unitypackage`
- Importuj do projektu cez `Assets > Import Package > Custom Package...` alebo priamo cez Unity asset store.

### 2. Pridaj komponenty

- Vytvor 2 `Empty GameObject`, jeden s názvom `NPC`, druhý s názvom LLMManager
- Pridaj komponenty `LLM` do LLMManager a `LLMCharacter` do NPC
- Nastav:
  - **Prompt:** napr. `You are a merchant helping the player explore the world.`
  - **AIName:** Merchant
  - **PlayerName:** Hero
  - **Save:** Zapnúť, pre uchovanie histórie
  - **Model Settings:** Stiahnuť vybratý model

---

## Implementácia skriptov

### 1. NPCChat.cs, ChatUI.cs

- `NPCChat.cs`: Priradiť k objektu NPC
- `ChatUI.cs`: Vytvoriť UI Panel a vložiť skript `ChatUI.cs`, do vnútra vytvoriť InputField, Button a TMP Text. Na Button nastaviť OnClick() event a priradiť `ChatUI.cs` a nastaviť ChatUI.OnSendButtonClicked

### 2. Quest.cs, QuestManager.cs, QuestUI.cs

- `QuestManager.cs`: Vytvorit nový `Empty GameObject` a vložiť skript
- `QuestUI.cs`: Vytvoriť nový UI Panel a v ňom 3 TextMechPro komponenty

### 3. PlayerActionTracker.cs

- Priradiť na objekt hráča

### 4. InventoryUI.cs, ItemDescriptionUI.cs

- `ItemDescriptionUI.cs`: Vytvoriť nový UI Panel a vložiť tento skript + vytvoriť 2 nové TextMeshPro komponenty a 1 jeden Empty GameObject a priradiť mu Image komponent
- `InventoryUI.cs`: Vytvoriť nový UI Panel nastaviť mu GridLayout komponent a vložiť skript

### 5. CameController.cs

- `CameraController.cs`: Priradiť na kameru

---

## 📚 Dokumentácia

Viac informácií o fungovaní nájdete na oficiálnej stránke:  
👉 https://undream.ai/LLMUnity/group__llm.html

---

## Autor

Tento projekt bol vytvorený ako súčasť diplomovej práce na tému využitia LLM v počítačových hrách.  
Autor: **Boris Rosival**

