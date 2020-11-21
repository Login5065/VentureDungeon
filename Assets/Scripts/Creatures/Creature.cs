﻿using System.Collections.Generic;
using UnityEngine;
using Dungeon.UI;
using Dungeon.Variables;
using Dungeon.Scripts;
using System.Linq;

namespace Dungeon.Creatures
{
    public class Creature : MonoBehaviour, ISellable
    {
        public Animator animator;
        private AudioSource audioSource;
        public AudioClip impact;
        public GameObject HPBar;
        public GameObject selection;
        public HashSet<Creature> seenCreatures = new HashSet<Creature>(); // HashSet - no duplicates allowed
        public CircleCollider2D sightCollider;
        public BoxCollider2D hitBox;
        public Rigidbody2D sightColliderRB;
        public Creature closestCreature;
        public GameObject moveOrder;
        public Vector3 lastPosition;
        public List<Vector2Int> path;
        public List<Vector2Int> Path
        {
            get
            {
                if (path == null)
                    path = new List<Vector2Int>();
                return path;
            }
            set
            {
                if (value == null)
                    path.Clear();
                else
                    path = value;
            }
        }
        public List<Vector2Int> idleBacktrackPath;
        public CreatureSpawner spawnerObject;
        private enum MonsterType
        {
            Hero,
            MaceSkeleton,
            BowSkeleton,
            SpearSkeleton,
        }
        public float
            height = 1,
            width = 1,
            maxHealth = 100,
            health = 100,
            maxResource = 0,
            resource = 0,
            armor = 0,
            speed = 0.1f,
            sightRange = 3.0f,
            timeToRecalculatePathToTreasure = 10f,
            idleTimer = 0f,
            attackRange = 1.0f
            ;

        public float Health
        {
            get => health;
            set
            {
                if (value < 0) health = 0;
                else health = value;
                HPBar.transform.localScale = new Vector3(health / maxHealth, 1, 2);
            }
        }

        public int
            type = 0,
            resourceType = 0,
            carriedGold = 0,
            carriedGoldTarget = 0,
            value = 0,
            creatureCost = 0
            ;

        private int
            maxAttackPriority = 0,
            maxIdlePriority = 0,
            attackPriority = 0,
            idlePriority = 0
            ;

        public bool
            dying = false,
            selected = false,
            controllable = true,
            allegiance = false,
            hasSight = true,
            shouldBeSeen = true,
            isAttacking = false,
            attackFound = false,
            idleFound = false
            ;

        public HashSet<AttackModule> attackModules;
        public HashSet<IdleModule> idleModules;
        private AttackModule chosenAttack;
        private IdleModule chosenIdle;
        public bool CanSell => controllable;
        public int GoldValue => (int)(creatureCost * health / maxHealth);

        void Start()
        {
            audioSource = gameObject.GetComponent<AudioSource>();
            animator = gameObject.GetComponent<Animator>();
            HPBar = gameObject.transform.Find("HP_UI").transform.Find("HP").gameObject;
            selection = gameObject.transform.transform.Find("Selection").gameObject;
            selection.SetActive(false);
            hitBox = gameObject.AddComponent<BoxCollider2D>();
            hitBox.size = new Vector2(width / 4, height / 4);
            hitBox.offset = new Vector2(0, height / 8);
            sightColliderRB = gameObject.AddComponent<Rigidbody2D>();
            sightColliderRB.isKinematic = true;
            lastPosition = gameObject.transform.position;
            idleTimer = Random.Range(5f, 20f);
            if (hasSight)
            {
                sightCollider = gameObject.AddComponent<CircleCollider2D>();
                sightCollider.offset = Vector3.zero;
                sightCollider.radius = sightRange;
                sightCollider.isTrigger = true;
            }
            attackModules = new HashSet<AttackModule>();
            idleModules = new HashSet<IdleModule>();
            foreach (var script in gameObject.GetComponents<MonoBehaviour>())
            {
                if (script.GetType().IsSubclassOf(typeof(AttackModule)))
                {
                    var prep = script as AttackModule;
                    prep.owner = this;
                    attackModules.Add(prep);
                    if (prep.priority > maxAttackPriority) maxAttackPriority = prep.priority;
                }
                else if (script.GetType().IsSubclassOf(typeof(IdleModule)))
                {
                    var prep = script as IdleModule;
                    prep.owner = this;
                    idleModules.Add(prep);
                    if (prep.priority > maxIdlePriority) maxIdlePriority = prep.priority;
                }
            }
        }

