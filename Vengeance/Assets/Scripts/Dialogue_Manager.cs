using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dialogue_Manager : MonoBehaviour {




    //The fked up way to do it:
    //Create a panel in UI and make that the dialogue box
    //Create a text UI for name display
    //Create another text UI for actual dialogue
    //image is broken/not quit working atm, need more work
    //arrow is for the little "go to next line" thing in UI

    //I havn't figured out how to end this thing when reach last line though, 
    //for now it's just an imaginary number that crushes afterwards

    //Removed audio crap for now, back up available

    GameObject loader;

    public string trigger; //section name
    
    public Text name;
    public Text conversation;
    public GameObject ImagePosition;
    private RawImage image;

    public Dictionary<string, List<Type_Dialogue>> Script = 
        new Dictionary<string, List<Type_Dialogue>>();

    int line_count = 0;
    int end_of_chapter;
    bool talking = true; 


    public GameObject Arrow;
    ///
    ///
    float wait_time = 0.1f;
    bool done = true;
    /// 
    /// </summary>
    /// 
    public GameObject panel;

   
    void Start () {
        
        //Get the panel object and shuts in in the beginning

        panel.SetActive(false);
        
        // find the loader who had the xml data
        loader = GameObject.FindGameObjectWithTag("Script_Data");
         Script =  loader.GetComponent<LoadXml>().sections;

        //set the protrait to null
        image = (RawImage)ImagePosition.GetComponent<RawImage>();
        image.texture = (Texture)Resources.Load("null");
        Arrow.SetActive(false);
        
        
        
    }

    private void OnEnable()
    {
        line_count = 0;
        name.text="";
        conversation.text = "";
        image = (RawImage)ImagePosition.GetComponent<RawImage>();
        image.texture = (Texture)Resources.Load("null");
        done = true;
        talking = true;

    }

    // Update is called once per frame
    void Update () {

        end_of_chapter = Script[trigger].Count;
        Debug.Log("count = " + Script[trigger].Count.ToString());
        if (done) //arrow display
        {
            Arrow.SetActive(true);
        }


        if (line_count >= end_of_chapter && done==true)
        {
            talking = false;
        }

        if (Input.GetMouseButtonDown(0) && talking == true)
        {

            if (!done)
            {
                wait_time = 0.04f;
                Arrow.SetActive(false);
            }
            if (done)
            {

                wait_time = 0.1f;
                name.text = Script[trigger][line_count].name;
                string processing = Script[trigger][line_count].text;
                Debug.Log(Script[trigger][line_count].icon);
                image.texture = (Texture)Resources.Load(Script[trigger][line_count].icon);
                StartCoroutine(PlayText(processing, conversation));
                Arrow.SetActive(false);
                line_count++;


            }

            //conversation.text = Script[line_count].text;


            //Debug.Log(line_count);
            //Debug.Log(end_of_chapter);
        }


        else if (Input.GetMouseButtonDown(0) && talking == false && done == true)
        {       
            panel.SetActive(false);
        }
    }


    IEnumerator PlayText(string story,Text conversation)
    {
        done = false;
        conversation.text = "";        
        string command_end = "</color>";
        string command_begin = "";
        bool color = false;

        string current;

        for (int i =0; i!= story.Length;i++)
        {
            
            if(story[i] == '<' && story[i+1] != '/')//if this is the beginning of a rich text, record the color
            { 
                command_begin = "";
                command_begin += story[i];
                int count = i + 1;
                while(story[count]!='>')
                {
                    command_begin += story[count];
                    count++;
                }//getting the color code

                command_begin += story[count];
                color = true; //begin coloring returns
                i = count + 1;
            }

            if (story[i] == '<' && story[i + 1] == '/') // if this is the end of rich text
            {
                i++; 
                int count =i;
                while (story[count] !='>')
                {
                    i++;
                    count++;
                }
                
                color = false;
            }


            if (story[i] == '>')
                continue;

            current = story[i].ToString();
            if (color == true)
             {
                string add = command_begin + story[i] + command_end;
                conversation.text += add;
          
             }
            else
             {
                conversation.text += story[i];
                
                
               }
           
            yield return new WaitForSeconds(wait_time);


        }

        done = true;          
    }
}
