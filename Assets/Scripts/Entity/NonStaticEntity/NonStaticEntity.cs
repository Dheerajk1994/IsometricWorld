using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonStaticEntity : MonoBehaviour
{
    [SerializeField] private string nonStaticEntityName;
    [SerializeField] private string description;
    [SerializeField] private int health;
    [SerializeField] private int age;
    [SerializeField] private bool isMale;

    //hunger

    public string Description { get => description; set => description = value; }
    public string NonStaticEntityName1 { get => nonStaticEntityName; set => nonStaticEntityName = value; }
    public int Health { get => health; set => health = value; }
    public int Age { get => age; set => age = value; }
    public bool IsMale { get => isMale; set => isMale = value; }

    public NonStaticEntity(string nonStaticEntityName, string description, int health, int age, bool isMale)
    {
        this.nonStaticEntityName = nonStaticEntityName;
        this.description = description;
        this.health = health;
        this.age = age;
        this.isMale = isMale;
    }
}
