﻿/// ---------------------------------------------
/// Ultimate Character Controller
/// Copyright (c) Opsive. All Rights Reserved.
/// https://www.opsive.com
/// ---------------------------------------------

namespace Opsive.UltimateCharacterController.Objects.CharacterAssist
{
    using System;
    using Opsive.Shared.Audio;
    using Opsive.Shared.Game;
    using Opsive.UltimateCharacterController.Game;
#if ULTIMATE_CHARACTER_CONTROLLER_MULTIPLAYER
    using Opsive.UltimateCharacterController.Networking.Game;
#endif
    using UnityEngine;
    using UnityEngine.Events;
    using EventHandler = Opsive.Shared.Events.EventHandler;

    /// <summary>
    /// Base class for any object that can be picked up.
    /// </summary>
    public abstract class ObjectPickup : MonoBehaviour, IObjectPickup
    {
        [Tooltip("The amount of time to enable the trigger after the object has been enabled and the rigidbody has stopped moving.")]
        [SerializeField] protected float m_TriggerEnableDelay = 4;
        [Tooltip("Should the item be picked up when the character enters the trigger?")]
        [SerializeField] protected bool m_PickupOnTriggerEnter = true;
        [Tooltip("The amount that the object should rotate while waiting to be picked up.")]
        [SerializeField] protected Vector3 m_RotationSpeed;
        [Tooltip("A set of AudioClips that can be played when the object is picked up.")]
        [SerializeField] protected AudioClipSet m_PickupAudioClipSet = new AudioClipSet();
        [Tooltip("The text that should be shown by the message monitor when the object is picked up.")]
        [SerializeField] protected string m_PickupMessageText;
        [Tooltip("The sprite that should be drawn by the message monitor when the object is picked up.")]
        [SerializeField] protected Sprite m_PickupMessageIcon;
        [Tooltip("The object was detected by a character.")]
        [SerializeField] protected UnityEvent m_OnSelect;
        [Tooltip("The object is no longer detected by the character.")]
        [SerializeField] protected UnityEvent m_OnDeselect;
        [Tooltip("Delay in seconds. If set to 0 there will be no delay, if -1 the item will not be destroyed.")]
        [SerializeField] protected float m_DestroyDelay;

        public float TriggerEnableDelay { get { return m_TriggerEnableDelay; } set { m_TriggerEnableDelay = value; } }
        public bool PickupOnTriggerEnter { get { return m_PickupOnTriggerEnter; } set { m_PickupOnTriggerEnter = value; } }
        public Vector3 RotationSpeed { get { return m_RotationSpeed; } set { m_RotationSpeed = value; enabled = m_RotationSpeed.sqrMagnitude > 0; } }
        public AudioClipSet PickupAudioClipSet { get { return m_PickupAudioClipSet; } set { m_PickupAudioClipSet = value; } }
        public string PickupMessageText { get { return m_PickupMessageText; } set { m_PickupMessageText = value; } }
        public Sprite PickupMessageIcon { get { return m_PickupMessageIcon; } set { m_PickupMessageIcon = value; } }
        public UnityEvent OnSelectEvent { get { return m_OnSelect; } set { m_OnSelect = value; } }
        public UnityEvent OnDeselectEvent { get { return m_OnDeselect; } set { m_OnDeselect = value; } }

        [System.NonSerialized] private GameObject m_GameObject;
        private Transform m_Transform;
        private Rigidbody m_Rigidbody;
        private Collider m_Trigger;

        private bool m_Selected;
        private int m_StartLayer;
        private bool m_IsDepleted = true;
        private ScheduledEventBase m_TriggerEnableEvent;
        private bool m_Initialized;

        public bool IsDepleted { get { return m_IsDepleted; } }

        /// <summary>
        /// Initialize the default values.
        /// </summary>
        protected virtual void Awake()
        {
            m_GameObject = gameObject;
            m_Transform = transform;
            m_Rigidbody = GetComponent<Rigidbody>();
            m_StartLayer = m_GameObject.layer;

            // Get a reference to the trigger and non-trigger collider. The collider will be disabled when the Rigidbody has stopped moving. The trigger will be enabled
            // when the Rigidbody has stopped moving.
            var colliders = GetComponents<Collider>();
            for (int i = 0; i < colliders.Length; ++i) {
                if (colliders[i].isTrigger) {
                    m_Trigger = colliders[i];
                    break;
                }
            }

            if (m_Trigger == null) {
                Debug.LogError("Error: A trigger must exist on the ObjectPickup component on the GameObject " + name + ".");
            }
        }

        /// <summary>
        /// The object has been enabled.
        /// </summary>
        protected virtual void OnEnable()
        {
            Initialize(false);
            EventHandler.RegisterEvent<GameObject, bool>(gameObject, "OnObjectDetected", OnObjectDetected);
            OnObjectDetected(null, false);
        }

