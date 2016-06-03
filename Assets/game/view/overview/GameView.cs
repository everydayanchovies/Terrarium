using UnityEngine;
using System.Collections;
using Assets.game.controller;
using Assets.game.notifications;
using Assets.Scripts.GameDB;
using Assets.Scripts.GameDB.DataModel;
using Tree = Assets.Scripts.GameDB.DataModel.Tree;

public class GameView : TerrariumElement
{
    private Terrarium terrarium;
    private Tree tree;

    public Tree Tree
    {
        get { return tree; }
        set
        {
            tree = value;
            terrarium.Tree = tree;
        }
    }

    void Start()
    {
        gameObject.AddComponent<GameController>();

        //tree=new Tree();
        terrarium = new Terrarium(tree, 0xFFFFFF, 0x000000);
    }

    void Update()
    {

    }

    public void SaveTerrarium()
    {
        DBHelper.SaveTerrarium(terrarium, 0);
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