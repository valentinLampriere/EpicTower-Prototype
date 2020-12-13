using UnityEngine;
using UnityEngine.UI;

public class MGR_Canvas : MonoBehaviour
{
    private static MGR_Canvas pInstance = null;
    public static MGR_Canvas Instance { get { return pInstance; } }


    public Slider sliderHP;
    public Text textGold;

    public GameObject GODeathPanel;
    public GameObject GORoomPanel;


    // Awake is called before Start
    private void Awake()
    {
        if (pInstance == null)
            pInstance = this;
        else if (pInstance != this)
            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);

        GODeathPanel.SetActive(false);
    }

    private void Start()
    {
        sliderHP.maxValue = MGR_Game.Instance.fMaxHP;
        UpdateHPUI();
        UpdateGoldUI();
    }


    public void UpdateHPUI()
    {
        sliderHP.value = MGR_Game.Instance.fHP;
    
        sliderHP.GetComponentInChildren<Text>().text = sliderHP.value.ToString() + " / " + sliderHP.maxValue.ToString();
    }

    public void UpdateGoldUI()
    {
        textGold.text = MGR_Game.Instance.fGolds.ToString() + " Gold";
    }

    public void UpdateRoomsPanel()
    {
        if(MGR_Game.Instance.GetPhase() == Phase.Phase1)
        {
            GORoomPanel.SetActive(true);
        }
        else
        {
            GORoomPanel.SetActive(false);
        }
    }


}