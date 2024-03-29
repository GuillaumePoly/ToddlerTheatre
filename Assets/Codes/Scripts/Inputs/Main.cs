//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.7.0
//     from Assets/Codes/Scripts/Inputs/Main.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public partial class @Main: IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @Main()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""Main"",
    ""maps"": [
        {
            ""name"": ""Pointer"",
            ""id"": ""db9952db-3697-4957-9174-a7a9b52a7562"",
            ""actions"": [
                {
                    ""name"": ""Action"",
                    ""type"": ""Button"",
                    ""id"": ""7c91fe3b-d497-4c51-8b48-69e40367dd27"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Position"",
                    ""type"": ""Value"",
                    ""id"": ""fb013d20-e103-4e86-b34a-a5fed276d5c6"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""13e7e3de-d099-471d-9b02-c3a2a3686cbd"",
                    ""path"": ""<Pointer>/press"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Action"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d9664803-6e6f-4708-94a1-89614f498491"",
                    ""path"": ""<Pointer>/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Position"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Pointer
        m_Pointer = asset.FindActionMap("Pointer", throwIfNotFound: true);
        m_Pointer_Action = m_Pointer.FindAction("Action", throwIfNotFound: true);
        m_Pointer_Position = m_Pointer.FindAction("Position", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    public IEnumerable<InputBinding> bindings => asset.bindings;

    public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
    {
        return asset.FindAction(actionNameOrId, throwIfNotFound);
    }

    public int FindBinding(InputBinding bindingMask, out InputAction action)
    {
        return asset.FindBinding(bindingMask, out action);
    }

    // Pointer
    private readonly InputActionMap m_Pointer;
    private List<IPointerActions> m_PointerActionsCallbackInterfaces = new List<IPointerActions>();
    private readonly InputAction m_Pointer_Action;
    private readonly InputAction m_Pointer_Position;
    public struct PointerActions
    {
        private @Main m_Wrapper;
        public PointerActions(@Main wrapper) { m_Wrapper = wrapper; }
        public InputAction @Action => m_Wrapper.m_Pointer_Action;
        public InputAction @Position => m_Wrapper.m_Pointer_Position;
        public InputActionMap Get() { return m_Wrapper.m_Pointer; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PointerActions set) { return set.Get(); }
        public void AddCallbacks(IPointerActions instance)
        {
            if (instance == null || m_Wrapper.m_PointerActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_PointerActionsCallbackInterfaces.Add(instance);
            @Action.started += instance.OnAction;
            @Action.performed += instance.OnAction;
            @Action.canceled += instance.OnAction;
            @Position.started += instance.OnPosition;
            @Position.performed += instance.OnPosition;
            @Position.canceled += instance.OnPosition;
        }

        private void UnregisterCallbacks(IPointerActions instance)
        {
            @Action.started -= instance.OnAction;
            @Action.performed -= instance.OnAction;
            @Action.canceled -= instance.OnAction;
            @Position.started -= instance.OnPosition;
            @Position.performed -= instance.OnPosition;
            @Position.canceled -= instance.OnPosition;
        }

        public void RemoveCallbacks(IPointerActions instance)
        {
            if (m_Wrapper.m_PointerActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IPointerActions instance)
        {
            foreach (var item in m_Wrapper.m_PointerActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_PointerActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public PointerActions @Pointer => new PointerActions(this);
    public interface IPointerActions
    {
        void OnAction(InputAction.CallbackContext context);
        void OnPosition(InputAction.CallbackContext context);
    }
}
