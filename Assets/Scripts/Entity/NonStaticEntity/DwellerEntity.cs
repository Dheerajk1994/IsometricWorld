using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DwellerEntity : NonStaticEntity
{
    [SerializeField] private float happiness;
    [SerializeField] private DwellerEntity father;
    [SerializeField] private DwellerEntity mother;
    [SerializeField] private List<DwellerEntity> children;

    public float Happiness { get => happiness; set => happiness = value; }
    public DwellerEntity Father { get => father; set => father = value; }
    public DwellerEntity Mother { get => mother; set => mother = value; }
    public List<DwellerEntity> Children { get => children; set => children = value; }
    
    public DwellerEntity(string nonStaticEntityName, string description, int health, int age, bool isMale) 
        :
        base(nonStaticEntityName, description, health, age, isMale)
    {
    }
}
