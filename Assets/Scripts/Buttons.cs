using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;
using Assets.src.Torres;

enum Option
{
    SELL,
    UPGRADE,
    NONE
}

public class Buttons : MonoBehaviour
{
    AudioSource audio_;
    private Option option = Option.NONE;

    private void Awake()
    {
        audio_ = GetComponent<AudioSource>();
    }

    public void Game()
    {
        AudioClip clip = Resources.Load<AudioClip>("Click");
        if (clip != null)
        {
            audio_.clip = clip;
            audio_.Play();
            StartCoroutine(LoadScene(audio_.clip.length, "Level1"));
        }
        else
        {
            Debug.LogError("AudioClip no encontrado en la ruta especificada.");
            SceneManager.LoadScene("Level1");
        }
    }

    public void Menu()
    {
        AudioClip clip = Resources.Load<AudioClip>("Click");
        if (clip != null)
        {
            audio_.clip = clip;
            audio_.Play();
            StartCoroutine(LoadScene(audio_.clip.length, "Menu"));
        }
        else
        {
            Debug.LogError("AudioClip no encontrado en la ruta especificada.");
            SceneManager.LoadScene("Level1");
        }
    }

    public void Salir()
    {
        AudioClip clip = Resources.Load<AudioClip>("Click");
        if (clip != null)
        {
            audio_.clip = clip;
            audio_.Play();
            StartCoroutine(LoadScene(audio_.clip.length));
        }
        else
        {
            Debug.LogError("AudioClip no encontrado en la ruta especificada.");
            SceneManager.LoadScene("Level1");
        }
    }

    private IEnumerator LoadScene(float clipLength, string scene="")
    {
        yield return new WaitForSeconds(clipLength);
        if (scene!="") {SceneManager.LoadScene(scene);}
        else 
        {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit(); 
#endif
        }
    }

//////////////////////////////////////////////////////////////////////

    public void CloseInterface()
    {
        ParentInputHandler.Instance.DeleteInterface();
    }

    public void OptionYes()
    {
        if (option == Option.UPGRADE)
        {
            FinishUpgradeTower();
        }
        if (option == Option.SELL)
        {
            FinishSellTower();
        }
        if (ParentInputHandler.Instance.gm != null)
        {
            if (ParentInputHandler.Instance.gm.GetComponent<Torreta>() != null) InstantiateTower(ParentInputHandler.Instance.gm);
            ParentInputHandler.Instance.DeleteInterfaceAndButton();
            ParentInputHandler.Instance.gm = null;
        }
        option = Option.NONE;
    }

    public void SellTower()
    {
        ParentInputHandler.Instance.DeleteInterface();
        ParentInputHandler.Instance.InstantiateInterface(ParentInputHandler.Instance.optionInterface);
        ParentInputHandler.Instance.txtDetails.text = "¿Quiere vender esta torre por \n" + ParentInputHandler.Instance.btn.GetComponent<Torreta>().CalculateProfit() + " monedas?";
        option = Option.SELL;
    }

    private void FinishSellTower()
    {
        Torreta tower = ParentInputHandler.Instance.btn.GetComponent<Torreta>();
        if (tower == null) return;
        tower.Sell();
        GameObject _base = ParentInputHandler.Instance.SearchObject("Base");
        ParentInputHandler.Instance.AddInstance(Instantiate(_base, ParentInputHandler.Instance.btn.transform.position, Quaternion.identity));
        Debug.Log(ParentInputHandler.Instance.btn.name + " vendida por " + tower.CalculateProfit());
        ParentInputHandler.Instance.DeleteInterfaceAndButton();
    }

    public void UpgradeTower()
    {
        ParentInputHandler.Instance.DeleteInterface();
        ParentInputHandler.Instance.InstantiateInterface(ParentInputHandler.Instance.optionInterface);
        ParentInputHandler.Instance.txtDetails.text = ParentInputHandler.Instance.btn.GetComponent<Torreta>().GetDetailsUpgrade();
        option = Option.UPGRADE;
    }

