// GENERATED AUTOMATICALLY FROM 'Assets/Input/InputMaster.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

namespace Jampacked.ProjectInca
{
    public class @InputMaster : IInputActionCollection, IDisposable
    {
        public InputActionAsset asset { get; }
        public @InputMaster()
        {
            asset = InputActionAsset.FromJson(@"{
    ""name"": ""InputMaster"",
    ""maps"": [
        {
            ""name"": ""Player"",
            ""id"": ""e1389408-651a-493d-9974-7d161245c7a4"",
            ""actions"": [
                {
                    ""name"": ""Move"",
                    ""type"": ""Value"",
                    ""id"": ""30997c03-a48a-466c-b697-e10202a80749"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Look"",
                    ""type"": ""Value"",
                    ""id"": ""c6a8715d-20cb-45ab-badb-da5624269437"",
                    ""expectedControlType"": ""Stick"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Jump"",
                    ""type"": ""Button"",
                    ""id"": ""b7d5efb6-6cbf-4e34-9f59-0d3342f9a085"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Shoot"",
                    ""type"": ""Button"",
                    ""id"": ""be4904ba-6b88-460c-af4b-4e61bc5de0c0"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press(behavior=2)""
                },
                {
                    ""name"": ""Reload"",
                    ""type"": ""Button"",
                    ""id"": ""3e8c14e6-16c0-4c5f-94d8-e2841e5bdbe6"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""WeaponSwap1"",
                    ""type"": ""Button"",
                    ""id"": ""77cd7841-46d2-4e3c-8335-fbfcd798dc78"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""WeaponSwap2"",
                    ""type"": ""Button"",
                    ""id"": ""a2d02f57-4014-429b-be71-6f62cfaf3266"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""WeaponSwap3"",
                    ""type"": ""Button"",
                    ""id"": ""9bf0a272-bf79-428d-8026-09c6ec213419"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""WeaponSwap4"",
                    ""type"": ""Button"",
                    ""id"": ""691733b4-5c00-43cc-91cd-174c9e4466a8"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Zoom"",
                    ""type"": ""Button"",
                    ""id"": ""ca731780-1587-4e48-8061-6f64f1eb3ca5"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press(behavior=2)""
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""1943c4ed-7152-4405-b769-e65fde4b2532"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": ""NormalizeVector2"",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""Up"",
                    ""id"": ""afdb310b-3cda-4470-9744-16c4b72f37bc"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Down"",
                    ""id"": ""5eb7e3b3-de20-441b-b322-f61cd63d1adc"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Left"",
                    ""id"": ""271877e7-14b4-4c99-8416-3f82987eac80"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Right"",
                    ""id"": ""5d39eb40-f86e-4ba8-94b4-c365925d0c97"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""fbe29e9d-5fdc-4089-bac2-573d40eed6ad"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""8517236d-c5a9-4c47-8f0f-a83de2359711"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Shoot"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""412e4df1-6611-4bc7-b836-34f1cb72e340"",
                    ""path"": ""<Keyboard>/r"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Reload"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a3ac9b59-9cab-4f97-ae19-9ebd3f3fe8fb"",
                    ""path"": ""<Keyboard>/1"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""WeaponSwap1"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a1317f9a-18d9-4b49-bcc8-fe9e5ebd7a66"",
                    ""path"": ""<Keyboard>/2"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""WeaponSwap2"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""927f6213-242f-4a97-a833-fffd41b3019c"",
                    ""path"": ""<Keyboard>/3"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""WeaponSwap3"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""3785037e-6cbe-4a2a-a747-71edb3159335"",
                    ""path"": ""<Mouse>/delta"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""bac278cb-c77c-4004-a14e-51a9ab931ebb"",
                    ""path"": ""<Keyboard>/4"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""WeaponSwap4"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""df3b158f-517d-4ddc-8208-124ddac46667"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": ""StickDeadzone"",
                    ""groups"": ""Gamepad;DualShock"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""41c9f60d-851b-43db-8028-053c9c4c2bb3"",
                    ""path"": ""<Gamepad>/rightStick"",
                    ""interactions"": """",
                    ""processors"": ""StickDeadzone,ScaleVector2(x=50,y=50)"",
                    ""groups"": ""Gamepad;DualShock"",
                    ""action"": ""Look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""468db6f4-a57a-4dd4-a66b-6e379dec0ea0"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad;DualShock"",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a8f0548f-5080-496a-83e2-10524ad48591"",
                    ""path"": ""<Gamepad>/rightTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Shoot"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""20dcf65f-1b2b-4079-84d7-42a5dc495a99"",
                    ""path"": ""<DualShockGamepad>/rightShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""DualShock"",
                    ""action"": ""Shoot"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""9ee934fe-e5c0-4ad1-8f9e-5e27f86bd640"",
                    ""path"": ""<Gamepad>/dpad/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad;DualShock"",
                    ""action"": ""WeaponSwap1"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""55bb6b17-9033-4370-bc7b-a0a6222740a8"",
                    ""path"": ""<Gamepad>/buttonWest"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad;DualShock"",
                    ""action"": ""Reload"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""3a1ad0f5-6aac-4be4-87b7-c98225a44ab4"",
                    ""path"": ""<Gamepad>/dpad/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad;DualShock"",
                    ""action"": ""WeaponSwap2"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f44562ad-ab5a-4eb5-b3fd-bebe7a8a70ac"",
                    ""path"": ""<Gamepad>/dpad/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad;DualShock"",
                    ""action"": ""WeaponSwap3"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""10be70bb-6f25-43bc-b36c-2eb1954aa74f"",
                    ""path"": ""<Gamepad>/dpad/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad;DualShock"",
                    ""action"": ""WeaponSwap4"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""9c570fee-5895-4ae0-bf99-20c9fea29553"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Zoom"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""7a63e19a-0c80-4863-bb28-ba2b334bd978"",
                    ""path"": ""<Gamepad>/leftTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Zoom"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f48e5d09-9c83-442d-baca-e1c0f8e2b3d9"",
                    ""path"": ""<DualShockGamepad>/leftShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""DualShock"",
                    ""action"": ""Zoom"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Keyboard"",
            ""bindingGroup"": ""Keyboard"",
            ""devices"": [
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": false,
                    ""isOR"": false
                },
                {
                    ""devicePath"": ""<Mouse>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        },
        {
            ""name"": ""Gamepad"",
            ""bindingGroup"": ""Gamepad"",
            ""devices"": [
                {
                    ""devicePath"": ""<Gamepad>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        },
        {
            ""name"": ""DualShock"",
            ""bindingGroup"": ""DualShock"",
            ""devices"": []
        }
    ]
}");
            // Player
            m_Player = asset.FindActionMap("Player", throwIfNotFound: true);
            m_Player_Move = m_Player.FindAction("Move", throwIfNotFound: true);
            m_Player_Look = m_Player.FindAction("Look", throwIfNotFound: true);
            m_Player_Jump = m_Player.FindAction("Jump", throwIfNotFound: true);
            m_Player_Shoot = m_Player.FindAction("Shoot", throwIfNotFound: true);
            m_Player_Reload = m_Player.FindAction("Reload", throwIfNotFound: true);
            m_Player_WeaponSwap1 = m_Player.FindAction("WeaponSwap1", throwIfNotFound: true);
            m_Player_WeaponSwap2 = m_Player.FindAction("WeaponSwap2", throwIfNotFound: true);
            m_Player_WeaponSwap3 = m_Player.FindAction("WeaponSwap3", throwIfNotFound: true);
            m_Player_WeaponSwap4 = m_Player.FindAction("WeaponSwap4", throwIfNotFound: true);
            m_Player_Zoom = m_Player.FindAction("Zoom", throwIfNotFound: true);
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

        // Player
        private readonly InputActionMap m_Player;
        private IPlayerActions m_PlayerActionsCallbackInterface;
        private readonly InputAction m_Player_Move;
        private readonly InputAction m_Player_Look;
        private readonly InputAction m_Player_Jump;
        private readonly InputAction m_Player_Shoot;
        private readonly InputAction m_Player_Reload;
        private readonly InputAction m_Player_WeaponSwap1;
        private readonly InputAction m_Player_WeaponSwap2;
        private readonly InputAction m_Player_WeaponSwap3;
        private readonly InputAction m_Player_WeaponSwap4;
        private readonly InputAction m_Player_Zoom;
        public struct PlayerActions
        {
            private @InputMaster m_Wrapper;
            public PlayerActions(@InputMaster wrapper) { m_Wrapper = wrapper; }
            public InputAction @Move => m_Wrapper.m_Player_Move;
            public InputAction @Look => m_Wrapper.m_Player_Look;
            public InputAction @Jump => m_Wrapper.m_Player_Jump;
            public InputAction @Shoot => m_Wrapper.m_Player_Shoot;
            public InputAction @Reload => m_Wrapper.m_Player_Reload;
            public InputAction @WeaponSwap1 => m_Wrapper.m_Player_WeaponSwap1;
            public InputAction @WeaponSwap2 => m_Wrapper.m_Player_WeaponSwap2;
            public InputAction @WeaponSwap3 => m_Wrapper.m_Player_WeaponSwap3;
            public InputAction @WeaponSwap4 => m_Wrapper.m_Player_WeaponSwap4;
            public InputAction @Zoom => m_Wrapper.m_Player_Zoom;
            public InputActionMap Get() { return m_Wrapper.m_Player; }
            public void Enable() { Get().Enable(); }
            public void Disable() { Get().Disable(); }
            public bool enabled => Get().enabled;
            public static implicit operator InputActionMap(PlayerActions set) { return set.Get(); }
            public void SetCallbacks(IPlayerActions instance)
            {
                if (m_Wrapper.m_PlayerActionsCallbackInterface != null)
                {
                    @Move.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMove;
                    @Move.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMove;
                    @Move.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMove;
                    @Look.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnLook;
                    @Look.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnLook;
                    @Look.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnLook;
                    @Jump.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnJump;
                    @Jump.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnJump;
                    @Jump.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnJump;
                    @Shoot.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnShoot;
                    @Shoot.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnShoot;
                    @Shoot.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnShoot;
                    @Reload.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnReload;
                    @Reload.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnReload;
                    @Reload.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnReload;
                    @WeaponSwap1.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnWeaponSwap1;
                    @WeaponSwap1.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnWeaponSwap1;
                    @WeaponSwap1.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnWeaponSwap1;
                    @WeaponSwap2.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnWeaponSwap2;
                    @WeaponSwap2.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnWeaponSwap2;
                    @WeaponSwap2.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnWeaponSwap2;
                    @WeaponSwap3.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnWeaponSwap3;
                    @WeaponSwap3.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnWeaponSwap3;
                    @WeaponSwap3.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnWeaponSwap3;
                    @WeaponSwap4.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnWeaponSwap4;
                    @WeaponSwap4.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnWeaponSwap4;
                    @WeaponSwap4.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnWeaponSwap4;
                    @Zoom.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnZoom;
                    @Zoom.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnZoom;
                    @Zoom.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnZoom;
                }
                m_Wrapper.m_PlayerActionsCallbackInterface = instance;
                if (instance != null)
                {
                    @Move.started += instance.OnMove;
                    @Move.performed += instance.OnMove;
                    @Move.canceled += instance.OnMove;
                    @Look.started += instance.OnLook;
                    @Look.performed += instance.OnLook;
                    @Look.canceled += instance.OnLook;
                    @Jump.started += instance.OnJump;
                    @Jump.performed += instance.OnJump;
                    @Jump.canceled += instance.OnJump;
                    @Shoot.started += instance.OnShoot;
                    @Shoot.performed += instance.OnShoot;
                    @Shoot.canceled += instance.OnShoot;
                    @Reload.started += instance.OnReload;
                    @Reload.performed += instance.OnReload;
                    @Reload.canceled += instance.OnReload;
                    @WeaponSwap1.started += instance.OnWeaponSwap1;
                    @WeaponSwap1.performed += instance.OnWeaponSwap1;
                    @WeaponSwap1.canceled += instance.OnWeaponSwap1;
                    @WeaponSwap2.started += instance.OnWeaponSwap2;
                    @WeaponSwap2.performed += instance.OnWeaponSwap2;
                    @WeaponSwap2.canceled += instance.OnWeaponSwap2;
                    @WeaponSwap3.started += instance.OnWeaponSwap3;
                    @WeaponSwap3.performed += instance.OnWeaponSwap3;
                    @WeaponSwap3.canceled += instance.OnWeaponSwap3;
                    @WeaponSwap4.started += instance.OnWeaponSwap4;
                    @WeaponSwap4.performed += instance.OnWeaponSwap4;
                    @WeaponSwap4.canceled += instance.OnWeaponSwap4;
                    @Zoom.started += instance.OnZoom;
                    @Zoom.performed += instance.OnZoom;
                    @Zoom.canceled += instance.OnZoom;
                }
            }
        }
        public PlayerActions @Player => new PlayerActions(this);
        private int m_KeyboardSchemeIndex = -1;
        public InputControlScheme KeyboardScheme
        {
            get
            {
                if (m_KeyboardSchemeIndex == -1) m_KeyboardSchemeIndex = asset.FindControlSchemeIndex("Keyboard");
                return asset.controlSchemes[m_KeyboardSchemeIndex];
            }
        }
        private int m_GamepadSchemeIndex = -1;
        public InputControlScheme GamepadScheme
        {
            get
            {
                if (m_GamepadSchemeIndex == -1) m_GamepadSchemeIndex = asset.FindControlSchemeIndex("Gamepad");
                return asset.controlSchemes[m_GamepadSchemeIndex];
            }
        }
        private int m_DualShockSchemeIndex = -1;
        public InputControlScheme DualShockScheme
        {
            get
            {
                if (m_DualShockSchemeIndex == -1) m_DualShockSchemeIndex = asset.FindControlSchemeIndex("DualShock");
                return asset.controlSchemes[m_DualShockSchemeIndex];
            }
        }
        public interface IPlayerActions
        {
            void OnMove(InputAction.CallbackContext context);
            void OnLook(InputAction.CallbackContext context);
            void OnJump(InputAction.CallbackContext context);
            void OnShoot(InputAction.CallbackContext context);
            void OnReload(InputAction.CallbackContext context);
            void OnWeaponSwap1(InputAction.CallbackContext context);
            void OnWeaponSwap2(InputAction.CallbackContext context);
            void OnWeaponSwap3(InputAction.CallbackContext context);
            void OnWeaponSwap4(InputAction.CallbackContext context);
            void OnZoom(InputAction.CallbackContext context);
        }
    }
}
