using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Init : MonoBehaviour
{
    [SerializeField] private Transform characterContainer;
    void Start()
    {
        GameObject selectedCharacter = CharacterSelect.selectedCharacter;
        if (selectedCharacter != null)
        {
            GameObject instantiatedCharacter = Instantiate(selectedCharacter, characterContainer);
        }
    }
}