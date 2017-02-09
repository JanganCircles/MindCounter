using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIDescription : MonoBehaviour {
    CharacterDescription CharDes;
    public int Index = 0;
    public Text txtName;
    public Text txtDes;
	// Use this for initialization
    void Awake(){
        CharDes = new CharacterDescription();
    }
	
	// Update is called once per frame
	void Update () {
        txtName.text = CharDes.CharacterName[Index];
        txtDes.text = CharDes.CharacterStr[Index];
	}
}
