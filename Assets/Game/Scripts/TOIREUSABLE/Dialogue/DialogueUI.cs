using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TOI2D
{
    public class DialogueUI : MonoBehaviour
    {
        [SerializeField] private GameObject dialogueBox;
        [SerializeField] private TMP_Text _textLabel;

        [SerializeField] private TMP_Text _name;
        [SerializeField] private Image potrait;

        private PlayerController _player;
        private TypewriterEffect typewriterEffect;

        private int currentDialogIndex = 0;
        private Coroutine typingCoroutine;

        public bool IsOpen { get; private set; }
        bool isTyping = false;
        bool showDialog = false;

        public float typingSpeed = 0.05f;
        public string[] dialogs;
        GameObject interactableNotifTemp = null;
        public Dialogue dialogObj = null;
        Interactable interactableObject = null;
        [SerializeField] Dialogue dialogObjTest;
        [SerializeField] RectTransform tearDrop;
        [SerializeField] RectTransform tearDropBackground;
        [SerializeField] GameObject tearDropReference;
        private void Awake()
        {

            typewriterEffect = GetComponent<TypewriterEffect>();

        }

        private void Start()
        {
            CloseDialogue();
        }
        void Update()
        {
            //if (Input.GetKeyDown(KeyCode.Equals))
            //{
            //    InitDialogue(dialogObjTest);
            //}

            if (showDialog)
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    if (isTyping)
                    {
                        // Skip typing effect
                        StopCoroutine(typingCoroutine);
                        _textLabel.text = dialogObj.Dialogues[currentDialogIndex].Dialogue;
                        isTyping = false;
                    }
                    else
                    {
                        // Display next dialog or end the conversation
                        currentDialogIndex++;
                        if (currentDialogIndex < dialogObj.Dialogues.Length)
                        {
                            ShowDialog();
                        }
                        else
                        {
                            // End of conversation
                            CloseDialogue();
                            //Debug.Log("End of conversation");
                        }
                    }
                }
        }
        void SetDialogsAndPotrait(Dialogue dialogObj)
        {
            dialogs = new string[dialogObj.Dialogues.Length];
        }

        void ShowDialog()
        {
            _textLabel.text = string.Empty;
            SetPotrait(dialogObj.Dialogues[currentDialogIndex].characterId, dialogObj.potraitDatas);
            typingCoroutine = StartCoroutine(TypeText(dialogObj.Dialogues[currentDialogIndex].Dialogue));
        }
        void ConvertnpcPositionToCanvas(GameObject target)
        {
            Vector3 ObjReferenceOnCanvasPosition = Camera.main.WorldToScreenPoint(target.transform.position);
            tearDropReference.transform.position = new Vector3(ObjReferenceOnCanvasPosition.x, ObjReferenceOnCanvasPosition.y, 0);
            Vector3 direction = tearDropReference.transform.position - tearDrop.transform.position;

            tearDrop.rotation = Quaternion.LookRotation(direction);
            tearDropBackground.rotation = Quaternion.LookRotation(direction);


        }
        IEnumerator TypeText(string text)
        {
            isTyping = true;
            foreach (char letter in text.ToCharArray())
            {
                _textLabel.text += letter;
                yield return new WaitForSeconds(typingSpeed);
            }
            isTyping = false;
        }
        public void InitDialogue(Dialogue dialogObj, GameObject interactableNotif = null, Interactable interactableObject = null, GameObject player = null)
        {
            currentDialogIndex = 0;
            GameplayManager.instance.player.CanMove = false;
            GameplayManager.instance.player.CanInteract = false;
            this.dialogObj = dialogObj;
            interactableNotifTemp = interactableNotif;
            this.interactableObject = interactableObject;

            showDialog = true;
            dialogueBox.SetActive(true);
            ConvertnpcPositionToCanvas(interactableObject.gameObject);
            ShowDialog();
            //StartCoroutine(RunDialogueTypewriter(dialogObj, interactableNotif, interactableObject, player));
        }

        private IEnumerator RunDialogueTypewriter(Dialogue dialogObj, GameObject interactableNotif = null, InteractableObject interactableObject = null, GameObject player = null)
        {


            for (int i = 0; i < dialogObj.Dialogues.Length; i++)
            {
                yield return typewriterEffect.Run(dialogObj.Dialogues[i].Dialogue, _textLabel);
                yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
            }
            CloseDialogue();

        }

        private void CloseDialogue()
        {
            IsOpen = false;
            _textLabel.text = string.Empty;
            GameplayManager.instance.player.CanMove = true;
            GameplayManager.instance.player.CanInteract = true;
            //potrait.gameObject.SetActive(false);
            dialogueBox.SetActive(false);

            if (interactableNotifTemp != null)
                interactableNotifTemp.SetActive(true);

            showDialog = false;
            dialogObj = null;
            interactableNotifTemp = null;
            interactableObject = null;
        }

        private void SetPotrait(string characterID = null, PotraitData[] potraitDatas = null)
        {
            if (characterID != string.Empty)
            {
                if (!potrait.gameObject.activeInHierarchy)
                    potrait.gameObject.SetActive(true);

                _name.text = characterID;
                for (int i = 0; i < potraitDatas.Length; i++)
                {
                    if (characterID == potraitDatas[i].name)
                    {
                        potrait.sprite = potraitDatas[i].potrait;
                        break;
                    }
                }
            }
        }

    }
}


