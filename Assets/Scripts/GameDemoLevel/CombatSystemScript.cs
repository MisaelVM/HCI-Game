using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BattleState { START, PARTYTURN, PLAYERACTION, ENEMIESTURN, VICTORY, DEFEAT }

public class CombatSystemScript : MonoBehaviour
{
    public BattleState state;

    public List<GameObject> partyMembersPrefabs;
    public List<GameObject> enemyMembersPrefabs;

    public Transform partyBattleStation;
    public Transform enemyBattleStation;

    List<GameObject> partyMembersGameObjects;
    List<GameObject> enemyMembersGameObjects;

    List<EntityAttributes> partyMembersAttributes;
    List<EntityAttributes> enemyMembersAttributes;

    int currentPartyMember = 0;

    // Start is called before the first frame update
    void Start()
    {
        state = BattleState.START;
        
        partyMembersAttributes = new List<EntityAttributes>();
        enemyMembersAttributes = new List<EntityAttributes>();

        partyMembersGameObjects = new List<GameObject>();
        enemyMembersGameObjects = new List<GameObject>();

        SetupEntities();
    }

    // Update is called once per frame
    void Update()
    {
        switch (state) {
            case BattleState.PARTYTURN:
                PartyTurn();
                break;
            case BattleState.PLAYERACTION:
                PlayerAction();
                break;
            case BattleState.ENEMIESTURN:
                EnemiesTurn();
                break;
            case BattleState.VICTORY:
                break;
            case BattleState.DEFEAT:
                break;

            default:
                break;
        }
    }

    void SetupEntities() {
        foreach (var partyMember in partyMembersPrefabs) {
            GameObject ally = Instantiate(partyMember, partyBattleStation);
            partyMembersGameObjects.Add(ally);
            partyMembersAttributes.Add(ally.GetComponent<EntityAttributes>());
        }

        foreach (var enemyMember in enemyMembersPrefabs) {
            GameObject enemy = Instantiate(enemyMember, enemyBattleStation);
            enemyMembersGameObjects.Add(enemy);
            enemyMembersAttributes.Add(enemy.GetComponent<EntityAttributes>());
        }

        state = BattleState.PARTYTURN;
        Debug.Log("Party member " + currentPartyMember + "'s turn. HP: " + partyMembersAttributes[currentPartyMember].health);
    }

    void PartyTurn() {
        /*foreach (var party in partyMembersGameObjects) {
            Vector3 forward = new Vector3(0.0f, 0.0f, 0.5f * Time.deltaTime);
            party.transform.position += forward;
        }*/

        // Atacar / Defender / Acciones Especiales (Defender otro daño, Curar, Elemental)
        if (Input.GetKeyDown(KeyCode.A)) {
            Debug.Log("Party member " + currentPartyMember + " set to attack action");
            partyMembersAttributes[currentPartyMember].action = EntityAction.ATTACK;
            state = BattleState.PLAYERACTION;
        }
        else if (Input.GetKeyDown(KeyCode.D)) {
            Debug.Log("Party member " + currentPartyMember + " set to defend action");
            partyMembersAttributes[currentPartyMember].action = EntityAction.DEFEND;
            NextPlayerTurn();
        }
        else if (Input.GetKeyDown(KeyCode.S)) {
            Debug.Log("Party member " + currentPartyMember + " set to special action");
            partyMembersAttributes[currentPartyMember].action = EntityAction.SPECIAL;
            state = BattleState.PLAYERACTION;
        }
    }

    void PlayerAction() {
        if (partyMembersAttributes[currentPartyMember].action == EntityAction.ATTACK) {
            if (Input.GetKeyDown(KeyCode.Alpha1)) {
                Debug.Log("Action set to enemy 0");
                PartyAttack(currentPartyMember, 0);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2)) {
                Debug.Log("Action set to enemy 1");
                PartyAttack(currentPartyMember, 1);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3)) {
                Debug.Log("Action set to enemy 2");
                PartyAttack(currentPartyMember, 2);
            }
        }
    }

    void NextPlayerTurn() {
        ++currentPartyMember;
        if (currentPartyMember >= partyMembersAttributes.Count) {
            currentPartyMember = 0;
            state = BattleState.ENEMIESTURN;
        }
        else {
            state = BattleState.PARTYTURN;
            Debug.Log("Party member " + currentPartyMember + "'s turn. HP: " + partyMembersAttributes[currentPartyMember].health);
        }
    }

    void EnemiesTurn() {
        for (int i = 0; i < enemyMembersAttributes.Count; i++) {
            Debug.Log("Enemy " + i + "'s turn. HP: " + enemyMembersAttributes[i].health);
            //StartCoroutine(EnemiesAttack(i, Random.Range(0, partyMembersAttributes.Count)));
            EnemiesAttack(i, Random.Range(0, partyMembersAttributes.Count));
        }

        state = BattleState.PARTYTURN;
        Debug.Log("Party member " + currentPartyMember + "'s turn. HP: " + partyMembersAttributes[currentPartyMember].health);
    }

    void PartyAttack(int partyMemberIndex, int enemyMemberIndex) {
        int damage = partyMembersAttributes[partyMemberIndex].AttackEnemy(enemyMembersAttributes[enemyMemberIndex]);
        //int damage = enemyMembersAttributes[enemyMemberIndex].TakeDamage(partyMembersAttributes[partyMemberIndex]);
        if (enemyMembersAttributes[enemyMemberIndex].health <= 0) {
            Destroy(enemyMembersGameObjects[enemyMemberIndex]);
            enemyMembersAttributes.Remove(enemyMembersAttributes[enemyMemberIndex]);
            enemyMembersGameObjects.Remove(enemyMembersGameObjects[enemyMemberIndex]);
            if (enemyMembersAttributes.Count == 0) {
                state = BattleState.VICTORY;
                Debug.Log("VICTORY!");
            }
        }
        NextPlayerTurn();
    }

    void EnemiesAttack(int enemyMemberIndex, int partyMemberIndex) {
        //yield return new WaitForSeconds(3.0f);
        Debug.Log("Enemy " + enemyMemberIndex + " attacks party member " + partyMemberIndex);
        //yield return new WaitForSeconds(2.0f);
        int damage = partyMembersAttributes[partyMemberIndex].TakeDamage(enemyMembersAttributes[enemyMemberIndex]);
        if (partyMembersAttributes[partyMemberIndex].health <= 0) {
            Destroy(partyMembersGameObjects[partyMemberIndex]);
            partyMembersAttributes.Remove(partyMembersAttributes[partyMemberIndex]);
            partyMembersGameObjects.Remove(partyMembersGameObjects[partyMemberIndex]);
            if (partyMembersAttributes.Count == 0) {
                state = BattleState.DEFEAT;
                Debug.Log("DEFEAT!");
            }
        }
        Debug.Log("Enemy " + enemyMemberIndex + " dealt " + damage + " damage");
        //yield return new WaitForSeconds(3.0f);
    }

}
