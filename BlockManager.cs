using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BLOCKSTATE
{
    OPEN_BLOCK,
    CLOSE_BLOCK,
    NUMBER_BLOCK,
    FLAG_BLOCK,
    MINE_BLOCK = 11
}

public class BlockManager : MonoBehaviour
{
    private static BlockManager instance;
    public static BlockManager GetInstance()
    {
        if (instance == null)
        {
            instance = new BlockManager();
        }
        return instance;
    }


    [Header("MAP SET")]
    public int MAPHIGH;
    public int MAPWIDTH;
    public int NumberOfMINE;
    [Header("EASY  09 X 09 / 10")]
    [Header("NOMAL 16 X 16 / 40")]
    [Header("HARD  30 X 16 / 99")]

    [Header("----------------------")]
    [Header("BLOCK SET")]
    public GameObject BackGround;
    public GameObject CloseBlockPrefabs;
    public GameObject OpenBlockPrefabs;
    public GameObject[] NumberBlockPreFabs = new GameObject[8];
    public GameObject flagPrefabs;
    public GameObject minePrefabs;
    public GameObject resetPrefabs;
    public Material CloseRedColorBlock;

    public List<List<GameObject>> CloseBlockMap = new List<List<GameObject>>();
    public List<List<GameObject>> BlockMapInfo = new List<List<GameObject>>();
    public List<List<GameObject>> BlockMapInfo2 = new List<List<GameObject>>();
    public List<List<GameObject>> NumberBlockMap = new List<List<GameObject>>();
    public List<List<GameObject>> MineBlockMap = new List<List<GameObject>>();
    public List<List<int>> MapListInfo = new List<List<int>>();

    public void Awake()
    {
        BlockManager.instance = this;

        InitBlock();
    }

    void Start()
    {

    }

