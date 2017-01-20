using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;

public class DialogueHandler : MonoBehaviour {

    //NOTES FOR EVENTUAL FEATURES
    /*
     * DIALOGUE STYLES:
     *    Fastforward (Press B to forward)
     *    Automated proceed (Proceed after given amount of time)
     *    Forced to wait (...)
     *    
     */

    static float DEFAULT_WIDTH = 400f;
    static float DEFAULT_HEIGHT = 400f;

    //For making a dictionary for Portraits
    [System.Serializable]
    public class PortraitDict
    {
        public string name;
        public Sprite img;
        public float width;
        public float height;
        public float xAdjust;
        public float yAdjust;
    }

    //For making a dictionary for background images
    [System.Serializable]
    public class BgEntry
    {
        public string name;
        public Sprite image;
    }

    [System.Serializable]
    public class ItemEntry
    {
        public string name;
        public Sprite image;
        public string description;
        public bool unlocked;
    }

    //External scripts
    //AudioManager audioCS;

    //UI Objects for Dialogue
    public GameObject dialogueUI;                               //Dialogue UI
    public GameObject backgroundImage;                          //Background image
    public GameObject itemImage;                                //Image for Item received
    public Image leftPortrait;                                  //Portrait of left-side character
    public Image rightPortrait;                                 //Portrait of right-side character
    public GameObject leftNameTag;                              //Name tag for left character
    public GameObject rightNameTag;                             //Name tag for right character
    public GameObject dialogueUIPanel;                          //Dialogue Panel
    public Text dialogueUIText;                                 //Dialogue Text
    public GameObject skipUIPanel;                              //Skip Dialogue Panel

    //Dialogue text and portraits
    public TextAsset[] dialogueText;                            //Array of dialogue text for scene
    public PortraitDict[] spritePorts;                          //Array of sprite portraits
    public BgEntry[] bgEntries;                                 //Array of backgrounds
    public ItemEntry[] itemEntries;                             //Array of item elements

    //Dictionaries
    private Dictionary<string, PortraitDict> portDict;          //Dictionary of sprite portraits
    private Dictionary<string, Sprite> bgDict;                  //Dictionary of background images
    private Dictionary<string, ItemEntry> itemDict;             //Dictionary of item imagges
  
    //Private variables for controlling dialogue display
    private int currentLine;                                    //Current line being read
    private int currentAsset;                                   //Current text asset being used
    private string[] dialogueArray;                             //Array storing dialogue to be read
    private int textShown;                                      //Amount of text of current line shown
    private bool isTyping;                                      //Determines if current dialogue still being typed

    //Colors for when portraits are speaking or not speaking
    static Color SPEAKING_COLOR = new Color(1f, 1f, 1f);        //Brights character portrait when speaking
    static Color FADE_COLOR = new Color(.5f, .5f, .5f);         //Darkens character when they are not speaking

    //Animation Controllers
    delegate void AnimFunc(RectTransform port, Vector2 returnPos);  //For storing animation functions
    AnimFunc leftAnimFuncPtr;                                   //Points at animation function for left side
    AnimFunc rightAnimFuncPtr;                                  //Points at animation function for right side
    private Vector2 lPortPos;                                   //Stores left portrait position
    private Vector2 rPortPos;                                   //Stores right portrait position
    private Vector2 lDefaultPos;                                //Defaults left portrait position with basic dimensions
    private Vector2 rDefaultPos;                                //Default right portrait position with basic dimensions
    private bool LeftIsAnimating;                               //Determine if animating left portrait
    private bool RightIsAnimating;                              //Determine if animating right portrait
    private float bounceHeight = (3f / 8f) * Screen.height;

    private bool typing = false;

    //Change for fine tuning
    KeyCode proceedKey = KeyCode.C;                             //Proceed with dialogue at normal speed
    KeyCode skipKey = KeyCode.Return;                           //Bring up skip scene panel
    KeyCode cardqueueKey = KeyCode.Z;

    void Awake(){
        //audioCS = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        lDefaultPos = lPortPos = leftPortrait.rectTransform.anchoredPosition;
        rDefaultPos = rPortPos = rightPortrait.rectTransform.anchoredPosition;
        ConstructDicts();
        Reset();
    }

    void Update(){
        RunAnimations();
        if (!LeftIsAnimating && !RightIsAnimating && dialogueUI.activeSelf && !skipUIPanel.activeSelf){
            WatchForProceed();
        }
        WatchForCardQueue();
        WatchForSkipButton();
    }

    void FixedUpdate(){
        if (!LeftIsAnimating && !RightIsAnimating && dialogueUI.activeSelf && typing){
            TypingLine();
        }
    }

    void RunAnimations()
    {
        if (LeftIsAnimating)
            leftAnimFuncPtr(leftPortrait.rectTransform, lPortPos);
        if (RightIsAnimating)
            rightAnimFuncPtr(rightPortrait.rectTransform, rPortPos);
    }

