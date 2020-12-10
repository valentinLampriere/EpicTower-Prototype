using UnityEngine;

public class MGR_Game : MonoBehaviour
{
    private static MGR_Game pInstance = null;
    public static MGR_Game Instance { get { return pInstance; } }

    public float fMaxHP;
    public float fHP;
    public float fGolds;

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

    ///<summary> Add hit points </summary>
    public void Heal(float dmg)
    {
        ModifyHP(dmg);
    }

    ///<summary> Remove hit points </summary>
    public void TakeDamage(float dmg) 
    {
        ModifyHP(-dmg);
    }

    void ModifyHP(float hp)
    {
        fHP += hp;
        fHP = Mathf.Clamp(fHP, 0, fMaxHP);
        MGR_Canvas.Instance.UpdateHPUI();
        CheckGameover();
    }


    void CheckGameover()
    {
        if(fHP <=0)
        {
            Debug.Log("VOUS ETES MORT WOW");
        }
    }

    public void BuyButton(float cost)
    {
        Buy(cost);
    }

    public bool Buy(float cost)
    {
        if (fGolds - cost > 0)
        {
            ModifyGold(-cost);
            return true;
        }
        else
        {
            Debug.Log("Pas assez de thunes");
            return false;
        }
    }


    public void EarnGold(float gold)
    {
        ModifyGold(gold);
    }


    void ModifyGold(float g)
    {
        fGolds += g;
        MGR_Canvas.Instance.UpdateGoldUI();
    }
}