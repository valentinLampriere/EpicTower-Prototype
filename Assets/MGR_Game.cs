using UnityEngine;

public class MGR_Game : MonoBehaviour
{
    private static MGR_Game pInstance = null;
    public static MGR_Game Instance { get { return pInstance; } }

    public float fMaxHP;
    public float fHP;
    public float fCoins;

    // Awake is called before Start
    private void Awake()
    {
        if (pInstance == null)
            pInstance = this;
        else if (pInstance != this)
            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);

        fHP = fMaxHP;
    }

    // Start is called before the first frame update
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
    }

    public void ModifyPV(float dmg)
    {
        fHP += dmg;
        fHP = Mathf.Clamp(fHP, 0, fMaxHP);
        MGR_Canvas.Instance.UpdateHPUI();
    }
}