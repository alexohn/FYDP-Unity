using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class menuScript : MonoBehaviour
{

    public Canvas loginMenu;
    public Canvas userMenu;
    public Canvas CreateUserFail;
    public Canvas CreateUserSuccess;
    public Canvas EnterUserFail;
    public Canvas EnterUserSuccess;
    //public Button login; //login button
    public Button startText; //play as guest
    public Button scores; //database interaction
    public string UserNameInput;
    public string UserNameInput2;
	public static string UserName;
    public string CreateUserURL = "localhost:8080/BilliardsBuddy/InserUser.php";
    public string CheckUserURL = "localhost:8080/BilliardsBuddy/CheckUser.php";
    //public Button CreateUserBtn;
    //public bool CreateBtnisClicked;

    // Use this for initialization
    void Start()
    {

        loginMenu = loginMenu.GetComponent<Canvas>();
        userMenu = userMenu.GetComponent<Canvas>();
        CreateUserFail = CreateUserFail.GetComponent<Canvas>();
        CreateUserSuccess = CreateUserSuccess.GetComponent<Canvas>();
        EnterUserFail = EnterUserFail.GetComponent<Canvas>();
        EnterUserSuccess = EnterUserSuccess.GetComponent<Canvas>();
        startText = startText.GetComponent<Button>();
        scores = scores.GetComponent<Button>();
        loginMenu.enabled = false;
        userMenu.enabled = false;
        CreateUserFail.enabled = false;
        CreateUserSuccess.enabled = false;
        EnterUserFail.enabled = false;
        EnterUserSuccess.enabled = false;
        //		CreateUserBtn = CreateUserBtn.GetComponent<Button> ();
        //		CreateUserBtn.onClick.AddListener (CreatePress);
        //		CreateBtnisClicked = false;

    }

    public void LoginPress()
    {
        loginMenu.enabled = true;
        userMenu.enabled = false;
        startText.enabled = false;
        scores.enabled = false;

    }

    public void userPress()
    {
        loginMenu.enabled = false;
        userMenu.enabled = true;
        startText.enabled = false;
        scores.enabled = false;

    }

    public void CreatePress()
    {
		if (UserNameInput != "") {
			StartCoroutine (CheckAndCreateUser (UserNameInput, CreateUserFail, CreateUserSuccess));
		} 
		else {
			CreateUserFail.enabled = true;
		}
        
    }

    public void EnterPress()
    {
		if (UserNameInput2 != "") {
			StartCoroutine (CheckUser (UserNameInput2, EnterUserFail, EnterUserSuccess));
		} 
		else {
			EnterUserFail.enabled = true;
		}

    }

	public void LoginSuccess() 
	{
		EnterUserSuccess.enabled = false;
		UserName = UserNameInput2;
		SceneManager.LoadScene("Pool table");
	}

    public void NoPress()
    {
        loginMenu.enabled = false;
        userMenu.enabled = false;
        CreateUserFail.enabled = false;
        CreateUserSuccess.enabled = false;
        EnterUserFail.enabled = false;
        startText.enabled = true;
        scores.enabled = true;

    }

    public void PlayAsGuest()
    {
        SceneManager.LoadScene("Pool table");

    }

    public void EditEnd(string input)
    {
        UserNameInput = input;
    }

    public void EditEndLogin(string input)
    {
        UserNameInput2 = input;
    }


    IEnumerator CheckAndCreateUser(string username, Canvas createUserFail, Canvas createUserSuccess)
    {

        WWWForm form = new WWWForm();
        form.AddField("usernamePost", username);
        WWW www = new WWW(CheckUserURL, form);
        yield return www;
        print(www.text);

        if (www.text == "False")
        {
            WWWForm form2 = new WWWForm();
            form2.AddField("usernamePost", username);
            WWW www2 = new WWW(CreateUserURL, form2);
            yield return www2;
            createUserSuccess.enabled = true;
        }
        else
        {
            print("Already Exists");
            createUserFail.enabled = true;
            //add popup here
        }
    }

    IEnumerator CheckUser(string username, Canvas enterUserFail, Canvas enterUserSuccess)
    {

        WWWForm form = new WWWForm();
        form.AddField("usernamePost", username);
        WWW www = new WWW(CheckUserURL, form);
        yield return www;
        print(www.text);

        if (www.text == "False")
        {
            print("Doesn't exist");
            enterUserFail.enabled = true;
            //createUserSuccess.enabled = true;
        }
        else
        {
            print("Success");
            enterUserSuccess.enabled = true;
            //createUserFail.enabled = true;
        }
    }


    // Update is called once per frame
    void Update()
    {

    }
}
