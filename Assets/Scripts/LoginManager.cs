using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.IO;
using Newtonsoft.Json;


public class LoginManager : MonoBehaviour
{
	private string registeredID;
	private string registeredPW;
	public GameObject mainPanel;
	public GameObject loginPanel;
	public GameObject signupPanel;
	public GameObject menuPanel;
	public GameObject warningPanel;
	public TMP_InputField inputID;
	public TMP_InputField inputPW;
	public TMP_InputField inputName;
	public TMP_InputField inputID_Sign;
	public TMP_InputField inputPW_Sign;
	public TMP_InputField inputAge;
	public TMP_InputField inputHeight;
	public TMP_InputField inputWeight;
	public TMP_Text warningText;

	[SerializeField] Users UserDatas;
	private void Awake()
	{
		string path = Path.Combine(Application.persistentDataPath,"User.json");
		print(path);
		if (File.Exists(path)) UserDatas = JsonConvert.DeserializeObject<Users>(File.ReadAllText(path));
		else UserDatas = new Users();
	}

	private void OnApplicationQuit()
	{
		string json = JsonConvert.SerializeObject(UserDatas);
		File.WriteAllText(Path.Combine(Application.persistentDataPath, "User.json"),json);
	}

	void Start()
	{
		ShowMain();
	}

	public void ShowMain()
	{
		mainPanel.SetActive(true);
		loginPanel.SetActive(false);
		signupPanel.SetActive(false);
		menuPanel.SetActive(false);
	}

	public void OpenLogin()
	{
		mainPanel.SetActive(false);
		loginPanel.SetActive(true);
	}

	public void OpenSignup()
	{
		loginPanel.SetActive(false);
		signupPanel.SetActive(true);
	}

	public void CompleteSignup()
	{
		string name = inputName.text;
		registeredID = inputID_Sign.text;
		registeredPW = inputPW_Sign.text;
		string age = inputAge.text;
		string height = inputHeight.text;
		string weight = inputWeight.text;

		foreach(var s in UserDatas.User)
		{
			if(registeredID.Equals(s.id))
			{
				ShowWarning("�̹� ���Ե� ���̵��Դϴ�.");
				Debug.Log("�̹� ���Ե� ȸ�� �Դϴ�.");
				return;
			}
		}
		UserDatas.User.Add(new UserData(registeredID,registeredPW,name,int.Parse(age),int.Parse(height),int.Parse(weight)));

		Debug.Log($"[ȸ������ ����] �̸�: {name}, ID: {registeredID}, PW: {registeredPW}, ����: {age}, Ű: {height}, ������: {weight}");

		signupPanel.SetActive(false);
		mainPanel.SetActive(true);  		// �Ǵ� loginPanel.SetActive(true); ������Ʈ �帧�� ���� ����
		// ���� ����, ���� ���� ������ Ȯ�� ����
	}

	public void BackToLoginFromSignup()
	{
		signupPanel.SetActive(false);
		loginPanel.SetActive(true);
	}

	public void BackToMainFromLogin()
	{
		loginPanel.SetActive(false);
		mainPanel.SetActive(true);
	}

	public void BackToLoginFromMenu()
	{
		menuPanel.SetActive(false);
		loginPanel.SetActive(true);
	}

	public void SignupComplete()
	{
		signupPanel.SetActive(false);
		mainPanel.SetActive(true);
	}

	public void TryLogin()
	{
		string id = inputID.text;
		string pw = inputPW.text;

		if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(pw))
		{
			ShowWarning("���̵� �Ǵ� ��й�ȣ�� �Է����ּ���.");
			Debug.LogWarning("���̵� �Ǵ� ��й�ȣ�� ��� �ֽ��ϴ�.");
			return;
		}

		foreach (var s in UserDatas.User)
		{
			if (id == s.id && pw == s.pw)
			{
				loginPanel.SetActive(false);
				menuPanel.SetActive(true);
				return;
			}
		}
		ShowWarning("���̵� �Ǵ� ��й�ȣ�� ��ġ���� �ʽ��ϴ�.");
		Debug.LogWarning("���̵� �Ǵ� ��й�ȣ�� ��ġ���� �ʽ��ϴ�.");
	}

	public void LoginSuccess()
	{
		loginPanel.SetActive(false);
		menuPanel.SetActive(true);
	}

	public void ShowWarning(string message)
	{
		warningText.text = message;
		warningPanel.SetActive(true);
		CancelInvoke(nameof(HideWarning));
		Invoke(nameof(HideWarning), 2.0f);
	}

	void HideWarning()
	{
		warningPanel.SetActive(false);
	}


	public void StartSinglePlay()
	{
		Debug.Log("�̱� �÷��� �� �ε� (����)");
		// SceneManager.LoadScene("SinglePlayerScene");
	}

	public void StartMultiPlay()
	{
		Debug.Log("��Ƽ �÷��� �� �ε� (����)");
		// SceneManager.LoadScene("MultiPlayerScene");
	}

	public void OpenMyPage()
	{
		Debug.Log("���������� �� �ε� (����)");
		// SceneManager.LoadScene("MyPageScene");
	}

	public void ExitApp()
	{
#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
	}
}

