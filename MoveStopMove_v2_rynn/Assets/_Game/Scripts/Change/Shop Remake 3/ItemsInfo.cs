using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "SO / Item")]
public class ItemsInfo : ScriptableObject
{
    [Header("Head Pos")]

    public GameObject[] HeadPos;

    [Header("Tail Pos")]

    public GameObject[] TailPos;

    [Header("Shield Pos")]

    public GameObject[] ShieldPos;

    [Header("Pants")]

    public Material[] PantsMaterials;

    [Header("Skin")]

    public Material[] SkinMaterials;

    [Header("Wing")]

    public GameObject[] WingPos;

}