    //Constructs dictionary of elements used by Dialogue
    void ConstructDicts()
    {
        portDict = new Dictionary<string, PortraitDict>();
        bgDict = new Dictionary<string, Sprite>();
        itemDict = new Dictionary<string, ItemEntry>();

        foreach (PortraitDict entry in spritePorts){
            portDict[entry.name] = entry;
        }

        foreach (BgEntry entry in bgEntries){
            bgDict[entry.name] = entry.image;
        }

        foreach (ItemEntry entry in itemEntries)
        {
            itemDict[entry.name] = entry;
        }
    }

    //Resets values that need to be reset by beginning of each dialogue
    void Reset(){
        Time.timeScale = 1.0f;
        textShown = 0;
        LeftIsAnimating = false;
        RightIsAnimating = false;
        ClearActors();
        skipUIPanel.SetActive(false);
        itemImage.SetActive(false);
    }

    //Clears all name tags and characters from scene for narration or to reset scene
    void ClearActors(){
        DeactiveCharPortrait(leftPortrait, leftNameTag);
        DeactiveCharPortrait(rightPortrait, rightNameTag);
    }

    void DeactiveCharPortrait(Image portrait, GameObject nameTag){
        portrait.gameObject.SetActive(false);
        nameTag.SetActive(false);
    }

    //Watches for input to activate or deactivate skip panel
    void WatchForSkipButton(){
        if (Input.GetKeyDown(skipKey) && !skipUIPanel.activeSelf){
            skipUIPanel.SetActive(true);
            Time.timeScale = 0.0f;
        }
        else if (Input.GetKeyDown(skipKey) && skipUIPanel.activeSelf){
            CancelSkip();
        }
    }

    //Cancel skip action
    public void CancelSkip(){
        skipUIPanel.SetActive(false);
        Time.timeScale = 1.0f;
    }

    public void setCurrentLine(int lineNum)
    {
        currentLine = lineNum;
    }

    //First line should always be a command or have [] and a blank line underneath it
    void BeginRead(){
        typing = true;
        currentLine = 0;
        InterpretTxtCommands();
        currentLine = 2;
    }

