using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEditor.ShortcutManagement;

enum Option
{
    SELL,
    UPGRADE,
    NONE
}

public class Buttons : MonoBehaviour
{
    private Option option = Option.NONE;
    public void Menu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void Game()
    {
        SceneManager.LoadScene("Level1");
    }

    public void Salir()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit(); 
#endif
    }

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
        option = Option.SELL;
    }

    private void FinishSellTower()
    {
        GameObject _base = ParentInputHandler.Instance.SearchObject("Base");
        ParentInputHandler.Instance.AddInstance(Instantiate(_base, ParentInputHandler.Instance.btn.transform.position, Quaternion.identity));
        Debug.Log(ParentInputHandler.Instance.btn.name + " vendida");
        ParentInputHandler.Instance.DeleteInterfaceAndButton();
    }

    public void UpgradeTower()
    {
        ParentInputHandler.Instance.DeleteInterface();
        ParentInputHandler.Instance.InstantiateInterface(ParentInputHandler.Instance.optionInterface);
        option = Option.UPGRADE;
    }

    private void FinishUpgradeTower()
    {
        if (ParentInputHandler.Instance.btn.GetComponent<Torreta>() == null) return;
        Debug.Log(ParentInputHandler.Instance.btn.name + " mejorada");
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
        ParentInputHandler.Instance.DeleteInterface();
        ParentInputHandler.Instance.InstantiateInterface(ParentInputHandler.Instance.optionInterface);
        ParentInputHandler.Instance.gm = ParentInputHandler.Instance.SearchObject("TorreAgua");
    }

    public void InstantiateMudTower()
    {
        ParentInputHandler.Instance.DeleteInterface();
        ParentInputHandler.Instance.InstantiateInterface(ParentInputHandler.Instance.optionInterface);
        ParentInputHandler.Instance.gm = ParentInputHandler.Instance.SearchObject("TorreBarro");
    }

    public void InstantiateStoneTower()
    {
        ParentInputHandler.Instance.DeleteInterface();
        ParentInputHandler.Instance.InstantiateInterface(ParentInputHandler.Instance.optionInterface);
        ParentInputHandler.Instance.gm = ParentInputHandler.Instance.SearchObject("TorrePiedra");
    }

    public void InstantiateFastTower()
    {
        ParentInputHandler.Instance.DeleteInterface();
        ParentInputHandler.Instance.InstantiateInterface(ParentInputHandler.Instance.optionInterface);
        ParentInputHandler.Instance.gm = ParentInputHandler.Instance.SearchObject("TorreRepeticionMultiple");
    }

}
