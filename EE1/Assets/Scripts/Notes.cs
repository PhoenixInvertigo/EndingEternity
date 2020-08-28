using UnityEngine;
using System.Collections;

public class Notes : MonoBehaviour
{

    /*
     You're doing great. This work matters. You matter. You are important. You are worthy. You are loved.
     Keep going. I know you can do it, and this is one of the greatest gifts you'll ever have for the world.
     But it is just the first of many, many more. 
     <3
      
To Do:

    Create delegate
    Create delegate holder
    Make CallAction about firing off the Delegate
    Make FrameAdvance clear Delegate
    Make AttackX and Dash load to Delegate
    (NextAction might be required here)
    Make NextGT a Delegate, also

    In Enemy, same thing.
    Use a protected delegate
        TestEnemy1 needs to make its AI calls through the delegate


    Meta Plan:
        Menu Script (simultaneously with world dev/levelbuilding)
        Enemy AI
        Simple conversations
        Start levelbuilding

    Need a "rotation switch" chart. Will have six elements. Will be a bool whether or not you're in it.
    Bool for each thing being unlocked
    Can select all 6 without being unlocked
        Will be faded for not-unlocked stuff
    Select button switches form if is unlocked
        Otherwise, makes a Nuh Uh sound effect or whatever
    Can't switch outside of save crystals at first

    Just make a UI element in the bottom center of screen that holds dialogue, then trip the CUTSCENE flag in game state,
        then keep up with conversations had. Also add a "talk to" button or somesuch. Can do "talk to" the same way we do
        TriggerHits. Then have the person whose hit was triggered make calls to the UI element to place text on screen.
    Explore sections to reach story checkpoint areas (camps?)
    Have cutscenes in story checkpoint areas
    These cutscenes are where mysteries can be found (the core ones)
    Mysteries are filled in in the world
    Various mysteries being found unlock different dialogue/fight options

    Loop only happens with New Game+ now.
    Save at crystals
    When you die, you go to OOT
    From OOT, move to last crystal you saved at

    Changing Realms will reset the other Realm's enemies
    Each Realm will be its own level in the build
    
    Start in Refraction (a mirror-hall of crystals)



    Also, that brings up a design question: Without eons, how are you going to reset each area?
    Reset by using savepoints, bonfire style
    It ain't broke. Don't fix it.

    Can probably use a very simplified pathfinding system, tbh
    Need to decide design-wise what we want this to function like
    Model the pathing off that

    AI will probably be fairly simple, too
    We're going to stick with pseudo-time rather than real time
    Add an in-game timer, which is part of the serialized savestates
    This allows a "segmented" speedrun innately
    RTA can be timed externally like normal

    A priority to-do operations list thing oh my God I can't think right now:
    Get a basic tileset
    Import it
    Figure out how to mark the tiles
        Ideas on this are trigger colliders with OnEnter and OnExit
        Might also use Tags
    If we can do this, we can flip a bool switch on the Player
    Which we can then treat as an environmental debuff (damage over time, like lava, can be represented as a -vitality modifier)


	Top priorities:

Void form is basically a gravity vampire. It does area attacks that often damage the player but siphon hp based on damage dealt.
	Builds a "well" of hp that will heal the player up to max health and save the rest of the healing for when they're damaged.
	Allows for frontloaded healing.
    Gameplay in this form is about mitigating large incoming damage by correctly stacking enemies and siphoning hp off them.
    Aesthetic is sinister, living off of others' life force. It's parasitic.
    Can exist in "the void", which is kinda like zones that produce a hitbox that just rips things with low void resistance to shreds
    Void bomb: PBAOE, self-damaging.
    Claw: Short melee attack.
    Gravity bomb: GT aoe root (cooldown)
    Gravity well: GT aoe suction (use negative knockback). Spam to stack enemies, root them, then aoe spam them.
Air form will be about increased movement speed, slowing enemies, and laying air mines, which explode on proximity.
    Areas will focus on quick movement, strategic placement of mines, enemy management, etc.
    Slowing proximity mine
    Damage mine, of which 3 can be laid. Detonated by:
    Lightning, a linear dps attack that burns high amounts of spirit (so not really spammable. Mostly used for damage and setting off mines)
    Wind Rip: PBAOE slow + personal speed boost
    Can have multiple switches that must be hit simultaneously to pass areas.
Water form's gameplay will be about "purification"--you're trying to heal things, or remove poisons, and such.
    Lategame, it will be a restorative form that the user shifts to to heal up.
    Gameplay will be about fixing problems more than engaging in combat directly.
    Player will work with indigenous populations to resolve their problems.
    Water form will have a "Purge" buff, which will load onto the buff list, then start unloading buffs every tick.
	Will also have a heal spell, probably. AoE will let us heal hurt npcs. Rain. Ground Targetted.
    Spirit recharging ability that's also Ground Targetted. Second Rain spell.
	Can breathe underwater infinitely.
    Deep water can act as a gating mechanic.
    Geyser - Kamehameha move. Water's only attack.
Light form can travel through Prisms. These are portals of both short-range variety and level-changing scope. Prisms are also the save point.
    Portals will be one-way, forcing certain pathing options.
    Light non-combat gameplay focuses on portal-based puzzle solving.
    Quick melee form focused on debuffing enemies. Like a portal rogue.
    Dispersion: Frontal light-cutting arc. Can eventually open portals. Bleeds enemies with focus debuff.
    Light-charge: Moves in a direction, hitting all in its path.
    Debuff strike: Lowers targets' resistances.
    Pierce: Short, near-instant attack that puts a *tiny* stackable vitality debuff on enemies.
Fire form is about raw damage output. It's destructive, and its abilities reflect this.
    It can cross lava, even having a few under-lava sections.
    It can clear obstructions that are vulnerable to fire.
    Ground-targetted fire attack. Cooldown. High damage dot.
    Fireball projectile. Mid-sized, adds a dot.
    Fire breath. High spirit dump that can be held for burst damage.
    Ember. Small, repeatable quick damage with no dot. Stacks a fire damage buff.
Earth form is about tanking damage and locking enemies into combat, stunning them so they can't do shenanigans.
    Powerful, melee range attack (Thud?)
    Earthquake: Aoe stun. (Cooldown)
    Endure: Buffs resistance, lowers personal movement.
    Tackle: Quick, short charge that stuns on hit. Movement not negated by Endure.
    Can eventually become strong enough to tank lava and the Void.
    Will be able to deal damage to fire-based creatures living under lava that Fire form can't affect.

Types of combat available at endgame:
    Earth > Void: Buff resists, stack healing well, tank near-insta-kills
    Fire > Water: Burn through spirit, shift, regen, repeat
    Air > Fire: Slow enemies, quicken self, shift, and kite
    Light > Void: Debuff enemies so that you can self-heal more
    Light > Earth: Stack bleeds on enemy, then shift to tank

	Add controls
	Refine the attacks a bit
	Program some AI for the enemies
	Give them some attacks
	Find some okayish sprites
	Start putting the world together
	Make a system for "+1/+1 counters" that buff you the longer you go without dying. Master Levels. A bonus to stats that resets on death.
	Prestige. Once a stat is capped, you can reset it for a permanent +1 that doesn't count towards cap.
	invulTimer will keep you from taking damage successively after each hit
	Set up some knockback effects, too

Design Stuff:
	Figure out intended controller layout
	Figure out intended menuing
	Figure out intended stat/level/growth chart stuff
	Figure out game-state map

World Stuff:
	Keep time travel as a theme
	Get abilities that let you open rifts
	Get abilities that let you find rift spots
	Gate content this way to keep people from going into super powerful areas before they're ready
	Make cultures that approach things differently than we do
	Make one that has its living remember its dead, ones they've never met, like John Green mentioned in Fault in Our Stars
	Loop back with a Prestige bonus each time you complete the game, making you more powerful and able to fight tougher stuff that you couldn't take on before

	First stage: Explore the world, find out how time is collapsing, gain all of your forms
	Second stage: Use your forms (still locked to one at a time) to explore areas you couldn't reach before, with the intent of reaching a boss you fight
		in order to gain the ability to shift forms in the overworld
	Third stage: You now have full access to your forms at will and can explore what you couldn't reach before
	Fourth stage: You can now choose to move between eras at will, instead of needing to use the 1 > 2 > 3 > 4 > 1 loop


The Eons:
	Make the eras like you used to envision.
	4 total.
	When you reach a way to clear the era, you can move to the next.
	1 > 2 > 3 > 4 > 1 etc
	You can't access everything your first time through because you haven't yet gotten the powers to do so
	Push time forward over and over until you make it how you want it.

	Wraiths of people trying to "get home"
	Starts from a portal, as per EE

"The Universe doesn't manifest anything it doesn't need. If you're here, you exist because the Universe felt you belonged here"

"This is it, right? This is what I was made for? To bring the End?
Why?
To watch us suffer? To watch us triumph? To watch our peaks and valleys, the melody of our lives?
To tell a story?
Why does any god create?
If there's an answer, it no longer matters.
I was made for one purpose.
I am Omega. I am the Endbringer"

How to add objects to the load list:
1) Make the Droppable class in Serializables (see Thingies in Serializable)

2) Put this script (modified for object) on the script:
		void Start () {
		GameManager.SaveEvent += SaveFunction;
	}

	void OnDestroy()
	{
		GameManager.SaveEvent -= SaveFunction;
	}

	public void SaveFunction(object sender, EventArgs args)
	{
		//You're probably going to be super confused at what I was doing here by the time you get around to using this functionality
		//So here's the lowdown: You're going to make spawnpoint objects
		//When the level loads, it's going to run some logic to figure out which enemies should exist
		//It's then going to spawn them at the spawnpoints
		//That's the idea, at least. Figure it out, future me.

		SavedDroppableEnemy enemy = new SavedDroppableEnemy ();
		//Set the constructor stats for the Enemy thingie
		enemy.positionX = transform.position.x;
		enemy.positionY = transform.position.y;

		GameManager.Instance.GetListForScene ().SavedEnemies.Add (enemy);
	}

3) Add a modded version of this script to the WorldManager:

	for (int i = 0; i < localList.SavedEnemies.Count; i++) {
					GameObject spawnedEnemy = (GameObject)Instantiate (EnemyPrefab);
					spawnedEnemy.transform.position = new Vector2 (localList.SavedEnemies [i].positionX, localList.SavedEnemies [i].positionY);
						
				}


    NOTES ON HOW TO MAKE THE HITBOXEN DO:
         GameObject myHitbox1 = (GameObject)Instantiate(redHitbox);
         ^ How you summon your visual hitbox from the depths
            HitboxScript myHitboxScript1 = myHitbox1.GetComponent<HitboxScript>();
            ^ This calls the script of your visual hitbox
            myHitbox1.transform.localScale = new Vector3(1.5f, 1.0f, 0); 
            ^ This is where you scale your visual hitbox. Multiplies by 32 here
            myHitbox1.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + .33f, 0);
            ^ This is where you set your visual box's position. Modify by .01 for each pixel. Overshoot yourself by 1 to be safe.
            myHitboxScript1.framesTilGone = 150;
            ^ This is how long your box visual lasts. 60 FPS standard.
            Entity.TriggerHits(this.transform.position.x - .24f, this.transform.position.x + .24f, this.transform.position.y + .49f, this.transform.position.y + .17f, 1, "Fire", 0, "None");
            ^ This is your *actual* hitbox. Again, it uses the center of *you* and modifies by 1 pixel per .01 in the transform

    NOTES ON SETTING KNOCKBACK:
        Set the mass of target's rb to 1
        Set the knockback close to 50 in the attack
        Set the linear drag on the target's rigidbody to 5
        Set the knockback coefficient to 1
        Tweak from there

    Environmental Colliders:
        Using the composite collider as a trigger results in OnTriggerEnter and OnTriggerExit only happening at the very
        boundary of the collider as a whole. Moving inside of that boundary counts as OnTriggerExit, which is less than helpful
        for sustained environmental checks.

    Use composite for events. Use non-composite for sustained triggers such as lava.
	*/ 
}