    //Loads text to be read to current dialogue 
    public void LoadTextAsset(int index){
        dialogueUI.SetActive(true);
        currentAsset = index;
        dialogueArray = dialogueText[index].ToString().Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);
        BeginRead();
    }
    

    //Loads portrait images
    void LoadPortraitImage(Image portrait, string key, ref Vector2 portPos, Vector2 defaultPos, int side){
        if (key != "None"){
            PortraitDict dimensions = new PortraitDict();
            dimensions = portDict[key];
            portrait.rectTransform.sizeDelta = new Vector2(dimensions.width, dimensions.height);
            portPos = portrait.rectTransform.anchoredPosition = new Vector2(defaultPos.x + dimensions.xAdjust * (float)side, defaultPos.y+dimensions.yAdjust);
            portrait.sprite = portDict[key].img;
            portrait.gameObject.SetActive(true);
        }
        else{
            portrait.gameObject.SetActive(false);
        }
    }

    //Types line to be displayed
   public void TypingLine(){
        Debug.Log(textShown);
        int length = dialogueArray[currentLine].Length;
        textShown += 1;
        textShown = Math.Min(length, textShown);
        dialogueUIText.text = dialogueArray[currentLine].Substring(0, textShown);
        isTyping = !(textShown == length);
    }

    void WatchForProceed(){
        if (Input.GetKeyDown(proceedKey)&&!isTyping){
            itemImage.SetActive(false);
            ProceedDialogue();
        }
        else if(Input.GetKeyDown(proceedKey)&&isTyping){
            textShown = dialogueArray[currentLine].Length;
        }
    }

    void WatchForCardQueue()
    {
        if (Input.GetKeyDown(cardqueueKey)&&!isTyping)
        {
            Debug.Log("Deck summoned");

        }
        else if(Input.GetKeyDown(cardqueueKey) && isTyping)
        {
            Debug.Log("Deck summoned, please wait until typing has finished");
        }
        
    }
    

    //Execute commands given from currenty examined line of text
    void InterpretTxtCommands(){
        
        string examinedLine = dialogueArray[currentLine];
        if (examinedLine.Length > 2){
            string[] commands = examinedLine.Substring(1, examinedLine.Length - 2).Split(new[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string str in commands){
                string[] commandInput = str.Split(new[] { ':', ' ' }, StringSplitOptions.RemoveEmptyEntries);
              
                ExecuteTxtCommand(commandInput);
            }
        }
        currentLine++;
    }

    //Executes given text command
    void ExecuteTxtCommand(string[] command){
        switch (command[0]){
            //SET PORTRAIT IMAGES
            case("LPort"): LoadPortraitImage(leftPortrait, command[1],ref lPortPos,lDefaultPos,1);
                break;
            case("RPort"): LoadPortraitImage(rightPortrait, command[1],ref rPortPos,rDefaultPos,-1);
                break;
            //SET NAME TAGS
            case("LName"):
                leftNameTag.SetActive(true);
                leftNameTag.transform.GetChild(0).GetComponent<Text>().text = command[1];
                break;
            case ("RName"):
                rightNameTag.SetActive(true);
                rightNameTag.transform.GetChild(0).GetComponent<Text>().text = command[1];
                break;
            //SET SPEAKING STATE
            case ("LSpeaking"):
                if (command[1] == "T") { setCharSpeaker(leftPortrait, leftNameTag); }
                else{ setCharListener(leftPortrait, leftNameTag); }
                break;
            case ("RSpeaking"):
                if (command[1] == "T") { setCharSpeaker(rightPortrait, rightNameTag); }
                else{ setCharListener(rightPortrait, rightNameTag); }
                break;
            case("Anim"):
                ExecuteAnim(command[1]);
                break;
            case("Music"):
                //audioCS.PlaySound(int.Parse(command[1]));
                break;
            case ("BG"):
                SetBackground(command[1]);
                break;
            case ("Item"):
                ShowItem(command[1]);
                break;
            case ("Clear"):
                ClearActors();
                break;
            default:
                break;
        }
    }

    //Proceeds with Dialogue
    void ProceedDialogue(){
            textShown = 0;
            currentLine ++;
            if (currentLine >= dialogueArray.Length){
                EndDialogue();
                return;
            }
            if (currentLine < dialogueArray.Length){
                if (String.IsNullOrEmpty(dialogueArray[currentLine]))
                    ProceedDialogue();
                else if (dialogueArray[currentLine][0] == '['){
                    InterpretTxtCommands();
                    ProceedDialogue();
                }
            }
    }

    //Gives character active color and reveals their nameplate
    void setCharSpeaker(Image portrait, GameObject namePanel){
        portrait.color = SPEAKING_COLOR;
        namePanel.SetActive(true);
    }

    void setCharListener(Image portrait, GameObject namePanel){
        portrait.color = FADE_COLOR;
        namePanel.SetActive(false);
    }

    //Ends current dialogue display
    public void EndDialogue(){
        Reset();
        dialogueUI.SetActive(false);
    }

    /*
     * ANIMATION FUNCTIONS
     */ 

    //Executes animations
    void ExecuteAnim(string command)
    {
        if (command[0] == 'L')
            LeftIsAnimating = true;
        else if (command[0] == 'R')
            RightIsAnimating = true;
        switch (command)
        {
            case ("LBounce"):
                StartCoroutine(leftBounce());
                break;
            case ("RBounce"):
                StartCoroutine(rightBounce());
                break;
            default:
                LeftIsAnimating = false;
                RightIsAnimating = false;
                break;
        }
    }

    //Upward motion of character for bouncing
    void BounceUp(RectTransform portrait, Vector2 returnHere)
    {
        Vector2 target = new Vector2(returnHere.x, returnHere.y + bounceHeight);
        portrait.anchoredPosition = Vector2.MoveTowards(portrait.anchoredPosition, target, 2f);
    }

    //Downward motion of character for bouncing
    void BounceDown(RectTransform portrait, Vector2 returnHere)
    {
        portrait.anchoredPosition = Vector2.MoveTowards(portrait.anchoredPosition, returnHere, 2f);
    }

    //Bounces left portrait
    IEnumerator leftBounce()
    {
        leftAnimFuncPtr = new AnimFunc(BounceUp);
        yield return new WaitForSeconds(.1f);
        leftAnimFuncPtr = new AnimFunc(BounceDown);
        yield return new WaitForSeconds(.1f);
        leftAnimFuncPtr = new AnimFunc(BounceUp);
        yield return new WaitForSeconds(.1f);
        leftAnimFuncPtr = new AnimFunc(BounceDown);
        yield return new WaitForSeconds(.1f);
        leftPortrait.rectTransform.anchoredPosition = lPortPos;
        LeftIsAnimating = false;
    }

    //Bounces right portrait
    IEnumerator rightBounce()
    {
        rightAnimFuncPtr = new AnimFunc(BounceUp);
        yield return new WaitForSeconds(.1f);
        rightAnimFuncPtr = new AnimFunc(BounceDown);
        yield return new WaitForSeconds(.1f);
        rightAnimFuncPtr = new AnimFunc(BounceUp);
        yield return new WaitForSeconds(.1f);
        rightAnimFuncPtr = new AnimFunc(BounceDown);
        yield return new WaitForSeconds(.1f);
        rightPortrait.rectTransform.anchoredPosition = rPortPos;
        RightIsAnimating = false;
    }

    public void SetBackground (string name)
    {
        backgroundImage.SetActive(true);
        if (name != "" && bgDict.ContainsKey(name))
            backgroundImage.GetComponent<Image>().sprite = bgDict[name];
        if (name == "")
            backgroundImage.SetActive(false);
    }

    void ShowItem(string name)
    {
        if (name != "" && itemDict.ContainsKey(name))
        {
            itemImage.SetActive(true);
            itemImage.GetComponent<Image>().sprite = itemDict[name].image;
        }
    }

    public ItemEntry GetItemStatus(int index)
    {
        return itemEntries[index];
    }



    
}
