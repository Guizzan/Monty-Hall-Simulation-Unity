using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Text.RegularExpressions;

public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI totalSimCount;
    public TextMeshProUGUI foundCarCount;
    public TextMeshProUGUI missedCarCount;
    public TextMeshProUGUI succesRate;
    public TextMeshProUGUI narator;
    public TextMeshProUGUI simulationSpeed;
    public Slider slider;

    public Transform ContentParrent;
    public GameObject SimulationPrefab;
    public List<GameObject> Doors = new List<GameObject>();
    public List<bool> OpenedDoors = new List<bool> { false, false, false };

    private int _simCount;
    private float _currentCount;
    private bool _autoPick;
    private bool _changeDesicion;
    private RuntimePlatform platform;

    private int _selectedDoor = -1;
    private int _hostSelected = -1;
    private int _carDoor = -1;
    private float _foundCarCount;
    private bool _canClick;
    bool isRunning;
    private GameObject Instance;

    private void Awake()
    {
        platform = Application.platform;
    }
    public void StartSimulation(bool auto)
    {
        if (Instance != null) return;
        narator.text = "Host: There are 2 goats and a car behind these doors. Select one and win what is behind.";
        _carDoor = Random.Range(0, 3);
        for (int i = 0; i < Doors.Count; i++)
        {
            GameObject door = Doors[i];
            if (i != _carDoor)
            {
                door.transform.Find("Goat").GetComponent<MeshRenderer>().enabled = true;
            }
        }
        Doors[_carDoor].transform.Find("Car").GetComponent<MeshRenderer>().enabled = true;

        Instance = Instantiate(SimulationPrefab, ContentParrent);
        Instance.gameObject.SetActive(false);
        if (!auto)
        {
            _canClick = true;
        }
        if (_autoPick)
        {
            StartCoroutine(SelectDoor(Random.Range(0, 3)));
        }
        isRunning = true;
    }
    public void ResetData()
    {
        foreach (Transform item in ContentParrent)
        {
            Destroy(item.gameObject);
        }
        _currentCount = 0;
        _foundCarCount = 0;
        totalSimCount.text = "Sim. Count: " + _currentCount;
        foundCarCount.text = "Found Car: " + _foundCarCount;
        missedCarCount.text = "Missed Car: " + (_currentCount - _foundCarCount);
        succesRate.text = "Succes Rate: %" + 0;
    }
    public void ResetSimButton()
    {
        StartCoroutine(ResetSimulation(false));
    }
    public IEnumerator ResetSimulation(bool auto)
    {
        _selectedDoor = -1;
        _carDoor = -1;
        _hostSelected = -1;
        Instance = null;
        narator.text = "Reseting...";
        foreach (GameObject door in Doors)
        {
            if (OpenedDoors[Doors.IndexOf(door)])
            {
                door.GetComponent<Animator>().SetTrigger("Close");
            }
            OpenedDoors[Doors.IndexOf(door)] = false;
            door.transform.Find("DoorMover/Selected").GetComponent<MeshRenderer>().enabled = false;
        }
        yield return new WaitForSeconds(0.5f);
        foreach (GameObject door in Doors) //There are 2 foreach because waited for animations
        {
            door.transform.Find("Goat").GetComponent<MeshRenderer>().enabled = false;
            door.transform.Find("Car").GetComponent<MeshRenderer>().enabled = false;
        }
        if (!auto)
        {
            isRunning = false;
        }
        else
        {
            if (_currentCount < _simCount)
            {
                StartSimulation(true);
            }
        }
        yield return new WaitForSeconds(0.5f);
        narator.text = "Welcome to the Monty Hall Problem Simulator. Please start the simulation.";
    }
    private IEnumerator SelectDoor(int doorNumber)
    {
        if (_selectedDoor == -1) // First click (Select Door)
        {
            Debug.Log("asdasd");
            _selectedDoor = doorNumber;
            Instance.transform.Find("firstSelected").GetComponent<TextMeshProUGUI>().text = "First Selected: " + (_selectedDoor + 1);
            foreach (GameObject door in Doors)
            {
                door.transform.Find("DoorMover/Selected").GetComponent<MeshRenderer>().enabled = false;
            }
            Doors[_selectedDoor].transform.Find("DoorMover/Selected").GetComponent<MeshRenderer>().enabled = true;
            narator.text = "Host: I'm making you a favor and opening a goat door.";
            int RandomDoor = GetRandomDoor(true);
            Doors[RandomDoor].GetComponent<Animator>().SetTrigger("Open");
            OpenedDoors[RandomDoor] = true;
            _hostSelected = RandomDoor;
            Instance.transform.Find("hostOpened").GetComponent<TextMeshProUGUI>().text = "Host Opened: " + (RandomDoor + 1);
            yield return new WaitForSeconds(1.5f);
            narator.text = "Host: Now, will you change your selection?";
            _canClick = true;
            if (_autoPick)
            {
                if (_changeDesicion)
                {
                    StartCoroutine(SelectDoor(GetRandomDoor(false)));
                }
                else
                {
                    StartCoroutine(SelectDoor(_selectedDoor));
                }
            }
            yield break;
        }
        bool changedDoor = (_selectedDoor != doorNumber) ? true : false;
        Instance.transform.Find("secondSelected").GetComponent<TextMeshProUGUI>().text = "Second Selected: " + doorNumber;
        if (!changedDoor)
        {
            if (_carDoor == doorNumber) //Won the game
            {
                narator.text = "Host: You didn't change your mind but you still won the car!";
                Instance.transform.Find("result").GetComponent<TextMeshProUGUI>().color = Color.green;
                Instance.transform.Find("result").GetComponent<TextMeshProUGUI>().text = "Won";
                _foundCarCount++;
            }
            else //Lost the game
            {
                narator.text = "Host: You didn't change your mind. Fool...";
                Instance.transform.Find("result").GetComponent<TextMeshProUGUI>().color = Color.red;
                Instance.transform.Find("result").GetComponent<TextMeshProUGUI>().text = "Lost";
            }
        }
        else
        {
            if (_carDoor == doorNumber) //Won the game
            {
                narator.text = "Host: You changed your mind and won the car!";
                Instance.transform.Find("result").GetComponent<TextMeshProUGUI>().color = Color.green;
                Instance.transform.Find("result").GetComponent<TextMeshProUGUI>().text = "Won";
                _foundCarCount++;
            }
            else //Lost the game
            {
                narator.text = "Host: You changed your mind. Smart move but you lost.";
                Instance.transform.Find("result").GetComponent<TextMeshProUGUI>().color = Color.red;
                Instance.transform.Find("result").GetComponent<TextMeshProUGUI>().text = "Lost";
            }
        }
        Doors[doorNumber].GetComponent<Animator>().SetTrigger("Open");
        OpenedDoors[doorNumber] = true;
        Instance.gameObject.SetActive(true);
        _currentCount++;
        totalSimCount.text = "Sim. Count: " + _currentCount;
        foundCarCount.text = "Found Car: " + _foundCarCount;
        if (_currentCount > 0 && _foundCarCount > 0)
        {
            Debug.Log("found: " + _foundCarCount + "Current" + _currentCount);
            int percantage = Mathf.RoundToInt(_foundCarCount / _currentCount * 100);
            succesRate.text = "Succes Rate: %" + percantage;
        }
        Instance.transform.Find("carPos").GetComponent<TextMeshProUGUI>().text = "Car Door: " + (_carDoor + 1);
        Instance.transform.Find("simNumber").GetComponent<TextMeshProUGUI>().text = "Simulation: " + _currentCount;
        missedCarCount.text = "Missed Car: " + (_currentCount - _foundCarCount);
        if (_autoPick && _currentCount < _simCount)
        {
            yield return new WaitForSeconds(1f);
            StartCoroutine(ResetSimulation(true));
        }
    }
    int GetRandomDoor(bool first)
    {
        int random = Random.Range(0, 3);
        if (first)
        {
            while (random == _carDoor || random == _selectedDoor)
            {
                random = Random.Range(0, 3);
            }
        }
        else
        {
            while (random == _selectedDoor || random == _hostSelected)
            {
                random = Random.Range(0, 3);
            }
        }
        return random;
    }
    public void BoolChanged(Toggle toggle)
    {
        switch (toggle.name)
        {
            case "AutoPick":
                _autoPick = toggle.isOn;
                break;
            case "ChangeDesicion":
                _changeDesicion = toggle.isOn;
                break;
        }
    }

    public void CountChanged(TMP_InputField input)
    {
        if (input.text != "")
        {
            string text = input.text;
            text = Regex.Replace(text, @"[^a-zA-Z0-9 ]", "1");
            _simCount = int.Parse(text);
        }
        else
        {
            _simCount = 1;
        }
    }

    void Update()
    {
        simulationSpeed.text = "Simulation Speed : " + slider.value;
        Time.timeScale = slider.value;
        if (Instance == null) return;
        if (platform == RuntimePlatform.Android || platform == RuntimePlatform.IPhonePlayer)
        {
            if (Input.touchCount > 0 && Input.touchCount < 2)
            {
                if (Input.GetTouch(0).phase == TouchPhase.Began)
                {
                    CheckTouch(Input.GetTouch(0).position);
                }
            }
        }
        else if (platform == RuntimePlatform.WindowsEditor || platform == RuntimePlatform.OSXEditor)
        {
            if (Input.GetMouseButtonDown(0))
            {
                CheckTouch(Input.mousePosition);
            }
        }

    }
    private void CheckTouch(Vector3 pos)
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(pos);

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.gameObject.CompareTag("Door"))
            {
                if (_canClick)
                {
                    Debug.Log("Selected Door: " + hit.collider.gameObject.name);
                    StartCoroutine(SelectDoor(int.Parse(hit.collider.gameObject.name) - 1));
                    _canClick = false;
                }
            }
        }
    }

}
