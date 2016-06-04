using UnityEngine;
using System.Collections;
using Assets.game.controller;
using Assets.game.notifications;
using Assets.game;
using Assets.game.model;
using Assets.game.model.gameview;
using Assets.Scripts.GameDB;
using Assets.Scripts.GameDB.DataModel;
using Assets.Scripts.ProceduralTree;
using Tree = Assets.Scripts.GameDB.DataModel.Tree;

public class GameView : TerrariumView
{
    private GameModel model;
    private Terrarium terrarium;

    public Terrarium Terrarium
    {
        get { return terrarium; }
        set { terrarium = value; }
    }

    void Start()
    {
        model = app.GetModel<GameModel>();
        StartCoroutine(LoadTerrariumWithDelay(GlobalModel.TerrariumId, 1));

        StartCoroutine(SaveTerrariumWithDelay(5));
    }

    void Update()
    {

    }

    IEnumerator SaveTerrariumWithDelay(int delay)
    {
        yield return new WaitForSeconds(delay);

        SaveTerrarium();
    }

    IEnumerator LoadTerrariumWithDelay(int id, int delay)
    {
        yield return new WaitForSeconds(delay);

        if (LoadTerrarium(id))
        {
            // Terrarium loaded
        }
        else
        {
            // Terrarium failed to load
            // TODO: replace terrarium.Tree with an actual, non null tree;
            terrarium = new Terrarium(TreeGenerator.GetBonsaiTree(), 0xFFFFFF, 0x000000);
            SaveTerrarium();
            LoadTerrarium(GlobalModel.TerrariumId);
        }
    }

    public void SaveTerrarium()
    {
        DBHelper.SaveTerrarium(terrarium, GlobalModel.TerrariumId);
    }

    public bool LoadTerrarium(int id)
    {
        Terrarium ter = DBHelper.GetTerrarium(id);

        if (ter == null)
        {
            return false;
        }

        terrarium = ter;

        app.Notify(GameNotification.TerrariumLoaded, this, ter);

        return true;
    }
}