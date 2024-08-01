using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class CharacterSelect : MonoBehaviour
{
    private int index;
    [SerializeField] GameObject characters;
    [SerializeField] Text characterName;
    [SerializeField] GameObject[] characterPrefab;
    private GameObject currentCharacter;
    public static GameObject selectedCharacter;
    void Start()
    {
        index = PlayerPrefs.GetInt("SelectedCharacterIndex", 0);
        SelectCharacter();
    }
    public void OnPlayClick()
    {
        PlayerPrefs.SetInt("SelectedCharacterIndex", index);
        PlayerPrefs.Save();
        Time.timeScale = 1;
        SceneManager.LoadScene("Game_Spline");
    }
    public void OnSelectClick()
    {
        PlayerPrefs.SetInt("SelectedCharacterIndex", index);
        PlayerPrefs.Save();
        selectedCharacter = characterPrefab[index];
    }
    public void OnPrevClick()
    {
        if (index <= 0) { index = characterPrefab.Length - 1;}
        else { index--; }
        SelectCharacter();
    }

    public void OnNextClick()
    {
        if (index >= characterPrefab.Length - 1){ index = 0; }
        else { index++; }
        SelectCharacter();
    }
    public void SelectCharacter()
    {
        if (currentCharacter != null) { Destroy(currentCharacter); }
        GameObject prefab = characterPrefab[index];
        currentCharacter = Instantiate(prefab, characters.transform);
        currentCharacter.transform.localPosition = new Vector3(0, transform.localPosition.y, 0);
        selectedCharacter = prefab;
        characterName.text = prefab.name;
        SpriteRenderer spriteRenderer = currentCharacter.GetComponent<SpriteRenderer>();
        if (spriteRenderer != null) { spriteRenderer.color = Color.white; }
    }
}
