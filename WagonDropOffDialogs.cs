using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script is attached to the WagonDropOff event. It controls the order of speeches and that movement of objects. 
public class WagonDropOffDialogs : MonoBehaviour
{
    int eventID = 1;
    [SerializeField] DialogContainer dialog; //Used for dialogs.
    [SerializeField] Actor thirdUncleActor;
    [SerializeField] Actor mo;
    [SerializeField] GameObject thirdUncle;
    [SerializeField] GameObject wagon;
    Animator playerAnim;
    NPCMovementController thirdUncleMovementController; 
    Character character;

    private void Start()
    {
        //First check whether the event has been played before. If yes, set the prefab inactive. 
        if (EventController.instance.GetEventStatus(eventID) == false)
        {
            gameObject.SetActive(false);
            return;
        }
        GamePauseController.instance.SetEvent(true);

        playerAnim = GameManager.instance.player.GetComponent<Animator>();
        character = GameManager.instance.player.GetComponent<Character>();
        thirdUncleMovementController = thirdUncle.GetComponent<NPCMovementController>();

        //3Uncle turns left. 
        thirdUncleMovementController.Idle(-1f, 0f);

        StartCoroutine(ThirdUncle_Dialog_1());
    }

    //The starting of the conversation. Third Uncle talks first. 
    private IEnumerator ThirdUncle_Dialog_1()
    {



        yield return new WaitForSeconds(1.5f);
        //Creating the dialog. 
        List<string> newDialog = new List<string>();
        newDialog.Add("坐了一天的车，精神有点累了吧？");
        newDialog.Add("来，快见过莫师傅");
        dialog = (DialogContainer)ScriptableObject.CreateInstance(typeof(DialogContainer));
        //Playing the dialog. 
        dialog.MakeDialogContainer(newDialog, thirdUncleActor);
        GameManager.instance.dialogSystem.Initialize(dialog);

        //the bool isDialogFinishes helps determine whether the event should go to the next step.
        //This is set to true in the Conclude() function of the dialog system script. 
        while(GameManager.instance.dialogSystem.GetIsDialogFinished()==false)
        {
            yield return new WaitForSeconds(1f);
        }
        //Player turns to Mo. 
        playerAnim.SetFloat("lastVertical", 1);
        yield return new WaitForSeconds(0.5f);
        //Play the ellipse speech bubble. 
        MessageManager.instance.CallSpeechBubble(GameManager.instance.player.transform, 3);
        yield return new WaitForSeconds(1f);
        StartCoroutine(Mo_Dialog_1());
    }

    //Master Mo starts talking. 
    public IEnumerator Mo_Dialog_1()
    {
        //Creating the dialog. 
        List<string> newDialog = new List<string>();
        newDialog.Add("你就是韩掌柜的侄子，"+character.Name+"啊？");
        newDialog.Add("。。。。。。");
        newDialog.Add("唔。。。不错，看上去挺精神的。");
        newDialog.Add("以后你就来跟着我学着干农活吧。我还会一些武功医术，要是你干得好，我自然会传授你一些。");
        dialog = (DialogContainer)ScriptableObject.CreateInstance(typeof(DialogContainer));
        dialog.MakeDialogContainer(newDialog, mo);

        GameManager.instance.dialogSystem.Initialize(dialog);
        while (GameManager.instance.dialogSystem.GetIsDialogFinished() == false)
        {
            yield return new WaitForSeconds(1f);
        }
        //Play some player animation after Mo's speech. 
        Debug.Log("*Play some player animation after Mo's speech");
        StartCoroutine(ThirdUncle_Dialog_2());
    }

    //Third Uncle starts talking back to Mo and the player. 
    public IEnumerator ThirdUncle_Dialog_2()
    {
        //Creating the dialog
        List<string> newDialog = new List<string>();
        newDialog.Add("啊，莫师傅，要是真这样，那可是我这侄子的大机缘啊！");
        newDialog.Add(character.Name + "，莫师傅可不仅是周围最有名的农户，还是数十年前称霸本镇的金龙帮的副帮主。");
        newDialog.Add("一身武功和医术可是很了不起的!");
        newDialog.Add("好好干，不要害怕吃苦，知道了吗？");
        dialog = (DialogContainer)ScriptableObject.CreateInstance(typeof(DialogContainer));
        dialog.MakeDialogContainer(newDialog, thirdUncleActor);
        //Playing the dialog.
        GameManager.instance.dialogSystem.Initialize(dialog);
        while (GameManager.instance.dialogSystem.GetIsDialogFinished() == false)
        {
            yield return new WaitForSeconds(1f);
        }
        //Play some player nodding or speech bubble. 
        
        StartCoroutine(ThirdUncle_Dialog_3());
    }