    private void FinishUpgradeTower()
    {
        Torreta tower = ParentInputHandler.Instance.btn.GetComponent<Torreta>();
        if (tower == null) return;
        if (tower.Upgrade()) { Debug.Log(ParentInputHandler.Instance.btn.name + " mejorada"); }
        ParentInputHandler.Instance.DeleteInterface();
    }

    private void InstantiateTower(GameObject tower)
    {
        if (tower==null) return;
        ParentInputHandler.Instance.AddInstance(Instantiate(tower, ParentInputHandler.Instance.btn.transform.position, Quaternion.identity));
        Debug.Log(tower.name + " instanciado");
    }

    public void InstantiateWaterTower()
    {
        GameObject gm = new GameObject("Tower");
        TorreAgua t = gm.AddComponent<TorreAgua>();
        if (GameState.gs.LevelIndex < t.AvailableLevel)
        {
            ParentInputHandler.Instance.txtDetails.text = "Desbloquee esta torre en el nivel " + t.AvailableLevel;
            Destroy(gm);
            return;
        }
        ParentInputHandler.Instance.DeleteInterface();
        ParentInputHandler.Instance.InstantiateInterface(ParentInputHandler.Instance.optionInterface);
        ParentInputHandler.Instance.gm = ParentInputHandler.Instance.SearchObject("TorreAgua");
        ParentInputHandler.Instance.txtDetails.text = t.GetDetailsTower();
        Destroy(gm);
    }

    public void InstantiateMudTower()
    {
        GameObject gm = new GameObject("Tower");
        TorreBarro t = gm.AddComponent<TorreBarro>();
        if (GameState.gs.LevelIndex < t.AvailableLevel)
        {
            ParentInputHandler.Instance.txtDetails.text = "Desbloquee esta torre en el nivel " + t.AvailableLevel;
            Destroy(gm);
            return;
        }
        ParentInputHandler.Instance.DeleteInterface();
        ParentInputHandler.Instance.InstantiateInterface(ParentInputHandler.Instance.optionInterface);
        ParentInputHandler.Instance.gm = ParentInputHandler.Instance.SearchObject("TorreBarro");
        ParentInputHandler.Instance.txtDetails.text = t.GetDetailsTower();
        Destroy(gm);
    }

    public void InstantiateStoneTower()
    {
        GameObject gm = new GameObject("Tower");
        TorrePiedra t = gm.AddComponent<TorrePiedra>();
        if (GameState.gs.LevelIndex < t.AvailableLevel)
        {
            ParentInputHandler.Instance.txtDetails.text = "Desbloquee esta torre en el nivel " + t.AvailableLevel;
            Destroy(gm);
            return;
        }
        ParentInputHandler.Instance.DeleteInterface();
        ParentInputHandler.Instance.InstantiateInterface(ParentInputHandler.Instance.optionInterface);
        ParentInputHandler.Instance.gm = ParentInputHandler.Instance.SearchObject("TorrePiedra");
        ParentInputHandler.Instance.txtDetails.text = t.GetDetailsTower();
        Destroy(gm);
    }

    public void InstantiateFastTower()
    {
        GameObject gm = new GameObject("Tower");
        TorreRepeticionMultiple t = gm.AddComponent<TorreRepeticionMultiple>();
        if (GameState.gs.LevelIndex < t.AvailableLevel)
        {
            ParentInputHandler.Instance.txtDetails.text = "Desbloquee esta torre en el nivel " + t.AvailableLevel;
            Destroy(gm);
            return;
        }
        ParentInputHandler.Instance.DeleteInterface();
        ParentInputHandler.Instance.InstantiateInterface(ParentInputHandler.Instance.optionInterface);
        ParentInputHandler.Instance.gm = ParentInputHandler.Instance.SearchObject("TorreRepeticionMultiple");
        ParentInputHandler.Instance.txtDetails.text = t.GetDetailsTower();
        Destroy(gm);
    }

}
