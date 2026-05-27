using Google.Play.AppUpdate;
using Google.Play.Common;
using System.Collections;
using UnityEngine;

public class m_InAppUpdateManager : MonoBehaviour
{
    private AppUpdateManager appUpdateManager;

    [Header("Custom Update UI")]
    [SerializeField] private GameObject updatePopup;
    [SerializeField]
    private m_Addressable_Script addressable_Script;
    private void Start()
    {
        // Hide popup initially
        updatePopup.SetActive(false);

        // Start update check
        StartCoroutine(CheckForUpdate());
       
       
    }
   
    IEnumerator CheckForUpdate()
    {
        appUpdateManager = new AppUpdateManager();

        // Request update info
        PlayAsyncOperation<AppUpdateInfo, AppUpdateErrorCode> appUpdateInfoOperation =
            appUpdateManager.GetAppUpdateInfo();

        yield return appUpdateInfoOperation;

        // Check if request successful
        if (appUpdateInfoOperation.IsSuccessful)
        {
            AppUpdateInfo appUpdateInfo = appUpdateInfoOperation.GetResult();

            // Is update available?
            if (appUpdateInfo.UpdateAvailability == UpdateAvailability.UpdateAvailable)
            {
                Debug.Log("Update Available");

                // SHOW YOUR CUSTOM POPUP HERE
                updatePopup.SetActive(true);
            }
            else
            {
                addressable_Script.BeginLoading();
                Debug.Log("No Update Available");
            }
        }
        else
        {
            addressable_Script.BeginLoading();
            Debug.Log("Update Check Failed");
        }
    }

    // Button Function
    public void OnClickUpdate()
    {
        StartCoroutine(StartFlexibleUpdate());
    }

    IEnumerator StartFlexibleUpdate()
    {
        PlayAsyncOperation<AppUpdateInfo, AppUpdateErrorCode> appUpdateInfoOperation =
            appUpdateManager.GetAppUpdateInfo();

        yield return appUpdateInfoOperation;

        if (appUpdateInfoOperation.IsSuccessful)
        {
            AppUpdateInfo appUpdateInfo = appUpdateInfoOperation.GetResult();

            // Start Flexible Update
            var startUpdateRequest = appUpdateManager.StartUpdate(
                appUpdateInfo,
                AppUpdateOptions.FlexibleAppUpdateOptions()
            );

            yield return startUpdateRequest;

           
        }
    }
}