        /// <summary>
        /// Initializes the object.
        /// </summary>
        /// <param name="forceInitialization">Should the object be initialized even if it isn't depleted?</param>
        public void Initialize(bool forceInitialization)
        {
            // If the item isn't depleted then it has already been initialized.
            if (!m_IsDepleted && !forceInitialization) {
                return;
            }

            m_IsDepleted = false;
            if (m_TriggerEnableDelay > 0) {
                m_Trigger.enabled = false;
                m_GameObject.layer = LayerManager.IgnoreRaycast;
                if (m_Initialized || forceInitialization) {
                    // If a rigidbody exists then the trigger event should be scheduled after the rigidbody has settled. If a rigidbody does not exist then
                    // the event should be scheduled immediately.
                    if (m_Rigidbody != null) {
                        if (m_TriggerEnableEvent != null) {
                            Scheduler.Cancel(m_TriggerEnableEvent);
                            m_TriggerEnableEvent = null;
                        }
                        Scheduler.Schedule(0.2f, CheckVelocity);
                    } else {
                        m_TriggerEnableEvent = Scheduler.Schedule(m_TriggerEnableDelay, EnableTrigger);
                    }
                } else {
                    // If the object isn't initialized yet then this is the first time the object has spawned.
                    EnableTrigger();
                }
            }
            m_Initialized = true;
        }

        /// <summary>
        /// Disable the component when the Rigidbody has settled.
        /// </summary>
        private void CheckVelocity()
        {
#if UNITY_6000_0_OR_NEWER
            if (m_Rigidbody.linearVelocity.sqrMagnitude < 0.01f) {
#else
            if (m_Rigidbody.velocity.sqrMagnitude < 0.01f) {
#endif
                m_TriggerEnableEvent = Scheduler.Schedule(m_TriggerEnableDelay, EnableTrigger);
                return;
            }
            // The Rigidbody hasn't settled yet - check the velocity again in the future.
            Scheduler.Schedule(0.2f, CheckVelocity);
        }

        /// <summary>
        /// Enables the trigger.
        /// </summary>
        private void EnableTrigger()
        {
            m_Trigger.enabled = true;
            m_TriggerEnableEvent = null;
            m_GameObject.layer = m_StartLayer;
        }

        /// <summary>
        /// The event executed when a character has detected the object with the DetectObjectAbilityBase.
        /// </summary>
        /// <param name="interactor">The character gameobject that detected the object.</param>
        /// <param name="selected">The object was selected?</param>
        protected virtual void OnObjectDetected(GameObject interactor, bool selected)
        {
            m_Selected = selected;
                
            if (selected) {
                m_OnSelect?.Invoke();
            } else {
                m_OnDeselect?.Invoke();
            }
        }

        /// <summary>
        /// Optionally rotates the object.
        /// </summary>
        private void Update()
        {
            m_Transform.rotation *= Quaternion.Euler(m_RotationSpeed);
        }

        /// <summary>
        /// An object has entered the trigger.
        /// </summary>
        /// <param name="other">The object which entered the trigger.</param>
        public virtual void OnTriggerEnter(Collider other)
        {
            // The object can't be picked up if it is depleted.
            if (m_IsDepleted) {
                return;
            }

            TriggerEnter(other.gameObject);
        }

        /// <summary>
        /// A GameObject has entered the trigger.
        /// </summary>
        /// <param name="other">The GameObject that entered the trigger.</param>
        public abstract void TriggerEnter(GameObject other);

        /// <summary>
        /// Picks up the object.
        /// </summary>
        /// <param name="target">The object doing the pickup.</param>
        public abstract void DoPickup(GameObject target);

        /// <summary>
        /// The object has been picked up.
        /// </summary>
        /// <param name="pickedUpBy">A reference to the object that picked up the object.</param>
        protected virtual void ObjectPickedUp(GameObject pickedUpBy)
        {
            // The object may not have been instantiated within the scene.
            if (m_GameObject == null) {
                return;
            }

            m_IsDepleted = true;

            // Send an event notifying of the pickup.
            EventHandler.ExecuteEvent(pickedUpBy, "OnObjectPickedUp", this);

            // Optionally play a pickup sound if the object picking up the item is attached to a camera.
            // A null GameObject indicates that the clip will play from the AudioManager.
            var foundCamera = Shared.Camera.CameraUtility.FindCamera(pickedUpBy);
            if (foundCamera != null) {
                m_PickupAudioClipSet.PlayAtPosition(m_Transform.position);
            }

            if (m_DestroyDelay == 0) {
                DestroyPickup();
            } else if (m_DestroyDelay > 0) {
                Scheduler.Schedule(m_DestroyDelay, DestroyPickup);
            }
        }

        /// <summary>
        /// Destroys the pickup.
        /// </summary>
        public virtual void DestroyPickup()
        {
            if (ObjectPoolBase.InstantiatedWithPool(m_GameObject)) {
#if ULTIMATE_CHARACTER_CONTROLLER_MULTIPLAYER
                if (NetworkObjectPool.IsNetworkActive()) {
                    NetworkObjectPool.Destroy(m_GameObject);
                    return;
                }
#endif
                ObjectPoolBase.Destroy(m_GameObject);
            } else {
                // Deactivate the pickup for now. It can appear again if a Respawner component is attached to the GameObject.
                m_GameObject.SetActive(false);
            }
        }

        /// <summary>
        /// The object has been disabled.
        /// </summary>
        protected virtual void OnDisable()
        {
            EventHandler.UnregisterEvent<GameObject, bool>(gameObject, "OnObjectDetected", OnObjectDetected);
            if (m_Selected) {
                OnObjectDetected(null, false);
            }
        }
    }
}