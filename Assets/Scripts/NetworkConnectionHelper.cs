using UnityEngine;
using Unity.Netcode;
using UnityEngine.Events;
using Unity.Netcode.Transports.UNET;
//using UnityEditor.XR.Management;
//using UnityEditor;
using UnityEngine.XR.Management;
//using UnityEditor.XR.Management.Metadata;

public class NetworkConnectionHelper : MonoBehaviour
{
    [SerializeField] private UnityEvent OnConnected;
    [SerializeField] private UnityEvent OnServerStarted;

    private void Awake()
    {
        SetXRPlugins();
    }

    private void Start()
    {
        NetworkManager.Singleton.OnClientConnectedCallback += ((ulong obj) =>
        {
            if (OnConnected != null)
            {
                OnConnected.Invoke();
            }
        });

        NetworkManager.Singleton.OnServerStarted += Singleton_OnServerStarted;
    }

    private void Singleton_OnServerStarted()
    {
        if (OnServerStarted != null)
        {
            OnServerStarted.Invoke();
        }
    }

    private void OnServerInitialized()
    {
        if (OnConnected != null)
        {
            OnConnected.Invoke();
        }
    }

    private void SetXRPlugins()
    {


#if UNITY_STANDALONE_LINUX
        //UnityEngine.XR.XRSettings.enabled = false;
        //nityEngine.XR.XRDisplaySubsystem.Stop();

        Debug.Log("[NetworkConnectionHelper] Disabling XR Subsystem on server.");

        XRGeneralSettings.Instance.Manager.StopSubsystems();
        XRGeneralSettings.Instance.Manager.DeinitializeLoader();

        //var xrSettings = XRGeneralSettingsPerBuildTarget.XRGeneralSettingsForBuildTarget(BuildTargetGroup.Standalone);

        //if (xrSettings == null)
        //{
        //    XRGeneralSettingsPerBuildTarget buildTargetSettings = null;
        //    EditorBuildSettings.TryGetConfigObject(XRGeneralSettings.k_SettingsKey, out buildTargetSettings);
        //    xrSettings = buildTargetSettings.SettingsForBuildTarget(BuildTargetGroup.Standalone);
        //}

        //xrSettings.InitManagerOnStart = true;

        //if (!XRPackageMetadataStore.RemoveLoader(xrSettings.Manager, typeof(UnityEngine.XR.OpenXR.OpenXRLoader).FullName, BuildTargetGroup.Standalone))
        //{
        //    Debug.LogWarning("[NetworkConnectionHelper] OpenXR plugin could not be disabled!");
        //}

        //EditorUtility.SetDirty(xrSettings);

#endif
    }


    public void StartHost()
    {
        NetworkManager.Singleton.StartHost();
    }

    public void StartServer()
    {
        NetworkManager.Singleton.StartServer();
    }

    public void StartClient()
    {
        NetworkManager.Singleton.StartClient();
    }
}
