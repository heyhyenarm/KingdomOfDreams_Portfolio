using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NHRCharacterPlayer : MonoBehaviour
{
    public GameObject nowCharacter;
    public GameObject preCharacter;
    public List<GameObject> characters;
    public List<GameObject> myCharacters;

    public GameObject savedCharacter;

    void Start()
    {
        this.myCharacters.Add(this.nowCharacter);

    }

}