    //Third Uncle starts to say good bye. 
    public IEnumerator ThirdUncle_Dialog_3()
    {
        //Creating the dialog. 
        List<string> newDialog = new List<string>();
        newDialog.Add("好了，那我就先回城里了。");
        newDialog.Add("你要是不干活时想找韩叔了，就来城里的醉仙楼找我吧。");
        dialog = (DialogContainer)ScriptableObject.CreateInstance(typeof(DialogContainer));
        dialog.MakeDialogContainer(newDialog, thirdUncleActor);
        //Playing the dialog
        GameManager.instance.dialogSystem.Initialize(dialog);
        while (GameManager.instance.dialogSystem.GetIsDialogFinished() == false)
        {
            yield return new WaitForSeconds(1f);
        }
        //Third Uncle turns towards Mo. 
        thirdUncleMovementController.Idle(0f, 1f);
        yield return new WaitForSeconds(0.5f);
        //Creating the dialog
        newDialog = new List<string>();
        newDialog.Add("莫师傅，我这侄子就拜托您啦。");
        newDialog.Add("有空常来喝茶啊！");
        dialog = (DialogContainer)ScriptableObject.CreateInstance(typeof(DialogContainer));
        dialog.MakeDialogContainer(newDialog, thirdUncleActor);
        //Playing the dialog. 
        GameManager.instance.dialogSystem.Initialize(dialog);
        while (GameManager.instance.dialogSystem.GetIsDialogFinished() == false)
        {
            yield return new WaitForSeconds(1f);
        }

        //Play some wagon leaving animation after saying goodbye. 
        StartCoroutine(ThirdUncleMoves());
    }

    //Third Uncle moves to the wagon and disappears. 
    private IEnumerator ThirdUncleMoves()
    {
        //Start playing 3 uncle walking animation.
        thirdUncle.GetComponent<BoxCollider2D>().enabled = false;
        float distance = (thirdUncle.transform.position - wagon.transform.position).sqrMagnitude;
        thirdUncleMovementController.Walk(-1, 0);
        while(distance > 0.5f)
        {
            thirdUncle.transform.position = Vector3.MoveTowards(thirdUncle.transform.position, 
                wagon.transform.position, 2f * Time.deltaTime);
            distance = (thirdUncle.transform.position - wagon.transform.position).sqrMagnitude;
            yield return null;
        }
        thirdUncle.SetActive(false);
        StartCoroutine(WagonMoves());
    }

    //After Third Uncle enters the wagon, the wagon starts to move out of the scene. 
    private IEnumerator WagonMoves()
    {
        for(int i = 0; i<25; i++)
        {
            wagon.transform.position = new Vector3(wagon.transform.position.x + 1,
                wagon.transform.position.y, wagon.transform.position.z);
            yield return new WaitForSeconds(0.2f);
        }
        //After the wagon is out of the sight, set it active.  
        wagon.SetActive(false);
        yield return new WaitForSeconds(1f);
        //Playing Mo's next dialog. 
        StartCoroutine(Mo_Dialog_2());
    }

    //After the wagon left, Mo starts talking.
    private IEnumerator Mo_Dialog_2()
    {
        //Creating Mo's a dialog. 
        List<string> newDialog = new List<string>();
        newDialog.Add("来，这边以后就是你住的茅屋。快把行李搬进来吧。");
        dialog = (DialogContainer)ScriptableObject.CreateInstance(typeof(DialogContainer));
        dialog.MakeDialogContainer(newDialog, mo);
        //Playing the dialog. 
        GameManager.instance.dialogSystem.Initialize(dialog); //Play the fifth dialog. 
        while (GameManager.instance.dialogSystem.GetIsDialogFinished() == false)
        {
            yield return new WaitForSeconds(1f);
        }
        //Player moves into their house. 
        StartCoroutine(PlayerMoveUp());
    }

    private IEnumerator PlayerMoveUp()
    {
        Animator anim = GameManager.instance.player.GetComponent<Animator>();
        anim.SetBool("moving", true); //Set moving animation. 
        anim.SetFloat("vertical", 1); //Set moving direction. 
        Vector3 target = new Vector3(-0.4f, 2.8f, 0f);
        float distance = (GameManager.instance.player.transform.position - target).sqrMagnitude;

        while(distance > 0.4f)
        {
            GameManager.instance.player.transform.position = Vector3.MoveTowards(GameManager.instance.player.transform.position,
                new Vector3(-0.4f, 4f, 0f), 3f * Time.deltaTime);
            distance = (GameManager.instance.player.transform.position - target).sqrMagnitude;
            yield return null;
        }
        anim.SetBool("moving", false);//Stop moving animation. 
        GameSceneManager.instance.InitSwitchScene("HanLi's Room", new Vector3(0f, -4f, 0f));
        EndEvent();
    }

    private void EndEvent()
    {
        EventController.instance.SetEventStatus(eventID, false); 
        GamePauseController.instance.SetEvent(false);
    }


}
