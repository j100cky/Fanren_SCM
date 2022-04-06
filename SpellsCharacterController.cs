using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellsCharacterController : MonoBehaviour
{
    [SerializeField] SkillBarController skillBarController;
    
    Animator animator;
    Rigidbody2D rbody;
    CharacterController2D character;
    [SerializeField] float offsetDistance = 1f;
    bool isSelectable;
    float useSpellRange; //The range within which the player can cast spells. 

    [SerializeField] SkillIconHighlight skillIconHighlight;

    [SerializeField] SkillDatasheet skill;


    void Start()
    {
        animator = GetComponent<Animator>();
        rbody = GetComponent<Rigidbody2D>();
        character = GetComponent<CharacterController2D>();
        useSpellRange = GetComponent<Character>().useToolRange;
    }

    void Update()
    {
        if (GamePauseController.instance.isPaused == true) { return; }
        if (skillBarController.isSkillBarActive == false) { return; }
        CanSelectCheck();
        if (Input.GetMouseButtonDown(0))
        {
            UseSpell();
        }
    }

    private void UseSpell()
    {
        Vector2 position = rbody.position + character.lastMotionVector * offsetDistance;
        
        skill = GameManager.instance.skillBarOnScreen.currentSkill;

        if (skill == null || skill.skillAction == null)
        {
            Debug.Log("skill not exist or skill does not contain a skillAction script");
            return;
        }

        skill.skillAction.OnSkillUsed();
        animator.SetTrigger("act");
        return;
    }

    private void CanSelectCheck()
    {
        Vector2 characterPosition = transform.position;
        Vector2 cameraPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        isSelectable = Vector2.Distance(characterPosition, cameraPosition) < useSpellRange;
        skillIconHighlight.RangeCheck(isSelectable);
    }

}
