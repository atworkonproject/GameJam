using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfigController : MonoBehaviour {

	public static ConfigurationObject Config;
	public ConfigurationObject ObjectToLink;
	public void Start()
	{
		ConfigController.Config = ObjectToLink;
	}
}