    void InitBlock()
    {
        #region CloseBlockMapInit
        for (int i = 0; i < MAPHIGH; i++)
        {
            CloseBlockMap.Add(new List<GameObject>());

            for (int j = 0; j < MAPWIDTH; j++)
            {
                CloseBlockMap[i].Add(CloseBlockPrefabs);
            }
        }
        #endregion

        #region OpenBlockMapInit
        for (int i = 0; i < MAPHIGH; i++)
        {
            BlockMapInfo.Add(new List<GameObject>());

            for (int j = 0; j < MAPWIDTH; j++)
            {
                BlockMapInfo[i].Add(OpenBlockPrefabs);
            }
        }
        #endregion

        SetMine();

        #region NumberMapInit
        for (int i = 0; i < MAPHIGH; i++)
        {
            for (int j = 0; j < MAPWIDTH; j++)
            {
                if (MapListInfo[i][j] == (int)BLOCKSTATE.MINE_BLOCK)
                {
                    if (i >= 0 && j >= 0 && j < MAPWIDTH && i < MAPHIGH)
                    {
                        if (i > 0 && j > 0)
                        {
                            if (MapListInfo[i - 1][j - 1] != (int)BLOCKSTATE.MINE_BLOCK)
                            {
                                MapListInfo[i - 1][j - 1] += 1;
                            }
                        }

                        if (i > 0)
                        {
                            if (MapListInfo[i - 1][j] != (int)BLOCKSTATE.MINE_BLOCK)
                            {
                                MapListInfo[i - 1][j] += 1;
                            }
                        }

                        if (j < MAPWIDTH - 1 && i > 0)
                        {
                            if (MapListInfo[i - 1][j + 1] != (int)BLOCKSTATE.MINE_BLOCK)
                            {
                                MapListInfo[i - 1][j + 1] += 1;
                            }
                        }

                        if (j > 0)
                        {
                            if (MapListInfo[i][j - 1] != (int)BLOCKSTATE.MINE_BLOCK)
                            {
                                MapListInfo[i][j - 1] += 1;
                            }
                        }

                        if (j < MAPWIDTH - 1)
                        {
                            if (MapListInfo[i][j + 1] != (int)BLOCKSTATE.MINE_BLOCK)
                            {
                                MapListInfo[i][j + 1] += 1;
                            }
                        }

                        if (i < MAPHIGH - 1 && j > 0)
                        {
                            if (MapListInfo[i + 1][j - 1] != (int)BLOCKSTATE.MINE_BLOCK)
                            {
                                MapListInfo[i + 1][j - 1] += 1;
                            }
                        }

                        if (i < MAPHIGH - 1)
                        {
                            if (MapListInfo[i + 1][j] != (int)BLOCKSTATE.MINE_BLOCK)
                            {
                                MapListInfo[i + 1][j] += 1;
                            }
                        }

                        if (i < MAPHIGH - 1 && j < MAPWIDTH - 1)
                        {
                            if (MapListInfo[i + 1][j + 1] != (int)BLOCKSTATE.MINE_BLOCK)
                            {
                                MapListInfo[i + 1][j + 1] += 1;
                            }
                        }
                    }
                }
            }
        }
        #endregion

        #region NumberMapBlockInit

        for (int i = 0; i < MAPHIGH; i++)
        {
            for (int j = 0; j < MAPWIDTH; j++)
            {
                if (MapListInfo[i][j] == 1)
                {
                    BlockMapInfo[i][j] = NumberBlockPreFabs[0];
                }
                if (MapListInfo[i][j] == 2)
                {
                    BlockMapInfo[i][j] = NumberBlockPreFabs[1];
                }
                if (MapListInfo[i][j] == 3)
                {
                    BlockMapInfo[i][j] = NumberBlockPreFabs[2];
                }
                if (MapListInfo[i][j] == 4)
                {
                    BlockMapInfo[i][j] = NumberBlockPreFabs[3];
                }
                if (MapListInfo[i][j] == 5)
                {
                    BlockMapInfo[i][j] = NumberBlockPreFabs[4];
                }
                if (MapListInfo[i][j] == 6)
                {
                    BlockMapInfo[i][j] = NumberBlockPreFabs[5];
                }
                if (MapListInfo[i][j] == 7)
                {
                    BlockMapInfo[i][j] = NumberBlockPreFabs[6];
                }
                if (MapListInfo[i][j] == 8)
                {
                    BlockMapInfo[i][j] = NumberBlockPreFabs[7];
                }
            }
        }
        #endregion

        #region NumberMapInit_Del
        //#region NumberMapInit
        //for (int i = 0; i < MAPHIGH; i++)
        //{
        //    for (int j = 0; j < MAPWIDTH; j++)
        //    {
        //        if (MapArrInfo[i, j] == (int)BLOCKSTATE.MINE_BLOCK)
        //        {
        //            if (i >= 0 && j >= 0 && j < MAPWIDTH && i < MAPHIGH)
        //            {
        //                if (i > 0 && j > 0)
        //                {
        //                    if (MapArrInfo[i - 1, j - 1] != (int)BLOCKSTATE.MINE_BLOCK)
        //                    {
        //                        MapArrInfo[i - 1, j - 1] += 1;
        //                    }
        //                }

        //                if (i > 0)
        //                {
        //                    if (MapArrInfo[i - 1, j] != (int)BLOCKSTATE.MINE_BLOCK)
        //                    {
        //                        MapArrInfo[i - 1, j] += 1;
        //                    }
        //                }

        //                if (j < MAPWIDTH - 1 && i > 0)
        //                {
        //                    if (MapArrInfo[i - 1, j + 1] != (int)BLOCKSTATE.MINE_BLOCK)
        //                    {
        //                        MapArrInfo[i - 1, j + 1] += 1;
        //                    }
        //                }

        //                if (j > 0)
        //                {
        //                    if (MapArrInfo[i, j - 1] != (int)BLOCKSTATE.MINE_BLOCK)
        //                    {
        //                        MapArrInfo[i, j - 1] += 1;
        //                    }
        //                }

        //                if (j < MAPWIDTH - 1)
        //                {
        //                    if (MapArrInfo[i, j + 1] != (int)BLOCKSTATE.MINE_BLOCK)
        //                    {
        //                        MapArrInfo[i, j + 1] += 1;
        //                    }
        //                }

        //                if (i < MAPHIGH - 1 && j > 0)
        //                {
        //                    if (MapArrInfo[i + 1, j - 1] != (int)BLOCKSTATE.MINE_BLOCK)
        //                    {
        //                        MapArrInfo[i + 1, j - 1] += 1;
        //                    }
        //                }

        //                if (i < MAPHIGH - 1)
        //                {
        //                    if (MapArrInfo[i + 1, j] != (int)BLOCKSTATE.MINE_BLOCK)
        //                    {
        //                        MapArrInfo[i + 1, j] += 1;
        //                    }
        //                }

        //                if (i < MAPHIGH - 1 && j < MAPWIDTH - 1)
        //                {
        //                    if (MapArrInfo[i + 1, j + 1] != (int)BLOCKSTATE.MINE_BLOCK)
        //                    {
        //                        MapArrInfo[i + 1, j + 1] += 1;
        //                    }
        //                }
        //            }
        //        }
        //    }
        //}
        //#endregion
        #endregion


        #region Instantiate
        for (int i = 0; i < CloseBlockMap.Count; i++)
        {
            for (int j = 0; j < CloseBlockMap[i].Count; j++)
            {

                //Quaternion 수정
                CloseBlockMap[i][j] = Instantiate(CloseBlockMap[i][j], new Vector3(i, j, 0),  Quaternion.Euler(new Vector3(90,180,0)));
                BlockMapInfo[i][j] = Instantiate(BlockMapInfo[i][j], new Vector3(i, j, 0.2f), Quaternion.Euler(new Vector3(90, 180, 0)));
                

            }
        }
        #endregion
    }

    void SetMine()
    {
        #region MineBlockMapInit
        int mineXPos;
        int mineYPos;
        int mine = 0;

        for (int i = 0; i < MAPHIGH; i++)
        {
            MapListInfo.Add(new List<int>());
            for (int j = 0; j < MAPWIDTH; j++)
            {
                MapListInfo[i].Add((int)BLOCKSTATE.OPEN_BLOCK);
            }
        }

        while (true)
        {
            mineXPos = Random.Range(0, MAPWIDTH);
            mineYPos = Random.Range(0, MAPHIGH);

            if (MapListInfo[mineYPos][mineXPos] != (int)BLOCKSTATE.MINE_BLOCK)
            {
                MapListInfo[mineYPos][mineXPos] = (int)BLOCKSTATE.MINE_BLOCK;
                BlockMapInfo[mineYPos][mineXPos] = minePrefabs;
                mine++;
            }

            if(mine == NumberOfMINE)
            {
                return;
            }
        }
    }
    #endregion
}