        void Update()
        {
            //idleTimer -= Time.deltaTime;
            //timeToRecalculatePathToTreasure -= Time.deltaTime;
            if (dying) return;
            if (!isAttacking && Statics.UIManager.SelectedCreature == this && Input.GetMouseButtonDown(1) && Statics.UIManager.mode == (int)UIManager.UIModes.Move && controllable) { HandleMoveOrder(); }
            HandleIdle();
            CheckFlip();
        }

        public void HandleIdle()
        {
            idlePriority = maxIdlePriority;
            idleFound = false;
            RefreshList(idleModules.Cast<CreatureModule>());
            IEnumerable<CreatureModule> preparedList = PreparePriotities(idlePriority, idleModules.Cast<CreatureModule>());
            while (!idleFound)
            {
                preparedList = preparedList.Where(c => !c.tried);
                if (idlePriority == 0) return;
                if (preparedList.Count() == 0)
                {
                    idlePriority -= 1;
                    preparedList = PreparePriotities(idlePriority, idleModules.Cast<CreatureModule>());
                }
                else
                {
                    chosenIdle = Roll(preparedList) as IdleModule;
                    idleFound = chosenIdle.Idle();
                    chosenIdle.tried = true;
                }
            }
        }

        public void HandleMoveOrder()
        {
            Vector3 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (moveOrder != null)
            {
                Destroy(moveOrder);
            }
            moveOrder = new GameObject();
            moveOrder.transform.position = position;
        }

        public void CheckFlip()
        {
            if (lastPosition.x != gameObject.transform.position.x)
            {
                if (lastPosition.x > gameObject.transform.position.x)
                {
                    gameObject.GetComponent<SpriteRenderer>().flipX = true;
                }
                else if (lastPosition.x < gameObject.transform.position.x)
                {
                    gameObject.GetComponent<SpriteRenderer>().flipX = false;
                }
            }
            lastPosition = this.gameObject.transform.position;
        }

        public void AttackCreature()
        {
            attackPriority = maxAttackPriority;
            attackFound = false;
            RefreshList(attackModules.Cast<CreatureModule>());
            IEnumerable<CreatureModule> preparedList = PreparePriotities(attackPriority, attackModules.Cast<CreatureModule>());
            while (!attackFound)
            {
                preparedList = preparedList.Where(c => !c.tried);
                if (attackPriority == 0) return;
                if (preparedList.Count() == 0)
                {
                    attackPriority -= 1;
                    preparedList = PreparePriotities(attackPriority, attackModules.Cast<CreatureModule>());
                }
                else
                {
                    chosenAttack = Roll(preparedList) as AttackModule;
                    attackFound = chosenAttack.Attack();
                    chosenAttack.tried = true;
                }
            }
        }

        public void Bonk()
        {
            chosenAttack.ExecuteBonk();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent<Creature>(out var creature) && other is BoxCollider2D && creature.hasSight && shouldBeSeen && creature.allegiance != allegiance) creature.seenCreatures.Add(this);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.TryGetComponent<Creature>(out var creature) && other is BoxCollider2D && creature.hasSight && shouldBeSeen && creature.allegiance != allegiance) creature.seenCreatures.Remove(this);
        }

        private void RefreshList(IEnumerable<CreatureModule> objects)
        {
            foreach (CreatureModule c in objects) c.tried = false;
        }

        private IEnumerable<CreatureModule> PreparePriotities(int priority, IEnumerable<CreatureModule> objects) => objects.Where(x => x.priority == priority && !x.tried && x.Requirement());

        private CreatureModule Roll(IEnumerable<CreatureModule> objects)
        {
            if (objects.Count() == 1) return objects.First();
            float sum = objects.Sum(c => c.chance);
            double diceRoll = Random.Range(0, sum);
            sum = 0.0f;
            foreach (CreatureModule c in objects)
            {
                sum += c.chance;
                if (diceRoll < sum)
                {
                    return c;
                }
            }
            return null;
        }
    }
}