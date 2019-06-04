using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QueueAiControl : MonoBehaviour
{
    public List<BaseCharacter> characters { get; protected set; }
    PlayerManage player;
    public static Tick tick { get; }
    /// Spawn a Character
    public void SpawnCharacter(BaseCharacter character)
    {
        this.characters.Add(character);
    }
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManage>();
    }

    // Update is called once per frame
    void Update()
    {
        foreach (BaseCharacter character in this.characters)
        {
            if (player.IsSeeIt())
            {
                character.UpdateDraw();
            }
        }
    }

    public delegate void TickDelegate();

    ///ai任务队列
    public class Tick
    {
        public int tick = 0;
        public int speed = 1;

        public Queue<TickDelegate> toAdd = new Queue<TickDelegate>();
        public Queue<TickDelegate> toDel = new Queue<TickDelegate>();
        public List<TickDelegate> updates = new List<TickDelegate>();

        public void DoTick()
        {
            this.tick++;

            while (this.toDel.Count != 0)
            {
                this.updates.Remove(this.toDel.Dequeue());
            }
            while (this.toAdd.Count != 0)
            {
                this.updates.Add(this.toAdd.Dequeue());
            }

            for (int i = 0; i < this.updates.Count; i++)
            {
                this.updates[i].Invoke();
            }
        }
    }

}
