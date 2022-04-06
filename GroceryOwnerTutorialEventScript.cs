using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroceryOwnerTutorialEventScript : MonoBehaviour
{
    int eventID = 3;
    [SerializeField] DialogContainer dialog;
    [SerializeField] Actor actor;
    [SerializeField] GameObject groceryOwnerPrefab;
    [SerializeField] Vector3 ownerEnterPoint;
    [SerializeField] Vector3 ownerStopPoint;
    [SerializeField] ItemContainer inventoryContainer;
    [SerializeField] Item radishSeed;
    Animator anim;
    Animator playerAnimator;
    Character character;
    GameObject ownerGameObject;

    
    void Start()
    {
        //If this event has been played, do not play again.
        if (EventController.instance.GetEventStatus(eventID) == false)
        {
            gameObject.SetActive(false);
            return;
        }

        GamePauseController.instance.SetEvent(true);
        character = GameManager.instance.player.GetComponent<Character>();
        playerAnimator = GameManager.instance.player.GetComponent<Animator>();
        StartCoroutine(OwnerWalksIn());
    }

    //Owner walks from left of the scene to the front of the player. 
    private IEnumerator OwnerWalksIn()
    {
        ownerGameObject = Instantiate(groceryOwnerPrefab);
        ownerGameObject.transform.position = ownerEnterPoint;
        float distance =(ownerGameObject.transform.position - ownerStopPoint).sqrMagnitude;
        anim = ownerGameObject.GetComponent<Animator>();
        anim.SetBool("moving", true);
        anim.SetFloat("horizontal", 1);
        while(distance>0)
        {
            ownerGameObject.transform.position = Vector3.MoveTowards(ownerGameObject.transform.position, ownerStopPoint, 2.5f*Time.deltaTime); 
            distance = (ownerGameObject.transform.position - ownerStopPoint).sqrMagnitude;
            yield return null;
        }
        //Owner faces the player. 
        ResetMoveAndFacing();
        anim.SetFloat("vertical", 1);

        yield return new WaitForSeconds(0.5f);

        //Owner Starts talking. 
        StartCoroutine(ZhaoOwnerDialog1());
    }


    //Owner dialog 1.
    private IEnumerator ZhaoOwnerDialog1()
    {
        List<string> dialog_1 = new List<string>();
        dialog_1.Add("�簡��");
        dialog_1.Add("�����Īʦ�������İ���,"+character.Name+"����");
        dialog_1.Add("����������ͦ����ġ�");
        dialog_1.Add("��ʦ������һ����͸������˸����ţ��������̽�����ô������");
        dialog_1.Add("���� Ҳ��֪����һ����ʹҴ�ææ��ȥ��ţɽ��ʲô�������ֲ���ʲô���ա�����");
        dialog_1.Add("�޶��ˣ����ǳ����ӻ�����ϰ壬���ƹ��Ҹ�Ī�����Ǻ����ѣ����������ͺ��ˡ�");
        dialog_1.Add("��ÿ�춼�ᵽ���ܣ��ռ�������Ҫ�����Ļ���");
        dialog_1.Add("�����Ҵ�������߿����ҷ�����ĳ����䡣");
        dialog = (DialogContainer)ScriptableObject.CreateInstance(typeof(DialogContainer));
        dialog.MakeDialogContainer(dialog_1, actor);
        GameManager.instance.dialogSystem.Initialize(dialog);

        while (GameManager.instance.dialogSystem.GetIsDialogFinished() == false)
        {
            yield return new WaitForSeconds(1f);
        }

        //Play the next action - Zhao Owner brings player to the export box. 
        StartCoroutine(OwnerMovesToExportBox());
        StartCoroutine(PlayerMoveToExportBox());
    }

    //Owner brings the player to the export box. 
    [SerializeField] Vector3 ownerExportBoxPoint1;
    [SerializeField] Vector3 ownerExportBoxPoint2;
    [SerializeField] Vector3 playerExportBoxPoint;
    private IEnumerator OwnerMovesToExportBox()
    {
        ResetMoveAndFacing();
        //Owner Zhao moving.
        anim.SetFloat("horizontal", -1);
        anim.SetBool("moving", true);
        float distance = (ownerGameObject.transform.position - ownerExportBoxPoint1).sqrMagnitude;
        while (distance > 0)
        {
            ownerGameObject.transform.position = Vector3.MoveTowards(ownerGameObject.transform.position,
                ownerExportBoxPoint1, 2.5f * Time.deltaTime);
            distance = (ownerGameObject.transform.position - ownerExportBoxPoint1).sqrMagnitude;
            yield return null;
        }

        //Moves up
        ResetMoveAndFacing();
        anim.SetFloat("vertical", 1);
        anim.SetBool("moving", true);
        float distance2 = (ownerGameObject.transform.position - ownerExportBoxPoint2).sqrMagnitude;
        while (distance2 > 0)
        {
            ownerGameObject.transform.position = Vector3.MoveTowards(ownerGameObject.transform.position,
                ownerExportBoxPoint2, 2.5f * Time.deltaTime);
            distance2 = (ownerGameObject.transform.position - ownerExportBoxPoint2).sqrMagnitude;
            yield return null;
        }

        //Play the next script - Owner introduces the export box.
        StartCoroutine(OwnerDialog2());
    }

    private IEnumerator PlayerMoveToExportBox()
    {
        yield return new WaitForSeconds(0.2f); //Player starts moving slightly after owner zhao moves. 
        //Player moving. 
        CharResetMoveAndFacing();
        playerAnimator.SetFloat("horizontal", -1);
        playerAnimator.SetBool("moving", true);
        float distance3 = (GameManager.instance.player.transform.position - playerExportBoxPoint).sqrMagnitude;
        while (distance3 > 0)
        {
            GameManager.instance.player.transform.position = Vector3.MoveTowards(GameManager.instance.player.transform.position,
                playerExportBoxPoint, 2.5f * Time.deltaTime);
            distance3 = (GameManager.instance.player.transform.position - playerExportBoxPoint).sqrMagnitude;
            yield return null;
        }
        CharResetMoveAndFacing();
        playerAnimator.SetFloat("vertical", 1);


        yield return null;
    }

    //Owner's dialog 2
    private IEnumerator OwnerDialog2()
    {
        ResetMoveAndFacing();
        anim.SetFloat("vertical", 1);//Facing the export box.
        List<string> dialog2 = new List<string>();
        dialog2.Add("������ǳ����䡣������ϵ���Ʒ�ӽ�������ӡ���ÿ�����ϻ�����ջ���Ȼ�󸶸�����Ӧ��������");
        dialog2.Add("Ҫ���㼱�Ż�Ǯ��Ҳ����ֱ��������Ʒ��������ӻ��̣���Ҳ����ͬ���ļ۸��չ��ġ�");
        dialog2.Add("��������,ͯ�����ۡ��ٺ١�");

        dialog.MakeDialogContainer(dialog2, actor);
        GameManager.instance.dialogSystem.Initialize(dialog);

        while (GameManager.instance.dialogSystem.GetIsDialogFinished() == false)
        {
            yield return new WaitForSeconds(1f);
        }

        yield return new WaitForSeconds(1f);

        //Play the next action - owner turns to the player and give him seeds to grow. 
        StartCoroutine(OwnerDialog3());
    }

    //Owner's dialog 3 and actions 
    private IEnumerator OwnerDialog3()
    {
        //Owner turns to the player
        ResetMoveAndFacing();
        anim.SetFloat("horizontal", 1);
        //Player turns to the owner.
        CharResetMoveAndFacing();
        playerAnimator.SetFloat("horizontal", -1);

        yield return new WaitForSeconds(1f);

        //Owner dialog 3. 
        List<string> dialog3 = new List<string>();
        dialog3.Add("�޶��ˡ�Īʦ�������Ҹ��㼸���ܲ����ӡ�");
        dialog3.Add("�ܲ����ǳ��úܿ�ġ�ȱǮ�Ļ����ܲ����Ǻܿ�����ջ�ġ�");
        dialog3.Add("���ú��ˡ�");

        dialog.MakeDialogContainer(dialog3, actor);
        GameManager.instance.dialogSystem.Initialize(dialog);

        while (GameManager.instance.dialogSystem.GetIsDialogFinished() == false)
        {
            yield return new WaitForSeconds(1f);
        }

        yield return new WaitForSeconds(1f);

        //Deliver 9 seeds to the player.
        inventoryContainer.Add(radishSeed, 9);
        yield return new WaitForSeconds(1f);
        //Show message - radish seeds obtained. 



        //Play the next action - owner's dialog 4 and leaves. 
        StartCoroutine(OwnerDialog4());
    }

    //Owner's dialog before leaving the farm. 
    [SerializeField] Vector3 ownerExitPoint;
    private IEnumerator OwnerDialog4()
    {
        List<string> dialog3 = new List<string>();
        dialog3.Add("���ˣ��Ͽ����°ѡ���ʦ��������������ô�ܸɻ���˵�");
        dialog3.Add("��Ҫȥ��ĵط��ջ��ˡ�");
        dialog3.Add("�пն����ҵ��ӻ��꿴���������ճ��ӻ����⣬��ʱ�����������ϵر��ĺö�����");

        dialog.MakeDialogContainer(dialog3, actor);
        GameManager.instance.dialogSystem.Initialize(dialog);

        while (GameManager.instance.dialogSystem.GetIsDialogFinished() == false)
        {
            yield return new WaitForSeconds(1f);
        }

        ResetMoveAndFacing();
        anim.SetFloat("horizontal", -1);
        anim.SetBool("moving", true);
        float distance = (ownerGameObject.transform.position - ownerExitPoint).sqrMagnitude;
        while (distance > 0)
        {
            ownerGameObject.transform.position = Vector3.MoveTowards(ownerGameObject.transform.position,
                ownerExitPoint, 2.5f * Time.deltaTime);
            distance = (ownerGameObject.transform.position - ownerExitPoint).sqrMagnitude;
            yield return null;
        }
        Destroy(ownerGameObject);

        //End the event.
        EndEvent();
    }

    private void EndEvent()
    {
        EventController.instance.SetEventStatus(eventID, false);
        GamePauseController.instance.SetEvent(false);
    }


    private void CharResetMoveAndFacing()
    {
        playerAnimator.SetBool("moving", false);
        playerAnimator.SetFloat("lastVertical", 0);
        playerAnimator.SetFloat("lastHorizontal", 0);
        playerAnimator.SetFloat("vertical", 0);
        playerAnimator.SetFloat("horizontal", 0);
    }

    private void ResetMoveAndFacing()
    {
        anim.SetBool("moving", false);
        anim.SetFloat("horizontal", 0);
        anim.SetFloat("vertical", 0);
    }
}
