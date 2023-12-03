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
        [SerializeField] GameObject tearDrop;
        [SerializeField] GameObject tearDropBackground;

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
            RectTransform canvasRectTransform = FindObjectOfType<Canvas>().GetComponent<RectTransform>();
            Vector3 worldPosition = target.transform.position;
            Vector3 canvasPosition;

            RectTransformUtility.ScreenPointToWorldPointInRectangle(canvasRectTransform, Camera.main.WorldToScreenPoint(worldPosition), Camera.main, out canvasPosition);

            //float rotationZ = Mathf.Atan2(canvasPosition.y, canvasPosition.x) * Mathf.Rad2Deg;
            //tearDrop.transform.rotation = Quaternion.Euler(0.0f, 0.0f, rotationZ);
            //tearDropBackground.transform.rotation = Quaternion.Euler(0.0f, 0.0f, rotationZ);

            Transform reference;
            //reference.LookAt(canvasPosition);
            Vector3 difference = canvasPosition - tearDrop.transform.position;
            Debug.Log("diff : " + difference + " ; canvas position : " + canvasPosition);
            tearDrop.transform.LookAt(difference);
            tearDropBackground.transform.LookAt(difference);
            //Quaternion rotation = Quaternion.LookRotation(difference, Vector3.up);
            //Vector3 lookAtPosition = new Vector3(canvasPosition.x, camva, canvasPosition.z);
            //tearDrop.transform.rotation = rotation;
            //tearDropBackground.transform.rotation = rotation;
            //tearDropBackground.transform.LookAt(new Vector3(0, 0, canvasPosition.z));

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
            //if (interactableObject != null)
            //    if (interactableObject.npc)
            //    {
            //        GameObject npcObject = interactableObject.gameObject;
            //        NPCController npc = interactableObject.gameObject.GetComponent<NPCController>();
            //        npc.PlayInteruptedAnimation(player.transform, npcObject.transform);

            //    }

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
            //_player.canMove = false;
            //if (interactableObject != null)
            //    if (interactableObject.npc)
            //    {
            //        GameObject npcObject = interactableObject.gameObject;
            //        NPCController npc = interactableObject.gameObject.GetComponent<NPCController>();
            //        npc.PlayInteruptedAnimation(player.transform, npcObject.transform);

            //    }

            for (int i = 0; i < dialogObj.Dialogues.Length; i++)
            {

                //old
                //SetPotrait(dialogObj.Dialogues[i].characterId, dialogObj.potraitDatas);
                yield return typewriterEffect.Run(dialogObj.Dialogues[i].Dialogue, _textLabel);
                yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));



                //SetPotrait(dialogObj.Dialogues[i].characterId, dialogObj.potraitDatas);
                //Coroutine typingCoroutine = StartCoroutine(typewriterEffect.TypeText(dialogObj.Dialogues[i].Dialogue, _textLabel));


                //yield return new WaitUntil(() => typingDone || Input.GetKeyDown(KeyCode.Space));
                //// Hentikan typewriterEffect jika masih berjalan
                //if (!typingDone)
                //{
                //    StopCoroutine(typingCoroutine);
                //    _textLabel.text = dialogObj.Dialogues[i].Dialogue;
                //    yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
                //}

                //yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
                //yield return typingCoroutine;
            }
            //if (interactableObject != null)
            //    if (interactableObject.npc)
            //    {
            //        GameObject npcObject = interactableObject.gameObject;
            //        NPCController npc = interactableObject.gameObject.GetComponent<NPCController>();
            //        npc.SetDefaultDirectionAnimation();
            //        npc.Interupted(false, 1);
            //    }
            CloseDialogue();

        }

        private void CloseDialogue()
        {
            //if (interactableObject != null)
            //    if (interactableObject.npc)
            //    {
            //        GameObject npcObject = interactableObject.gameObject;
            //        //NPCController npc = interactableObject.gameObject.GetComponent<NPCController>();
            //        //npc.SetDefaultDirectionAnimation();
            //        //npc.Interupted(false, 1);
            //    }

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


