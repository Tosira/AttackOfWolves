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
        AudioClip clip = Resources.Load<AudioClip>("Audio/Click");
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
        AudioClip clip = Resources.Load<AudioClip>("Audio/Click");
        if (clip != null)
        {
            audio_.clip = clip;
            audio_.Play();
            StartCoroutine(LoadScene(audio_.clip.length, "Menu"));
        }
        else
        {
            Debug.LogError("AudioClip no encontrado en la ruta especificada.");
            SceneManager.LoadScene("Menu");
        }
    }

    public void Salir()
    {
        AudioClip clip = Resources.Load<AudioClip>("Audio/Click");
        if (clip != null)
        {
            audio_.clip = clip;
            audio_.Play();
            StartCoroutine(LoadScene(audio_.clip.length));
        }
        else
        {
            Debug.LogError("AudioClip no encontrado en la ruta especificada.");
            #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
            #else
                Application.Quit();
            #endif
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
        if (GameState.gs != null) GameState.gs.UpdateCurrentScene();
    }

//////////////////////////////////////////////////////////////////////

    public void CloseInterface()
    {
        InputHandler.Instance.DeleteInterface();
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
        if (InputHandler.Instance.objectToInstantiate != null)
        {
            InstantiateTower(InputHandler.Instance.objectToInstantiate);
        }
        option = Option.NONE;
    }

    public void SellTower()
    {
        InputHandler.Instance.DeleteInterface();
        InputHandler.Instance.InstantiateInterface(InputHandler.Instance.optionInterface);
        InputHandler.Instance.txtDetails.text = "¿Quiere vender esta torre por \n" + InputHandler.Instance.clickedObject.GetComponent<Torreta>().CalculateProfit() + " monedas?";
        option = Option.SELL;
    }

    private void FinishSellTower()
    {
        Torreta tower = InputHandler.Instance.clickedObject.GetComponent<Torreta>();
        if (tower == null) return;
        tower.Sell();
        GameObject _base = InputHandler.Instance.SearchObject("Base");
        InputHandler.Instance.AddInstance(Instantiate(_base, InputHandler.Instance.clickedObject.transform.position, Quaternion.identity));
        Debug.Log(InputHandler.Instance.clickedObject.name + " vendida por " + tower.CalculateProfit());
        InputHandler.Instance.DeleteInterfaceAndButton();
    }

    public void UpgradeTower()
    {
        InputHandler.Instance.DeleteInterface();
        InputHandler.Instance.InstantiateInterface(InputHandler.Instance.optionInterface);
        InputHandler.Instance.txtDetails.text = InputHandler.Instance.clickedObject.GetComponent<Torreta>().GetDetailsUpgrade();
        option = Option.UPGRADE;
    }

    private void FinishUpgradeTower()
    {
        Torreta tower = InputHandler.Instance.clickedObject.GetComponent<Torreta>();
        if (tower == null) return;
        if (!tower.Upgrade())
        {
            InputHandler.Instance.txtDetails.text = "Dinero insuficiente";
            return;
        }
        Debug.Log(InputHandler.Instance.clickedObject.name + " mejorada");
        InputHandler.Instance.DeleteInterface();
    }

    private void InstantiateTower(GameObject tower)
    {
        if (tower == null) return;

        GameObject t = Instantiate(tower, InputHandler.Instance.clickedObject.transform.position, Quaternion.identity);
        if (!GameState.gs.Buy(t.GetComponent<Torreta>().Price))
        {
            Destroy(t);
            InputHandler.Instance.txtDetails.text = "Dinero insuficiente";
            // Puedo devolver ParentInputHandler::objectToInstantiate a null aqui, pero si jugador llega al dinero suficiente
            // la interfaz seguira activa y ya no tendra la referencia. Deje que la funcion ParentInputHandler::DeleteInterfaceAndButton se encargue.
            return;
        }
        InputHandler.Instance.DeleteInterfaceAndButton();
        InputHandler.Instance.AddInstance(t);
        Debug.Log(tower.name + " instanciado");
    }

    public void InstantiateWaterTower()
    {
        GameObject gm = new GameObject("Tower");
        TorreAgua t = gm.AddComponent<TorreAgua>();
        if (GameState.gs.LevelIndex < t.AvailableLevel)
        {
            InputHandler.Instance.txtDetails.text = "Desbloquee esta torre en el nivel " + t.AvailableLevel;
            Destroy(gm);
            return;
        }
        InputHandler.Instance.DeleteInterface();
        InputHandler.Instance.InstantiateInterface(InputHandler.Instance.optionInterface);
        InputHandler.Instance.objectToInstantiate = InputHandler.Instance.SearchObject("TorreAgua");
        InputHandler.Instance.txtDetails.text = t.GetDetailsTower();
        Destroy(gm);
    }

    public void InstantiateMudTower()
    {
        GameObject gm = new GameObject("Tower");
        TorreBarro t = gm.AddComponent<TorreBarro>();
        if (GameState.gs.LevelIndex < t.AvailableLevel)
        {
            InputHandler.Instance.txtDetails.text = "Desbloquee esta torre en el nivel " + t.AvailableLevel;
            Destroy(gm);
            return;
        }
        InputHandler.Instance.DeleteInterface();
        InputHandler.Instance.InstantiateInterface(InputHandler.Instance.optionInterface);
        InputHandler.Instance.objectToInstantiate = InputHandler.Instance.SearchObject("TorreBarro");
        InputHandler.Instance.txtDetails.text = t.GetDetailsTower();
        Destroy(gm);
    }

    public void InstantiateStoneTower()
    {
        GameObject gm = new GameObject("Tower");
        TorrePiedra t = gm.AddComponent<TorrePiedra>();
        if (GameState.gs.LevelIndex < t.AvailableLevel)
        {
            InputHandler.Instance.txtDetails.text = "Desbloquee esta torre en el nivel " + t.AvailableLevel;
            Destroy(gm);
            return;
        }
        InputHandler.Instance.DeleteInterface();
        InputHandler.Instance.InstantiateInterface(InputHandler.Instance.optionInterface);
        InputHandler.Instance.objectToInstantiate = InputHandler.Instance.SearchObject("TorrePiedra");
        InputHandler.Instance.txtDetails.text = t.GetDetailsTower();
        Destroy(gm);
    }

    public void InstantiateFastTower()
    {
        GameObject gm = new GameObject("Tower");
        TorreRepeticionMultiple t = gm.AddComponent<TorreRepeticionMultiple>();
        if (GameState.gs.LevelIndex < t.AvailableLevel)
        {
            InputHandler.Instance.txtDetails.text = "Desbloquee esta torre en el nivel " + t.AvailableLevel;
            Destroy(gm);
            return;
        }
        InputHandler.Instance.DeleteInterface();
        InputHandler.Instance.InstantiateInterface(InputHandler.Instance.optionInterface);
        InputHandler.Instance.objectToInstantiate = InputHandler.Instance.SearchObject("TorreRepeticionMultiple");
        InputHandler.Instance.txtDetails.text = t.GetDetailsTower();
        Destroy(gm);
    }

}
