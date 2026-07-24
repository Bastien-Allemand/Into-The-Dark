using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class RebindRow : MonoBehaviour
{
    [SerializeField] private TMP_Text actionName;
    [SerializeField] private TMP_Text bindingName;
    [SerializeField] private Button button;

    private InputAction action;
    private int bindingIndex;
    private string saveKey;
    private InputActionRebindingExtensions.RebindingOperation rebindOperation;

    public void Initialize(InputAction action, int bindingIndex, string saveKey)
    {
        this.action = action;
        this.bindingIndex = bindingIndex;
        this.saveKey = saveKey;

        // Si la touche fait partie d'un composite (ex: WASD), afficher "Move (Up)" au lieu de "Move"
        var binding = action.bindings[bindingIndex];
        if (binding.isPartOfComposite)
            actionName.text = $"{action.name} ({binding.name})";
        else
            actionName.text = action.name;

        Refresh();

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(StartRebind);
    }

    private void Refresh()
    {
        bindingName.text = action.GetBindingDisplayString(bindingIndex);
    }

    private void StartRebind()
    {
        button.interactable = false;
        bindingName.text = "Appuyez sur une touche...";

        action.Disable();

        // Annuler toute opération existante
        rebindOperation?.Cancel();

        // RÈGLE 3 : Exclure la position de la souris ET le clic de souris initial 
        // pour éviter le conflit avec le clic du bouton UI
        rebindOperation = action.PerformInteractiveRebinding(bindingIndex)
            .WithCancelingThrough("<Keyboard>/escape")
            .WithControlsExcluding("<Pointer>/position")
            .WithControlsExcluding("<Pointer>/delta")
            .WithControlsExcluding("<Mouse>/press") // Évite le relâchement de clic immédiat
            .OnComplete(operation => EndRebind())
            .OnCancel(operation => EndRebind())
            .Start();
    }

    private void EndRebind()
    {
        // Nettoyage de l'opération sans crash mémoire C++
        rebindOperation?.Dispose();
        rebindOperation = null;

        action.Enable();

        // Sauvegarde de l'état
        string json = action.actionMap.asset.SaveBindingOverridesAsJson();
        PlayerPrefs.SetString(saveKey, json);
        PlayerPrefs.Save();

        Refresh();
        button.interactable = true;
    }

    private void OnDisable()
    {
        // RÈGLE 4 : Si la ligne UI est masquée/détruite pendant une réassignation, 
        // on coupe l'opération proprement pour éviter un Access Violation Crash.
        if (rebindOperation != null)
        {
            rebindOperation.Cancel();
            rebindOperation.Dispose();
            rebindOperation = null;
        }
    }
}