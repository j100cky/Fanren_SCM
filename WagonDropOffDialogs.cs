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
        newDialog.Add("����һ��ĳ��������е����˰ɣ�");
        newDialog.Add("���������Īʦ��");
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
        newDialog.Add("����Ǻ��ƹ��ֶ�ӣ�"+character.Name+"����");
        newDialog.Add("������������");
        newDialog.Add("��������������ȥͦ����ġ�");
        newDialog.Add("�Ժ������������ѧ�Ÿ�ũ��ɡ��һ���һЩ�书ҽ����Ҫ����ɵúã�����Ȼ�ᴫ����һЩ��");
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
        newDialog.Add("����Īʦ����Ҫ�����������ǿ�������ֶ�ӵĴ��Ե����");
        newDialog.Add(character.Name + "��Īʦ���ɲ�������Χ��������ũ����������ʮ��ǰ�ưԱ���Ľ�����ĸ�������");
        newDialog.Add("һ���书��ҽ�����Ǻ��˲����!");
        newDialog.Add("�úøɣ���Ҫ���³Կ֪࣬������");
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
        newDialog.Add("���ˣ����Ҿ��Ȼس����ˡ�");
        newDialog.Add("��Ҫ�ǲ��ɻ�ʱ���Һ����ˣ��������������¥���Ұɡ�");
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
        newDialog.Add("Īʦ��������ֶ�ӾͰ���������");
        newDialog.Add("�пճ����Ȳ谡��");
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
        newDialog.Add("��������Ժ������ס��é�ݡ�������������ɡ�");
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
