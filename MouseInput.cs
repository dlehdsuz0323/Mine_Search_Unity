using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseInput : MonoBehaviour
{
    public enum GAMESTATE
    {
        GAME_PLAY,
        GAME_OVER,
        GAME_CLEAR

    }

    private static MouseInput instance;
    public static MouseInput GetInstance()
    {
        if (instance == null)
        {
            instance = new MouseInput();
        }
        return instance;
    }

    void Start ()
    {

	}
	
	void Update ()
    {
       // if (GameSuperVisor.GetInstance().GameState == (int)GAMESTATE.GAME_PLAY)
        { 
            BlockController();
        }
    }

    void BlockController()
    {
        #region MouseLeftClick
       if (GameSuperVisor.GetInstance().GameState == (int)GAMESTATE.GAME_PLAY)
        {
            if (Input.GetMouseButtonUp(0) /*&& Input.GetMouseButton(1)*/) // 0 왼쪽, 1 오른쪽, 2 휠
            {
                RaycastHit hit;
                Ray ray = GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider != null)
                    {
                        string name = hit.collider.gameObject.name;
                        int x;
                        int y;


                        x = (int)hit.transform.position.x;
                        y = (int)hit.transform.position.y;


                        if (hit.transform.tag == "CloseBlock")
                        {
                            for (int i = 0; i < BlockManager.GetInstance().MAPHIGH; i++)
                            {
                                for (int j = 0; j < BlockManager.GetInstance().MAPWIDTH; j++)
                                {
                                    if (hit.transform.position.x == BlockManager.GetInstance().BlockMapInfo[i][j].transform.position.x &&
                                       hit.transform.position.y == BlockManager.GetInstance().BlockMapInfo[i][j].transform.position.y)
                                    {
                                        BlockOpen(x, y);
                                        RepeatOpen(x, y);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            #endregion

            #region MouseRightClick
            if (Input.GetMouseButtonUp(1))
            {
                RaycastHit hit;
                Ray ray = GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider != null)
                    {
                        int x;
                        int y;

                        x = (int)hit.transform.position.x;
                        y = (int)hit.transform.position.y;

                        DrawingAndPutFlag(x, y);
                    }
                }
            }
            #endregion

            if (Input.GetMouseButton(0) && Input.GetMouseButton(1))
            {
                RaycastHit hit;
                Ray ray = GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit))
                {
                    int x;
                    int y;

                    x = (int)hit.transform.position.x;
                    y = (int)hit.transform.position.y;

                    if (hit.collider != null)
                    {
                        ClickedAroundOpen(x, y);
                    }
                }
            }
            ClearGame();
        }
    }

    void BlockOpen(int MouseXPos, int MouseYPos)
    {
        int x = BlockManager.GetInstance().MAPWIDTH;
        int y = BlockManager.GetInstance().MAPHIGH;


        {
            if(MouseXPos < x && MouseYPos < y)
            {
                BlockManager.GetInstance().CloseBlockMap[MouseXPos][MouseYPos].GetComponent<Rigidbody>().useGravity = true;
                BlockManager.GetInstance().CloseBlockMap[MouseXPos][MouseYPos].GetComponent<BoxCollider>().isTrigger = false;
                BlockManager.GetInstance().CloseBlockMap[MouseXPos][MouseYPos].GetComponent<Rigidbody>().AddForce(new Vector3(0, 0, Random.Range(-10,-1)), ForceMode.VelocityChange);
                //Destroy(BlockManager.GetInstance().CloseBlockMap[MouseXPos][MouseYPos]);
                BlockManager.GetInstance().CloseBlockMap[MouseXPos][MouseYPos] = null;
            }
        }

        GameObject go;

        go = BlockManager.GetInstance().BlockMapInfo[MouseXPos][MouseYPos];

        if (go.tag == BlockManager.GetInstance().minePrefabs.tag)
        {
            for (int i = 0; i < BlockManager.GetInstance().MAPHIGH; i++)
            { 
                for (int j = 0; j < BlockManager.GetInstance().MAPWIDTH; j++) 
                {
                    if (BlockManager.GetInstance().BlockMapInfo[i][j].CompareTag("MineBlock"))
                    {
                          BlockManager.GetInstance().CloseBlockMap[i][j].GetComponent<Rigidbody>().useGravity = true;
                         BlockManager.GetInstance().CloseBlockMap[i][j].GetComponent<BoxCollider>().isTrigger = false;
                         BlockManager.GetInstance().CloseBlockMap[i][j].GetComponent<Rigidbody>().AddForce(new Vector3(0, 0, Random.Range(-10, -1)), ForceMode.VelocityChange);
                       // Destroy(BlockManager.GetInstance().CloseBlockMap[i][j]);
                       if(BlockManager.GetInstance().CloseBlockMap[i][j] == null)
                        {
                            Debug.Log(i + j);
                        }
                    }
                }
            }
            //GameSuperVisor.GetInstance().GameState = (int)GAMESTATE.GAME_OVER;
            Debug.Log("Game Over");
        }
    }

    bool RepeatOpen(int MouseXPos, int MouseYPos)
    {
        if (BlockManager.GetInstance().BlockMapInfo[MouseXPos][MouseYPos].tag == "OpenBlock")
        {
            //StartCoroutine(WaitSecond());
            //↖
            if (MouseXPos > 0 && MouseYPos > 0)
            {
                if (BlockManager.GetInstance().CloseBlockMap[MouseXPos - 1][MouseYPos - 1] != null)
                {
                    if (BlockManager.GetInstance().CloseBlockMap[MouseXPos - 1][MouseYPos - 1].tag == "CloseBlock")
                    {
                        if (BlockManager.GetInstance().BlockMapInfo[MouseXPos - 1][MouseYPos - 1].tag == "OpenBlock")
                        {
                            BlockOpen(MouseXPos - 1, MouseYPos - 1);
                            RepeatOpen(MouseXPos - 1, MouseYPos - 1);
                        }

                        else if (0 < BlockManager.GetInstance().MapListInfo[MouseXPos - 1][MouseYPos - 1])
                        {
                            BlockOpen(MouseXPos - 1, MouseYPos - 1);
                        }
                    }
                }
            }


            //↑
            if (MouseYPos > 0)
            {
                if (BlockManager.GetInstance().CloseBlockMap[MouseXPos][MouseYPos - 1] != null)
                {
                    if (BlockManager.GetInstance().CloseBlockMap[MouseXPos][MouseYPos - 1].tag == "CloseBlock")
                    {
                        if (BlockManager.GetInstance().BlockMapInfo[MouseXPos][MouseYPos - 1].tag == "OpenBlock")
                        {
                            BlockOpen(MouseXPos, MouseYPos - 1);
                            RepeatOpen(MouseXPos, MouseYPos - 1);
                        }

                        else if (0 < BlockManager.GetInstance().MapListInfo[MouseXPos][MouseYPos - 1])
                        {
                            BlockOpen(MouseXPos, MouseYPos - 1);
                        }
                    }
                }
            }


            //↗
            if (MouseXPos < BlockManager.GetInstance().MAPWIDTH - 1 && MouseYPos > 0)
            {
                if (BlockManager.GetInstance().CloseBlockMap[MouseXPos + 1][MouseYPos - 1] != null)
                {
                    if (BlockManager.GetInstance().CloseBlockMap[MouseXPos + 1][MouseYPos - 1].tag == "CloseBlock")
                    {
                        if (BlockManager.GetInstance().BlockMapInfo[MouseXPos + 1][MouseYPos - 1].tag == "OpenBlock")
                        {
                            BlockOpen(MouseXPos + 1, MouseYPos - 1);
                            RepeatOpen(MouseXPos + 1, MouseYPos - 1);
                        }

                        else if (0 < BlockManager.GetInstance().MapListInfo[MouseXPos + 1][MouseYPos - 1])
                        {
                            BlockOpen(MouseXPos + 1, MouseYPos - 1);
                        }
                    }
                }
            }


            //→
            if (MouseXPos < BlockManager.GetInstance().MAPWIDTH - 1)
            {
                if (BlockManager.GetInstance().CloseBlockMap[MouseXPos + 1][MouseYPos] != null)
                {
                    if (BlockManager.GetInstance().CloseBlockMap[MouseXPos + 1][MouseYPos].tag == "CloseBlock")
                    {
                        if (BlockManager.GetInstance().BlockMapInfo[MouseXPos + 1][MouseYPos].tag == "OpenBlock")
                        {
                            BlockOpen(MouseXPos + 1, MouseYPos);
                            RepeatOpen(MouseXPos + 1, MouseYPos);
                        }

                        else if (0 < BlockManager.GetInstance().MapListInfo[MouseXPos + 1][MouseYPos])
                        {
                            BlockOpen(MouseXPos + 1, MouseYPos);
                        }
                    }
                }
            }


            // ↘
            if (MouseXPos < BlockManager.GetInstance().MAPWIDTH - 1 && MouseYPos < BlockManager.GetInstance().MAPHIGH - 1)
            {
                if (BlockManager.GetInstance().CloseBlockMap[MouseXPos + 1][MouseYPos + 1] != null)
                {
                    if (BlockManager.GetInstance().CloseBlockMap[MouseXPos + 1][MouseYPos + 1].tag == "CloseBlock")
                    {
                        if (BlockManager.GetInstance().BlockMapInfo[MouseXPos + 1][MouseYPos + 1].tag == "OpenBlock")
                        {
                            BlockOpen(MouseXPos + 1, MouseYPos + 1);
                            RepeatOpen(MouseXPos + 1, MouseYPos + 1);
                        }

                        else if (0 < BlockManager.GetInstance().MapListInfo[MouseXPos + 1][MouseYPos + 1])
                        {
                            BlockOpen(MouseXPos + 1, MouseYPos + 1);
                        }
                    }
                }
            }


            //↓
            if (MouseYPos < BlockManager.GetInstance().MAPHIGH - 1)
            {
                if (BlockManager.GetInstance().CloseBlockMap[MouseXPos][MouseYPos + 1] != null)
                {
                    if (BlockManager.GetInstance().CloseBlockMap[MouseXPos][MouseYPos + 1].tag == "CloseBlock")
                    {
                        if (BlockManager.GetInstance().BlockMapInfo[MouseXPos][MouseYPos + 1].tag == "OpenBlock")
                        {
                            BlockOpen(MouseXPos, MouseYPos + 1);
                            RepeatOpen(MouseXPos, MouseYPos + 1);
                        }

                        else if (0 < BlockManager.GetInstance().MapListInfo[MouseXPos][MouseYPos + 1])
                        {
                            BlockOpen(MouseXPos, MouseYPos + 1);
                        }
                    }
                }
            }


            // ↙
            if (MouseXPos > 0 && MouseYPos < BlockManager.GetInstance().MAPHIGH - 1)
            {
                if (BlockManager.GetInstance().CloseBlockMap[MouseXPos - 1][MouseYPos + 1] != null)
                {
                    if (BlockManager.GetInstance().CloseBlockMap[MouseXPos - 1][MouseYPos + 1].tag == "CloseBlock")
                    {
                        if (BlockManager.GetInstance().BlockMapInfo[MouseXPos - 1][MouseYPos + 1].tag == "OpenBlock")
                        {
                            BlockOpen(MouseXPos - 1, MouseYPos + 1);
                            RepeatOpen(MouseXPos - 1, MouseYPos + 1);
                        }

                        else if (0 < BlockManager.GetInstance().MapListInfo[MouseXPos - 1][MouseYPos + 1])
                        {
                            BlockOpen(MouseXPos - 1, MouseYPos + 1);
                        }
                    }
                }
            }


            //←  
            if (MouseXPos > 0)
            {
                if (BlockManager.GetInstance().CloseBlockMap[MouseXPos - 1][MouseYPos] != null)
                {
                    if (BlockManager.GetInstance().CloseBlockMap[MouseXPos - 1][MouseYPos].tag == "CloseBlock")
                    {
                        if (BlockManager.GetInstance().BlockMapInfo[MouseXPos - 1][MouseYPos].tag == "OpenBlock")
                        {
                            BlockOpen(MouseXPos - 1, MouseYPos);
                            RepeatOpen(MouseXPos - 1, MouseYPos);
                        }

                        else if (0 < BlockManager.GetInstance().MapListInfo[MouseXPos - 1][MouseYPos])
                        {
                            BlockOpen(MouseXPos - 1, MouseYPos);
                        }
                    }
                }
            }
        }

        return true;
    }

    void DrawingAndPutFlag(int MouseXPos, int MouseYPos)
    {
        if (BlockManager.GetInstance().CloseBlockMap[MouseXPos][MouseYPos] == null)
        {
            return;
        }

        else if(BlockManager.GetInstance().CloseBlockMap[MouseXPos][MouseYPos].tag == "CloseBlock")
        {
            Destroy(BlockManager.GetInstance().CloseBlockMap[MouseXPos][MouseYPos]);
            BlockManager.GetInstance().CloseBlockMap[MouseXPos][MouseYPos] = Instantiate(BlockManager.GetInstance().flagPrefabs,
                new Vector3(MouseXPos, MouseYPos,0 ), Quaternion.Euler(new Vector3(90, 0, 180)));
        }

        else if (BlockManager.GetInstance().CloseBlockMap[MouseXPos][MouseYPos].tag == "FlagBlock")
        {
            Destroy(BlockManager.GetInstance().CloseBlockMap[MouseXPos][MouseYPos]);
            BlockManager.GetInstance().CloseBlockMap[MouseXPos][MouseYPos] = Instantiate(BlockManager.GetInstance().CloseBlockPrefabs,
                new Vector3(MouseXPos, MouseYPos, 0), Quaternion.Euler(new Vector3(90, 0, 180)));
        }
    }

    int FindFlag(int MouseXPos, int MouseYPos)
    {
        int iFlagAroundNum = 0;

        if (BlockManager.GetInstance().CloseBlockMap[MouseXPos][MouseYPos] == null)
        {
            if (MouseXPos > 0 && MouseYPos > 0)
            {
                if(BlockManager.GetInstance().CloseBlockMap[MouseXPos - 1][MouseYPos - 1] == null)
                {
                    iFlagAroundNum += 0;
                }

                else if (BlockManager.GetInstance().CloseBlockMap[MouseXPos - 1][MouseYPos - 1].tag == "FlagBlock")
                {
                    iFlagAroundNum += 1;
                }
            }

            if (MouseYPos > 0)
            {
                if (BlockManager.GetInstance().CloseBlockMap[MouseXPos][MouseYPos - 1] == null)
                {
                    iFlagAroundNum += 0;
                }

                else if (BlockManager.GetInstance().CloseBlockMap[MouseXPos][MouseYPos - 1].tag == "FlagBlock")
                {
                    iFlagAroundNum += 1;
                }
            }

            if (MouseXPos < BlockManager.GetInstance().MAPWIDTH - 1 && MouseYPos > 0)
            {
                if (BlockManager.GetInstance().CloseBlockMap[MouseXPos + 1][MouseYPos - 1] == null)
                {
                    iFlagAroundNum += 0;
                }

                else if (BlockManager.GetInstance().CloseBlockMap[MouseXPos + 1][MouseYPos - 1].tag == "FlagBlock") 
                {
                    iFlagAroundNum += 1;
                }
            }

            if (MouseXPos < BlockManager.GetInstance().MAPWIDTH - 1)
            {
                if (BlockManager.GetInstance().CloseBlockMap[MouseXPos + 1][MouseYPos] == null)
                {
                    iFlagAroundNum += 0;
                }

                else if (BlockManager.GetInstance().CloseBlockMap[MouseXPos + 1][MouseYPos].tag == "FlagBlock") 
                {
                    iFlagAroundNum += 1;
                }
            }

            if (MouseXPos < BlockManager.GetInstance().MAPWIDTH - 1 && MouseYPos < BlockManager.GetInstance().MAPHIGH - 1)
            {
                if (BlockManager.GetInstance().CloseBlockMap[MouseXPos + 1][MouseYPos + 1] == null)
                {
                    iFlagAroundNum += 0;
                }

                else if (BlockManager.GetInstance().CloseBlockMap[MouseXPos + 1][MouseYPos + 1].tag == "FlagBlock")
                {
                    iFlagAroundNum += 1;
                }
            }

            if (MouseYPos < BlockManager.GetInstance().MAPHIGH - 1)
            {
                if (BlockManager.GetInstance().CloseBlockMap[MouseXPos][MouseYPos + 1] == null)
                {
                    iFlagAroundNum += 0;
                }

                else if (BlockManager.GetInstance().CloseBlockMap[MouseXPos][MouseYPos + 1].tag == "FlagBlock")
                {
                    iFlagAroundNum += 1;
                }
            }

            if (MouseXPos > 0 && MouseYPos < BlockManager.GetInstance().MAPHIGH - 1)
            {
                if (BlockManager.GetInstance().CloseBlockMap[MouseXPos - 1][MouseYPos + 1] == null)
                {
                    iFlagAroundNum += 0;
                }

                else if (BlockManager.GetInstance().CloseBlockMap[MouseXPos - 1][MouseYPos + 1].tag == "FlagBlock")
                {
                    iFlagAroundNum += 1;
                }
            }

            if (MouseXPos > 0)
            {
                if (BlockManager.GetInstance().CloseBlockMap[MouseXPos - 1][MouseYPos] == null)
                {
                    iFlagAroundNum += 0;
                }

                else if (BlockManager.GetInstance().CloseBlockMap[MouseXPos - 1][MouseYPos].tag == "FlagBlock")
                {
                    iFlagAroundNum += 1;
                }
            }

        }

        //Debug.Log(iFlagAroundNum);
            return iFlagAroundNum;
    }

    int ClickedAroundOpen(int MouseXPos, int MouseYPos)
    {
        int i = 0;
        if (FindFlag(MouseXPos, MouseYPos) == BlockManager.GetInstance().MapListInfo[MouseXPos][MouseYPos])
        {
            //↖
            if (MouseXPos > 0 && MouseYPos > 0)
            {
                if (BlockManager.GetInstance().CloseBlockMap[MouseXPos - 1][MouseYPos - 1] == null)
                {
                    i++;
                }
                else if (BlockManager.GetInstance().CloseBlockMap[MouseXPos - 1][MouseYPos - 1].tag != "FlagBlock")
                {
                    BlockOpen(MouseXPos - 1, MouseYPos - 1);
                    RepeatOpen(MouseXPos - 1, MouseYPos - 1);
                }
            }

            //↑
            if (MouseYPos > 0)
            {
                if (BlockManager.GetInstance().CloseBlockMap[MouseXPos][MouseYPos - 1] == null)
                {
                    i++;
                }
                else if (BlockManager.GetInstance().CloseBlockMap[MouseXPos][MouseYPos - 1].tag != "FlagBlock")
                {
                    BlockOpen(MouseXPos, MouseYPos - 1);
                    RepeatOpen(MouseXPos, MouseYPos - 1);
                }
            }

            //↗
            if (MouseXPos < BlockManager.GetInstance().MAPWIDTH - 1 && MouseYPos > 0)
            {
                if (BlockManager.GetInstance().CloseBlockMap[MouseXPos + 1][MouseYPos - 1] == null)
                {
                    i++;
                }
                else if (BlockManager.GetInstance().CloseBlockMap[MouseXPos + 1][MouseYPos - 1].tag != "FlagBlock")
                {
                    BlockOpen(MouseXPos + 1, MouseYPos - 1);
                    RepeatOpen(MouseXPos + 1, MouseYPos - 1);
                }
            }

            //→
            if (MouseXPos < BlockManager.GetInstance().MAPWIDTH - 1)
            {
                if (BlockManager.GetInstance().CloseBlockMap[MouseXPos + 1][MouseYPos] == null)
                {
                    i++;
                }
                else if (BlockManager.GetInstance().CloseBlockMap[MouseXPos + 1][MouseYPos].tag != "FlagBlock")
                {
                    BlockOpen(MouseXPos + 1, MouseYPos);
                    RepeatOpen(MouseXPos + 1, MouseYPos);
                }
            }

            // ↘
            if (MouseXPos < BlockManager.GetInstance().MAPWIDTH - 1 && MouseYPos < BlockManager.GetInstance().MAPHIGH - 1)
            {
                if (BlockManager.GetInstance().CloseBlockMap[MouseXPos + 1][MouseYPos + 1] == null)
                {
                    i++;
                }
                else if (BlockManager.GetInstance().CloseBlockMap[MouseXPos + 1][MouseYPos + 1].tag != "FlagBlock")
                {
                    BlockOpen(MouseXPos + 1, MouseYPos + 1);
                    RepeatOpen(MouseXPos + 1, MouseYPos + 1);
                }
            }

            //↓
            if (MouseYPos < BlockManager.GetInstance().MAPHIGH - 1)
            {
                if (BlockManager.GetInstance().CloseBlockMap[MouseXPos][MouseYPos + 1] == null)
                {
                    i++;
                }
                else if (BlockManager.GetInstance().CloseBlockMap[MouseXPos][MouseYPos + 1].tag != "FlagBlock")
                {
                    BlockOpen(MouseXPos, MouseYPos + 1);
                    RepeatOpen(MouseXPos, MouseYPos + 1);
                }
            }

            // ↙
            if (MouseXPos > 0 && MouseYPos < BlockManager.GetInstance().MAPHIGH - 1)
            {
                if (BlockManager.GetInstance().CloseBlockMap[MouseXPos - 1][MouseYPos + 1] == null)
                {
                    i++;
                }
                else if (BlockManager.GetInstance().CloseBlockMap[MouseXPos - 1][MouseYPos + 1].tag != "FlagBlock")
                {
                    BlockOpen(MouseXPos - 1, MouseYPos + 1);
                    RepeatOpen(MouseXPos - 1, MouseYPos + 1);
                }
            }

            //←  
            if (MouseXPos > 0)
            {
                if (BlockManager.GetInstance().CloseBlockMap[MouseXPos - 1][MouseYPos] == null)
                {
                    i++;
                }
                else if (BlockManager.GetInstance().CloseBlockMap[MouseXPos - 1][MouseYPos].tag != "FlagBlock")
                {
                    BlockOpen(MouseXPos - 1, MouseYPos);
                    RepeatOpen(MouseXPos - 1, MouseYPos);
                }
            }
        }
        return 0;
    }


    void ClearGame()
    {
        int iCloseBlockNum = 0;
        int iGameClearnullBlock = 0;

        for (int i = 0; i < BlockManager.GetInstance().MAPHIGH; i++)
        {
            for(int j = 0; j< BlockManager.GetInstance().MAPWIDTH; j++)
            {
                if(BlockManager.GetInstance().CloseBlockMap[i][j] == null)
                {
                    iCloseBlockNum += 1;
                }


                iGameClearnullBlock = (BlockManager.GetInstance().MAPHIGH * BlockManager.GetInstance().MAPWIDTH)
                                       - BlockManager.GetInstance().NumberOfMINE;

                if (iCloseBlockNum == iGameClearnullBlock)
                {
                    //Debug.Log("GAME CLEAR");
                }
            }
        }
    }

    IEnumerator WaitSecond()
    {
        yield return new WaitForSeconds(0.1f);
    }
}