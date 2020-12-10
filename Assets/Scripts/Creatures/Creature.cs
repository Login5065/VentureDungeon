using System.Collections.Generic;
using UnityEngine;
using Dungeon.UI;
using Dungeon.Variables;
using Dungeon.Scripts;
using Dungeon.Graphics;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;

namespace Dungeon.Creatures
{
    public class Creature : MonoBehaviour, ISellable
    {
        public float
            hpbaroffset = 0.85f,
            height = 1,
            width = 1,
            maxHealth = 100,
            health = 100,
            maxResource = 0,
            resource = 0,
            armor = 0,
            speed = 0.1f,
            sightRange = 3.0f
            ;

        public int
            type = 0,
            value = 0,
            creatureCost = 0,
            maxAttackPriority = 0,
            attackPriority = 0
            ;

        public bool
            dying = false,
            controllable = true,
            allegiance = false,
            hasSight = true,
            shouldBeSeen = true,
            isAttacking = false,
            busy = false,
            seeksTreasure = false,
            canIdle = true,
            moving = false
            ;
        public float Health
        {
            get => health;
            set
            {
                float temp = health;
                if (value < 0) health = 0;
                else health = value;
                hpmaterial.AddOperation(hpbaroffset * (1f - (temp / maxHealth)), "_ClipUvRight", 0.1f, hpbaroffset * (1f - (health / maxHealth)));
            }
        }

        public List<Vector2Int> path;
        public List<Vector2Int> Path
        {
            get
            {
                if (path == null) path = new List<Vector2Int>();
                return path;
            }
            set
            {
                if (value == null) path.Clear();
                else path = value;
            }
        }

        public BehaviorTree ai;
        public ShaderEffects material;
        public ShaderEffects hpmaterial;
        public ShaderEffects outerhpmaterial;
        public Animator animator;
        private AudioSource audioSource;
        public AudioClip impact;
        public GameObject HPBar;
        public HashSet<Creature> seenCreatures = new HashSet<Creature>(); // HashSet - no duplicates allowed
        public BoxCollider2D hitBox;
        public Creature closestCreature;
        public GameObject moveOrder;
        public Vector3 lastPosition;
        public Vector2Int anchor;
        public Action currentMoveAction;
        public Action previousMoveAction;
        public Vector2Int? recallPosition;
        public Color selectcolor = Color.red;
        public HashSet<AttackModule> attackModules;
        public AttackModule chosenAttack;
        public bool CanSell => controllable;
        public int GoldValue => (int)(creatureCost * health / maxHealth);
        public Vector3 Position => gameObject.transform.position;
        private string currentState;

        void Start()
        {
            ai = gameObject.GetComponent<BehaviorTree>();
            ai.enabled = true;
            material = gameObject.AddComponent<ShaderEffects>();
            material.material.SetColor("_OutlineColor", selectcolor);
            audioSource = gameObject.GetComponent<AudioSource>();
            animator = gameObject.GetComponent<Animator>();
            HPBar = gameObject.transform.Find("HP_UI").transform.Find("HP").gameObject;
            hpmaterial = HPBar.AddComponent<ShaderEffects>();
            outerhpmaterial = gameObject.transform.Find("HP_UI").gameObject.AddComponent<ShaderEffects>();
            hitBox = gameObject.AddComponent<BoxCollider2D>();
            hitBox.size = new Vector2(width / 4, height / 4);
            hitBox.offset = new Vector2(0, height / 8);
            lastPosition = gameObject.transform.position;
            attackModules = new HashSet<AttackModule>();
            foreach (var attac in gameObject.transform.Find("AttackModules").GetComponents<MonoBehaviour>())
            {
                var prepa = attac as AttackModule;
                prepa.owner = this;
                attackModules.Add(prepa);
                if (prepa.priority > maxAttackPriority) maxAttackPriority = prepa.priority;
            }
        }

        void Update()
        {
            if (!isAttacking && Statics.UIManager.SelectedCreature == this && Input.GetMouseButtonDown(1) && Statics.UIManager.mode == (int)UIManager.UIModes.Move && controllable) { HandleMoveOrder(); }
            if (moving && path.Count > 0) TryMove();
            CheckFlip();
        }

        public bool ChangeAnimationState(string newState)
        {
            if (currentState == newState) return false;
            animator.Play(newState, -1, 0f);
            currentState = newState;
            return true;
        }

        public void HandleMoveOrder()
        {
            Vector3 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (moveOrder != null) { Destroy(moveOrder); }
            moveOrder = new GameObject();
            moveOrder.transform.position = position;
        }

        public void CheckFlip()
        {
            if (lastPosition.x != gameObject.transform.position.x)
            {
                if (lastPosition.x > gameObject.transform.position.x) { gameObject.GetComponent<SpriteRenderer>().flipX = true; }
                else if (lastPosition.x < gameObject.transform.position.x) { gameObject.GetComponent<SpriteRenderer>().flipX = false; }
            }
            lastPosition = gameObject.transform.position;
        }

        public void TryMove()
        {
            Vector3 distcalc = Statics.TileMapFG.CellToWorld(new Vector3Int(Path[0].x, Path[0].y, 0));
            distcalc.x += 0.5f;
            if (Vector2.Distance(transform.position, distcalc) < 0.1)
            {
                Path.RemoveAt(0);
            }
            if (Path.Count != 0)
            {
                Vector2 movePosition = Statics.TileMapFG.CellToWorld(new Vector3Int(Path[0].x, Path[0].y, 0));
                movePosition.x += 0.5f;
                transform.position = Vector2.MoveTowards(transform.position, movePosition, speed * Time.deltaTime);
            }
            else
            {
                previousMoveAction = null;
                ChangeAnimationState("Idle");
            }
        }

        public void Busy() => busy = true;

        public void EndBusy()
        {
            currentState = null;
            busy = false;
        }

        public void Bonk() => chosenAttack.ExecuteBonk();
    }
}