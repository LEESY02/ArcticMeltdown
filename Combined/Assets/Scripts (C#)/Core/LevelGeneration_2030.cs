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

    // Exit
    public GameObject exit_layout;

    // Traps
    public GameObject[] trap_layout;


    // Fill in the borders
    void fill_all_rows(int[] open)
    {
        fillrow(1, open[0], 4); // r1
        for (int i = 2; i < 4; i += 1)
        {
            fillrow(i, open[i - 1], open[i - 2]);
        }
        fillrow(4, 4, open[2]); // r4

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
    void fill_interior(int row, int col, int exit)
    {
        // End case
        if (row == 5)
        {
            return;
        }

        // End of row
        if (col == 5)
        {
            fill_interior(row + 1, 1, exit);
            return;
        }

        // Fill one room
        fill_interior_once(row, col, exit);

        // Recursive case
        fill_interior(row, col + 1, exit);
    }
    void fill_interior_once(int row, int col, int exit)
    {
        // Exit room?
        if (col == exit + 1 && row == 1)
        {           
            transform.position = row1[col - 1].transform.position;
            Instantiate(exit_layout, transform.position, Quaternion.identity);
        }
        else
        {
            // Get random layout
            int top = Random.Range(0, top_layout.Length);
            int bot = Random.Range(0, bot_layout.Length);

            // Get spawn location
            switch (row)
            {
                case 1:
                    transform.position = row1[col - 1].transform.position;
                    break;
                case 2:
                    transform.position = row2[col - 1].transform.position;
                    break;
                case 3:
                    transform.position = row3[col - 1].transform.position;
                    break;
                case 4:
                    transform.position = row4[col - 1].transform.position;
                    break;
                default:
                    Debug.Log("In default (fill_interior_once)");
                    break;
            }

            // Spawn random layouts
            Instantiate(top_layout[top], transform.position, Quaternion.identity);
            Instantiate(bot_layout[bot], transform.position, Quaternion.identity);
        }
    }

    // Fill in the traps
    void fill_trap(int row, int col, int exit, int[] open)
    {
        // End case
        if (row == 5)
        {
            return;
        }

        // End of row
        if (col == 5)
        {
            fill_trap(row + 1, 1, exit, open);
            return;
        }

        if (!(row == 1 && col == exit + 1) && !(row < 4 && col == open[row - 1] + 1) && !(row == 4 && col == 1)) 
        { // Not in exit room || No opening on the floor || Not in Start room
            // Debug.Log("Generating trap");
            // Debug.Log(row); 
            // Debug.Log(col);
            int layout = Random.Range(0, trap_layout.Length);
            switch (row)
            {
                case 1:
                    transform.position = row1[col - 1].transform.position;
                    break;
                case 2:
                    transform.position = row2[col - 1].transform.position;
                    break;
                case 3:
                    transform.position = row3[col - 1].transform.position;
                    break;
                case 4:
                    transform.position = row4[col - 1].transform.position;
                    break;
                default:
                    Debug.Log("In default (fill_trap)");
                    break;
            }
            if (layout == 0 || layout == 1) // if is firetrap, spawn slightly lower into the ground
            {
                Instantiate(trap_layout[layout], 
                    new Vector3(
                        transform.position.x,
                        transform.position.y - 0.5f,
                        transform.position.z
                    ), 
                    Quaternion.identity);
            } else {
                Instantiate(trap_layout[layout], transform.position, Quaternion.identity);
            }
        }

        fill_trap(row, col + 1, exit, open);
    }

    // Start is called before the first frame update
    void Start()
    {
        // Fill borders
        int[] open = { Random.Range(0, 4), Random.Range(0, 4), Random.Range(0, 4) };
        fill_all_rows(open);

        // Fill interior
        int exit = Random.Range(0, 4);
        if (exit == open[0]) // Reassign exit if it collides with the opening
        {
            if (open[0] == 0)
            {
                exit += Random.Range(1, 4);
            } else if (open[0] == 3)
            {
                exit -= Random.Range(1, 4);
            } else
            {
                int plusMinus = Random.Range(0, 2);
                if (plusMinus == 0)
                {
                    exit += 1;
                } else
                {
                    exit -= 1;
                }
            }
        }
        /*
        Debug.Log("Openings: \n");
        Debug.Log(open[0]);
        Debug.Log(open[1]); 
        Debug.Log(open[2]);
        Debug.Log("Exit: \n");
        Debug.Log(exit);
        */
        fill_interior(1,1, exit);

        // Fill Traps
        fill_trap(1, 1, exit, open);
    }
    /*
        // Update is called once per frame
        void Update()
        {

        }
    */
}
