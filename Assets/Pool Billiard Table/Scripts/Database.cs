using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Database : MonoBehaviour {

	public string[] items;
	// Use this for initialization
	IEnumerator Start () {
		WWW usersData = new WWW ("http://localhost:8080/BilliardsBuddy/billiardsusers.php");
		yield return usersData;
		string usersDataString = usersData.text;
		print (usersDataString);
		items = usersDataString.Split (';');
		print(GetDataValue(items[0],"User_Id:"));

	}

	string GetDataValue (string data, string index){
	
		string value = data.Substring (data.IndexOf (index) + index.Length);
		if (value.Contains ("|"))value = value.Remove (value.IndexOf ("|"));
		return value;
	}
		
}