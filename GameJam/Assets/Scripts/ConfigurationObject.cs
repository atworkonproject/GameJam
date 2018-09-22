using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//to access use static field ConfigurationController.Configutation

[CreateAssetMenu(menuName = "My Assets/ConfigurationObject")]
public class ConfigurationObject : ScriptableObject {
	public float maxPlayerCredits = 200;
}

