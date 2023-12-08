using UnityEngine;
using DG.Tweening;

[CreateAssetMenu(fileName = "PlayerData", menuName = "ScriptableObject/PlayerData", order = 0)]
public class PlayerData : ScriptableObject {
    
    [SerializeField] private float playerSpeed;
    [SerializeField] private float smooth;
    [SerializeField] private float flipDuration;
    [SerializeField] private Ease flipEase;

    public float PlayerSpeed => playerSpeed;
    public float Smooth => smooth;
    public float Flip => flipDuration;
    public Ease PlayerEase => flipEase;

}
