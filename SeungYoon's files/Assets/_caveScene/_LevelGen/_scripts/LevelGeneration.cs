using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGeneration : MonoBehaviour
{
    public GameObject[] row1;
    public GameObject[] row2;
    public GameObject[] row3;
    public GameObject[] row4;
    public GameObject[] room_layout;
    /*
    static int opening12 = Random.Range(0, 3);
    static int opening23 = Random.Range(0, 3);
    static int opening34 = Random.Range(0, 3);
    */

        //{opening12, opening23, opening34};

    void fill_all_rows(int[] open) 
    {
        fillrow(1, 4, open[0]);
        for(int i = 2; i < 4; i += 1) {
            fillrow(i, open[i - 2], open[i - 1]);    
        }
        fillrow(4, open[2], 4);
    }

    void remaining_rooms_r1(int bottom, int top)
    {
        for (int i = 0; i < 4; i += 1)
        {
            if (i != top)
            {
                transform.position = row1[i].transform.position;
                if (i == 0) {
                    Instantiate(room_layout[5], transform.position, Quaternion.identity);
                } else if (i == 3) {
                    Instantiate(room_layout[3], transform.position, Quaternion.identity);
                } else
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
            if (i != bottom)
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
        if (row_num == 1)
        {
            transform.position = row1[top].transform.position;
            if (top == 0) {
                //Fill in the position with opening
                Instantiate(room_layout[9], transform.position, Quaternion.identity);
                //Fill in the other positions
                remaining_rooms_r1(bottom, top);
            } else if (top == 3)
            {
                Instantiate(room_layout[7], transform.position, Quaternion.identity);
                remaining_rooms_r1(bottom, top);
            } else
            {
                Instantiate(room_layout[8], transform.position, Quaternion.identity);
                remaining_rooms_r1(bottom, top);
            }
        } else if (row_num == 4)
        {
            transform.position = row4[bottom].transform.position;
            if (bottom == 0)
            {
                //Fill in the position with opening
                Instantiate(room_layout[2], transform.position, Quaternion.identity);
                //Fill in the other positions
                remaining_rooms_r4(bottom, top);
            }
            else if (bottom == 3)
            {
                Instantiate(room_layout[0], transform.position, Quaternion.identity);
                remaining_rooms_r4(bottom, top);
            }
            else
            {
                Instantiate(room_layout[1], transform.position, Quaternion.identity);
                remaining_rooms_r4(bottom, top);
            }
        }  
        else 
        {
            if (row_num == 2)
            {
                if (top == bottom)
                {
                    transform.position = row2[bottom].transform.position;
                    Instantiate(room_layout[6], transform.position, Quaternion.identity);
                } else
                {
                    //bottom
                    transform.position = row2[bottom].transform.position;
                    Instantiate(room_layout[1], transform.position, Quaternion.identity);
                    //top
                    transform.position = row2[top].transform.position;
                    Instantiate(room_layout[8], transform.position, Quaternion.identity);
                }
                //remaining
                remaining_rooms_r23(bottom, top, row_num);
            } else
            {
                if (top == bottom)
                {
                    transform.position = row3[bottom].transform.position;
                    Instantiate(room_layout[6], transform.position, Quaternion.identity);
                    remaining_rooms_r23(bottom, top, row_num);
                } else
                {
                    //bottom
                    transform.position = row3[bottom].transform.position;
                    Instantiate(room_layout[1], transform.position, Quaternion.identity);
                    //top
                    transform.position = row3[top].transform.position;
                    Instantiate(room_layout[8], transform.position, Quaternion.identity);
                }
                //remaining
                remaining_rooms_r23(bottom, top, row_num);
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        int[] open = { Random.Range(0, 4), Random.Range(0, 4), Random.Range(0, 4) };
        fill_all_rows(open);

    }
    /*
        // Update is called once per frame
        void Update()
        {

        }
    */
}
