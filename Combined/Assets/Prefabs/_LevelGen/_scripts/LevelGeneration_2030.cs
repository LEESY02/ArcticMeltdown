using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGeneration_2030 : MonoBehaviour
{
    // Coordinates
    public GameObject[] row1;
    public GameObject[] row2;
    public GameObject[] row3;
    public GameObject[] row4;

    // Borders
    public GameObject[] room_layout;

    // Interior layouts
    public GameObject[] top_layout;
    public GameObject[] bot_layout;

    // Fill in the borders
    void fill_all_rows(int[] open)
    {
        fillrow(4, 4, open[0]);
        for (int i = 3; i > 1; i -= 1)
        {
            fillrow(i, open[-i + 3], open[-i + 4]);
        }
        fillrow(1, open[2], 4);
    }
    void remaining_rooms_r1(int bottom, int top)
    {
        for (int i = 0; i < 4; i += 1)
        {
            if (i != bottom)
            {
                transform.position = row1[i].transform.position;
                if (i == 0)
                {
                    Instantiate(room_layout[5], transform.position, Quaternion.identity);
                }
                else if (i == 3)
                {
                    Instantiate(room_layout[3], transform.position, Quaternion.identity);
                }
                else
                {
                    Instantiate(room_layout[4], transform.position, Quaternion.identity);
                }
            }
        }
    }
    void remaining_rooms_r23(int bottom, int top, int row_num)
    {
        for (int i = 0; i < 4; i += 1)
        {
            if (i != top && i != bottom)
            {
                if (row_num == 2)
                {
                    transform.position = row2[i].transform.position;
                }
                else
                {
                    transform.position = row3[i].transform.position;
                }

                if (i == 0)
                {
                    Instantiate(room_layout[5], transform.position, Quaternion.identity);
                }
                else if (i == 3)
                {
                    Instantiate(room_layout[3], transform.position, Quaternion.identity);
                }
                else
                {
                    Instantiate(room_layout[4], transform.position, Quaternion.identity);
                }
            }
        }
    }
    void remaining_rooms_r4(int bottom, int top)
    {
        for (int i = 0; i < 4; i += 1)
        {
            if (i != top)
            {
                transform.position = row4[i].transform.position;
                if (i == 0)
                {
                    Instantiate(room_layout[5], transform.position, Quaternion.identity);
                }
                else if (i == 3)
                {
                    Instantiate(room_layout[3], transform.position, Quaternion.identity);
                }
                else
                {
                    Instantiate(room_layout[4], transform.position, Quaternion.identity);
                }
            }
        }
    }
    void fillrow(int row_num, int bottom, int top)
    {
        if (row_num == 4)
        {
            transform.position = row4[top].transform.position;
            if (top == 0)
            {
                //Fill in the position with opening
                Instantiate(room_layout[11], transform.position, Quaternion.identity);
                //Fill in the other positions
                remaining_rooms_r4(bottom, top);
            }
            else if (top == 3)
            {
                Instantiate(room_layout[9], transform.position, Quaternion.identity);
                remaining_rooms_r4(bottom, top);
            }
            else
            {
                Instantiate(room_layout[10], transform.position, Quaternion.identity);
                remaining_rooms_r4(bottom, top);
            }
        }
        else if (row_num == 1)
        {
            transform.position = row1[bottom].transform.position;
            if (bottom == 0)
            {
                //Fill in the position with opening
                Instantiate(room_layout[2], transform.position, Quaternion.identity);
                //Fill in the other positions
                remaining_rooms_r1(bottom, top);
            }
            else if (bottom == 3)
            {
                Instantiate(room_layout[0], transform.position, Quaternion.identity);
                remaining_rooms_r1(bottom, top);
            }
            else
            {
                Instantiate(room_layout[1], transform.position, Quaternion.identity);
                remaining_rooms_r1(bottom, top);
            }
        }
        else
        {
            if (row_num == 3)
            {
                if (top == bottom)
                {
                    transform.position = row3[bottom].transform.position;
                    if (bottom == 0)
                    {
                        Instantiate(room_layout[8], transform.position, Quaternion.identity);
                    }
                    else if (bottom == 3)
                    {
                        Instantiate(room_layout[6], transform.position, Quaternion.identity);
                    }
                    else
                    {
                        Instantiate(room_layout[7], transform.position, Quaternion.identity);
                    }
                }
                else
                {
                    //bottom
                    transform.position = row3[bottom].transform.position;
                    if (bottom == 0)
                    {
                        Instantiate(room_layout[2], transform.position, Quaternion.identity);
                    }
                    else if (bottom == 3)
                    {
                        Instantiate(room_layout[0], transform.position, Quaternion.identity);
                    }
                    else
                    {
                        Instantiate(room_layout[1], transform.position, Quaternion.identity);
                    }
                    //top
                    transform.position = row3[top].transform.position;
                    if (top == 0)
                    {
                        Instantiate(room_layout[11], transform.position, Quaternion.identity);
                    }
                    else if (top == 3)
                    {
                        Instantiate(room_layout[9], transform.position, Quaternion.identity);
                    }
                    else
                    {
                        Instantiate(room_layout[10], transform.position, Quaternion.identity);
                    }
                }
                //remaining
                remaining_rooms_r23(bottom, top, row_num);
            }
            else
            {
                if (top == bottom)
                {
                    transform.position = row2[bottom].transform.position;
                     if (bottom == 0)
                     {
                        Instantiate(room_layout[8], transform.position, Quaternion.identity);
                    } else if (bottom == 3)
                    {
                        Instantiate(room_layout[6], transform.position, Quaternion.identity);
                    } else
                    {
                        Instantiate(room_layout[7], transform.position, Quaternion.identity);
                    }
                    remaining_rooms_r23(bottom, top, row_num);
                }
                else
                {
                    //bottom
                    transform.position = row2[bottom].transform.position;
                    if (bottom == 0)
                    {
                        Instantiate(room_layout[2], transform.position, Quaternion.identity);
                    }
                    else if (bottom == 3)
                    {
                        Instantiate(room_layout[0], transform.position, Quaternion.identity);
                    }
                    else
                    {
                        Instantiate(room_layout[1], transform.position, Quaternion.identity);
                    }
                    //top
                    transform.position = row2[top].transform.position;
                    if (top == 0)
                    {
                        Instantiate(room_layout[11], transform.position, Quaternion.identity);
                    }
                    else if (top == 3)
                    {
                        Instantiate(room_layout[9], transform.position, Quaternion.identity);
                    }
                    else
                    {
                        Instantiate(room_layout[10], transform.position, Quaternion.identity);
                    }
                }
                //remaining
                remaining_rooms_r23(bottom, top, row_num);
            }
        }
    }

    // Fill in the interior

    void fill_interior(int x, int y)
    {
        // End case
        if (y == 5)
        {
            return;
        }

        // End of row
        if (x == 5)
        {
            fill_interior(1, y + 1);
            return;
        }

        // Fill one room
        fill_interior_once(x, y);

        // Recursive case
        fill_interior(x + 1, y);
    }

    void fill_interior_once(int x, int y)
    {
        // Get random layout
        int top = Random.Range(0, top_layout.Length);
        int bot = Random.Range(0, bot_layout.Length);

        // Get spawn location
        switch (y)
        {
            case 1:
                transform.position = row1[x - 1].transform.position;
                break;
            case 2:
                transform.position = row2[x - 1].transform.position;
                break;
            case 3:
                transform.position = row3[x - 1].transform.position;
                break;
            case 4:
                transform.position = row4[x - 1].transform.position;
                break;
            default:
                Debug.Log("In default (fill_interior_once)");
                break;
        }

        // Spawn random layouts
        Instantiate(top_layout[top], transform.position, Quaternion.identity);
        Instantiate(bot_layout[bot], transform.position, Quaternion.identity);
    }

    // Start is called before the first frame update
    void Start()
    {
        // Fill borders
        int[] open = { Random.Range(0, 4), Random.Range(0, 4), Random.Range(0, 4) };
        fill_all_rows(open);

        // Fill interior
        fill_interior(1,1);
    }
    /*
        // Update is called once per frame
        void Update()
        {

        }
    */
}
