using UnityEngine;

public class Health : MonoBehaviour
{
    public enum UnitAffiliation 
    { 
        player,
        enemy
    }

    #region variables

    [Header("Unit Health Values")]
    [SerializeField] UnitAffiliation thisUnitType;

    [SerializeField] private int maxHealthPoints = 10; // set to 10 by default, this value will be changed depending on who/what it is applied to
    public int GetMaxHealthPoints() { return maxHealthPoints; }
    private int currentHealthPoints;
    public int GetCurrentHealthPoints() { return currentHealthPoints; }

    [Header("Invinicibility Frames")]
    [SerializeField] private bool hasIFrames = false;
    [SerializeField] private float maxInvincibilityTimer = 0.6f;
    private float currentInvincibilityTimer = 0; // timer for invincibility; as long as this value is greater than 0, this unit will be unable to lose health points (user can still gain health points) 

    private bool canTakeDamage = true; // manual override for taking damage; setting this value to false will prevent this user from losing health points (user can still gain health points)
    public void SetCanTakeDamage(bool value) { canTakeDamage = value; }

    public bool UnitIsDamageable() { return canTakeDamage && currentInvincibilityTimer <= 0; } // function returns true if this unit can take damage, and false if it can't take damage

    #endregion


    #region events

    public delegate void ModifyHealth(int newHealthValue, int maxHealthValue, int healthDelta);
    public static ModifyHealth OnHealthChangeEvent;

    public delegate void UnitDeath(UnitAffiliation unitAffiliation, GameObject unit);
    public static UnitDeath OnUnitDeathEvent;

    #endregion


    private void Start()
    {
        currentHealthPoints = maxHealthPoints;
    }

    public void ChangeHealthBy(int value)
    {
        if (value < 0)
        {
            if (!UnitIsDamageable())
            {
                return; // if this unit is unable to take damage currently and the health is attempted to be changed by a negative amount, end this function
            }
        
            if (hasIFrames)
            {
                currentInvincibilityTimer = maxInvincibilityTimer;
            }
        }

        currentHealthPoints += value;
        currentHealthPoints = Mathf.Clamp(currentHealthPoints, 0, maxHealthPoints);

        OnHealthChangeEvent?.Invoke(currentHealthPoints, maxHealthPoints, value); // calls an event to functions with an 3 integer paramaters, with currentHealthPoints defining the integer parameter's value
        //The function also sends the players maxhealth and the value that changed for expanded functionality
        
        if (currentHealthPoints <= 0)
        {
            OnUnitDeathEvent?.Invoke(thisUnitType, this.gameObject); // calls an event to functions with UnitAffiliation as a paramater, and thisUnitType defines the parameter's value

                // examples of subscribed functions
                        // if (thisUnitType == UnitAffiliation.player) { reload the level from the beginning; }
                        // if (thisUnitType == UnitAffiliation.enemy) { increase the combo kill counter by one; }
            
        }
    }

    private void Update()
    {
        if (currentInvincibilityTimer > 0)
        {
            currentInvincibilityTimer -= Time.deltaTime;
        }
    }
}